using EventSimulation.Simulations;
using System.Collections;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class ListBoxObserver : IObserver {
        private ListBox workerListBox;
        private ListBox orderListBox;
        private ListBox workstationListBox;

        public ListBoxObserver(ListBox workerListBox, ListBox orderListBox, ListBox workstationListBox) {
            this.workerListBox = workerListBox;
            this.orderListBox = orderListBox;
            this.workstationListBox = workstationListBox;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore esc) {
                workerListBox.Items.Clear();
                workerListBox.Items.Add(esc.Workshop.WorkersA);

                orderListBox.Items.Clear();
                orderListBox.Items.Add(esc.Workshop.QueueA);
            }
        }
    }
}
