using System;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    public interface ISubscriber
    {
        void Subscribe(Func<byte[], Task> receiverCommand);
        void Unsubscribe();
    }
}