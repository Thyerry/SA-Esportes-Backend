using Domain.Dtos;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApi.Hubs;

namespace WebApi.RabbitMQ;

public class RabbitMqConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IHubContext<MessageHub> _hubContext;

    public RabbitMqConsumer(IHubContext<MessageHub> hubContext, IConfiguration configuration)
    {
        _hubContext = hubContext;
        var factory = new ConnectionFactory
        {
            Uri = new Uri(configuration["RabbitMqConfigs:Url"]),
            ClientProvidedName = configuration["RabbitMqConfigs:ClientProvidedName"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare("MessageQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void StartListening()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonSerializer.Deserialize<MessageDto>(message);
            Debug.WriteLine(message);
            SendToSignalR(deserializedMessage);
        };

        _channel.BasicConsume("MessageQueue", true, consumer);
    }

    private void SendToSignalR(MessageDto message)
    {
        _hubContext.Clients.Group("Consumers").SendAsync("ReceiveMessage", message);
    }
}