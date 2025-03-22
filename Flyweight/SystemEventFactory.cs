using EventSimulation.Simulations;
using EventSimulation.Structures.Events;

namespace EventSimulation.Flyweight {
    public class SystemEventFactory {
        private static SystemEventFactory? factoryInstance = null;
        private static readonly object lockObj = new();
        private SystemEvent? eventInstance = null;

        private SystemEventFactory() { }

        public static SystemEventFactory GetInstance() {
            if (factoryInstance == null) {
                lock (lockObj) {
                    factoryInstance ??= new SystemEventFactory();
                }
            }

            return factoryInstance;
        }

        public SystemEvent GetEvent(EventSimulationCore simulationCore, double time) {
            lock (lockObj) {
                if (eventInstance == null) {
                    eventInstance = new SystemEvent(simulationCore, time);
                } else {
                    eventInstance.Time = time;
                }

                return eventInstance;
            }
        }
    }
}
