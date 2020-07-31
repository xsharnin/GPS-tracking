using System.Collections.Generic;
using System.Threading.Tasks;
using QueryServiceEngine.Entities;

namespace QueryServiceEngine
{
    public interface IJourneyService
    {
        Task<List<Journey>> GetAsync(string id, string from, string to);
    }
}