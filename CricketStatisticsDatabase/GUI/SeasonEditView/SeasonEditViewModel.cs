using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cricket.Interfaces;
using GUI.Dialogs.ViewModels;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class SeasonEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        private List<ICricketSeason> fSeasons;

        private string fTeamHomeLocation;

        public List<ICricketSeason> Seasons
        {
            get
            {
                return fSeasons;
            }
            set
            {
                fSeasons = value;
                OnPropertyChanged();
            }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get
            {
                return fSelectedSeason;
            }
            set
            {
                fSelectedSeason = value;
                OnPropertyChanged();
                SelectedSeasonViewModel.UpdateSelected(value, fTeamHomeLocation);
            }
        }

        private SelectedSeasonEditViewModel fSelectedSeasonViewModel;
        public SelectedSeasonEditViewModel SelectedSeasonViewModel
        {
            get
            {
                return fSelectedSeasonViewModel;
            }
            set
            {
                fSelectedSeasonViewModel = value;
                OnPropertyChanged();
            }
        }

        public SeasonEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Season Edit", team)
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Seasons = team.Seasons;
            fTeamHomeLocation = team.HomeLocation;
            SelectedSeasonViewModel = new SelectedSeasonEditViewModel(null, updateTeam, fileService, dialogService, fTeamHomeLocation);
            AddSeasonCommand = new RelayCommand(ExecuteAddSeason);
            EditSeasonCommand = new RelayCommand<object[]>(ExecuteEditSeason);
            DeleteSeasonCommand = new RelayCommand(ExecuteDeleteSeason);
        }

        public ICommand AddSeasonCommand
        {
            get;
        }
        private void ExecuteAddSeason()
        {
            Action<DateTime, string> getName = (year, name) => UpdateTeam(team => team.AddSeason(year, name));
            fDialogService.DisplayCustomDialog(new CreateSeasonDialogViewModel(getName));
        }

        public ICommand EditSeasonCommand
        {
            get;
        }
        private void ExecuteEditSeason(object[] array)
        {
            if (SelectedSeason != null)
            {
                if (array.Length == 2)
                {
                    int year = int.Parse(array[0].ToString());
                    SelectedSeason.EditSeasonName(new DateTime(year, 1, 1), array[1].ToString());
                }
            }
        }

        public ICommand DeleteSeasonCommand
        {
            get;
        }
        private void ExecuteDeleteSeason()
        {
            if (SelectedSeason != null)
            {
                UpdateTeam(team => team.RemoveSeason(SelectedSeason.Year, SelectedSeason.Name));
            }
        }

        public override void UpdateData(ICricketTeam team)
        {
            Seasons = team.Seasons;
            DataStore = team;
            fTeamHomeLocation = team.HomeLocation;
            SelectedSeasonViewModel.UpdateSelected(SelectedSeason, team.HomeLocation);
            ;
        }
    }
}
