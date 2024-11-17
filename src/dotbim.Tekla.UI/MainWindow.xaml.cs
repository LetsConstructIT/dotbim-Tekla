using System;
using TSD = Tekla.Structures.Dialog;

namespace dotbimTekla.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : TSD.PluginWindowBase
    {
        private readonly MainWindowViewModel _dataModel;

        public MainWindow(MainWindowViewModel DataModel)
        {
            InitializeComponent();
            _dataModel = DataModel;
        }

        private void WPFOkApplyModifyGetOnOffCancel_ApplyClicked(object sender, EventArgs e)
        {
            Apply();
        }

        private void WPFOkApplyModifyGetOnOffCancel_CancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_GetClicked(object sender, EventArgs e)
        {
            Get();
        }

        private void WPFOkApplyModifyGetOnOffCancel_ModifyClicked(object sender, EventArgs e)
        {
            Modify();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OkClicked(object sender, EventArgs e)
        {
            Apply();
            Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OnOffClicked(object sender, EventArgs e)
        {
            ToggleSelection();
        }
    }
}
