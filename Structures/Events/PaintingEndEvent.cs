using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingEndEvent : Event {
        public Worker Worker { get; set; }

        public PaintingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 5) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.AddEvent(new CuttingStartEvent(SimulationCore, Time));
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.Assembling;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Painted;
                SimulationCore.Workshop.QueueB.Enqueue(Worker.CurrentOrder);
            }

            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
            SimulationCore.EventCalendar.AddEvent(new AssemblyStartEvent(SimulationCore, Time + movingTime));
        }
    }
}
