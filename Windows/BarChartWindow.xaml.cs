using EventSimulation.Presentation;
using System.Windows;

namespace EventSimulation.Windows {
    public partial class BarChartWindow : Window {
        public BarChartWindow(Window? window, string title, double[] costs) {
            InitializeComponent();

            Owner = window;

            _ = new BarChart(plotView, title, costs);
        }
    }
}
