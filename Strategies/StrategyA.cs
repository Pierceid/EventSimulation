namespace EventSimulation.Strategies {
    public class StrategyA : Strategy {
        public override void DetermineSupplier(int week) {
            if (week < SUPPLIER1_SWITCH) {
                if (rng.Next() < supplier1Initial.Next()) {
                    RestockComponents();
                }
            } else {
                if (rng.Next() < supplier1Adjusted.Next()) {
                    RestockComponents();
                }
            }
        }
    }
}
