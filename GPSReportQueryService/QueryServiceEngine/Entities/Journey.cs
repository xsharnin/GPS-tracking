using System;
using System.Collections.Generic;

namespace QueryServiceEngine.Entities
{
    public class Journey : Plot
    {
        public string JourneyStart { get; set; }
        public string JourneyEnd { get; set; }

        public Journey()
        {

        }

        public Journey(Plot plot)
        {
            VId = plot.VId;
            Lat = plot.Lat;
            Lon = plot.Lon;
            EventCode = plot.EventCode;
            TimeStamp = plot.TimeStamp;
        }

        public override string ToString()
        {
            return $" VId: {VId} Lat: {Lat} Lon: {Lon} EventCode: {EventCode} TimeStamp: {TimeStamp} TimeStamp: {JourneyStart ?? String.Empty} TimeStamp: {JourneyEnd ?? String.Empty}";
        }
    }   
}