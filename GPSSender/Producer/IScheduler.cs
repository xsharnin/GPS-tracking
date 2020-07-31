using System;
using System.Collections.Generic;
using System.Text;

namespace Producer
{
    public interface IScheduler
    {
        void Start(int interval, string vehicleId);
        void Stop();
    }
}
