using EventSimulation.Structures.Events;

namespace EventSimulation.Simulations {
    public class Carpentry(int replicationStock, double endOfSimulationTime) : EventSimulationCore(replicationStock, endOfSimulationTime) {
        public override void AfterSimulation() {

        }

        public override void AfterSimulationRun() {

        }

        public override void BeforeSimulation() {

        }

        public override void BeforeSimulationRun() {
            this.EventCalendar.Enqueue(new OrderStartEvent(this, 0), 0);
        }
    }
}
