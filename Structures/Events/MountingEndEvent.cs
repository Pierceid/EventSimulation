using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingEndEvent : Event {
        public Worker Worker { get; set; }

        public MountingEndEvent(EventSimulationCore simulationCore, double time, Worker worker) : base(simulationCore, time, 5) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time), Time);
            }

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Mounted;
            }

            Worker.FinishTask();

            SimulationCore.Workshop.ProcessOrders();
            SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time), Time);
        }
    }
}
