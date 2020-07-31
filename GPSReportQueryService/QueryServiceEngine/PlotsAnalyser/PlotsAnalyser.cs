using QueryServiceEngine.Entities;
using System.Collections.Generic;

namespace QueryServiceEngine
{
    public class PlotsAnalyser : IPlotsAnalyser
    {
        public List<Journey> GetJourneys(IList<Plot> orderedPlots)
        {
            return ConvertToJourney(orderedPlots, 0, -1);
        }

        private List<Journey> ConvertToJourney(IList<Plot> plots, int position, int currentStart)
        {
            var journeys = new List<Journey>();

            if (position < plots.Count)
            {
                //Journey starts
                if (plots[position].EventCode == EventCode.IgnitionOn)
                {
                    currentStart = position;
                }

                //Journey ends
                if (currentStart != -1 && plots[position].EventCode == EventCode.IgnitionOff)
                {
                    journeys.Add(
                        new Journey(plots[position])
                        {
                            JourneyStart = plots[currentStart].TimeStamp,
                            JourneyEnd = plots[position].TimeStamp
                        });
                    currentStart = -1;
                }

                if (currentStart != -1)
                {
                    journeys.Add(
                        new Journey(plots[position])
                        {
                            JourneyStart = plots[currentStart].TimeStamp
                        });
                }

                journeys.AddRange(ConvertToJourney(plots, position + 1, currentStart));
            }


            return journeys;
        }

    }
}
