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
            if (SimulationCore.Data is not ProductionManager workshop) return;

            if (Workplace.Worker?.Order != null) {
                Workplace.Worker.Order.State = ProductState.Painted;

                workshop.QueueB.AddLast(Workplace.Worker.Order);
            }

            Workplace.FinishWork();

            Worker? worker = workshop.GetAvailableWorker(ProductState.Painted);

            if (workshop.QueueB.Count > 0 && worker != null) {
                Workplace.Assign(workshop.QueueB.First!.Value, worker);
                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}
