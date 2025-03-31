namespace EventSimulation.Statistics {
    public class AverageCount {
        public int Count { get; set; }

        public void AddSample(int count) {
            Count += count;
        }

        public void Clear() {
            Count = 0;
        }
    }
}
