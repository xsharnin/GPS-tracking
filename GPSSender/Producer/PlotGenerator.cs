using System;
using System.Collections.Generic;
using System.Text;

namespace Producer
{
    public class PlotGenerator : IGenerator
    {
        private readonly Random _random;
        private Plot _lastPlot;
        private int _step;
        public PlotGenerator()
        {
            _random = new Random();
        }

        public byte[] Generate(string vehicleId)
        {
            _lastPlot = GenerateRandomPlot(vehicleId, _lastPlot);

            return _lastPlot.ToBytes();
        }

        public Plot GenerateRandomPlot(string vehicleId, Plot _lastPlot = null)
        {
            return GetRandomPlot(vehicleId, _lastPlot);
        }

        private Plot GetRandomPlot(string vehicleId, Plot _lastPlot = null)
        {
            var currentPlot = new Plot
            {
                VId = vehicleId,
                Lat = "la" + _step,
                Lon = "lo" + _step,
                TimeStamp = "t" + _step
            };

            if (_lastPlot == null)
                currentPlot.EventCode = (EventCode)_random.Next(3);
            else if (_lastPlot.EventCode == EventCode.IgnitionOff)
                currentPlot.EventCode = EventCode.IgnitionOn;
            else
                currentPlot.EventCode = (EventCode)_random.Next(1, 3);

            _step++;
            return currentPlot;
        }
    }
}

