using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

        factory.ClientProvidedName = "Rabbit Receiver-1 App";

        IConnection cnn = factory.CreateConnection();

        IModel channel = cnn.CreateModel();

        string exchangeName = "SAExchange";
        string routingKey = "sa-routing-key";
        string queueName = "MessageQueue";

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, false, false, false, null);
        channel.QueueBind(queueName, exchangeName, routingKey, null);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Message Received: {message}");
            channel.BasicAck(args.DeliveryTag, false);
        };

        string consumerTag = channel.BasicConsume(queueName, false, consumer);

        Console.ReadLine();

        channel.BasicCancel(consumerTag);

        channel.Close();
        cnn.Close();
    }
}