using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Common.Structure.FileAccess;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures;

namespace CSD.ViewModels
{
    internal sealed class MainWindowVM : PropertyChangedBase
    {
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
            DisplayTabs.Add(new PlayerEditViewModel(Database, UpdateDatabase, fUiGlobals));
            DisplayTabs.Add(new SeasonEditViewModel(Database, UpdateDatabase, fUiGlobals));
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
            System.Windows.MessageBoxResult result = fUiGlobals.DialogCreationService.ShowMessageBox("Are you sure you want a new team?", "New Team?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
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
            FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile(string.Empty);
            if (result.Success)
            {
                var database = CricketTeamFactory.CreateFromFile(fUiGlobals.CurrentFileSystem, result.FilePath, out string error);
                if (string.IsNullOrEmpty(error))
                {
                    Database = database;
                    UpdateSubWindows();
                }
                else
                {
                    _ = fUiGlobals.DialogCreationService.ShowMessageBox(error, "Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public ICommand SaveTeamCommand
        {
            get;
        }
        private void ExecuteSaveTeamCommand()
        {
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("xml", string.Empty, string.Empty, "XML Files|*.xml|All Files|*.*");
            if (result.Success)
            {
                XmlFileAccess.WriteToXmlFile(result.FilePath, Database, out string error);
                if (!string.IsNullOrEmpty(error))
                {
                    _ = fUiGlobals.DialogCreationService.ShowMessageBox(error, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public ICommand LoadOldTeamCommand
        {
            get;
        }

        private void ExecuteLoadOldTeamCommand()
        {
            FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile(string.Empty);
            if (result.Success)
            {
                var database = CricketTeamFactory.CreateFromOldStyleFile(fUiGlobals.CurrentFileSystem, result.FilePath, out string error);
                if (string.IsNullOrEmpty(error))
                {
                    Database = database;
                    UpdateSubWindows();
                }
                else
                {
                    _ = fUiGlobals.DialogCreationService.ShowMessageBox(error, "Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
