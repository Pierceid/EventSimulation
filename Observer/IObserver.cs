using EventSimulation.Structures.Events;

namespace EventSimulation.Observer {
    public interface IObserver {
        void Update(Event eventData);
    }
}
