using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;

public class AssemblyStartEvent : Event<ProductionManager> {
    public Workplace Workplace { get; }

    public AssemblyStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 4) {
        Workplace = workplace;
    }

    public override void Execute() {
        if (SimulationCore.Data is not ProductionManager manager) return;

        if (manager.QueueB.Count == 0) return;

        Order order = manager.QueueB.First!.Value;
        manager.QueueB.RemoveFirst();
        Worker? worker = manager.GetAvailableWorker(ProductState.Painted);

        if (Workplace.Order == null || worker == null) return;

        Workplace.Assign(Workplace.Order, worker);

        double assemblyTime = 0.0;

        if (Workplace.Worker?.Workplace == -1) {
            assemblyTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
        } else if (Workplace.Worker?.Workplace != Workplace.Id) {
            assemblyTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
        }

        Workplace.StartWork();

        assemblyTime += order.Type switch {
            ProductType.Chair => SimulationCore.Generators.ChairAssemblyTime.Next(),
            ProductType.Table => SimulationCore.Generators.TableAssemblyTime.Next(),
            ProductType.Wardrobe => SimulationCore.Generators.WardrobeAssemblyTime.Next(),
            _ => 0.0
        };

        Time += assemblyTime;

        SimulationCore.EventCalendar.Enqueue(new AssemblyEndEvent(SimulationCore, Time, Workplace), Time);
    }
}