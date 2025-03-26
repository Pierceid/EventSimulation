using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class DataGridObserver : IObserver {
        private DataGrid orderDataGrid;
        private DataGrid workerDataGrid;

        private ObservableCollection<Worker> Workers { get; set; } = new();
        private ObservableCollection<Order> Orders { get; set; } = new();

        public DataGridObserver(DataGrid orderDataGrid, DataGrid workerDataGrid) {
            this.orderDataGrid = orderDataGrid;
            this.workerDataGrid = workerDataGrid;

            this.orderDataGrid.ItemsSource = Orders;
            this.workerDataGrid.ItemsSource = Workers;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore esc) {
                AddNewOrders(esc.Workshop.QueueA);
                AddNewOrders(esc.Workshop.QueueB);
                AddNewOrders(esc.Workshop.QueueC);
                RemoveOldOrders(esc.Workshop.QueueA, esc.Workshop.QueueB, esc.Workshop.QueueC);

                AddNewWorkers(esc.Workshop.WorkersA);
                AddNewWorkers(esc.Workshop.WorkersB);
                AddNewWorkers(esc.Workshop.WorkersC);
                RemoveOldWorkers(esc.Workshop.WorkersA, esc.Workshop.WorkersB, esc.Workshop.WorkersC);
            }
        }

        private void AddNewOrders(IEnumerable<Order> newOrders) {
            foreach (var order in newOrders) {
                if (!Orders.Contains(order)) {
                    Orders.Add(order);
                }
            }
        }

        private void RemoveOldOrders(IEnumerable<Order> queueA, IEnumerable<Order> queueB, IEnumerable<Order> queueC) {
            var allQueues = queueA.Concat(queueB).Concat(queueC).ToList();
            foreach (var order in Orders.ToList()) {
                if (!allQueues.Contains(order)) {
                    Orders.Remove(order);
                }
            }
        }

        private void AddNewWorkers(IEnumerable<Worker> newWorkers) {
            foreach (var worker in newWorkers) {
                if (!Workers.Contains(worker)) {
                    Workers.Add(worker);
                }
            }
        }

        private void RemoveOldWorkers(IEnumerable<Worker> workersA, IEnumerable<Worker> workersB, IEnumerable<Worker> workersC) {
            var allWorkers = workersA.Concat(workersB).Concat(workersC).ToList();
            foreach (var worker in Workers.ToList()) {
                if (!allWorkers.Contains(worker)) {
                    Workers.Remove(worker);
                }
            }
        }
    }
}
