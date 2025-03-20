using EventSimulation.Structures;

namespace EventSimulation.Randoms.Continuous {
    public class UniformC : GeneralRandom<double> {
        private double min;
        private double max;

        public UniformC(double min, double max) {
            this.min = min;
            this.max = max;
        }

        public override double Next() {
            return min + (max - min) * Generator.NextDouble();
        }
    }
}
