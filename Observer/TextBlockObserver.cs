using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using EventSimulation.Utilities;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class TextBlockObserver : IObserver {
        private TextBlock timeTextBlock;
        private TextBlock txtFinishedOrders;
        private TextBlock txtPendingOrders;
        private TextBlock txtQueueA;
        private TextBlock txtQueueB;
        private TextBlock txtQueueC;
        private TextBlock txtUtilityA;
        private TextBlock txtUtilityB;
        private TextBlock txtUtilityC;

        public TextBlockObserver(TextBlock txtTime, TextBlock txtFinishedOrders, TextBlock txtPendingOrders, TextBlock txtQueueA, TextBlock txtQueueB, TextBlock txtQueueC, TextBlock txtUtilityA, TextBlock txtUtilityB, TextBlock txtUtilityC) {
            this.timeTextBlock = txtTime;
            this.txtFinishedOrders = txtFinishedOrders;
            this.txtPendingOrders = txtPendingOrders;
            this.txtQueueA = txtQueueA;
            this.txtQueueB = txtQueueB;
            this.txtQueueC = txtQueueC;
            this.txtUtilityA = txtUtilityA;
            this.txtUtilityB = txtUtilityB;
            this.txtUtilityC = txtUtilityC;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                if (esc is Carpentry c) {
                    if (c.Speed != double.MaxValue) {
                        this.timeTextBlock.Text = Util.FormatTime(c.SimulationTime);

                        this.txtQueueA.Text = $"{c.Data.QueueA.Count:F0}";
                        this.txtQueueB.Text = $"{c.Data.QueueB.Count:F0}";
                        this.txtQueueC.Text = $"{c.Data.QueueC.Count:F0}";
                    }

                    this.txtFinishedOrders.Text = $"{c.AverageFinishedOrders.GetAverage():F2}";
                    this.txtPendingOrders.Text = $"{c.AveragePendingOrders.GetAverage():F2}";

                    this.txtUtilityA.Text = $"{(100 * c.AverageUtilityA.GetAverage()):F2}%";
                    this.txtUtilityB.Text = $"{(100 * c.AverageUtilityB.GetAverage()):F2}%";
                    this.txtUtilityC.Text = $"{(100 * c.AverageUtilityC.GetAverage()):F2}%";
                }
            }
        }
    }
}
