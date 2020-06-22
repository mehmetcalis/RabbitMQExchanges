using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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
                    channel.ExchangeDeclare("topic-exchange", durable: true, type: ExchangeType.Topic);

                    var queueName = channel.QueueDeclare().QueueName;

                    //string routingKey = "Warning.*.Info";
                    string routingKey = "#.Warning";
                    channel.QueueBind(queueName, exchange: "topic-exchange", routingKey: routingKey);




                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);

                    Console.WriteLine("Custom Loglar bekleniyor...");

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume(queueName, false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var log = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine("log alındı: " + log);
                        var time = int.Parse(GetMessage(args));
                        Thread.Sleep(time);

                        File.AppendAllText("logs_custom_erros.txt", log + "/n");


                        Console.WriteLine("loglama bitti...");

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
