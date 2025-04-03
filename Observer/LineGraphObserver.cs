using EventSimulation.Presentation;
using EventSimulation.Simulations;
using System.Windows;
using System.Windows.Threading;

namespace EventSimulation.Observer {
    public class LineGraphObserver : IObserver {
        private Window window;
        private LineGraph lineGraph;

        public LineGraphObserver(Window window, LineGraph lineGraph) {
            this.window = window;
            this.lineGraph = lineGraph;
        }

        public void Refresh(SimulationCore simulationCore) {
            window.Dispatcher.Invoke(() => {
                lineGraph.UpdatePlot(simulationCore);
            }, DispatcherPriority.Input);
        }
    }
}
