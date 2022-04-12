using RabbitMQ.Client;
using System.Configuration;

namespace BoaSaudeRefund.Infra
{

    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }


    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message, string queueName = "")
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = System.Text.Encoding.UTF8.GetBytes(message.ToString());

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void SendMessage<T>(T message)
        {
            throw new System.NotImplementedException();
        }
    }

}
