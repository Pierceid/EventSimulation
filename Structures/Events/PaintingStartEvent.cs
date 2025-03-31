using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class PaintingStartEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public PaintingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 6) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (manager.QueueC.Count == 0) return;

            Order order = manager.QueueC.First!.Value;
            manager.QueueC.RemoveFirst();
            Worker? worker = manager.GetAvailableWorker(ProductState.Cut);

            if (Workplace.Order == null || worker == null) return;

            Workplace.Assign(Workplace.Order, worker);

            if (order.Type == ProductType.Wardrobe && order.State == ProductState.Assembled) {
                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, Workplace), Time);
                return;
            }

            double paintingTime = 0.0;

            if (Workplace.Worker?.Workplace == -1) {
                paintingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            } else if (Workplace.Worker?.Workplace != Workplace.Id) {
                paintingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            }

            Workplace.StartWork();

            paintingTime += order.Type switch {
                ProductType.Chair => SimulationCore.Generators.ChairPaintingTime.Next(),
                ProductType.Table => SimulationCore.Generators.TablePaintingTime.Next(),
                ProductType.Wardrobe => SimulationCore.Generators.WardrobePaintingTime.Next(),
                _ => 0.0
            };

            Time += paintingTime;

            SimulationCore.EventCalendar.Enqueue(new PaintingEndEvent(SimulationCore, Time, Workplace), Time);
        }
    }
}
