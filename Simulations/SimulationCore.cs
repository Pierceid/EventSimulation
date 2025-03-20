namespace EventSimulation.Simulations {
    public abstract class SimulationCore {
        protected Thread? thread;
        protected bool isRunning;
        public int CurrentReplication { get; private set; }
        public int ReplicationStock { get; private set; }

        protected SimulationCore(int replicationStock) {
            thread = null;
            isRunning = false;
            CurrentReplication = 0;
            ReplicationStock = replicationStock;
        }

        public void Start() {
            isRunning = true;
        }

        public void Stop() {
            isRunning = false;
        }

        public void RunSimulation() {
            isRunning = true;
            thread = new(Simulate);
            thread.Start();
        }

        private void Simulate() {
            BeforeSimulationRun();

            for (CurrentReplication = 0; CurrentReplication < ReplicationStock && isRunning; CurrentReplication++) {
                BeforeSimulation();
                Experiment();
                AfterSimulation();
            }

            AfterSimulationRun();
        }

        public abstract void Experiment();
        public abstract void BeforeSimulation();
        public abstract void AfterSimulation();
        public abstract void BeforeSimulationRun();
        public abstract void AfterSimulationRun();
    }
}
