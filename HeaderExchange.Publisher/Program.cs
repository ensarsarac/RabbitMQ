

// Header Exchange : routekey'i header içerisinde gönderir

using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

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

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");

IBasicProperties basicProperties = channel.CreateBasicProperties();

basicProperties.Headers = headers;
basicProperties.Persistent = true; // mesajlarıu kalıcı hale getirmek için kullanılır

// diğer exchange türlerinde de mesajları kalıcı hale getirmek için aşağıda ki gibi kullanıp publish ederken ilgili parametreyi verebilriiz.

//IBasicProperties basicProperties = channel.CreateBasicProperties();

//basicProperties.Persistent = true;

var product = new Product
{
	Id = 1,
	Name = "Kalem",
	Price = 100,
	Stock = 10
};

var jsonData = JsonSerializer.Serialize(product);

channel.BasicPublish("header-exchange", string.Empty, basicProperties,Encoding.UTF8.GetBytes(jsonData));

Console.WriteLine("Mesaj gönderildi");

Console.ReadLine();
