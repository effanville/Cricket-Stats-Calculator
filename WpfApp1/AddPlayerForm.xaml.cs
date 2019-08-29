using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cricket;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPlayerForm : Window
    {
        public AddPlayerForm()
        {
            InitializeComponent();

        }

        void AddNameClick(object sender, RoutedEventArgs e)
        {
            string name = PlayerAddBox.Text;
            Cricket_Player newMan = new Cricket_Player(name);
            Globals.Ardeley.Add(newMan);
            PlayerAddBox.Clear();
            Close();
        }


    }
}
