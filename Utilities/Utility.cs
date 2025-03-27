namespace EventSimulation.Utilities {
    public static class Utility {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }

        public static string FormatTime(double time) {
            int daySeconds = (int)(time % 28800);
            int days = (int)(time / 28800);
            int hours = (daySeconds / 3600) % 8 + 6;
            int minutes = (daySeconds % 3600) / 60;
            int seconds = daySeconds % 60;

            return $"{days:D2}:{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
