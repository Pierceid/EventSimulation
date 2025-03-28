using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;

public class AssemblyStartEvent : Event<Workshop> {
    public AssemblyStartEvent(EventSimulationCore<Workshop> simulationCore, double time) : base(simulationCore, time, 4) {
    }

    public override void Execute() {
        Worker? worker = SimulationCore.Data.GetAvailableWorker(WorkerGroup.B);

        if (worker == null) return;

        if (!SimulationCore.Data.QueueB.TryDequeue(out var order)) return;

        worker.StartTask(order);

        double assemblyTime = 0.0;

        if (worker.CurrentPlace == null) {
            assemblyTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            worker.CurrentPlace = Place.WorkplaceB;
        }

        if (worker.CurrentPlace != Place.WorkplaceB) {
            assemblyTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            worker.CurrentPlace = Place.WorkplaceB;
        }

        switch (order.Type) {
            case ProductType.Chair:
                assemblyTime += SimulationCore.Generators.ChairAssemblyTime.Next();
                break;
            case ProductType.Table:
                assemblyTime += SimulationCore.Generators.TableAssemblyTime.Next();
                break;
            case ProductType.Wardrobe:
                assemblyTime += SimulationCore.Generators.WardrobeAssemblyTime.Next();
                break;
            default:
                break;
        }

        Time += assemblyTime;

        SimulationCore.EventCalendar.Enqueue(new AssemblyEndEvent(SimulationCore, Time, order, worker), Time);
    }
}