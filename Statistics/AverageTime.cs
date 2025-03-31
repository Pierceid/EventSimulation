namespace EventSimulation.Statistics {
    public class AverageTime {
        public int Count { get; set; }
        public double Sum { get; set; }

        public void AddSample(double time) {
            Count++;
            Sum += time;
        }

        public double GetAverage() {
            return Count == 0 ? 0 : Sum / Count;
        }

        public void Clear() {
            Count = 0;
            Sum = 0.0;
        }
    }
}
