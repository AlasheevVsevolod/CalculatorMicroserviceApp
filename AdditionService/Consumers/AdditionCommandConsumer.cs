using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AdditionService.Consumers;

public class AdditionCommandConsumer : BackgroundService
{
    private readonly IModel _channel;

    public AdditionCommandConsumer()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }

    private void Consume(object? sender, BasicDeliverEventArgs e)
    {
        var stringCommand = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine(stringCommand);

        _channel.BasicAck(e.DeliveryTag, false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += Consume;

        _channel.BasicConsume("test.queue", autoAck: false, consumer);

        return Task.CompletedTask;
    }
}
