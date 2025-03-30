using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class CuttingStartEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public CuttingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 3) {
            Workplace = workplace;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (manager.QueueA.Count == 0) return;

            Order order = manager.QueueA.First!.Value;
            manager.QueueA.RemoveFirst();

            double cuttingTime = 0.0;

            if (Workplace.Worker?.Workplace == null) {
                cuttingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            } else if (Workplace.Worker?.Workplace != Workplace.Id) {
                cuttingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            }

            Workplace.StartWork();

            cuttingTime += order.Type switch {
                ProductType.Chair => SimulationCore.Generators.ChairCuttingTime.Next(),
                ProductType.Table => SimulationCore.Generators.TableCuttingTime.Next(),
                ProductType.Wardrobe => SimulationCore.Generators.WardrobeCuttingTime.Next(),
                _ => 0.0
            };

            Time += cuttingTime;

            SimulationCore.EventCalendar.Enqueue(new CuttingEndEvent(SimulationCore, Time, Workplace), Time);
        }
    }
}
