using System.Collections.Generic;
using QueryServiceEngine.Entities;

namespace QueryServiceEngine
{
    public interface IPlotsAnalyser
    {
        List<Journey> GetJourneys(IList<Plot> orderedPlots);
    }
}