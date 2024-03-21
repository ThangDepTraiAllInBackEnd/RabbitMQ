using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using RabbitMQ.Client.Exceptions;

namespace TheProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenMessageController : ControllerBase
    {
        [HttpPost("addproduct")]
        public IActionResult AddProduct(string msg)
        {

            //var factory = new ConnectionFactory()
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


            ////Serialize the message
            //var json = JsonConvert.SerializeObject(msg);
            //var body = Encoding.UTF8.GetBytes(json);
            ////put the data on to the product queue
            //channel.BasicPublish(exchange: "", routingKey: "product", body: body);




            //byte[] messagebuffer = Encoding.Default.GetBytes("Direct Message");
            //
            //Console.WriteLine("Message Sent");






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

            //Console.WriteLine("creating exchange");
            //// create exchange
            //model.ExchangeDeclare("productExchange", ExchangeType.Direct);

            ////---------------------------------
            //model.QueueDeclare("product", true, false, false, null);
            //Console.WriteLine("Creating Queue");

            //// Bind Queue to Exchange
            //model.QueueBind("product", "productExchange", "directexchange_key");

            //Console.WriteLine("Creating Binding");

            //Console.ReadLine();

            ////---------------------------------
            var properties = model.CreateBasicProperties();
            properties.Persistent = false;

            byte[] messagebuffer = Encoding.Default.GetBytes("Direct Message");
            model.BasicPublish("productExchange", "directexchange_key", properties, messagebuffer);
            Console.WriteLine("Message Sent");


            return Ok();
        }
    }
}
