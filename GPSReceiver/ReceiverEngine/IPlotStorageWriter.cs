using System.Threading.Tasks;

namespace ReceiverEngine
{
    public interface IPlotStorageWriter
    {
        Task<bool> AddPlotAsync(Plot plot);
    }
}