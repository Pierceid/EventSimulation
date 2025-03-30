using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public CuttingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 3) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (Workplace.Worker?.Order != null) {
                Workplace.Worker.Order.State = ProductState.Cut;
                manager.QueueC.AddLast(Workplace.Worker.Order);
            }

            Workplace.FinishWork();

            Worker? worker = manager.GetAvailableWorker(ProductState.Cut);

            if (manager.QueueC.Count > 0 && worker != null) {
                Workplace.Assign(manager.QueueC.First!.Value, worker);
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (manager.QueueA.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}
