using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public PaintingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 6) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (Workplace.Order != null) {
                Workplace.Order.State = ProductState.Painted;
                manager.QueueB.AddLast(Workplace.Order);
            }

            Workplace.FinishWork();

            if (manager.QueueB.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (manager.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}
