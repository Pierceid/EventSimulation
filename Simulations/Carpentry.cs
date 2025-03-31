using EventSimulation.Statistics;
using EventSimulation.Structures.Events;
using EventSimulation.Structures.Objects;
using System.Windows;

namespace EventSimulation.Simulations {
    public class Carpentry : EventSimulationCore<ProductionManager> {
        public AverageTime AverageOrderTime { get; set; }
        public AverageCount AverageFinishedOrdersCount { get; set; }
        public AverageCount AverageNotStartedOrdersCount { get; set; }

        public Carpentry(int replicationStock, double endOfSimulationTime) : base(replicationStock, endOfSimulationTime) {
            this.Data = new ProductionManager();
            this.AverageOrderTime = new();
            this.AverageFinishedOrdersCount = new();
            this.AverageNotStartedOrdersCount = new();
        }

        public override void BeforeSimulation() {
            base.BeforeSimulation();

            this.Data.Clear();

            EventCalendar.Enqueue(new SystemEvent<ProductionManager>(this, SimulationTime), SimulationTime);
            EventCalendar.Enqueue(new OrderStartEvent(this, SimulationTime), SimulationTime);
        }

        public override void AfterSimulation() {
            if (CurrentReplication >= ReplicationStock * 0.01 && (ReplicationStock < 1000 || CurrentReplication % (ReplicationStock / 1000) == 0)) {
                Notify();
            }

            this.Data.AverageNotStartedOrdersCount.AddSample(this.Data.QueueA.Count);
            this.AverageOrderTime.AddSample(this.Data.AverageOrderTime.GetAverage());
            this.AverageFinishedOrdersCount.AddSample(this.Data.AverageFinishedOrdersCount.Count);
            this.AverageNotStartedOrdersCount.AddSample(this.Data.AverageNotStartedOrdersCount.Count);

            base.AfterSimulation();
        }

        public override void BeforeSimulationRun() {
            base.BeforeSimulationRun();

            this.Data.Clear();
            this.AverageOrderTime.Clear();
            this.AverageFinishedOrdersCount.Clear();
            this.AverageNotStartedOrdersCount.Clear();
        }

        public override void AfterSimulationRun() {
            base.AfterSimulationRun();
        }
    }
}
