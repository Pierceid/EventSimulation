namespace EventSimulation.Strategies {
    public class StrategyX : Strategy {
        private int mufflerSupply;
        private int brakeSupply;
        private int lightSupply;
        private bool isSupplier1;
        private bool isSupplier2;
        private int supplier1Period;
        private int supplier2Period;
        private int supplier1Offset;
        private int supplier2Offset;

        public StrategyX(int mufflerSupply, int brakeSupply, int lightSupply, bool isSupplier1, bool isSupplier2, int supplier1Period, int supplier2Period, int supplier1Offset, int supplier2Offset) {
            this.mufflerSupply = mufflerSupply;
            this.brakeSupply = brakeSupply;
            this.lightSupply = lightSupply;
            this.isSupplier1 = isSupplier1;
            this.isSupplier2 = isSupplier2;
            this.supplier1Period = supplier1Period;
            this.supplier2Period = supplier2Period;
            this.supplier1Offset = supplier1Offset;
            this.supplier2Offset = supplier2Offset;
        }

        public override void DetermineSupplier(int week) {
            if (isSupplier1) {
                HandleSupplier(week, supplier1Offset, supplier1Period, SUPPLIER1_SWITCH, supplier1Initial.Next(), supplier1Adjusted.Next());
            }

            if (isSupplier2) {
                HandleSupplier(week, supplier2Offset, supplier2Period, SUPPLIER2_SWITCH, supplier2Initial.Next(), supplier2Adjusted.Next());
            }
        }

        private void HandleSupplier(int week, int offset, int period, int switchWeek, double initialProbability, double adjustedProbability) {
            if (week >= offset && week % period == 0) {
                double probability = week < switchWeek ? initialProbability : adjustedProbability;

                if (rng.Next() < probability) {
                    mufflerStock += mufflerSupply;
                    brakeStock += brakeSupply;
                    lightStock += lightSupply;
                }
            }
        }
    }
}
