using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingEndEvent : Event {
        public Worker Worker { get; set; }

        public PaintingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 6) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time), Time);
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.Assembling;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Painted;
                SimulationCore.Workshop.QueueB.Enqueue(Worker.CurrentOrder);
            }

            Time += movingTime;
            
            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
            SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time), Time);
        }
    }
}
