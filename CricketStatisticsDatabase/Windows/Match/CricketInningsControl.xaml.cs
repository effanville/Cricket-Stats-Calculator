using System.Windows.Controls;
using System.Windows.Data;

using CricketStatisticsDatabase.UIHelpers.Converters;

using CricketStructures.Match.Innings;
using CricketStructures.Player;

using CSD.ViewModels.Match;

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

            if (e.PropertyType == typeof(Over))
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                var con = new OverToStringConverter();
                (dgtc.Binding as Binding).Converter = con;
            }
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (DataContext is CricketInningsViewModel vm)
            {
                int numberEntries = vm.Batting.Count;
                e.NewItem = new BattingEntry() { Order = numberEntries + 1 };
            }
        }
    }
}
