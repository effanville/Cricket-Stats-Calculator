using System;
using System.Reflection;
using System.Windows;
using CSD.ViewModels;
using Common.UI.Services;
using Common.UI;
using System.IO.Abstractions;

namespace CSD.Windows
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UiGlobals fUiGlobals;
        public MainWindow()
        {
            InitializeComponent();
            var programInfo = Assembly.GetExecutingAssembly().GetName();
            Version version = programInfo.Version;
            Title = programInfo.Name + " v" + version.ToString();
            FileInteractionService FileInteractionService = new FileInteractionService(this);
            DialogCreationService DialogCreationService = new DialogCreationService(this);
            fUiGlobals = new UiGlobals(null, new DispatcherInstance(), new FileSystem(), FileInteractionService, DialogCreationService, null);
            var dc = new MainWindowVM(fUiGlobals);
            DataContext = dc;
        }
    }
}
