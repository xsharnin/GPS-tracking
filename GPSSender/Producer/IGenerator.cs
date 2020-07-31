using System;
using System.Collections.Generic;
using System.Text;

namespace Producer
{
    public interface IGenerator
    {
        byte[] Generate(string vehicleId);
    }
}
