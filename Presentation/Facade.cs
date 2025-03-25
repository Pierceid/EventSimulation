using EventSimulation.Observer;
using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using OxyPlot.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace EventSimulation.Presentation {
    public class Facade {
        private Window? mainWindow;
        private Carpentry? carpentry;
        private LineGraph? graph;
        private double replicationTime;
        private Thread? simulationThread;
        private bool isRunning;

        public Facade(Window? window) {
            mainWindow = window;
            carpentry = null;
            graph = null;
            replicationTime = 249 * 8 * 60 * 60;
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

        public void PauseSimulation() {
            if (carpentry == null) return;

            carpentry.IsPaused = !carpentry.IsPaused;
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
        }

        public void InitCarpentry(int replications, double speed, int workersA, int workersB, int workersC) {
            if (isRunning) {
                StopSimulation();
            }

            carpentry = new(replications, replicationTime) { Speed = speed };
            carpentry.Workshop.InitComponents(workersA, workersB, workersC);
        }

        public void UpdateCarpentry(double speed) {
            if (carpentry == null) return;

            carpentry.Speed = speed;
        }

        public void InitObservers(TextBlock timeTextBlock, DataGrid orderDataGrid, DataGrid workerDataGrid) {
            if (carpentry == null || mainWindow == null || graph == null) return;

            //LineGraphObserver lineGraphObserver = new(mainWindow, graph);
            TextBlockObserver textBlockObserver = new(timeTextBlock);
            DataGridObserver dataGridObserver = new(orderDataGrid, workerDataGrid);
            //carpentry.Attach(lineGraphObserver);
            carpentry.Attach(textBlockObserver);
            carpentry.Attach(dataGridObserver);
        }
    }
}
