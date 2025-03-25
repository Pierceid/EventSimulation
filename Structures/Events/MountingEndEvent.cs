using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingEndEvent : Event {
        public Worker Worker { get; set; }

        public MountingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 3) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.AddEvent(new MountingStartEvent(SimulationCore, Time));
            }

            var movingTime = SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

            Worker.CurrentPlace = Place.PaintingAndMounting;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Mounted;
            }

            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
            SimulationCore.EventCalendar.AddEvent(new OrderEndEvent(SimulationCore, Time + movingTime));
        }
    }
}
