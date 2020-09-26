using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Team;
using StructureCommon.FileAccess;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;

        public CricketTeam TeamToPlayWith
        {
            get;
            set;
        }

        public ObservableCollection<object> DisplayTabs { get; set; } = new ObservableCollection<object>();

        private ReportingViewModel fReportingView;
        public ReportingViewModel ReportingView
        {
            get
            {
                return fReportingView;
            }
            set
            {
                fReportingView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IFileInteractionService fileService, IDialogCreationService dialogService)
        {
            TeamToPlayWith = new CricketTeam();
            fFileService = fileService;
            fDialogService = dialogService;
            NewTeamCommand = new RelayCommand(ExecuteNewTeamCommand);
            LoadTeamCommand = new RelayCommand(ExecuteLoadTeamCommand);
            SaveTeamCommand = new RelayCommand(ExecuteSaveTeamCommand);

            DisplayTabs.Add(new TeamOverviewViewModel(UpdateDatabase, TeamToPlayWith.Players, TeamToPlayWith.Seasons));
            DisplayTabs.Add(new PlayerEditViewModel(TeamToPlayWith, UpdateDatabase, fFileService, fDialogService));
            DisplayTabs.Add(new SeasonEditViewModel(TeamToPlayWith, UpdateDatabase, fFileService, fDialogService));
            DisplayTabs.Add(new StatsViewModel(TeamToPlayWith, UpdateDatabase, fFileService, fDialogService));

            ReportingView = new ReportingViewModel(TeamToPlayWith);
        }
        public Action<Action<ICricketTeam>> UpdateDatabase
        {
            get
            {
                return action => UpdateDatabaseFromAction(action);
            }
        }

        private void UpdateDatabaseFromAction(Action<ICricketTeam> updateTeam)
        {
            updateTeam(TeamToPlayWith);
            UpdateSubWindows();
        }

        private void UpdateSubWindows()
        {
            foreach (object tab in DisplayTabs)
            {
                if (tab is ViewModelBase<ICricketTeam> vmb)
                {
                    vmb.UpdateData(TeamToPlayWith);
                }
            }

            ReportingView?.UpdateData(TeamToPlayWith);
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
                TeamToPlayWith = new CricketTeam();
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
            if (result.Success != null && (bool)result.Success)
            {
                CricketTeam database = XmlFileAccess.ReadFromXmlFile<CricketTeam>(result.FilePath, out string error);
                if (error == null)
                {
                    TeamToPlayWith = database;
                    TeamToPlayWith.SetupEventListening();
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
            if (result.Success != null && (bool)result.Success)
            {
                XmlFileAccess.WriteToXmlFile<CricketTeam>(result.FilePath, TeamToPlayWith, out string error);
            }
        }
    }
}
