using QueryServiceEngine;
using QueryServiceEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestsAndLoadTests
{
    public class PlotsAnalyserUnitTests
    {
        [Fact]
        public void ShouldReturnJourneysAsPerTask()
        {
            var journeys = new PlotsAnalyser().GetJourneys(StubPlots);

            Assert.Equal(5, journeys.Count);
            Assert.Equal("la2", journeys[0].Lat);
            Assert.Equal("lo2", journeys[0].Lon);
            Assert.Equal("t2", journeys[0].JourneyStart);
            Assert.Null(journeys[0].JourneyEnd);

            Assert.Equal("la3", journeys[1].Lat);
            Assert.Equal("lo3", journeys[1].Lon);
            Assert.Equal("t2", journeys[1].JourneyStart);
            Assert.Null(journeys[1].JourneyEnd);

            Assert.Equal("la4", journeys[2].Lat);
            Assert.Equal("lo4", journeys[2].Lon);
            Assert.Equal("t2", journeys[2].JourneyStart);
            Assert.Equal("t4", journeys[2].JourneyEnd);

            Assert.Equal("la5", journeys[3].Lat);
            Assert.Equal("lo5", journeys[3].Lon);
            Assert.Equal("t5", journeys[3].JourneyStart);
            Assert.Null(journeys[3].JourneyEnd);

            Assert.Equal("la6", journeys[4].Lat);
            Assert.Equal("lo6", journeys[4].Lon);
            Assert.Equal("t5", journeys[4].JourneyStart);
            Assert.Equal("t6", journeys[4].JourneyEnd);
        }

        [Fact]
        public void ShouldReturnEmptyJourney()
        {
            var journeys = new PlotsAnalyser().GetJourneys(StubPlots);
            Assert.Equal(5, journeys.Count);
            journeys = new PlotsAnalyser().GetJourneys(new List<Plot>());
            Assert.Empty(journeys);
        }

        public static List<Plot> StubPlots => new List<Plot>()
            {
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.Movement,
                    Lat = "la0",
                    Lon = "lo0",
                    TimeStamp = "t0"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.IgnitionOff,
                    Lat = "la1",
                    Lon = "lo1",
                    TimeStamp = "t1"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.IgnitionOn,
                    Lat = "la2",
                    Lon = "lo2",
                    TimeStamp = "t2"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.Movement,
                    Lat = "la3",
                    Lon = "lo3",
                    TimeStamp = "t3"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.IgnitionOff,
                    Lat = "la4",
                    Lon = "lo4",
                    TimeStamp = "t4"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.IgnitionOn,
                    Lat = "la5",
                    Lon = "lo5",
                    TimeStamp = "t5"

                },
                new Plot()
                {
                    VId = "v1",
                    EventCode = EventCode.IgnitionOff,
                    Lat = "la6",
                    Lon = "lo6",
                    TimeStamp = "t6"

                }
         };
    
    }
}

