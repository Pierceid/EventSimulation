﻿namespace EventSimulation.Simulations {
    public class Carpentry(int replicationStock) : EventSimulationCore(replicationStock) {
        public override void AfterSimulation() {

        }

        public override void AfterSimulationRun() {

        }

        public override void BeforeSimulation() {
            Notify();
        }

        public override void BeforeSimulationRun() {

        }

        public override void Experiment() {

        }
    }
}
