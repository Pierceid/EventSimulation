using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class AssemblyEndEvent : Event<Workshop> {
        public Order Order { get; set; }
        public Worker Worker { get; set; }

        public AssemblyEndEvent(EventSimulationCore<Workshop> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time, 4) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not Workshop workshop) return;

            if (workshop.QueueB.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time), Time);
            }

            var movingTime = 0.0;

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Assembled;

                if (Worker.CurrentOrder.Type == ProductType.Wardrobe) {
                    movingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();

                    Time += movingTime;

                    Worker.CurrentPlace = Place.WorkplaceC;

                    workshop.QueueC.Enqueue(Worker.CurrentOrder);
                    SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time), Time);
                } else {
                    SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time), Time);
                }
            }

            Worker.FinishTask();

            workshop.ProcessOrders();
        }
    }
}