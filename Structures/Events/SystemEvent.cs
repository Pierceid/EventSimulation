using EventSimulation.Flyweight;
using EventSimulation.Simulations;

namespace EventSimulation.Structures.Events {
    public class SystemEvent(EventSimulationCore simulationCore, double time) : Event(simulationCore, time, 1) {        
        public override void Execute() {
            Thread.Sleep(1000 / SimulationCore.UpdateInterval);

            if (SimulationCore.IsSlowed) {
                var adjustedTime = Time + (SimulationCore.Speed / SimulationCore.UpdateInterval);

                if (SimulationCore.EventCalendar.PriorityQueue.Count > 0) {
                    var systemEvent = SystemEventFactory.GetInstance().GetEvent(SimulationCore, adjustedTime);
                    SimulationCore.EventCalendar.AddEvent(systemEvent);
                }

                SimulationCore.UpdateData();
            }
        }
    }
}
