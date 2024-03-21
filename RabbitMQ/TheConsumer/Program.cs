// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using System.Reflection;
Console.WriteLine("Hello, World!");


//var factory = new ConnectionFactory
//{
//    HostName = "localhost",
//    UserName = ConnectionFactory.DefaultUser,
//    Password = ConnectionFactory.DefaultPass,
//    Port = AmqpTcpEndpoint.UseDefaultPort,
//};
////Create the RabbitMQ connection using connection factory details as i mentioned above
//var connection = factory.CreateConnection();
////Here we create channel with session and model
//using
//var channel = connection.CreateModel();
////declare the queue after mentioning name and a few property related to that
//channel.QueueDeclare("product", exclusive: false);
////channel.QueueDeclare("product", true, true, false, null);
////Set Event object which listen message from chanel which is sent by producer
//var consumer = new EventingBasicConsumer(channel);
//consumer.Received += (model, eventArgs) =>
//{
//    var body = eventArgs.Body.ToArray();
//    var message = Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Product message received: {message}");
//};
////read the message
//channel.BasicConsume(queue: "product", autoAck: true, consumer: consumer);

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = AmqpTcpEndpoint.UseDefaultPort,
    UserName = ConnectionFactory.DefaultUser,
    Password = ConnectionFactory.DefaultPass,
    VirtualHost = "/"
};

var connection = factory.CreateConnection();
var model = connection.CreateModel();

model.ExchangeDeclare("productExchange", ExchangeType.Direct);
model.QueueDeclare("product", true, false, false, null);
model.QueueBind("product", "productExchange", "directexchange_key");

var consumer = new EventingBasicConsumer(model);
consumer.Received += (sender, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Received message: {0}", message);
};

model.BasicConsume(queue: "product",
                          autoAck: true,
                          consumer: consumer);

Console.WriteLine("Waiting for messages. To exit press CTRL+C");
Console.ReadKey();
