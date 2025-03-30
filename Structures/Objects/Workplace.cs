namespace EventSimulation.Structures.Objects {
    public class Workplace {
        public int Id { get; }
        public bool IsOccupied { get; set; }
        public Order? Order { get; set; }
        public Worker? Worker { get; set; }

        public Workplace(int id) {
            Id = id;
            IsOccupied = false;
            Order = null;
            Worker = null;
        }

        public void Assign(Order order, Worker worker) {
            IsOccupied = true;
            Order = order;
            Worker = worker;
        }

        public void StartWork() {
            IsOccupied = true;
            if (Worker != null && Order != null) Worker.StartTask(Order, this);
        }

        public void FinishWork() {
            IsOccupied = false;
            if (Worker != null && Order != null) Worker.FinishTask();
        }
    }
}
