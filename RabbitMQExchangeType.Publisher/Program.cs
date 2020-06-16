﻿using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQExchangeType.Publisher
{

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
                    channel.ExchangeDeclare("direct-exchange", durable: true, type: ExchangeType.Direct);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));

                    for (int i = 1; i < 11; i++)
                    {

                        Random rnd = new Random();
                        LogNames log = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));

                        var bodyByte = Encoding.UTF8.GetBytes($"log={log.ToString()}");


                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish("direct-exchange", routingKey: log.ToString(), properties, bodyByte);

                        Console.WriteLine($"Log Mesajınız gönderilmiştir: {log.ToString()}-{i}");
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
