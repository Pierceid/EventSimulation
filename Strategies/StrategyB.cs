namespace EventSimulation.Strategies {
    public class StrategyB : Strategy {
        public override void DetermineSupplier(int week) {
            if (week < SUPPLIER2_SWITCH) {
                if (rng.Next() < supplier2Initial.Next()) {
                    RestockComponents();
                }
            } else {
                if (rng.Next() < supplier2Adjusted.Next()) {
                    RestockComponents();
                }
            }
        }
    }
}
