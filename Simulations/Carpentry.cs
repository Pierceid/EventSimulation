using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Simulations {
    public class Carpentry : EventSimulationCore<ProductionManager> {
        public Carpentry(int replicationStock, double endOfSimulationTime) : base(replicationStock, endOfSimulationTime) {
            this.Data = new ProductionManager();
        }

        public override void BeforeSimulation() {
            base.BeforeSimulation();

            EventCalendar.Enqueue(new SystemEvent<ProductionManager>(this, SimulationTime), SimulationTime);
            EventCalendar.Enqueue(new OrderStartEvent(this, SimulationTime), SimulationTime);
        }

        public override void AfterSimulation() {
            if (CurrentReplication >= ReplicationStock * 0.01 && (ReplicationStock < 1000 || CurrentReplication % (ReplicationStock / 1000) == 0)) {
                Notify();
            }

            this.Data.Clear();

            base.AfterSimulation();
        }
    }
}
