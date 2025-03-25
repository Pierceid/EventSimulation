using EventSimulation.Presentation;
using EventSimulation.Simulations;
using System.Windows;
using System.Windows.Threading;

namespace EventSimulation.Observer {
    public class LineGraphObserver(Window window, LineGraph lineGraph) : IObserver {
        private Window window = window;
        private LineGraph lineGraph = lineGraph;

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore esc) {
                window.Dispatcher.Invoke(() => {
                    lineGraph.UpdatePlot(esc.CurrentReplication, esc.SimulationTime);
                }, DispatcherPriority.Input);
            }
        }
    }
}
