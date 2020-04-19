using Cricket.Player;
using System.Windows.Controls;
using System.Windows.Data;

namespace GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for EditBowlingDialog.xaml
    /// </summary>
    public partial class EditBowlingDialog : UserControl
    {
        public EditBowlingDialog()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
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
