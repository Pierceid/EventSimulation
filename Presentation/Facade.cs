using EventSimulation.Simulations;
using EventSimulation.Strategies;
using EventSimulation.Windows;
using OxyPlot.Wpf;
using System.Windows;
using System.Windows.Threading;

namespace EventSimulation.Presentation {
    public class Facade {
        private Window? mainWindow;
        private Warehouse? warehouse;
        private LineGraph? graph;
        private Thread? simulationThread;
        private bool isRunning;

        public Facade(Window? window) {
            mainWindow = window;
            warehouse = null;
            graph = null;
            simulationThread = null;
            isRunning = false;
        }

        public void StartSimulation() {
            if (warehouse == null || warehouse.Strategy == null || graph == null || isRunning) return;

            isRunning = true;
            graph.RefreshGraph();

            simulationThread = new(warehouse.RunSimulation) { IsBackground = true };
            simulationThread.Start();
        }

        public void StopSimulation() {
            if (warehouse == null || !isRunning) return;

            warehouse.Stop();
            isRunning = false;

            simulationThread?.Join();
            simulationThread = null;
        }

        public void AnalyzeReplication() {
            if (warehouse == null || warehouse.Strategy == null) return;

            BarChartWindow barChartWindow = new(mainWindow, $"Costs Analysis ({warehouse.CurrentReplication})", warehouse.Strategy.DailyCosts);
            barChartWindow.Show();
        }

        public void SetStrategy(Strategy strategy) {
            if (warehouse == null || graph == null) return;

            if (isRunning) {
                StopSimulation();
            }

            warehouse.Strategy = strategy;
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
        }

        public void InitWarehouse(int replications) {
            if (isRunning) {
                StopSimulation();
            }

            warehouse = new Warehouse(replications) { Callback = OnReplicationCompleted };
        }

        private void OnReplicationCompleted(int replication, double cost) {
            if (graph == null || mainWindow == null || !isRunning) return;

            mainWindow.Dispatcher.Invoke(() => {
                if (isRunning) {
                    graph.UpdatePlot(replication, cost);
                }
            }, DispatcherPriority.Input);
        }
    }
}
