using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingStartEvent : Event {
        public PaintingStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 6) {
        }

        public override void Execute() {
            Worker? worker = SimulationCore.Workshop.GetAvailableWorker(WorkerGroup.C);

            if (worker == null) return;

            if (!SimulationCore.Workshop.QueueC.TryDequeue(out var order)) return;

            worker.StartTask(order);

            double paintingTime = 0.0;

            if (worker.CurrentPlace == null) {
                paintingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
                worker.CurrentPlace = Place.WorkplaceC;
            }

            if (worker.CurrentPlace != Place.WorkplaceC) {
                paintingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
                worker.CurrentPlace = Place.WorkplaceC;
            }

            switch (order.Type) {
                case ProductType.Chair:
                    paintingTime += SimulationCore.Generators.ChairPaitingTime.Next();
                    break;
                case ProductType.Table:
                    paintingTime += SimulationCore.Generators.TablePaitingTime.Next();
                    break;
                case ProductType.Wardrobe:
                    paintingTime += SimulationCore.Generators.WardrobePaitingTime.Next();
                    break;
                default:
                    break;
            }

            Time += paintingTime;

            SimulationCore.EventCalendar.Enqueue(new PaintingEndEvent(SimulationCore, Time, worker), Time);
        }
    }
}