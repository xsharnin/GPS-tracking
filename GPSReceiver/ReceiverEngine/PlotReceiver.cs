using System.Threading.Tasks;

namespace ReceiverEngine
{
    public class PlotReceiver : IPlotReceiver
    {
        private readonly IPlotStorageWriter _plotStorageWriter;
        public PlotReceiver(IPlotStorageWriter plotStorageWriter)
        {
            _plotStorageWriter = plotStorageWriter;
        }

        public async Task ReceiveAsync(byte[] plot)
        {
            await _plotStorageWriter.AddPlotAsync(plot.ToPlot());
        }        
    }
}
