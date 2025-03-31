﻿using EventSimulation.Simulations;
using EventSimulation.Structures.Objects;

namespace EventSimulation.Structures.Events {
    public class MountingStartEvent : Event<ProductionManager> {
        public Workplace Workplace { get; }

        public MountingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Workplace workplace) : base(simulationCore, time, 5) {
            Workplace = workplace;
        }

        public override void Execute() {
            Workplace.StartWork();

            var mountingTime = SimulationCore.Generators.WardrobeMountingTime.Next();

            Time += mountingTime;

            SimulationCore.EventCalendar.Enqueue(new MountingEndEvent(SimulationCore, Time, Workplace), Time);
        }
    }
}
