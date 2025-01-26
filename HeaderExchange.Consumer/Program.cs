


// Header Exchange : routekey'i header içerisinde gönderir


using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

// bağlantı adresi
var factory = new ConnectionFactory();

factory.Uri = new Uri("rabbitmq_server");

// bağlantı sağlandı
using var connection = factory.CreateConnection();

// kanal oluşturuldu
using var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "all");

channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

channel.BasicConsume(queueName, autoAck: false, consumer: consumer);


consumer.Received += (model, data) =>
{
	var body = data.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine("Kuyruk Mesajı : " + message);

	Console.WriteLine("\n\n");

	Product product = JsonSerializer.Deserialize<Product>(message);

	Thread.Sleep(1500);
	Console.WriteLine($"Kuyruk Mesajı : \nProduct Id:{product.Id}\nProduct Name:{product.Name}\nProduct Price:{product.Price}\nProduct Stock:{product.Stock}");

	//File.AppendAllText("log-critical.txt", message + "\n");

	channel.BasicAck(data.DeliveryTag,false);
};



Console.ReadLine();