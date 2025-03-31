using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using EventSimulation.Utilities;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class TextBlockObserver : IObserver {
        private TextBlock timeTextBlock;
        private TextBlock txtFinishedOrders;
        private TextBlock txtNotStartedOrders;

        public TextBlockObserver(TextBlock timeTextBlock, TextBlock txtFinishedOrders, TextBlock txtNotStartedOrders) {
            this.timeTextBlock = timeTextBlock;
            this.txtFinishedOrders = txtFinishedOrders;
            this.txtNotStartedOrders = txtNotStartedOrders;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                if (esc is Carpentry c) {
                    if (c.Speed != double.MaxValue) {
                        this.timeTextBlock.Text = Utility.FormatTime(c.SimulationTime);
                    }
                    this.txtFinishedOrders.Text = $"{(c.AverageFinishedOrdersCount.Count / (c.CurrentReplication + 1.0)):F0}";
                    this.txtNotStartedOrders.Text = $"{(c.AverageNotStartedOrdersCount.Count / (c.CurrentReplication + 1.0)):F2}";
                }
            }
        }
    }
}
