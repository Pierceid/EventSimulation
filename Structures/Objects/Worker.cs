using EventSimulation.Structures.Enums;

namespace EventSimulation.Structures.Objects {
    public class Worker {
        public int Id { get; set; }
        public WorkerGroup Group { get; set; }
        public bool IsBusy { get; set; }
        public Order? CurrentOrder { get; set; }
        public Place? CurrentPlace { get; set; }

        public Worker(int id, WorkerGroup group) {
            Id = id;
            Group = group;
            IsBusy = false;
            CurrentOrder = null;
            CurrentPlace = Place.Storage;
        }

        public void StartTask(Order order) {
            IsBusy = true;
            CurrentOrder = order;
        }

        public void FinishTask() {
            IsBusy = false;
            CurrentOrder = null;
        }
    }
}
