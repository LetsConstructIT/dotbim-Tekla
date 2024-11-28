using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace dotbimTekla.UI;
/// <summary>
/// Interaction logic for MainControl.xaml
/// </summary>
public partial class MainControl : UserControl
{
    public MainControl()
    {
        InitializeComponent();
    }

    private void PickOutput_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "dotbim files(*.bim)|*.bim"
        };

        var result = saveFileDialog.ShowDialog();
        if (result.HasValue && result.Value)
        {
            var fileName = saveFileDialog.FileName;
            TrySetOutputPath(fileName);
        }
    }

    private void TrySetOutputPath(string fileName)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            viewModel.OutputPath = fileName;
        }
    }
}
