using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQExchangeType.Publisher
{
    //Critical.Error.Info Info.Warning.Critical   
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Info = 3,
        Warning = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://sejocreq:VgwWgwRr9uSpcKNWvHDetO8k2Fk7rj_I@fox.rmq.cloudamqp.com/sejocreq");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

                    Dictionary<string, object> headers = new Dictionary<string, object>();

                    headers.Add("format", "pdf");
                    headers.Add("shape", "A4");

                    var properties = channel.CreateBasicProperties();
                    properties.Headers = headers;

                    channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("Header mesajım"));

                    
                }
                Console.WriteLine("Çıkış yapmak için tıklanıyınız.");
                Console.ReadLine();
            }
        }

        private static string GetMessage(string[] arg)
        {
            return arg[0].ToString();
        }
    }
}
