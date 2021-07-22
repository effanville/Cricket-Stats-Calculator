﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CricketStructures;
using GUI.ViewModels;
using Common.Structure.FileAccess;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

namespace CSD.GUI.ViewModels
{
    internal sealed class MainWindowVM : PropertyChangedBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;

        public CricketTeam Database
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

        public MainWindowVM(IFileInteractionService fileService, IDialogCreationService dialogService)
        {
            Database = new CricketTeam();
            fFileService = fileService;
            fDialogService = dialogService;
            NewTeamCommand = new RelayCommand(ExecuteNewTeamCommand);
            LoadTeamCommand = new RelayCommand(ExecuteLoadTeamCommand);
            SaveTeamCommand = new RelayCommand(ExecuteSaveTeamCommand);
            ReportingView = new ReportingViewModel(null);
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
            if (result.Success != null && (bool)result.Success)
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
            if (result.Success != null && (bool)result.Success)
            {
                XmlFileAccess.WriteToXmlFile<CricketTeam>(result.FilePath, Database, out string error);
            }
        }
    }
}
