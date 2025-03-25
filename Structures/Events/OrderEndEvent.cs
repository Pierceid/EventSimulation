using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public class OrderEndEvent : Event {

        public OrderEndEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 7) {
        }

        public override void Execute() {

        }
    }
}
