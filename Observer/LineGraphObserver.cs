using EventSimulation.Presentation;
using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
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
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                if (esc is Carpentry c) {
                    window.Dispatcher.Invoke(() => {
                        lineGraph.UpdatePlot(c.CurrentReplication, c.AverageOrderTime.GetAverage());
                    }, DispatcherPriority.Input);
                }
            }
        }
    }
}
