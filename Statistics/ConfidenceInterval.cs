namespace EventSimulation.Statistics {
    public class ConfidenceInterval {
        public int Count { get; set; }
        public double Sum { get; set; }
        public double SumOfSquares { get; set; }

        public ConfidenceInterval() {
            Count = 0;
            Sum = 0;
            SumOfSquares = 0;
        }

        public void AddSample(double value) {
            Count++;
            Sum += value;
            SumOfSquares += value * value;
        }

        public (double bottom, double top) GetConfidenceInterval() {
            var mean = Sum / Count;
            var variation = 1.96 * Sigma() / Math.Sqrt(Count);

            return (mean - variation, mean + variation);
        }

        public void Clear() {
            Count = 0;
            Sum = 0;
            SumOfSquares = 0;
        }

        private double Sigma() {
            if (Count < 2) return 0;

            var top = SumOfSquares - ((Sum * Sum) / Count);
            var bottom = Count - 1;

            return Math.Sqrt(top / bottom);
        }
    }
}
