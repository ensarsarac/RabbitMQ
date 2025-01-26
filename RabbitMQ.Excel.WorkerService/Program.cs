using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Excel.WorkerService;
using RabbitMQ.Excel.WorkerService.Models;
using RabbitMQExample.Excel.Services;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext,services) =>
	{

		IConfiguration Configuration = hostContext.Configuration;

		services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });

		services.AddSingleton<RabbitMQClientService>();

		services.AddHostedService<Worker>();

		services.AddDbContext<AdventureWorks2019Context>(opt =>
		{
			opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		});
	})
	.Build();

await host.RunAsync();
