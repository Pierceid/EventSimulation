namespace EventSimulation.Strategies {
    public class StrategyD : Strategy {
        public override void DetermineSupplier(int week) {
            if (week % 2 == 1) {
                if (week < SUPPLIER1_SWITCH) {
                    if (rng.Next() < supplier1Initial.Next()) {
                        RestockComponents();
                    }
                } else {
                    if (rng.Next() < supplier1Adjusted.Next()) {
                        RestockComponents();
                    }
                }
            } else {
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
}
