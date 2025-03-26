using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class OrderStartEvent : Event {
        private static int orderId = 0;

        public OrderStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 2) {
        }

        public override void Execute() {
            double orderingTime = SimulationCore.Generators.OrderArrivalTime.Next();
            double rng = SimulationCore.Generators.RNG.Next();
            ProductType type = (rng < 0.15) ? ProductType.Chair : (rng < 0.65) ? ProductType.Table : ProductType.Wardrobe;

            SimulationCore.Workshop.QueueA.Enqueue(new Order(orderId++, type, Time));
            SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time), Time);

            Time += orderingTime;

            SimulationCore.EventCalendar.Enqueue(new OrderStartEvent(SimulationCore, Time), Time);
        }
    }
}
