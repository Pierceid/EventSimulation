using EventSimulation.Strategies;

namespace EventSimulation.Simulations {
    public class Warehouse(int replicationStock) : SimulationCore(replicationStock) {
        public Strategy? Strategy { get; set; } = null;
        public Action<int, double>? Callback { get; set; } = null;

        public override void AfterSimulation() {
            if (Strategy == null || CurrentReplication < ReplicationStock * 0.01) return;

            if (ReplicationStock < 1000 || CurrentReplication % (ReplicationStock / 1000) == 0) {
                double averageCost = Math.Round(Strategy.OverallCost / (CurrentReplication + 1));
                Callback?.Invoke(CurrentReplication, averageCost);
            }
        }

        public override void AfterSimulationRun() {

        }

        public override void BeforeSimulation() {

        }

        public override void BeforeSimulationRun() {

        }

        public override void Experiment() {
            Strategy?.RunStrategy();
        }
    }
}