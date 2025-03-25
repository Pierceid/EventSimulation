using EventSimulation.Observer;
using EventSimulation.Simulations;
using EventSimulation.Strategies;
using OxyPlot.Wpf;
using System.Windows;

namespace EventSimulation.Presentation {
    public class Facade {
        private Window? mainWindow;
        private Carpentry? carpentry;
        private LineGraph? graph;
        private Thread? simulationThread;
        private bool isRunning;

        public Facade(Window? window) {
            mainWindow = window;
            carpentry = null;
            graph = null;
            simulationThread = null;
            isRunning = false;
        }

        public void StartSimulation() {
            if (carpentry == null || graph == null || isRunning) return;

            isRunning = true;
            graph.RefreshGraph();

            simulationThread = new(carpentry.RunSimulation) { IsBackground = true };
            simulationThread.Start();
        }

        public void StopSimulation() {
            if (carpentry == null || !isRunning) return;

            carpentry.Stop();
            isRunning = false;

            simulationThread?.Join();
            simulationThread = null;
        }

        public void AnalyzeReplication() {
            if (carpentry == null) return;


        }

        public void SetStrategy(Strategy strategy) {
            if (carpentry == null || graph == null) return;

            if (isRunning) {
                StopSimulation();
            }

            graph.RefreshGraph();
        }

        public void InitGraph(PlotView plotView) {
            if (isRunning) {
                StopSimulation();
            }

            graph = new(
                modelTitle: "Costs Over Time",
                xAxisTitle: "Replications",
                yAxisTitle: "Costs",
                seriesTitle: "Costs",
                plotView: plotView
            );

            InitObservers();
        }

        public void InitCarpentry(int replications, double speed) {
            if (isRunning) {
                StopSimulation();
            }

            carpentry = new Carpentry(replications) { Speed = speed };

            InitObservers();
        }

        public void UpdateCarpentry(double speed) {
            if (carpentry == null) return;

            carpentry.Speed = speed;
        }

        private void InitObservers() {
            if (carpentry == null || mainWindow == null || graph == null) return;

            LineGraphObserver lineGraphObserver = new(mainWindow, graph);

            carpentry.Attach(lineGraphObserver);
        }
    }
}
