

using RabbitMQ.Client;
using System.Text;

// bağlantı adresi
var factory = new ConnectionFactory();

factory.Uri = new Uri("rabbitmq_server");

// bağlantı sağlandı
using var connection = factory.CreateConnection();


// kanal oluşturuldu
var channel = connection.CreateModel();

// consumera tek bir seferde kaç mesaj ileteceğini söyler
channel.BasicQos(0, 1, false);

// durable : rabbitmq ram de mi olsun bellekde mi olsun. Resetlendiğinde ram sıfırlanır rabbitmq da veriler silinir.
// exclusive : farklı kanallardan erişim sağlamak için kullanılır. true ise aynı kanal false ise farklı kanal. Genellikle false kullanlılır.
// autoDelete : son consumer kuyruktan koptuğu zaman kuyruk silinir. Genellikle false kullanılır


// kuyruk oluşturuldu
channel.QueueDeclare("first-queue", durable: true, exclusive: false, autoDelete: false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
	string message = $"Message {x}";

	byte[] convertMessage = Encoding.UTF8.GetBytes(message);

	// exchange kullanmadığımız için direk kuyruğa göndermek için routingKey'e kuyruk adını verdik.
	channel.BasicPublish(exchange: string.Empty, routingKey: "first-queue", body: convertMessage);

	Console.WriteLine($"Mesaj Gönderildi : {message}");
});



Console.ReadLine();

