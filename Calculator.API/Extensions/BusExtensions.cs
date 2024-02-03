using MassTransit;

namespace Calculator.API.Extensions;

public static class BusExtensions
{
    public static Uri GetEndpointUri(this IBus bus, string endpointName)
    {
        return new Uri($"{bus.Address.Scheme}://{bus.Address.Host}/{endpointName}?bind=true");
    }

    public static Task<ISendEndpoint> GetSendEndpoint(this IBus bus, string endpointName)
    {
        return bus.GetSendEndpoint(bus.GetEndpointUri(endpointName));
    }
}
