using EventSimulation.Flyweight;
using EventSimulation.Generators;
using EventSimulation.Observer;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Simulations {
    public abstract class EventSimulationCore : SimulationCore, ISubject {
        private List<IObserver> observers = [];
        public Workshop Workshop { get; set; }
        public EventCalendar EventCalendar { get; set; }
        public RandomGenerators Generators { get; set; }
        public int UpdateInterval { get; set; }
        public bool IsPaused { get; set; }
        public bool IsSlowed { get; set; }
        public bool IsGeneratedSystemEvent { get; set; }
        public double Speed { get; set; }
        public double SimulationTime { get; set; }
        public double EndOfSimulationTime { get; set; }

        protected EventSimulationCore(int replicationStock) : base(replicationStock) {
            this.Workshop = new(this);
            this.EventCalendar = new();
            this.Generators = new();
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

                    Notify();

                    while (this.IsPaused) {
                        Thread.Sleep(200);
                    }
                }
            }

            this.IsGeneratedSystemEvent = false;
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
            if (Workshop == null) return;

            foreach (var observer in observers) {
                observer.Refresh(this);
            }
        }

        public virtual void Tick() { }
    }
}
