using EventSimulation.Generators;
using EventSimulation.Observer;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;
using System.Windows;
using System.Windows.Threading;

namespace EventSimulation.Simulations {
    public abstract class EventSimulationCore : SimulationCore, ISubject {
        private List<IObserver> observers = [];
        public Workshop Workshop { get; set; }
        public PriorityQueue<Event, double> EventCalendar { get; set; }
        public RandomGenerators Generators { get; set; }
        public bool IsPaused { get; set; }
        public double Speed { get; set; }
        public double SimulationTime { get; set; }
        public double EndOfSimulationTime { get; set; }

        protected EventSimulationCore(int replicationStock, double endOfSimulationTime) : base(replicationStock) {
            this.Workshop = new(this, 10, 10, 10);
            this.EventCalendar = new();
            this.Generators = new();
            this.IsPaused = false;
            this.Speed = 1.0;
            this.SimulationTime = 0.0;
            this.EndOfSimulationTime = endOfSimulationTime;
        }

        public override void Experiment() {
            try {
                while (this.SimulationTime < this.EndOfSimulationTime && this.EventCalendar.Count > 0 && this.isRunning) {
                    while (this.IsPaused) {
                        Thread.Sleep(200);
                    }

                    if (this.EventCalendar.TryDequeue(out var nextEvent, out var priority)) {
                        this.SimulationTime = priority;
                        nextEvent.Execute();
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void Pause() {
            this.IsPaused = true;
        }

        public void Resume() {
            this.IsPaused = false;
        }

        public void Attach(IObserver observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer) {
            observers.Remove(observer);
        }

        public void Notify() {
            Application.Current.Dispatcher.Invoke(() => {
                foreach (var observer in observers) {
                    observer.Refresh(this);
                }
            }, DispatcherPriority.Background);
        }
    }
}
