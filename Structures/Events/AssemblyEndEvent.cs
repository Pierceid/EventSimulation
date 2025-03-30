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
            if (SimulationCore.Data is not ProductionManager workshop) return;

            if (Workplace.Worker?.Order != null) {
                Workplace.Worker.Order.State = ProductState.Assembled;

                if (Workplace.Worker.Order.Type == ProductType.Wardrobe) {
                    workshop.QueueC.AddFirst(Workplace.Worker.Order);
                }
            }

            Workplace.FinishWork();

            Worker? worker = SimulationCore.Data.GetAvailableWorker(ProductState.Assembled);

            if (workshop.QueueC.Count > 0 && worker != null) {
                Workplace.Assign(workshop.QueueC.First!.Value, worker);
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (workshop.QueueB.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}