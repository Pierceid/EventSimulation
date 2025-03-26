using EventSimulation.Structures.Enums;
using EventSimulation.Utilities;
using System.ComponentModel;

namespace EventSimulation.Structures.Objects {
    public class Order : INotifyPropertyChanged {
        public int Id { get; set; }
        public ProductType Type { get; set; }
        public double ArrivalTime { get; set; } 
        public string FormattedTime { get; set; } 

        public Order(int id, ProductType type, double time) {
            Id = id;
            Type = type;
            ArrivalTime = time;
            FormattedTime = Utility.FormatTime(time);
            State = ProductState.Raw;
        }

        private ProductState state;
        public ProductState State {
            get => state;
            set {
                if (state != value) {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
