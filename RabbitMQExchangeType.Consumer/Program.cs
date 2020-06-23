using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace RabbitMQExchangeType.Consumer
{

    public enum LogNames
    {
        Critical,
        Error
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

                    channel.QueueDeclare("Kuyruk1", false, false, false, null);

                    Dictionary<string, object> headers = new Dictionary<string, object>();

                    headers.Add("format", "pdf");
                    headers.Add("shape", "A4");
                    headers.Add("x-match", "any");


                    channel.QueueBind("Kuyruk1", "header-exchange", string.Empty, headers);

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("Kuyruk1", false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var log = Encoding.UTF8.GetString(ea.Body.ToArray());

                        Console.WriteLine($"gelen mesaj: {log}");
                     

                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    Console.WriteLine("Çıkış yapmak için tıklayınız...");
                    Console.ReadLine();
                }
            }

        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}
