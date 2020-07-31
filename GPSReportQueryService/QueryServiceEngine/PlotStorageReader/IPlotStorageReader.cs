using QueryServiceEngine.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceiverEngine
{
    public interface IPlotStorageReader
    {
        Task<IList<Plot>> SearchAsync(string id, string from, string to);
    }
}