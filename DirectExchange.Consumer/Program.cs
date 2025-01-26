

// Direct Exchange : Gelen mesajı route bilgisine göre ilgili kuyruğa gönderir.

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

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = "direct-queue-Critical";

channel.BasicConsume(queueName, autoAck: false, consumer: consumer);


consumer.Received += (model, data) =>
{
	var body = data.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);
	Thread.Sleep(1500);
	Console.WriteLine("Kuyruk Mesajı : " + message);

	File.AppendAllText("log-critical.txt", message + "\n");

	channel.BasicAck(data.DeliveryTag, false);
};



Console.ReadLine();