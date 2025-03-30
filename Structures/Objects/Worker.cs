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

        private int workplace;
        public int Workplace {
            get => workplace;
            set {
                if (workplace != value) {
                    workplace = value;
                    OnPropertyChanged(nameof(Workplace));
                    OnPropertyChanged(nameof(FormattedWorkplace));
                }
            }
        }

        public string FormattedWorkplace => workplace == -1 ? string.Empty : workplace.ToString();

        public Worker(int id, WorkerGroup group) {
            Id = id;
            Group = group;
            IsBusy = false;
            Order = null;
            Workplace = -1;
        }

        public void StartTask(Order order, int workplace) {
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
