using System;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    public interface IPublisher
    {
        Task<bool> PublishAsync(byte[] plot);
    }
}
