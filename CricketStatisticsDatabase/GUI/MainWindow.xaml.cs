using GUI.ViewModels;
using GUISupport.Services;
using System.Windows;

namespace CricketStatisticsDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IFileInteractionService fFileService;
        IDialogCreationService fDialogCreation;
        public MainWindow()
        {
            InitializeComponent();
            fFileService = new FileInteractionService(this);
            fDialogCreation = new DialogCreationService(this);

            var data = new MainWindowViewModel(fFileService, fDialogCreation);
            DataContext = data;
        }
    }
}
