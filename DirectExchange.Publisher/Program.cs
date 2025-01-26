

// Direct Exchange : Gelen mesajı route bilgisine göre ilgili kuyruğa gönderir.

using DirectExchange.Publisher;
using RabbitMQ.Client;
using System.Text;

// bağlantı adresi
var factory = new ConnectionFactory();

factory.Uri = new Uri("rabbitmq_server");

// bağlantı sağlandı
using var connection = factory.CreateConnection();


// kanal oluşturuldu
var channel = connection.CreateModel();


// durable : rabbitmq ram de mi olsun bellekde mi olsun. Resetlendiğinde ram sıfırlanır rabbitmq da veriler silinir.
// exclusive : farklı kanallardan erişim sağlamak için kullanılır. true ise aynı kanal false ise farklı kanal. Genellikle false kullanlılır.
// autoDelete : son consumer kuyruktan koptuğu zaman kuyruk silinir. Genellikle false kullanılır



channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(name =>
{
	var queueName = $"direct-queue-{name}";
	channel.QueueDeclare(queueName, true, false, false);
	var routeKey = $"route-{name}";

	channel.QueueBind(queueName, "logs-direct", routeKey, null);
});


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
	LogNames log = (LogNames)new Random().Next(1, 5);

	string message = $"log-type: {log}";

	byte[] convertMessage = Encoding.UTF8.GetBytes(message);

	var routeKey = $"route-{log}";

	channel.BasicPublish("logs-direct", routeKey, null, body: convertMessage);

	Console.WriteLine($"Mesaj Gönderildi : {message}");
});



Console.ReadLine();
