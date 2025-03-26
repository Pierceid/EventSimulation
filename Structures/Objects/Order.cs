using EventSimulation.Structures.Enums;
using EventSimulation.Utilities;

namespace EventSimulation.Structures.Objects {
    public class Order {
        public static int Id { get; set; }
        public ProductType Type { get; set; }
        public double ArrivalTime { get; set; } 
        public string FormattedTime { get; set; } 
        public ProductState State { get; set; }

        public Order(ProductType type, double time) {
            Id++;
            Type = type;
            ArrivalTime = time;
            FormattedTime = Utility.FormatTime(time);
            State = ProductState.Raw;
        }
    }
}
