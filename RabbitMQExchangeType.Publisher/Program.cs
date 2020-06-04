using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQExchangeType.Publisher
{
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
                    channel.ExchangeDeclare("logs", durable: true, type: ExchangeType.Fanout);
                    var message = GetMessage(args);

                    for (int i = 1; i < 11; i++)
                    {
                        var bodyByte = Encoding.UTF8.GetBytes($"{message}-{i}");
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish("logs", routingKey: "", properties, bodyByte);

                        Console.WriteLine($"Mesajınız gönderilmiştir: {message}-{i}");
                    }
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
