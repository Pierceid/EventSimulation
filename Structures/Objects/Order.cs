using EventSimulation.Structures.Enums;

namespace EventSimulation.Structures.Objects {
    public class Order {
        public ProductType Type { get; set; }
        public double ArrivalTime { get; set; } 
        public ProductState State { get; set; }

        public Order(ProductType type, double time) {
            Type = type;
            ArrivalTime = time;
            State = ProductState.Raw;
        }
    }
}
