using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingStartEvent : Event<Workshop> {
        public MountingStartEvent(EventSimulationCore<Workshop> simulationCore, double time) : base(simulationCore, time, 5) {
        }

        public override void Execute() {
            Worker? worker = SimulationCore.Data.GetAvailableWorker(WorkerGroup.C);

            if (worker == null) return;

            if (!SimulationCore.Data.QueueC.TryDequeue(out var order)) return;

            if (order.Type != ProductType.Wardrobe) return;

            worker.StartTask(order);

            double mountingTime = 0.0;

            if (worker.CurrentPlace == null) {
                mountingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
                worker.CurrentPlace = Place.WorkplaceC;
            }

            if (worker.CurrentPlace != Place.WorkplaceC) {
                mountingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
                worker.CurrentPlace = Place.WorkplaceC;
            }

            mountingTime += SimulationCore.Generators.WardrobeMountingTime.Next();

            Time += mountingTime;

            SimulationCore.EventCalendar.Enqueue(new MountingEndEvent(SimulationCore, Time, worker), Time);
        }
    }
}
