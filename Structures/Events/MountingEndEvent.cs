using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public MountingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 5) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (Workplace.Order != null) {
                Workplace.Order.State = ProductState.Mounted;
            }

            Workplace.FinishWork();

            SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Workplace), Time);
        }
    }
}
