using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class OrderEndEvent : Event<Workshop> {

        public OrderEndEvent(EventSimulationCore<Workshop> simulationCore, double time) : base(simulationCore, time, 7) {
        }

        public override void Execute() {

        }
    }
}
