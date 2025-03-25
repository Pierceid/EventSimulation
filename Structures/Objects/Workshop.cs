using EventSimulation.Simulations;
using EventSimulation.Structures.Enums;
using EventSimulation.Structures.Events;

namespace EventSimulation.Structures.Objects {
    public class Workshop {
        private EventSimulationCore simulationCore;
        public Worker[] WorkersA { get; set; }
        public Worker[] WorkersB { get; set; }
        public Worker[] WorkersC { get; set; }
        public Queue<Order> QueueA { get; set; }
        public Queue<Order> QueueB { get; set; }
        public Queue<Order> QueueC { get; set; }

        public Workshop(EventSimulationCore core, int workersA, int workersB, int workersC) {
            simulationCore = core;

            WorkersA = new Worker[workersA];
            WorkersB = new Worker[workersB];
            WorkersC = new Worker[workersC];
            QueueA = new();
            QueueB = new();
            QueueC = new();

            InitComponents(workersA, workersB, workersC);
        }

        public void InitComponents(int workersA, int workersB, int workersC) {
            if (QueueA.Count != 0) QueueA.Clear();
            if (QueueB.Count != 0) QueueB.Clear();
            if (QueueC.Count != 0) QueueC.Clear();

            if (WorkersA.Length != workersA) WorkersA = new Worker[workersA];
            if (WorkersB.Length != workersB) WorkersB = new Worker[workersB];
            if (WorkersC.Length != workersC) WorkersC = new Worker[workersC];

            for (int i = 0; i < workersA; i++) WorkersA[i] = new Worker(i, WorkerGroup.A);
            for (int i = 0; i < workersB; i++) WorkersB[i] = new Worker(i + workersA, WorkerGroup.B);
            for (int i = 0; i < workersC; i++) WorkersC[i] = new Worker(i + workersA + workersB, WorkerGroup.C);
        }

        public void AddOrder(Order order) {
            QueueA.Enqueue(order);
        }

        public void ProcessOrders() {
            for (int i = 0; i < WorkersA.Length; i++) {
                if (!WorkersA[i].IsBusy && QueueA.Count > 0) {
                    simulationCore.EventCalendar.AddEvent(new CuttingStartEvent(simulationCore, simulationCore.SimulationTime));
                }
            }
            for (int i = 0; i < WorkersB.Length; i++) {
                if (!WorkersB[i].IsBusy && QueueB.Count > 0) {
                    simulationCore.EventCalendar.AddEvent(new AssemblyStartEvent(simulationCore, simulationCore.SimulationTime));
                }
            }
            for (int i = 0; i < WorkersC.Length; i++) {
                if (!WorkersC[i].IsBusy && QueueC.Count > 0) {
                    bool mountingAssigned = false;

                    for (int j = 0; j < QueueC.Count; j++) {
                        var order = QueueC.ElementAt(j);

                        if (order.Type == ProductType.Wardrobe && order.State == ProductState.Assembled) {
                            simulationCore.EventCalendar.AddEvent(new MountingStartEvent(simulationCore, simulationCore.SimulationTime));
                            mountingAssigned = true;
                            break;
                        }
                    }

                    if (!mountingAssigned) {
                        for (int j = 0; j < QueueC.Count; j++) {
                            var order = QueueC.ElementAt(j);

                            if (order.State == ProductState.Cut) {
                                simulationCore.EventCalendar.AddEvent(new PaintingStartEvent(simulationCore, simulationCore.SimulationTime));
                                break;
                            }
                        }
                    }
                }
            }
        }

        public Worker? GetAvailableWorker(WorkerGroup workerGroup) {
            switch (workerGroup) {
                case WorkerGroup.A:
                    for (int i = 0; i < WorkersA.Length; i++) {
                        if (!WorkersA[i].IsBusy) {
                            return WorkersA[i];
                        }
                    }
                    break;
                case WorkerGroup.B:
                    for (int i = 0; i < WorkersB.Length; i++) {
                        if (!WorkersB[i].IsBusy) {
                            return WorkersB[i];
                        }
                    }
                    break;
                case WorkerGroup.C:
                    for (int i = 0; i < WorkersC.Length; i++) {
                        if (!WorkersC[i].IsBusy) {
                            return WorkersC[i];
                        }
                    }
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
