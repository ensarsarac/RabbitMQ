

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// bağlantı adresi
var factory = new ConnectionFactory();

factory.Uri = new Uri("rabbitmq_server");

// bağlantı sağlandı
using var connection = factory.CreateConnection();


// kanal oluşturuldu
using var channel = connection.CreateModel();


// durable : rabbitmq ram de mi olsun bellekde mi olsun. Resetlendiğinde ram sıfırlanır rabbitmq da veriler silinir.
// exclusive : farklı kanallardan erişim sağlamak için kullanılır. true ise aynı kanal false ise farklı kanal. Genellikle false kullanlılır.
// autoDelete : son consumer kuyruktan koptuğu zaman kuyruk silinir. Genellikle false kullanılır


// Consumer tarafında bu kuyruk  oluşturma kodu olmayabilir eğer böyle bir kuyruk yoksa kuyruk oluşturur var ise bir şey yapmaz sorun teşkil etmez ama parametreler aynı olması önemli.
//channel.QueueDeclare("first-queue", durable: true, exclusive: false, autoDelete: false);

var consumer = new EventingBasicConsumer(channel);

// autoAck true olduğunda aldığı mesajı kuyruktan siler.
channel.BasicConsume("first-queue", autoAck: false, consumer: consumer);


consumer.Received += (model, data) =>
{
	var body = data.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);
	Thread.Sleep(1500);
	Console.WriteLine("Kuyruk Mesajı : " + message);
	// consumer tarafından veriyi aldım diye kuyruğa bildirir ve mesajın silinmesini sağlar.
	channel.BasicAck(data.DeliveryTag, false);
};



Console.ReadLine();