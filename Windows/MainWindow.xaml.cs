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

    private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (sender is ComboBox comboBox) {
            if (comboBox == cbStrategies) {
                UpdateUI();
            }
        }
    }

    private void TextBoxLostFocus(object sender, RoutedEventArgs e) {
        if (sender is TextBox textBox) {
            if (textBox == txtReplications) {

            }
        }
    }

    private void UpdateCarpentry() {
        if (!int.TryParse(txtReplications.Text, out int replications)) replications = 0;

    }

    private void UpdateUI() {
        if (cbStrategies.SelectedIndex == 4) {
            gbStrategyXControls.Visibility = Visibility.Visible;
        } else {
            gbStrategyXControls.Visibility = Visibility.Collapsed;
        }
    }

    private void InitUI() {
        cbStrategies.SelectedIndex = 0;
        txtReplications.Text = "1000000";

        UpdateCarpentry();
    }
}