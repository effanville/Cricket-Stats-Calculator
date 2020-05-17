using GUI.ViewModels;
using System.Windows;
using UICommon.Services;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFileInteractionService fFileService;
        private IDialogCreationService fDialogCreation;
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
