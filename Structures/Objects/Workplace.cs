﻿namespace EventSimulation.Structures.Objects {
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

        public void AssignOrder(Order? order) {
            Order = order;
            IsOccupied = order != null;
        }

        public void AssignWorker(Worker? worker) {
            Worker = worker;
        }

        public void SetState(bool isOccupied) {
            if (!isOccupied) Order = null;

            IsOccupied = isOccupied;
        }
    }
}
