using System.Runtime.CompilerServices;
using Domain.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class MessageHub : Hub
{
    private readonly IConsumerService _consumerService;

    public MessageHub(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    public async Task SendMessage(MessageDto message)
    {
        await Clients.Group("Consumers").SendAsync("ReceiveMessage", message);
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Consumers");
        _consumerService.AddConsumer(Context.ConnectionId);
        await DisplayOnlineConsumers();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Consumers");
        var consumer = _consumerService.GetConsumerById(Context.ConnectionId);
        _consumerService.DeleteConsumer(consumer);

        await DisplayOnlineConsumers();
        await base.OnDisconnectedAsync(exception);
    }

    private async Task DisplayOnlineConsumers()
    {
        var onlineConsumers = _consumerService.GetOnlineConsumers();
        await Clients.Groups("Consumers").SendAsync("OnlineConsumers", onlineConsumers);
    }
}