using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class AssemblyEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public AssemblyEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 4) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (Workplace.Order != null) {
                Workplace.Order.State = ProductState.Assembled;

                if (Workplace.Order.Type == ProductType.Wardrobe) {
                    manager.QueueC.AddFirst(Workplace.Order);
                } else {
                    SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Workplace), Time);
                }
            }

            Workplace.FinishWork();
            manager.AverageUtilityB.AddSample(Time, false);

            if (manager.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (manager.QueueB.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}