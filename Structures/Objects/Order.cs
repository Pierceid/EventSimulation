using EventSimulation.Structures.Enums;

namespace EventSimulation.Structures.Objects {
    public class Order {
        public static int Id { get; set; }
        public ProductType Type { get; set; }
        public double ArrivalTime { get; set; } 
        public ProductState State { get; set; }

        public Order(ProductType type, double time) {
            Id++;
            Type = type;
            ArrivalTime = time;
            State = ProductState.Raw;
        }
    }
}
