using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMQExample.Excel.Services
{
    public class RabbitMQPublisher
    {
		private readonly RabbitMQClientService _rabbitmqClientService;

		public RabbitMQPublisher(RabbitMQClientService rabbitmqClientService)
		{
			_rabbitmqClientService = rabbitmqClientService;
		}

		public void Publish(CreateExcelMessage model)
		{
			var channel = _rabbitmqClientService.Connection();

			var bodyString = JsonSerializer.Serialize(model);

			var bodyByte = Encoding.UTF8.GetBytes(bodyString);

			var property = channel.CreateBasicProperties();

			property.Persistent = true;

			channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingName, property, bodyByte);
		}
	}
}
