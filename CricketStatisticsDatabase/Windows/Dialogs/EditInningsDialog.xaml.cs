using System.Windows.Controls;
using System.Windows.Data;
using CricketStructures.Player;
using CricketStatisticsDatabase.UIHelpers.Converters;

namespace CSD.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for EditBattingDialog.xaml
    /// </summary>
    public partial class EditInningsDialog : ContentControl
    {
        public EditInningsDialog()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //your date time property
            if (e.PropertyType == typeof(PlayerName))
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                var con = new PlayerNameToStringConverter();
                (dgtc.Binding as Binding).Converter = con;
            }
        }
    }
}
