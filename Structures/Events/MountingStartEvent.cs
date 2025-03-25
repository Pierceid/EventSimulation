using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingStartEvent : Event {
        public MountingStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 3) {
        }

        public override void Execute() {
            Worker? worker = SimulationCore.Workshop.GetAvailableWorker(WorkerGroup.C);

            if (worker == null) return;

            Order order = SimulationCore.Workshop.QueueC.Dequeue();

            if (order.Type != ProductType.Wardrobe) return;

            worker.StartTask(order);

            double mountingTime = 0.0;

            if (worker.CurrentPlace == null) {
                mountingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
                worker.CurrentPlace = Place.PaintingAndMounting;
            }

            if (worker.CurrentPlace != Place.PaintingAndMounting) {
                mountingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
                worker.CurrentPlace = Place.PaintingAndMounting;
            }

            mountingTime += SimulationCore.Generators.WardrobeMountingTime.Next();

            SimulationCore.EventCalendar.AddEvent(new MountingEndEvent(SimulationCore, Time + mountingTime, worker));
        }
    }
}
