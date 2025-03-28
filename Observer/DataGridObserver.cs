using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class DataGridObserver : IObserver {
        private readonly DataGrid orderDataGrid;
        private readonly DataGrid workerDataGrid;

        private ObservableCollection<Worker> Workers { get; } = new();
        private ObservableCollection<Order> Orders { get; } = new();

        public DataGridObserver(DataGrid orderDataGrid, DataGrid workerDataGrid) {
            this.orderDataGrid = orderDataGrid;
            this.workerDataGrid = workerDataGrid;

            this.orderDataGrid.ItemsSource = Orders;
            this.workerDataGrid.ItemsSource = Workers;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<Workshop> esc) {
                SyncCollection(Orders, esc.Data.QueueA.Concat(esc.Data.QueueB).Concat(esc.Data.QueueC));
                SyncCollection(Workers, esc.Data.WorkersA.Concat(esc.Data.WorkersB).Concat(esc.Data.WorkersC));
            }
        }

        private void SyncCollection<T>(ObservableCollection<T> collection, IEnumerable<T> newItems) where T : class {
            var newItemsSet = newItems.ToHashSet();

            for (int i = collection.Count - 1; i >= 0; i--) {
                if (!newItemsSet.Contains(collection[i])) {
                    collection.RemoveAt(i);
                }
            }

            foreach (var item in newItems) {
                if (!collection.Contains(item)) {
                    collection.Add(item);
                }
            }
        }
    }
}
