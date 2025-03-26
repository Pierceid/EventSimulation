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
            SimulationTime = 0.0;
            EventCalendar.Enqueue(new SystemEvent(this, SimulationTime), SimulationTime);
            EventCalendar.Enqueue(new OrderStartEvent(this, SimulationTime), SimulationTime);
        }
    }
}
