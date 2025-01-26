using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQExample.Excel.Models;
using RabbitMQExample.Excel.Services;

namespace RabbitMQExample.Excel.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

		public ProductController(UserManager<IdentityUser> userManager, AppDbContext appDbContext, RabbitMQPublisher rabbitMQPublisher)
		{
			_userManager = userManager;
			_appDbContext = appDbContext;
			_rabbitMQPublisher = rabbitMQPublisher;
		}

		public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateProduct()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if(user == null)
            {
                return RedirectToAction("Login","Account");
            }

            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1,10)}";

            UserFile userFile = new UserFile
            {
                FileName = fileName,
                UserId = user.Id,
                FileStatus = FileStatus.Creating
            };

            _appDbContext.UserFiles.Add(userFile);

            await _appDbContext.SaveChangesAsync();

            _rabbitMQPublisher.Publish(new Shared.CreateExcelMessage
            {
                FileId = userFile.Id,
            });

            TempData["StartCreatingExcel"] = true;

            return RedirectToAction("Files", "Product");
        }

        public async Task<IActionResult> Files()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var data = await _appDbContext.UserFiles.Where(x => x.UserId == user.Id).ToListAsync();
            return View(data);

        }
        
    }
}
