using Domain.Contracts;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Domain.Dtos;
using Microsoft.Extensions.Configuration;
using System;

namespace Domain.Service;

public class RabbitMQService : IRabbitMQService
{
    private IConnection _connection { get; set; }
    private IModel _channel { get; set; }

    private readonly string _exchangeName;
    private readonly string _routingKey;
    private readonly string _queueName;

    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(configuration["RabbitMqConfigs:Url"] ?? throw new InvalidOperationException()),
            ClientProvidedName = configuration["RabbitMqConfigs:ClientProvidedName"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _exchangeName = configuration["RabbitMqConfigs:ExchangeName"] ?? string.Empty;
        _routingKey = configuration["RabbitMqConfigs:RoutingKey"] ?? string.Empty;
        _queueName = configuration["RabbitMqConfigs:QueueName"] ?? string.Empty;
    }


    public void Send(MessageDto message)
    {
        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        _channel.QueueDeclare(_queueName, false, false, false, null);
        _channel.QueueBind(_queueName, _exchangeName, _routingKey, null);

        var messageSerialized = JsonSerializer.Serialize(message);

        byte[] messageBodyBytes = Encoding.UTF8.GetBytes(messageSerialized);
        _channel.BasicPublish(_exchangeName, _routingKey, null, messageBodyBytes);

        _channel.Close();
        _connection.Close();
    }
}