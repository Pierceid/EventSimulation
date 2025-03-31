using EventSimulation.Observer;
using EventSimulation.Simulations;
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
            carpentry.Resume();
            carpentry.Data.Clear();
            graph.RefreshGraph();

            simulationThread = new(carpentry.RunSimulation) { IsBackground = true };
            simulationThread.Start();
        }

        public bool PauseSimulation() {
            if (carpentry == null) return false;

            carpentry.IsPaused = !carpentry.IsPaused;

            return carpentry.IsPaused;
        }

        public void StopSimulation() {
            if (carpentry == null || graph == null || !isRunning) return;

            isRunning = false;
            carpentry.Pause();
            carpentry.Stop();

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
            carpentry.Data.InitComponents(workersA, workersB, workersC);
        }

        public void UpdateCarpentry(double speed) {
            if (carpentry == null) return;

            carpentry.Speed = speed;
        }

        public void InitObservers(TextBlock timeTextBlock, TextBlock txtFinishedOrders, TextBlock txtNotStartedOrders, DataGrid orderDataGrid, DataGrid workerDataGrid) {
            if (carpentry == null || mainWindow == null || graph == null) return;

            LineGraphObserver lineGraphObserver = new(mainWindow, graph);
            TextBlockObserver textBlockObserver = new(timeTextBlock, txtFinishedOrders, txtNotStartedOrders);
            DataGridObserver dataGridObserver = new(orderDataGrid, workerDataGrid);

            carpentry.Attach(lineGraphObserver);
            carpentry.Attach(textBlockObserver);
            carpentry.Attach(dataGridObserver);
        }
    }
}
