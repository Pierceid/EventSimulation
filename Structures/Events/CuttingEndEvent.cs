using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingEndEvent : Event {
        public Worker Worker { get; set; }

        public CuttingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 2) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueA.Count > 0) {
                SimulationCore.EventCalendar.AddEvent(new CuttingStartEvent(SimulationCore, Time));
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.PaintingAndMounting;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Cut;
                SimulationCore.Workshop.QueueC.Enqueue(Worker.CurrentOrder);
            }

            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
            SimulationCore.EventCalendar.AddEvent(new AssemblyStartEvent(SimulationCore, Time + movingTime));
        }
    }
}
