using Domain.Contracts;

namespace Domain.Service;

public class ConsumerService : IConsumerService
{
    private readonly List<string?> _consumers = new List<string?>();

    public void AddConsumer(string consumer)
    {
        lock (_consumers)
        {
            if (!_consumers.Contains(consumer))
                _consumers.Add(consumer);
        }
    }

    public void DeleteConsumer(string consumer)
    {
        lock (_consumers)
        {
            if (_consumers.Contains(consumer))
                _consumers.Remove(consumer);
        }
    }

    public int GetOnlineConsumers()
    {
        lock (_consumers)
        {
            return _consumers.Count;
        }
    }

    public string? GetConsumerById(string consumer)
    {
        lock (_consumers)
        {
            return _consumers.FirstOrDefault(c => c == consumer);
        }
    }
}