namespace EventSimulation.Simulations {
    public abstract class SimulationCore {
        protected Thread? thread;
        protected bool isRunning;
        public int CurrentReplication { get; set; }
        public int ReplicationStock { get; set; }

        protected SimulationCore(int replicationStock) {
            this.thread = null;
            this.isRunning = false;
            this.CurrentReplication = 0;
            this.ReplicationStock = replicationStock;
        }

        public void Start() {
            this.isRunning = true;
        }

        public void Stop() {
            this.isRunning = false;
            this.thread = null;
        }

        public void RunSimulation() {
            this.isRunning = true;
            this.thread = new(Simulate);
            this.thread.Start();
        }

        private void Simulate() {
            BeforeSimulationRun();

            for (this.CurrentReplication = 0; this.CurrentReplication < this.ReplicationStock && this.isRunning; this.CurrentReplication++) {
                BeforeSimulation();
                Experiment();
                AfterSimulation();
            }

            AfterSimulationRun();

            this.isRunning = false;
        }

        public abstract void Experiment();
        public abstract void BeforeSimulation();
        public abstract void AfterSimulation();
        public abstract void BeforeSimulationRun();
        public abstract void AfterSimulationRun();
    }
}
