using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using CSD.ViewModels.Dialogs;
using Common.UI.Commands;
using Common.UI.ViewModelBases;
using CricketStructures;
using CricketStructures.Season;
using Common.UI;

namespace CSD.ViewModels
{
    public class SeasonEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly UiGlobals fUiGlobals;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        private List<ICricketSeason> fSeasons;

        public List<ICricketSeason> Seasons
        {
            get => fSeasons;
            set => SetAndNotify(ref fSeasons, value, nameof(Seasons));
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get => fSelectedSeason;
            set
            {
                fSelectedSeason = value;
                OnPropertyChanged();
                SelectedSeasonViewModel.UpdateSelected(value);
            }
        }

        private SelectedSeasonEditViewModel fSelectedSeasonViewModel;
        public SelectedSeasonEditViewModel SelectedSeasonViewModel
        {
            get => fSelectedSeasonViewModel;
            set => SetAndNotify(ref fSelectedSeasonViewModel, value);
        }

        public SeasonEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, UiGlobals uiGlobals)
            : base("Season Edit", team)
        {
            fUiGlobals = uiGlobals;
            UpdateTeam = updateTeam;
            Seasons = team.Seasons.ToList();
            SelectedSeasonViewModel = new SelectedSeasonEditViewModel(null, updateTeam, fUiGlobals);
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
            void getName(DateTime year, string name) => UpdateTeam(team => team.AddSeason(year, name));
            fUiGlobals.DialogCreationService.DisplayCustomDialog(new CreateSeasonDialogViewModel(getName));
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
            Seasons = team.Seasons.ToList();
            DataStore = team;
            SelectedSeasonViewModel.UpdateSelected(SelectedSeason);
        }
    }
}
