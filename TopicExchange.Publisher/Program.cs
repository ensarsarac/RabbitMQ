

// Topix Exchange : özel route key belirleyerek ilgili kuyuruklara mesajı gönderir.
// örnek : *.Information.* ortası Information olacak, consumer tarafından kuyruk oluşturulacak.

using RabbitMQ.Client;
using System.Text;
using TopicExchange.Publisher;

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

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

Random rnd = new Random();

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

	LogNames log1 = (LogNames)rnd.Next(1, 5);
	LogNames log2 = (LogNames)rnd.Next(1, 5);
	LogNames log3 = (LogNames)rnd.Next(1, 5);

	var routeKey = $"{log1}.{log2}.{log3}";

	string message = $"log-type: {log1}-{log2}-{log3}";

	byte[] convertMessage = Encoding.UTF8.GetBytes(message);

	channel.BasicPublish("logs-topic", routeKey, null, body: convertMessage);

	Console.WriteLine($"Mesaj Gönderildi : {message}");
});



Console.ReadLine();
