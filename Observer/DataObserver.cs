using EventSimulation.Presentation;
using EventSimulation.Structures.Events;

namespace EventSimulation.Observer {
    public class DataObserver : IObserver {
        public LineGraph LineGraph { get; set; }

        public DataObserver() {

        }

        public void Update(Event eventData) {

        }
    }
}
