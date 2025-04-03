using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public MountingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time, 4) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Mounted;
            Order.Workplace = null;

            Worker.SetOrder(null);
            Worker.SetState(false);
            manager.AverageUtilityC.AddSample(Time, false);

            SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Order, Worker), Time);

            List<Worker> availableMontageWorkers = manager.GetAvailableWorkers(ProductState.Assembled);

            if (manager.QueueD.Count > 0 && availableMontageWorkers.Count > 0) {
                Worker nextWorker = availableMontageWorkers.First();
                Order nextOrder = manager.QueueD.First();
                manager.QueueD.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }
        }
    }
}
