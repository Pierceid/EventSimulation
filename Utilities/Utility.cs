namespace EventSimulation.Utilities {
    public static class Utility {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }

        public static string FormatTime(double time) {
            return TimeSpan.FromSeconds(time % 28800).Add(TimeSpan.FromHours(6)).ToString(@"hh\:mm\:ss");
        }
    }
}
