using EventSimulation.Presentation;
using EventSimulation.Strategies;
using EventSimulation.Utilities;
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
                UpdateStrategy();
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
                UpdateWarehouse();
            }
        }
    }

    private void UpdateStrategy() {
        switch (cbStrategies.SelectedIndex) {
            case 0:
                facade.SetStrategy(new StrategyA());
                break;
            case 1:
                facade.SetStrategy(new StrategyB());
                break;
            case 2:
                facade.SetStrategy(new StrategyC());
                break;
            case 3:
                facade.SetStrategy(new StrategyD());
                break;
            case 4:
                facade.SetStrategy(Utility.ParseStrategyX(
                    txtMufflers.Text,
                    txtBrakes.Text,
                    txtLights.Text,
                    chkSupplier1.IsChecked,
                    chkSupplier2.IsChecked,
                    (int)sldrSupplier1Period.Value,
                    (int)sldrSupplier2Period.Value,
                    txtSupplier1Offset.Text,
                    txtSupplier2Offset.Text
                ));
                break;
            default:
                break;
        }
    }

    private void UpdateWarehouse() {
        if (!int.TryParse(txtReplications.Text, out int replications)) replications = 0;

        facade.InitWarehouse(replications);
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

        UpdateWarehouse();
        UpdateStrategy();
    }
}