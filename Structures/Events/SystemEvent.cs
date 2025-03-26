using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public class SystemEvent(EventSimulationCore simulationCore, double time) : Event(simulationCore, time, 1) {
        public override void Execute() {
            Thread.Sleep((int)(1000 / SimulationCore.Speed));

            Time = SimulationCore.SimulationTime + 1;
            SimulationCore.EventCalendar.Enqueue(this, Time);

            SimulationCore.Notify();
        }
    }
}
