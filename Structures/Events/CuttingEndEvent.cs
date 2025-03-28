using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingEndEvent : Event<Workshop> {
        public Worker Worker { get; set; }

        public CuttingEndEvent(EventSimulationCore<Workshop> simulationCore, double time, Worker worker) : base(simulationCore, time, 3) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not Workshop workshop) return;

            if (workshop.QueueA.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time), Time);
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.WorkplaceC;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Cut;
                workshop.QueueC.Enqueue(Worker.CurrentOrder);
            }

            Time += movingTime;

            Worker.FinishTask();

            workshop.ProcessOrders();

            if (workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time), Time);
            }
        }
    }
}
