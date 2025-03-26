using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingEndEvent : Event {
        public Worker Worker { get; set; }

        public CuttingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 3) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueA.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time), Time);
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.WorkplaceC;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Cut;
                SimulationCore.Workshop.QueueC.Enqueue(Worker.CurrentOrder);
            }

            Time += movingTime;

            Worker.FinishTask();
            SimulationCore.Workshop.ProcessOrders();

            if (SimulationCore.Workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time), Time);
            }
        }
    }
}
