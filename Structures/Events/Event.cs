using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public abstract class Event(EventSimulationCore simulationCore, double time, int priority) {
        public EventSimulationCore SimulationCore { get; set; } = simulationCore;
        public double Time { get; set; } = time;
        public int Priority { get; set; } = priority;

        public abstract void Execute();
    }
}
