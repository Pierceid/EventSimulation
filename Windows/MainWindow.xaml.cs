using EventSimulation.Presentation;
using System.Windows;
using System.Windows.Controls;

namespace EventSimulation.Windows;

public partial class MainWindow : Window {
    private Facade facade;

    public MainWindow() {
        InitializeComponent();

        facade = new(this);
        facade.InitGraph(plotView);

        InitUI();
    }

    private void ButtonClick(object sender, RoutedEventArgs e) {
        if (sender is Button button) {
            if (button == btnStart) {
                facade.StartSimulation();
                btnStart.IsEnabled = false;
            } else if (button == btnStop) {
                facade.StopSimulation();
                btnStart.IsEnabled = true;
            } else if (button == btnAnalyze) {
                facade.AnalyzeReplication();
            }
        }
    }

    private void TextBoxLostFocus(object sender, RoutedEventArgs e) {
        if (sender is TextBox textBox) {
            if (textBox == txtReplications) {
                InitCarpentry();
            }
        }
    }

    private void InitCarpentry() {
        if (!int.TryParse(txtReplications.Text, out int replications)) replications = 0;

        facade.InitCarpentry(replications, sldSpeed.Value);
    }

    private void InitUI() {
        txtReplications.Text = "1000000";

        InitCarpentry();
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (sender is Slider slider) {
            if (slider == sldSpeed) {
                facade.UpdateCarpentry(sldSpeed.Value);
            }
        }
    }
}