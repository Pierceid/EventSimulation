﻿using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;
using System.Windows;

namespace EventSimulation.Structures.Events {
    public class OrderStartEvent : Event<ProductionManager> {
        private static int orderId = 0;

        public OrderStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time) : base(simulationCore, time, 2) {
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            double orderingTime = SimulationCore.Generators.OrderArrivalTime.Next();
            double rng = SimulationCore.Generators.RNG.Next();
            ProductType type = (rng < 0.15) ? ProductType.Chair : (rng < 0.65) ? ProductType.Table : ProductType.Wardrobe;

            Order order = new(orderId++, type, Time);
            manager.QueueA.AddLast(order);
            Worker? worker = manager.GetAvailableWorker(ProductState.Raw);

            if (worker != null) {
                Workplace workplace = manager.GetOrCreateWorkplace();
                workplace.Assign(order, worker);
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, workplace), Time);
            }

            Time += orderingTime;

            SimulationCore.EventCalendar.Enqueue(new OrderStartEvent(SimulationCore, Time), Time);
        }
    }
}
