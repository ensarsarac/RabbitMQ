﻿using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQExample.Watermark.Services
{
	public class RabbitMQPublisher
	{
		private readonly RabbitMQClientService _rabbitmqClientService;

		public RabbitMQPublisher(RabbitMQClientService rabbitmqClientService)
		{
			_rabbitmqClientService = rabbitmqClientService;
		}

		public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
		{
			var channel = _rabbitmqClientService.Connect();

			var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

			var bodyByte = Encoding.UTF8.GetBytes(bodyString);

			var property = channel.CreateBasicProperties();

			property.Persistent = true;

			channel.BasicPublish(exchange:RabbitMQClientService.ExchangeName, routingKey:RabbitMQClientService.RoutingWatermark, property, bodyByte);
		}
	}
}
