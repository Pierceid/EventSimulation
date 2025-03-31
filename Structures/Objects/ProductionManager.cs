using EventSimulation.Statistics;
using EventSimulation.Structures.Enums;

namespace EventSimulation.Structures.Objects {
    public class ProductionManager {
        public LinkedList<Order> QueueA { get; } = new();
        public LinkedList<Order> QueueB { get; } = new();
        public LinkedList<Order> QueueC { get; } = new();
        public List<Worker> WorkersA { get; } = new();
        public List<Worker> WorkersB { get; } = new();
        public List<Worker> WorkersC { get; } = new();
        public List<Workplace> Workplaces { get; } = new();
        public Average AverageOrderTime { get; set; } = new();
        public Counter AverageFinishedOrders { get; set; } = new();
        public Counter AveragePendingOrders { get; set; } = new();
        public Utility AverageUtilityA { get; set; } = new();
        public Utility AverageUtilityB { get; set; } = new();
        public Utility AverageUtilityC { get; set; } = new();

        private static int nextWorkplaceId = 0;

        public ProductionManager() { }

        public ProductionManager(int workersA, int workersB, int workersC) {
            InitComponents(workersA, workersB, workersC);
        }

        public void InitComponents(int workersA, int workersB, int workersC) {
            Parallel.For(0, workersA, a => { lock (WorkersA) { WorkersA.Add(new Worker(a, WorkerGroup.A)); } });
            Parallel.For(0, workersB, b => { lock (WorkersB) { WorkersB.Add(new Worker(b + workersA, WorkerGroup.B)); } });
            Parallel.For(0, workersC, c => { lock (WorkersC) { WorkersC.Add(new Worker(c + workersA + workersB, WorkerGroup.C)); } });
        }

        public void Clear() {
            int workersA = WorkersA.Count;
            int workersB = WorkersB.Count;
            int workersC = WorkersC.Count;

            WorkersA.Clear();
            WorkersB.Clear();
            WorkersC.Clear();
            Workplaces.Clear();

            InitComponents(workersA, workersB, workersC);

            AverageOrderTime.Clear();
            AverageFinishedOrders.Clear();
            AveragePendingOrders.Clear();
            AverageUtilityA.Clear();
            AverageUtilityB.Clear();
            AverageUtilityC.Clear();
        }

        public void AssignOrderToWorker(Order order) {
            LinkedList<Order>? targetQueue = order.State switch {
                ProductState.Raw => QueueA,
                ProductState.Painted => QueueB,
                ProductState.Cut => QueueC,
                ProductState.Assembled => QueueC,
                _ => null
            };

            if (targetQueue == null || targetQueue.Count == 0) return;

            while (targetQueue.Count > 0) {
                Order currentOrder = targetQueue.First!.Value;
                Worker? availableWorker = GetAvailableWorker(currentOrder.State);

                if (availableWorker != null) {
                    Workplace workplace = GetOrCreateWorkplace();
                    workplace.Assign(currentOrder, availableWorker);
                    workplace.StartWork();
                    targetQueue.RemoveFirst();
                } else {
                    break;
                }
            }
        }

        public Worker? GetAvailableWorker(ProductState state) {
            return state switch {
                ProductState.Raw => WorkersA.FirstOrDefault(w => !w.IsBusy),
                ProductState.Painted => WorkersB.FirstOrDefault(w => !w.IsBusy),
                ProductState.Cut => WorkersC.FirstOrDefault(w => !w.IsBusy),
                ProductState.Assembled => WorkersC.FirstOrDefault(w => !w.IsBusy),
                _ => null
            };
        }

        public Workplace GetOrCreateWorkplace() {
            Workplace? workplace = Workplaces.FirstOrDefault(w => !w.IsOccupied);

            if (workplace == null) {
                workplace = new Workplace(nextWorkplaceId++);
                Workplaces.Add(workplace);
            }

            return workplace;
        }
    }
}
