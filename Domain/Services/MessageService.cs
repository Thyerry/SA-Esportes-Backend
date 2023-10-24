using Domain.Contracts;
using Domain.Dtos;
using Domain.Validators;
using FluentValidation;

namespace Domain.Service;

public class MessageService : IMessageService
{
    private readonly IRabbitMQService _rabbitMqService;

    public MessageService(IRabbitMQService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService; 
    }
    public async Task<bool> SendText(MessageDto message)
    {
        var validator = new MessageDtoValidator();
        var validation = await validator.ValidateAsync(message);

        message.isValid = true;
        if (validation.Errors.Any())
        {
            message.isValid = false;
            message.ErrorList = validation.Errors.Select(e => e.ErrorMessage).ToArray();
        }

        _rabbitMqService.Send(message);
        return true;
    }
}