using MessagePack;

namespace Producer
{
    [MessagePackObject]
    public class Plot
    {
        [Key(0)]
        public string VId { get; set; }
        [Key(2)]
        public string Lat { get; set; }
        [Key(3)]
        public string Lon { get; set; }
        [Key(4)]
        public EventCode EventCode { get; set; }
        [Key(5)]
        public string TimeStamp { get; set; }

        public override string ToString()
        {
            return $" VId: {VId} Lat: {Lat} Lon: {Lon} EventCode: {EventCode} TimeStamp: {TimeStamp}";
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;

            var plot = obj as Plot;
            if (plot == null)
                return false;

            return plot.VId == VId &&
                   plot.Lat == Lat &&
                   plot.Lon == Lon &&
                   plot.EventCode == EventCode &&
                   plot.TimeStamp == TimeStamp;
        }
    }

    public enum EventCode
    {
        IgnitionOn = 0,
        Movement = 1,
        IgnitionOff = 2
    }

    public static class PlotExt
    {
        public static byte[] ToBytes(this Plot plot)
        {
            return MessagePackSerializer.Serialize(plot);
        }

        public static Plot ToPlot(this byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<Plot>(bytes);
        }
    }
}