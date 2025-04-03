using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public PaintingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time, 4) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Painted;
            manager.QueueB.AddLast(Order);

            Worker.SetOrder(null);
            manager.AverageUtilityC.AddSample(Time, false);

            List<Worker> availableAssemblyWorkers = manager.GetAvailableWorkers(ProductState.Painted);
            List<Worker> availableMontageWorkers = manager.GetAvailableWorkers(ProductState.Assembled);
            List<Worker> availablePaintingWorkers = manager.GetAvailableWorkers(ProductState.Cut);

            if (availableAssemblyWorkers.Count > 0 && manager.QueueB.Count > 0) {
                Worker nextWorker = availableAssemblyWorkers.First();
                Order nextOrder = manager.QueueB.First();
                manager.QueueB.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }

            if (manager.QueueD.Count > 0 && availableMontageWorkers.Count > 0) {
                Worker nextWorker = availableMontageWorkers.First();
                Order nextOrder = manager.QueueD.First();
                manager.QueueD.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            } else if (manager.QueueC.Count > 0 && availablePaintingWorkers.Count > 0) {
                Worker nextWorker = availablePaintingWorkers.First();
                Order nextOrder = manager.QueueC.First();
                manager.QueueC.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }
        }
    }
}
