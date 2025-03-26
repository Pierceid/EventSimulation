using EventSimulation.Structures.Enums;
using System.ComponentModel;

namespace EventSimulation.Structures.Objects {
    public class Worker : INotifyPropertyChanged {
        public int Id { get; set; }
        public WorkerGroup Group { get; set; }
        private bool isBusy;
        public bool IsBusy {
            get => isBusy;
            set {
                if (isBusy != value) {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        private Order? currentOrder;
        public Order? CurrentOrder {
            get => currentOrder;
            set {
                if (currentOrder != value) {
                    currentOrder = value;
                    OnPropertyChanged(nameof(CurrentOrder));
                }
            }
        }

        private Place? currentPlace;
        public Place? CurrentPlace {
            get => currentPlace;
            set {
                if (currentPlace != value) {
                    currentPlace = value;
                    OnPropertyChanged(nameof(CurrentPlace));
                }
            }
        }

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
