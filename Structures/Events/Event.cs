using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public abstract class Event<T>(EventSimulationCore<T> simulationCore, double time, int priority) where T : class, new() {
        public EventSimulationCore<T> SimulationCore { get; set; } = simulationCore;
        public double Time { get; set; } = time;
        public int Priority { get; set; } = priority;

        public abstract void Execute();
    }
}
