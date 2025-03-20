using EventSimulation.Strategies;

namespace EventSimulation.Utilities {
    public static class Utility {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }

        public static StrategyX ParseStrategyX(string? m, string? b, string? l, bool? s1, bool? s2, int? p1, int? p2, string? o1, string? o2) {
            if (!int.TryParse(m, out int mufflerSupply)) mufflerSupply = 0;
            if (!int.TryParse(b, out int brakeSupply)) brakeSupply = 0;
            if (!int.TryParse(l, out int lightSupply)) lightSupply = 0;
            bool isSupplier1 = s1 ?? false;
            bool isSupplier2 = s2 ?? false;
            int supplier1Period = p1 ?? 1;
            int supplier2Period = p2 ?? 1;
            if (!int.TryParse(o1, out int supplier1Offset)) supplier1Offset = 0;
            if (!int.TryParse(o2, out int supplier2Offset)) supplier2Offset = 0;

            return new StrategyX(mufflerSupply, brakeSupply, lightSupply, isSupplier1, isSupplier2, supplier1Period, supplier2Period, supplier1Offset, supplier2Offset);
        }
    }
}
