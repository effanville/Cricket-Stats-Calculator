using Cricket.Interfaces;
using GUI.Dialogs.ViewModels;
using GUISupport;
using GUISupport.Services;
using GUISupport.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class SeasonEditViewModel : ViewModelBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;

        List<ICricketSeason> fSeasons;
        public List<ICricketSeason> Seasons
        {
            get { return fSeasons; }
            set { fSeasons = value; OnPropertyChanged(); }
        }

        ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get { return fSelectedSeason; }
            set { fSelectedSeason = value; OnPropertyChanged(); SelectedSeasonViewModel.UpdateSelected(value); }
        }

        private SelectedSeasonEditViewModel fSelectedSeasonViewModel;
        public SelectedSeasonEditViewModel SelectedSeasonViewModel
        {
            get { return fSelectedSeasonViewModel; }
            set { fSelectedSeasonViewModel = value; OnPropertyChanged(); }
        }

        public SeasonEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Season Edit")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Seasons = team.Seasons;
            SelectedSeasonViewModel = new SelectedSeasonEditViewModel(null, updateTeam, fileService, dialogService);
            AddSeasonCommand = new BasicCommand(ExecuteAddSeason);
            EditSeasonCommand = new BasicCommand(ExecuteEditSeason);
            DeleteSeasonCommand = new BasicCommand(ExecuteDeleteSeason);
        }

        public ICommand AddSeasonCommand { get; }
        private void ExecuteAddSeason(object obj)
        {
            Action<DateTime, string> getName = (year, name) => UpdateTeam(team => team.AddSeason(year, name));
            fDialogService.DisplayCustomDialog(new CreateSeasonDialogViewModel(getName));
        }

        public ICommand EditSeasonCommand { get; }
        private void ExecuteEditSeason(object obj)
        {
            if (SelectedSeason != null)
            {
                if (obj is object[] array)
                {
                    if (array.Length == 2)
                    {
                        var date = DateTime.Parse(array[0].ToString());
                        SelectedSeason.EditSeasonName(date, array[1].ToString());
                    }
                }
            }
        }

        public ICommand DeleteSeasonCommand { get; }
        private void ExecuteDeleteSeason(object obj)
        {
            if (SelectedSeason != null)
            {
                UpdateTeam(team => team.RemoveSeason(SelectedSeason.Year, SelectedSeason.Name));
            }
        }

        public override void UpdateData(ICricketTeam team)
        {
            Seasons = team.Seasons;
            SelectedSeasonViewModel.UpdateSelected(SelectedSeason);
        }
    }
}
