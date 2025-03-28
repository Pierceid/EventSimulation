﻿using EventSimulation.Presentation;
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
                InitCarpentry();
                facade?.StartSimulation();
                btnStart.IsEnabled = false;
            } else if (button == btnPause) {
                bool isPaused = facade?.PauseSimulation() ?? false;
                btnStart.IsEnabled = !isPaused;
                btnStop.IsEnabled = !isPaused;
            } else if (button == btnStop) {
                facade?.StopSimulation();
                btnStart.IsEnabled = true;
            } else if (button == btnAnalyze) {
                facade?.AnalyzeReplication();
            }
        }
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (sender is Slider slider) {
            if (slider == sldSpeed) {
                UpdateCarpentry();
            }
        }
    }

    private void InitCarpentry() {
        if (!int.TryParse(txtReplications.Text, out int replications)) replications = 0;
        if (!int.TryParse(txtWorkersA.Text, out int workersA)) workersA = 0;
        if (!int.TryParse(txtWorkersB.Text, out int workersB)) workersB = 0;
        if (!int.TryParse(txtWorkersC.Text, out int workersC)) workersC = 0;

        facade?.InitCarpentry(replications, sldSpeed.Value, workersA, workersB, workersC);
        facade?.InitObservers(txtTime, dgOrders, dgWorkers);

        UpdateCarpentry();
    }

    private void UpdateCarpentry() {
        double[] snapValues = [1, 60, 3600, 36000, 360000, 3600000, double.MaxValue];
        int index = (int)(sldSpeed.Value - 1);
        double speed = snapValues[index];

        facade?.UpdateCarpentry(speed);
        lblSpeed.Content = $"Speed: {(index == snapValues.Length - 1 ? "VIRTUAL" : speed):0x}";
    }

    private void InitUI() {
        txtReplications.Text = "1000000";
        sldSpeed.Value = 4;
        txtWorkersA.Text = "4";
        txtWorkersB.Text = "4";
        txtWorkersC.Text = "8";
        txtTime.Text = "00d 00h 00m 00s";

        InitCarpentry();
    }
}