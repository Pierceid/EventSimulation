namespace EventSimulation.Structures.Events {
    public class EventCalendar {
        public PriorityQueue<Event, int> PriorityQueue { get; set; }

        public EventCalendar() {
            PriorityQueue = new();
        }

        public Event GetFirstEvent() {
            return PriorityQueue.Peek();
        }

        public void AddEvent(Event newEvent) {
            PriorityQueue.Enqueue(newEvent, (int)newEvent.Time);
        }

        public void RemoveFirstEvent() {
            PriorityQueue.Dequeue();
        }
    }
}
