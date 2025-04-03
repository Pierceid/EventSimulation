using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class OrderStartEvent : Event<ProductionManager> {
        private static int orderId = 0;

        public OrderStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time) : base(simulationCore, time, 2) { }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            var rng = SimulationCore.Generators.RNG.Next();
            ProductType orderType = rng < 0.5 ? ProductType.Table : rng < 0.65 ? ProductType.Chair : ProductType.Wardrobe;

            Order order = new(orderId++, orderType, Time);
            manager.Orders.Add(order);

            if (manager.Orders.Count > 500) {
                manager.Orders.RemoveAll(o => o.State == ProductState.Finished);
            }

            Workplace workPlace = manager.GetOrCreateWorkplace();
            order.Workplace = workPlace;
            workPlace.AssignOrder(order);

            if (manager.WorkersA.Any(w => !w.IsBusy)) {
                Worker nextWorker = manager.WorkersA.First(w => !w.IsBusy);
                Order nextOrder;

                if (manager.QueueA.Count != 0) {
                    nextOrder = manager.QueueA.First();
                    manager.QueueA.RemoveFirst();
                } else {
                    nextOrder = order;
                }

                if (nextOrder != order) {
                    manager.QueueA.AddLast(order);
                }

                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            } else {
                manager.QueueA.AddLast(order);
            }

            double nextArrivalTime = Time + SimulationCore.Generators.OrderArrivalTime.Next();

            SimulationCore.EventCalendar.Enqueue(new OrderStartEvent(SimulationCore, nextArrivalTime), nextArrivalTime);
        }
    }
}
