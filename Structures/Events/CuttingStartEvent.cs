using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingStartEvent : Event {

        public CuttingStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 3) {
        }

        public override void Execute() {
            Worker? worker = SimulationCore.Workshop.GetAvailableWorker(WorkerGroup.A);

            if (worker == null) return;

            if (!SimulationCore.Workshop.QueueA.TryDequeue(out var order)) return;

            worker.StartTask(order);

            double cuttingTime = 0.0;

            if (worker.CurrentPlace == null) {
                cuttingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
                worker.CurrentPlace = Place.WorkplaceA;
            }

            if (worker.CurrentPlace != Place.WorkplaceA) {
                cuttingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
                worker.CurrentPlace = Place.WorkplaceA;
            }

            if (worker.CurrentPlace == Place.WorkplaceA) {
                cuttingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
                cuttingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            }

            switch (order.Type) {
                case ProductType.Chair:
                    cuttingTime += SimulationCore.Generators.ChairCuttingTime.Next();
                    break;
                case ProductType.Table:
                    cuttingTime += SimulationCore.Generators.TableCuttingTime.Next();
                    break;
                case ProductType.Wardrobe:
                    cuttingTime += SimulationCore.Generators.WardrobeCuttingTime.Next();
                    break;
                default:
                    break;
            }

            Time += cuttingTime;

            SimulationCore.EventCalendar.Enqueue(new CuttingEndEvent(SimulationCore, Time, worker), Time);
        }
    }
}
