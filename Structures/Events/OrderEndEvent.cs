using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class OrderEndEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public OrderEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 7) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;
            
            if (Workplace.Order != null) {
                Workplace.Order.EndTime = Time;
                manager.AverageOrderTime.AddSample(Workplace.Order.EndTime - Workplace.Order.StartTime);
                manager.AverageFinishedOrders.AddSample(1);
            }
        }
    }
}
