using EventSimulation.Simulations;

namespace EventSimulation.Observer {
    public interface IObserver {
        void Refresh(SimulationCore simulationCore);
    }
}
