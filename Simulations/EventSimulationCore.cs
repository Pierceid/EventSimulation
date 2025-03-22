using EventSimulation.Flyweight;
using EventSimulation.Observer;
using EventSimulation.Structures.Events;

namespace EventSimulation.Simulations {
    public abstract class EventSimulationCore : SimulationCore, ISubject {
        private readonly List<IObserver> observers = [];
        public Event? Data { get; set; }
        public EventCalendar EventCalendar { get; set; }
        public int UpdateInterval { get; set; }
        public bool IsPaused { get; set; }
        public bool IsSlowed { get; set; }
        public bool IsGeneratedSystemEvent { get; set; }
        public double Speed { get; set; }
        public double SimulationTime { get; set; }
        public double EndOfSimulationTime { get; set; }

        protected EventSimulationCore(int replicationStock) : base(replicationStock) {
            this.Data = null;
            this.EventCalendar = new();
            this.UpdateInterval = 5;
            this.IsPaused = false;
            this.IsSlowed = false;
            this.IsGeneratedSystemEvent = false;
            this.Speed = 1.0;
            this.SimulationTime = 0.0;
            this.EndOfSimulationTime = 0.0;
        }

        public override void Experiment() {
            while (this.EventCalendar.PriorityQueue.Count > 0 && this.isRunning) {
                var nextEvent = this.EventCalendar.GetFirstEvent();

                this.SimulationTime = nextEvent.Time;
                this.Data = nextEvent;

                UpdateData();

                nextEvent.Execute();

                if (this.IsSlowed) {
                    Tick();
                }

                if (this.IsSlowed && !this.IsGeneratedSystemEvent) {
                    this.IsGeneratedSystemEvent = true;
                    var adjustedTime = this.SimulationTime + (this.Speed / this.UpdateInterval);
                    var systemEvent = SystemEventFactory.GetInstance().GetEvent(this, adjustedTime);
                    this.EventCalendar.AddEvent(systemEvent);
                } else if (!this.IsSlowed && this.IsGeneratedSystemEvent) {
                    this.IsGeneratedSystemEvent = false;
                }

                if (this.IsPaused) {
                    Tick();

                    UpdateData();

                    while (this.IsPaused) {
                        Thread.Sleep(200);
                    }
                }
            }

            this.IsGeneratedSystemEvent = false;
        }

        public virtual void UpdateData() {
            Notify();
        }

        public virtual void Tick() { }

        public void Attach(IObserver observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer) {
            observers.Remove(observer);
        }

        public void Notify() {
            if (this.Data != null) {
                foreach (var observer in observers) {
                    observer.Update(this.Data);
                }
            }
        }
    }
}
