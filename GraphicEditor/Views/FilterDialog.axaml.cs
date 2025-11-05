using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GraphicEditor.Models;

namespace GraphicEditor.Views;

public partial class FilterDialog : Window
{
    public float Intensity { get; private set; }
    public bool IsAplied { get; private set; }
    public FilterDialog(IFilter filter)
    {
        InitializeComponent();
        IntensitySlider.Value = filter.Intensity;
        IntensitySlider.Maximum = filter.MaxIntensity;
        IntensitySlider.Minimum = filter.MinIntensity;
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        Intensity = (float)IntensitySlider.Value;
        IsAplied = true;
        Close(true);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        IsAplied = false;
        Close(false);
    }
}