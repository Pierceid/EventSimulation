using EventSimulation.Structures.Enums;
using System.ComponentModel;

namespace EventSimulation.Structures.Objects {
    public class Worker : INotifyPropertyChanged {
        public int Id { get; }
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

        private Order? order;
        public Order? Order {
            get => order;
            set {
                if (order != value) {
                    order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }

        private Workplace? workplace;
        public Workplace? Workplace {
            get => workplace;
            set {
                if (workplace != null) {
                    workplace = value;
                    OnPropertyChanged(nameof(Workplace));
                }
            }
        }

        public Worker(int id, WorkerGroup group) {
            Id = id;
            Group = group;
            IsBusy = false;
            Order = null;
            Workplace = null;
        }

        public void StartTask(Order order, Workplace workplace) {
            IsBusy = true;
            Order = order;
            Workplace = workplace;
        }

        public void FinishTask() {
            IsBusy = false;
            Order = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
