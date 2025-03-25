using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class AssemblyEndEvent : Event {
        public Order Order { get; set; }
        public Worker Worker { get; set; }

        public AssemblyEndEvent(EventSimulationCore simulationCore, double time, Order order, Worker worker) : base(simulationCore, time, 4) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueB.Count > 0) {
                SimulationCore.EventCalendar.AddEvent(new AssemblyStartEvent(SimulationCore, Time));
            }

            var movingTime = 0.0;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Assembled;

                if (Worker.CurrentOrder.Type == ProductType.Wardrobe) {
                    movingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

                    Worker.CurrentPlace = Place.PaintingAndMounting;

                    SimulationCore.Workshop.QueueC.Enqueue(Worker.CurrentOrder);
                    SimulationCore.EventCalendar.AddEvent(new MountingStartEvent(SimulationCore, Time + movingTime));
                } else {
                    SimulationCore.EventCalendar.AddEvent(new OrderEndEvent(SimulationCore, Time + movingTime));
                }
            }

            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
        }
    }
}