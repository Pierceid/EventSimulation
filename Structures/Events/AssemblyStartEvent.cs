using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;

public class AssemblyStartEvent : Event {
    public AssemblyStartEvent(EventSimulationCore simulationCore, double time) : base(simulationCore, time, 4) {
    }

    public override void Execute() {
        Worker? worker = SimulationCore.Workshop.GetAvailableWorker(WorkerGroup.B);

        if (worker == null) return;

        Order order = SimulationCore.Workshop.QueueB.Dequeue();

        worker.StartTask(order);

        double assemblyTime = 0.0;

        if (worker.CurrentPlace == null) {
            assemblyTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            worker.CurrentPlace = Place.Assembling;
        }

        if (worker.CurrentPlace != Place.Assembling) {
            assemblyTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            worker.CurrentPlace = Place.Assembling;
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