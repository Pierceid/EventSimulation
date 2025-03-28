using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingEndEvent : Event<Workshop> {
        public Worker Worker { get; set; }

        public MountingEndEvent(EventSimulationCore<Workshop> simulationCore, double time, Worker worker) : base(simulationCore, time, 5) {
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not Workshop workshop) return;

            if (workshop.QueueC.Count > 0) {
                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time), Time);
            }

            if (Worker.CurrentOrder != null) {
                Worker.CurrentOrder.State = ProductState.Mounted;
            }

            Worker.FinishTask();

            workshop.ProcessOrders();

            SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time), Time);
        }
    }
}
