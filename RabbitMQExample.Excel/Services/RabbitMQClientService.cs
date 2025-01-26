using RabbitMQ.Client;

namespace RabbitMQExample.Excel.Services
{
	public class RabbitMQClientService : IDisposable
    {

        private readonly ConnectionFactory _connectionFactory;
		private readonly ILogger<RabbitMQClientService> _logger;	
        private IModel _channel;
        private IConnection _connection;

		public static string QueueName = "ExcelDirectExchange";
		public static string RoutingName = "excel-route";
		public static string ExchangeName = "excel-file";

		public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
		{
			_connectionFactory = connectionFactory;
			_logger = logger;
		}

		public IModel Connection()
		{

			_connection = _connectionFactory.CreateConnection();

			if (_channel is {IsOpen : true}) return _channel;

			_channel = _connection.CreateModel();

			_channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, true, false);

			_channel.QueueDeclare(queue: QueueName, true, false, false, null);

			_channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingName);

			_logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

			return _channel;
		}

		public void Dispose()
		{
			_channel?.Close();
			_channel?.Dispose();

			_connection?.Close();
			_connection?.Dispose();

			_logger.LogInformation("RabbitMQ ile bağlantı koptu...");
		}
	}
}
