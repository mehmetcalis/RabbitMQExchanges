using RabbitMQ.Client;
using System;
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
                    channel.ExchangeDeclare("topic-exchange", durable: true, type: ExchangeType.Topic);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));

                    for (int i = 1; i < 11; i++)
                    {

                        Random rnd = new Random();
                        LogNames log1 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log2 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log3 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));

                        var bodyByte = Encoding.UTF8.GetBytes($"log={log1.ToString()}-{log2.ToString()}-{log3.ToString()}");
                        string routingKey = $"{ log1}.{log2}.{log3}";

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish("topic-exchange", routingKey: routingKey, properties, bodyByte);

                        Console.WriteLine($"Log Mesajınız gönderilmiştir => mesaj: {routingKey}");
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
