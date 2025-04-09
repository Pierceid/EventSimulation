﻿using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingStartEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public CuttingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.InCutting;

            double movingTime = 0.0;

            if (Worker.Workplace != null) {
                movingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            }

            movingTime += SimulationCore.Generators.MaterialPreparationTime.Next();
            movingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();

            Worker.Workplace = Order.Workplace;
            Worker.SetOrder(Order);
            manager.AverageUtilityA.AddSample(Time, true);

            double cuttingTime = Order.Type switch {
                ProductType.Chair => SimulationCore.Generators.ChairCuttingTime.Next(),
                ProductType.Table => SimulationCore.Generators.TableCuttingTime.Next(),
                ProductType.Wardrobe => SimulationCore.Generators.WardrobeCuttingTime.Next(),
                _ => 0.0
            };

            double nextEventTime = Time + movingTime + cuttingTime;

            SimulationCore.EventCalendar.Enqueue(new CuttingEndEvent(SimulationCore, nextEventTime, Order, Worker), nextEventTime);
        }
    }
}
