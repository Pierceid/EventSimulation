﻿using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class OrderStartEvent : Event {

        public OrderStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 2) {
        }

        public override void Execute() {
            double orderingTime = SimulationCore.Generators.OrderArrivalTime.Next();
            double rng = SimulationCore.Generators.RNG.Next();
            ProductType type = (rng < 0.15) ? ProductType.Chair : (rng < 0.65) ? ProductType.Table : ProductType.Wardrobe;

            SimulationCore.Workshop.AddOrder(new Order(type, Time));

            if (SimulationCore.Workshop.QueueA.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time), Time);
            }

            SimulationCore.EventCalendar.Enqueue(new OrderStartEvent(SimulationCore, Time + orderingTime), Time);
        }
    }
}
