using ClosedXML.Excel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Excel.WorkerService.Models;
using RabbitMQExample.Excel.Services;
using Shared;
using System.Data;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Excel.WorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly RabbitMQClientService _rabbitMQClientService;
		private readonly IServiceProvider _serviceProvider;
		private IModel _channel;

		public Worker(ILogger<Worker> logger, RabbitMQClientService rabbitMQClientService, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_rabbitMQClientService = rabbitMQClientService;
			_serviceProvider = serviceProvider;

		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_channel = _rabbitMQClientService.Connection();

			_channel.BasicQos(0, 1, false);

			return base.StartAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new AsyncEventingBasicConsumer(_channel);

			_channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);

			consumer.Received += Consumer_Received;

			return Task.CompletedTask;
		}

		private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
		{
			await Task.Delay(1000);

			var excel = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

			using var ms = new MemoryStream();

			var workbook = new XLWorkbook();

			var dataset = new DataSet();

			dataset.Tables.Add(GetTable("products"));

			workbook.Worksheets.Add(dataset);
			workbook.SaveAs(ms);

			MultipartFormDataContent multipartFormDataContent = new();

			multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()),"file",Guid.NewGuid().ToString()+".xlsx");

			var baseUrl = "https://localhost:7191/api/files";

			using(var httpClient = new HttpClient() ) {

				var response = await httpClient.PostAsync($"{baseUrl}?fileId={excel.FileId}",multipartFormDataContent);

				if (response.IsSuccessStatusCode)
				{
					_logger.LogInformation($"File ( Id: {excel.FileId} was created by successfull)");
					_channel.BasicAck(@event.DeliveryTag, false);
				}
			};
		}

		private DataTable GetTable(string tableName)
		{
            List<WorkerService.Models.Product> products;

			using(var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<AdventureWorks2019Context>();

				products = context.Products.ToList();
			}

			DataTable table = new DataTable { TableName = tableName };

			table.Columns.Add("ProductId", typeof(int));
			table.Columns.Add("Name", typeof(string));
			table.Columns.Add("ProductNumber", typeof(string));
			table.Columns.Add("MakeFlaq", typeof(bool));
			table.Columns.Add("Color", typeof(string));


			products.ForEach(x =>
			{
				table.Rows.Add(x.ProductId, x.Name, x.ProductNumber, x.MakeFlag, x.Color);
			});

			return table;

		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{


			return base.StopAsync(cancellationToken);
		}
	}
}
