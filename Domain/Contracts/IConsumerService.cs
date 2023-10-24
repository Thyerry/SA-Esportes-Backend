namespace Domain.Contracts;

public interface IConsumerService
{
    void AddConsumer(string consumer);

    void DeleteConsumer(string consumer);

    int GetOnlineConsumers();

    string? GetConsumerById(string consumer);
}