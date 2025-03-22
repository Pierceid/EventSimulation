namespace EventSimulation.Utilities {
    public static class Utility {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }
    }
}
