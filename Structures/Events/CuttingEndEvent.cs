using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;
using System.Windows;

namespace EventSimulation.Structures.Events {
    public class CuttingEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public CuttingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 3) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager workshop) return;

            if (Workplace.Worker?.Order != null) {
                Workplace.Worker.Order.State = ProductState.Cut;
                workshop.QueueC.AddLast(Workplace.Worker.Order);
            }

            Workplace.FinishWork();

            Worker? worker = workshop.GetAvailableWorker(ProductState.Cut);

            if (workshop.QueueC.Count > 0 && worker != null) {
                Workplace.Assign(workshop.QueueC.First!.Value, worker);
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, Workplace), Time);
            }

            if (workshop.QueueA.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, Workplace), Time);
            }
        }
    }
}
