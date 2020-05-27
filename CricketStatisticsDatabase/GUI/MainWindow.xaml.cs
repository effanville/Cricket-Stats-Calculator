using System;
using System.Reflection;
using System.Windows;
using GUI.ViewModels;
using UICommon.Services;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            MainWindowViewModel data = new MainWindowViewModel(fFileService, fDialogCreation);
            DataContext = data;
        }
    }
}
