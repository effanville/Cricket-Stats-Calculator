using System.Windows.Controls;
using System.Windows.Data;

using CricketStatisticsDatabase.UIHelpers.Converters;

using CricketStructures.Player;

namespace CSD.Windows.Match
{
    /// <summary>
    /// Interaction logic for CricketInningsView.xaml
    /// </summary>
    public partial class CricketInningsView : ContentControl
    {
        public CricketInningsView()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(PlayerName))
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                var con = new PlayerNameToStringConverter();
                (dgtc.Binding as Binding).Converter = con;
            }
        }
    }
}
