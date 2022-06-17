using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Common.Structure.FileAccess;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures;
using CricketStructures.Migration;

namespace CSD.ViewModels
{
    internal sealed class MainWindowVM : PropertyChangedBase
    {
        private IFileInteractionService fFileService => fUiGlobals.FileInteractionService;
        private IDialogCreationService fDialogService => fUiGlobals.DialogCreationService;

        private readonly UiGlobals fUiGlobals;
        public CricketTeam Database
        {
            get;
            set;
        }

        public ObservableCollection<object> DisplayTabs { get; set; } = new ObservableCollection<object>();

        private ReportingViewModel fReportingView;
        public ReportingViewModel ReportingView
        {

            get => fReportingView;
            set
            {
                fReportingView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowVM(UiGlobals uiGlobals)
        {
            fUiGlobals = uiGlobals;
            Database = new CricketTeam();
            NewTeamCommand = new RelayCommand(ExecuteNewTeamCommand);
            LoadTeamCommand = new RelayCommand(ExecuteLoadTeamCommand);
            SaveTeamCommand = new RelayCommand(ExecuteSaveTeamCommand);
            LoadOldTeamCommand = new RelayCommand(ExecuteLoadOldTeamCommand);
            ReportingView = new ReportingViewModel(null);

            DisplayTabs.Add(new TeamOverviewViewModel(UpdateDatabase, Database.Players(), Database.Seasons));
            DisplayTabs.Add(new PlayerEditViewModel(Database, UpdateDatabase, fFileService, fDialogService));
            DisplayTabs.Add(new SeasonEditViewModel(Database, UpdateDatabase, fFileService, fDialogService));
            DisplayTabs.Add(new StatsViewModel(Database, fUiGlobals));
        }

        public Action<Action<ICricketTeam>> UpdateDatabase => action => UpdateDatabaseFromAction(action);

        private void UpdateDatabaseFromAction(Action<ICricketTeam> updateTeam)
        {
            updateTeam(Database);
            UpdateSubWindows();
        }

        private void UpdateSubWindows()
        {
            foreach (object tab in DisplayTabs)
            {
                if (tab is ViewModelBase<ICricketTeam> vmb)
                {
                    vmb.UpdateData(Database);
                }
            }

            ReportingView?.UpdateData(null);
        }


        public ICommand NewTeamCommand
        {
            get;
        }

        private void ExecuteNewTeamCommand()
        {
            System.Windows.MessageBoxResult result = fDialogService.ShowMessageBox("Are you sure you want a new team?", "New Team?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                Database = new CricketTeam();
                UpdateSubWindows();
            }
        }

        public ICommand LoadTeamCommand
        {
            get;
        }

        private void ExecuteLoadTeamCommand()
        {
            FileInteractionResult result = fFileService.OpenFile(string.Empty);
            if (result.Success)
            {
                CricketTeam database = XmlFileAccess.ReadFromXmlFile<CricketTeam>(result.FilePath, out string error);
                if (error == null)
                {
                    Database = database;
                    Database.SetupEventListening();
                    UpdateSubWindows();
                }
            }
        }

        public ICommand SaveTeamCommand
        {
            get;
        }
        private void ExecuteSaveTeamCommand()
        {
            FileInteractionResult result = fFileService.SaveFile("xml", string.Empty, string.Empty, "XML Files|*.xml|All Files|*.*");
            if (result.Success)
            {
                XmlFileAccess.WriteToXmlFile<CricketTeam>(result.FilePath, Database, out string error);
            }
        }

        public ICommand LoadOldTeamCommand
        {
            get;
        }

        private void ExecuteLoadOldTeamCommand()
        {
            FileInteractionResult result = fFileService.OpenFile(string.Empty);
            if (result.Success)
            {
                Cricket.Team.CricketTeam database = XmlFileAccess.ReadFromXmlFile<Cricket.Team.CricketTeam>(result.FilePath, out string error);
                var newStyle = TeamConverter.Conversion(database);
                if (error == null)
                {
                    Database = newStyle;
                    Database.SetupEventListening();
                    UpdateSubWindows();
                }
            }
        }
    }
}
