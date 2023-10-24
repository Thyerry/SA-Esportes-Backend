using Domain.Dtos;

namespace Domain.Contracts;

public interface IRabbitMQService
{
    void Send(MessageDto message);
}