using EventSimulation.Simulations;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace EventSimulation.Presentation {
    public class LineGraph {
        private PlotModel model;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private LineSeries mainSeries;
        private LineSeries upperSeries;
        private LineSeries lowerSeries;
        private TextAnnotation valueAnnotation;
        private PlotView plotView;

        public LineGraph(string modelTitle, string xAxisTitle, string yAxisTitle, string seriesTitle, PlotView plotView) {
            model = new PlotModel { Title = modelTitle };

            xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = xAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(xAxis);

            yAxis = new LinearAxis { Position = AxisPosition.Left, Title = yAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(yAxis);

            mainSeries = new LineSeries { Title = seriesTitle, Color = OxyColors.Green };
            model.Series.Add(mainSeries);

            upperSeries = new LineSeries { Title = "Upper Bound (95%)", Color = OxyColors.Blue, LineStyle = LineStyle.Dash };
            model.Series.Add(upperSeries);

            lowerSeries = new LineSeries { Title = "Lower Bound (95%)", Color = OxyColors.Red, LineStyle = LineStyle.Dash };
            model.Series.Add(lowerSeries);

            valueAnnotation = new TextAnnotation {
                Text = "0",
                StrokeThickness = 0,
                TextColor = OxyColors.Red,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextHorizontalAlignment = HorizontalAlignment.Right,
                TextVerticalAlignment = VerticalAlignment.Top,
                TextPosition = new DataPoint(0, 0)
            };
            model.Annotations.Add(valueAnnotation);

            this.plotView = plotView;
            this.plotView.Model = model;
        }

        public void UpdatePlot(SimulationCore simulationCore) {
            if (simulationCore is Carpentry c) {
                double rep = c.CurrentReplication;
                double mean = c.AverageOrderTime.GetMean();
                (double bottom, double top) = c.AverageOrderTime.GetConfidenceInterval();

                mainSeries.Points.Add(new DataPoint(rep, mean));

                if (c.CurrentReplication % 2 == 0) {
                    upperSeries.Points.Add(new DataPoint(rep, top));
                    lowerSeries.Points.Add(new DataPoint(rep, bottom));
                }

                xAxis.Maximum = rep;
                yAxis.Minimum = Math.Min(mainSeries.MinY, Math.Min(lowerSeries.MinY, upperSeries.MinY));
                yAxis.Maximum = Math.Max(mainSeries.MaxY, Math.Max(lowerSeries.MaxY, upperSeries.MaxY));

                valueAnnotation.Text = $"{mean:F0}";
                valueAnnotation.TextPosition = new DataPoint(xAxis.Maximum * 0.99, yAxis.Maximum);
                model.InvalidatePlot(true);
            }
        }

        public void RefreshGraph() {
            mainSeries.Points.Clear();
            upperSeries.Points.Clear();
            lowerSeries.Points.Clear();

            xAxis.Minimum = 0;
            xAxis.Maximum = 1000;
            yAxis.Minimum = 0;
            yAxis.Maximum = 1000;
            model.InvalidatePlot(true);
        }

        public void InvalidatePlot() {
            model.InvalidatePlot(true);
        }
    }
}
