using System.Threading.Tasks;

namespace ReceiverEngine
{
    public interface IPlotReceiver
    {
        Task ReceiveAsync(byte[] plot);
    }
}