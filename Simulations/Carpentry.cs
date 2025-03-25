namespace EventSimulation.Simulations {
    public class Carpentry(int replicationStock, double endOfSimulationTime) : EventSimulationCore(replicationStock, endOfSimulationTime) {
        public override void AfterSimulation() {
            Notify();
        }

        public override void AfterSimulationRun() {

        }

        public override void BeforeSimulation() {

        }

        public override void BeforeSimulationRun() {

        }

        public override void Experiment() {

        }
    }
}
