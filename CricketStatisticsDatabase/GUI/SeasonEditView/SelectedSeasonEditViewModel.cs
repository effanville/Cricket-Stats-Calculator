using Cricket.Interfaces;
using Cricket.Match;
using GUI.Dialogs.ViewModels;
using GUISupport;
using GUISupport.Services;
using GUISupport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class SelectedSeasonEditViewModel : ViewModelBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;

        ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get { return fSelectedSeason; }
            set { fSelectedSeason = value; OnPropertyChanged(nameof(SelectedSeason)); }
        }

        List<ICricketMatch> fSelectedMatches;
        public List<ICricketMatch> SelectedMatches
        {
            get { return fSelectedMatches; }
            set { fSelectedMatches = value; OnPropertyChanged(nameof(SelectedMatches)); }
        }

        private ICricketMatch fSelectedMatch;
        public ICricketMatch SelectedMatch
        {
            get { return fSelectedMatch; }
            set { fSelectedMatch = value; OnPropertyChanged(nameof(SelectedMatch)); SelectedBowling = value?.Bowling; SelectedBatting = value?.Batting; }
        }

        private BowlingInnings fSelectedBowling;
        public BowlingInnings SelectedBowling
        {
            get { return fSelectedBowling; }
            set { fSelectedBowling = value; OnPropertyChanged(nameof(SelectedBowling)); }
        }
        private BattingInnings fSelectedBatting;
        public BattingInnings SelectedBatting
        {
            get { return fSelectedBatting; }
            set { fSelectedBatting = value; OnPropertyChanged(nameof(SelectedBatting)); }
        }

        public List<MatchType> MatchTypes
        {
            get { return Enum.GetValues(typeof(MatchType)).Cast<MatchType>().ToList(); }
        }

        public List<ResultType> MatchResultTypes
        {
            get { return Enum.GetValues(typeof(ResultType)).Cast<ResultType>().ToList(); }
        }

        public SelectedSeasonEditViewModel(ICricketSeason season, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
    : base("Selected Season Edit")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            SelectedSeason = season;
            AddMatchCommand = new BasicCommand(ExecuteAddMatch);
            EditMatchCommand = new BasicCommand(ExecuteEditMatch);
            DeleteMatchCommand = new BasicCommand(ExecuteDeleteMatch);
            EditBattingCommand = new BasicCommand(ExecuteEditBatting);
            EditBowlingCommand = new BasicCommand(ExecuteEditBowling);
            EditFieldingCommand = new BasicCommand(ExecuteEditFielding);
        }

        public ICommand EditBattingCommand { get; }
        private void ExecuteEditBatting(object obj)
        {
            if (SelectedMatch != null)
            {
                Action<BattingInnings> updateBatting = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetBatting(innings));
                fDialogService.DisplayCustomDialog(new EditBattingDialogViewModel(updateBatting, SelectedMatch.Batting.Copy()));
            }
        }

        public ICommand EditBowlingCommand { get; }
        private void ExecuteEditBowling(object obj)
        {
            if (SelectedMatch != null)
            {
                Action<BowlingInnings> updateBowling = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetBowling(innings));
                fDialogService.DisplayCustomDialog(new EditBowlingDialogViewModel(updateBowling, SelectedMatch.Bowling.Copy()));
            }
        }


        public ICommand EditFieldingCommand { get; }
        private void ExecuteEditFielding(object obj)
        {
            if (SelectedMatch != null)
            {
                Action<Fielding> updateBatting = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetFielding(innings));
                fDialogService.DisplayCustomDialog(new EditFieldingDialogViewModel(updateBatting, SelectedMatch.FieldingStats.Copy()));
            }
        }

        public ICommand AddMatchCommand { get; }
        private void ExecuteAddMatch(object obj)
        {
            Action<MatchInfo> getName = (info) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).AddMatch(info));
            fDialogService.DisplayCustomDialog(new CreateMatchDialogViewModel(getName));
        }

        public ICommand EditMatchCommand { get; }
        private void ExecuteEditMatch(object obj)
        {
            if (SelectedSeason != null)
            {
                if (obj is object[] array)
                {
                    if (array.Length == 5)
                    {
                        var dateParse = DateTime.TryParse(array[1].ToString(), out DateTime dateResult);

                        var matchTypeParse = Enum.TryParse<MatchType>(array[3].ToString(), out MatchType resultMatch);
                        var resultTypeParse = Enum.TryParse<ResultType>(array[4].ToString(), out ResultType resultResult);
                        if (dateParse && matchTypeParse && resultTypeParse)
                        {
                            UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).EditInfo(array[0].ToString(), dateResult, array[2].ToString(), resultMatch, resultResult));
                        }
                    }
                }
            }
        }

        public ICommand DeleteMatchCommand { get; }
        private void ExecuteDeleteMatch(object obj)
        {
            if (SelectedMatch != null)
            {
                UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).RemoveMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition));
            }
        }

        public void UpdateSelected(ICricketSeason selectedSeason)
        {
            if (SelectedMatches != null)
            {
                int index = SelectedMatches.IndexOf(SelectedMatch);
                SelectedMatch = null;
                SelectedSeason = selectedSeason;
                SelectedMatches = selectedSeason?.Matches;
                if (index > 0)
                {
                    SelectedMatch = SelectedMatches[index];
                }
            }
            else
            {
                SelectedSeason = selectedSeason;
                SelectedMatches = selectedSeason?.Matches;
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
