using EventSimulation.Structures;

namespace EventSimulation.Randoms.Discrete {
    public class EmpiricD : GeneralRandom<int> {
        private List<EmpiricData<int>> samples;
        private List<UniformD> generators;

        public EmpiricD(EmpiricData<int>[] samples) {
            this.samples = new(samples.Length);
            generators = new(samples.Length);

            double cumProb = 0.0;

            samples.ToList().ForEach(s => {
                EmpiricData<int> data = new(s.Range.First, s.Range.Second, s.Probability + cumProb);
                this.samples.Add(data);
                generators.Add(new(s.Range.First, s.Range.Second));
                cumProb += s.Probability;
            });
        }

        public override int Next() {
            double rng = Generator.NextDouble();

            for (int i = 0; i < samples.Count; i++) {
                if (rng < samples[i].Probability) {
                    return generators[i].Next();
                }
            }

            throw new ArgumentException("Invalid probability distribution");
        }
    }
}
