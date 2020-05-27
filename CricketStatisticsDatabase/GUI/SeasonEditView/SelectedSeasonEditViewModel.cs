using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Match;
using GUI.Dialogs.ViewModels;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class SelectedSeasonEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
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
                OnPropertyChanged(nameof(SelectedSeason));
            }
        }

        private List<ICricketMatch> fSelectedMatches;
        public List<ICricketMatch> SelectedMatches
        {
            get
            {
                return fSelectedMatches;
            }
            set
            {
                fSelectedMatches = value;
                OnPropertyChanged(nameof(SelectedMatches));
            }
        }

        private ICricketMatch fSelectedMatch;
        public ICricketMatch SelectedMatch
        {
            get
            {
                return fSelectedMatch;
            }
            set
            {
                fSelectedMatch = value;
                OnPropertyChanged(nameof(SelectedMatch));
                SelectedBowling = value?.Bowling;
                SelectedBatting = value?.Batting;
            }
        }

        private BowlingInnings fSelectedBowling;
        public BowlingInnings SelectedBowling
        {
            get
            {
                return fSelectedBowling;
            }
            set
            {
                fSelectedBowling = value;
                OnPropertyChanged(nameof(SelectedBowling));
            }
        }
        private BattingInnings fSelectedBatting;
        public BattingInnings SelectedBatting
        {
            get
            {
                return fSelectedBatting;
            }
            set
            {
                fSelectedBatting = value;
                OnPropertyChanged(nameof(SelectedBatting));
            }
        }

        public List<MatchType> MatchTypes
        {
            get
            {
                return Enum.GetValues(typeof(MatchType)).Cast<MatchType>().ToList();
            }
        }

        public List<ResultType> MatchResultTypes
        {
            get
            {
                return Enum.GetValues(typeof(ResultType)).Cast<ResultType>().ToList();
            }
        }

        public List<TeamInnings> InningsPlaceTypes
        {
            get
            {
                return Enum.GetValues(typeof(TeamInnings)).Cast<TeamInnings>().ToList();
            }
        }

        public SelectedSeasonEditViewModel(ICricketSeason season, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
    : base("Selected Season Edit")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            SelectedSeason = season;
            AddMatchCommand = new RelayCommand(ExecuteAddMatch);
            EditMatchCommand = new RelayCommand<object[]>(ExecuteEditMatch);
            DeleteMatchCommand = new RelayCommand(ExecuteDeleteMatch);
            EditBattingCommand = new RelayCommand(ExecuteEditBatting);
            EditBowlingCommand = new RelayCommand(ExecuteEditBowling);
            EditFieldingCommand = new RelayCommand(ExecuteEditFielding);
        }

        public ICommand EditBattingCommand
        {
            get;
        }
        private void ExecuteEditBatting()
        {
            if (SelectedMatch != null)
            {
                Action<BattingInnings> updateBatting = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetBatting(innings));
                fDialogService.DisplayCustomDialog(new EditBattingDialogViewModel(updateBatting, SelectedMatch.Batting.Copy()));
            }
        }

        public ICommand EditBowlingCommand
        {
            get;
        }
        private void ExecuteEditBowling()
        {
            if (SelectedMatch != null)
            {
                Action<BowlingInnings> updateBowling = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetBowling(innings));
                fDialogService.DisplayCustomDialog(new EditBowlingDialogViewModel(updateBowling, SelectedMatch.Bowling.Copy()));
            }
        }


        public ICommand EditFieldingCommand
        {
            get;
        }
        private void ExecuteEditFielding()
        {
            if (SelectedMatch != null)
            {
                Action<Fielding> updateBatting = (innings) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).SetFielding(innings));
                fDialogService.DisplayCustomDialog(new EditFieldingDialogViewModel(updateBatting, SelectedMatch.FieldingStats.Copy()));
            }
        }

        public ICommand AddMatchCommand
        {
            get;
        }
        private void ExecuteAddMatch()
        {
            if (SelectedSeason != null)
            {
                Action<MatchInfo> getName = (info) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name)?.AddMatch(info));
                fDialogService.DisplayCustomDialog(new CreateMatchDialogViewModel(getName));
            }
        }

        public ICommand EditMatchCommand
        {
            get;
        }
        private void ExecuteEditMatch(object[] array)
        {
            if (SelectedSeason != null)
            {
                if (array.Length == 6)
                {
                    bool dateParse = DateTime.TryParse(array[1].ToString(), out DateTime dateResult);

                    bool matchTypeParse = Enum.TryParse<MatchType>(array[3].ToString(), out MatchType resultMatch);
                    bool resultTypeParse = Enum.TryParse<ResultType>(array[4].ToString(), out ResultType resultResult);
                    bool inningsPlaceParse = Enum.TryParse<TeamInnings>(array[5].ToString(), out TeamInnings firstOrSecondResult);
                    string place = array[2].ToString();
                    if (dateParse && matchTypeParse && resultTypeParse && inningsPlaceParse)
                    {
                        UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.Opposition).EditInfo(array[0].ToString(), dateResult, place, place.Equals(team.HomeLocation) ? Location.Home : Location.Away,
                        resultMatch, resultResult, firstOrSecondResult));
                    }
                }
            }
        }

        public ICommand DeleteMatchCommand
        {
            get;
        }
        private void ExecuteDeleteMatch()
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
                if (index > 0 && index < SelectedMatches.Count)
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
    }
}
