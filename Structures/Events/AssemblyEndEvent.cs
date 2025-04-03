using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class AssemblyEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public AssemblyEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time, 3) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Assembled;

            Worker.SetOrder(null);
            Worker.SetState(false);
            manager.AverageUtilityB.AddSample(Time, false);

            if (Order.Type == ProductType.Wardrobe) {
                manager.QueueD.AddLast(Order);

                List<Worker> availableMontageWorkers = manager.GetAvailableWorkers(ProductState.Assembled);

                if (availableMontageWorkers.Count > 0 && manager.QueueD.Count > 0) {
                    Worker nextWorker = availableMontageWorkers.First();
                    Order nextOrder = manager.QueueD.First();
                    manager.QueueD.RemoveFirst();

                    SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
                }
            } else {
                Order.Workplace = null;

                SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Order, Worker), Time);
                //manager.AverageProductionTime.AddSample(Order.TimeOfWork);
            }

            List<Worker> availableAssemblyWorkers = manager.GetAvailableWorkers(ProductState.Painted);

            if (availableAssemblyWorkers.Count > 0 && manager.QueueB.Count > 0) {
                Worker nextAssemblyWorker = availableAssemblyWorkers.First();
                Order nextAssemblyOrder = manager.QueueB.First();
                manager.QueueB.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, nextAssemblyOrder, nextAssemblyWorker), Time);
            }
        }
    }
}
