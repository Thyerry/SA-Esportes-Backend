using Domain.Dtos;

namespace Domain.Contracts;

public interface IMessageService
{
    Task<bool> SendText(MessageDto message);
}