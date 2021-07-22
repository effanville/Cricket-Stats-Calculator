using System;
using System.Reflection;
using System.Windows;
using CSD.GUI.ViewModels;
using Common.UI.Services;

namespace CSD.GUI.Windows
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreation;
        public MainWindow()
        {
            InitializeComponent();
            fFileService = new FileInteractionService(this);
            fDialogCreation = new DialogCreationService(this);
            var programInfo = Assembly.GetExecutingAssembly().GetName();
            Version version = programInfo.Version;
            Title = programInfo.Name + " v" + version.ToString();
            var dc = new MainWindowVM(fFileService, fDialogCreation);
            DataContext = dc;
        }
    }
}
