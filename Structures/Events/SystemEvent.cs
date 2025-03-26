using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public class SystemEvent(EventSimulationCore simulationCore, double time) : Event(simulationCore, time, 1) {
        public override void Execute() {
            Thread.Sleep((int)SimulationCore.UpdateTime);

            Time = SimulationCore.SimulationTime + (SimulationCore.Speed / SimulationCore.UpdateTime);
            SimulationCore.EventCalendar.Enqueue(this, Time);

            SimulationCore.Notify();
        }
    }
}
