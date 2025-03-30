using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;
using EventSimulation.Utilities;
using System.Windows.Controls;

namespace EventSimulation.Observer {
    public class TextBlockObserver : IObserver {
        private TextBlock timeTextBlock;

        public TextBlockObserver(TextBlock timeTextBlock) {
            this.timeTextBlock = timeTextBlock;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                this.timeTextBlock.Text = Utility.FormatTime(esc.SimulationTime);
            }
        }
    }
}
