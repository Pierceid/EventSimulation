using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

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
                Orders.Clear();

                foreach (var order in esc.Workshop.QueueA) Orders.Add(order);
                foreach (var order in esc.Workshop.QueueB) Orders.Add(order);
                foreach (var order in esc.Workshop.QueueC) Orders.Add(order);

                Workers.Clear();

                foreach (var worker in esc.Workshop.WorkersA) Workers.Add(worker);
                foreach (var worker in esc.Workshop.WorkersB) Workers.Add(worker);
                foreach (var worker in esc.Workshop.WorkersC) Workers.Add(worker);
            }
        }
    }
}
