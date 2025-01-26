

// Fanaout exchange : Gelen veriyi tüm kuyruklara gönderir.


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


// Kuyruğu publisher olarak biz oluşturmayacağız. Consumer kendi kuyruğunu oluşturacak işi bittiğine bağlantı kesildiğinde kuyruk silinecek.

channel.ExchangeDeclare("logs-fanaout", durable: true, type: ExchangeType.Fanout);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
	string message = $"log {x}";

	byte[] convertMessage = Encoding.UTF8.GetBytes(message);

	// exchange kullanmadığımız için direk kuyruğa göndermek için routingKey'e kuyruk adını verdik.
	channel.BasicPublish("logs-fanaout","",null, body: convertMessage);

	Console.WriteLine($"Mesaj Gönderildi : {message}");
});



Console.ReadLine();

