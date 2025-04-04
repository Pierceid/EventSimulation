﻿using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class AssemblyEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public AssemblyEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Assembled;

            Worker.SetOrder(null);
            manager.AverageUtilityB.AddSample(Time, false);

            if (Order.Type == ProductType.Wardrobe) {
                manager.QueueD.AddLast(Order);

                List<Worker> availableWorkersC = manager.GetAvailableWorkers(ProductState.Assembled);

                if (availableWorkersC.Count > 0 && manager.QueueD.Count > 0) {
                    Worker nextWorker = availableWorkersC.First();
                    Order nextOrder = manager.QueueD.First();
                    manager.QueueD.RemoveFirst();

                    SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
                }
            } else {
                manager.Workplaces.FirstOrDefault(wp => wp.Order == Order)?.SetState(false);

                SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Order, Worker), Time);
            }

            List<Worker> availableWorkersB = manager.GetAvailableWorkers(ProductState.Painted);

            if (availableWorkersB.Count > 0 && manager.QueueB.Count > 0) {
                Worker nextWorker = availableWorkersB.First();
                Order nextOrder = manager.QueueB.First();
                manager.QueueB.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }
        }
    }
}
