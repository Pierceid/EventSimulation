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
        private LineSeries series;
        private LineSeries upperSeries;
        private PlotView plotView;

        public LineGraph(string modelTitle, string xAxisTitle, string yAxisTitle, string seriesTitle, PlotView plotView) {
            model = new PlotModel { Title = modelTitle };

            xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = xAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(xAxis);

            yAxis = new LinearAxis { Position = AxisPosition.Left, Title = yAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(yAxis);

            series = new LineSeries { Title = seriesTitle, Color = OxyColors.Green };
            model.Series.Add(series);

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

        public void UpdatePlot(double xValue, double yValue) {
            series.Points.Add(new DataPoint(xValue, yValue));
            xAxis.Maximum = xValue;
            yAxis.Minimum = series.MinY;
            yAxis.Maximum = series.MaxY;
            valueAnnotation.Text = $"{yValue:F0}";
            valueAnnotation.TextPosition = new DataPoint(xAxis.Maximum * 0.99, yAxis.Maximum);
            model.InvalidatePlot(true);
        }

        public void RefreshGraph() {
            series.Points.Clear();
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
