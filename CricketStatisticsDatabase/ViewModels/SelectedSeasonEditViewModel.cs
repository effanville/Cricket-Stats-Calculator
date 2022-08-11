using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common.UI.Commands;
using Common.UI.ViewModelBases;
using CSD.ViewModels.Dialogs;
using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Match.Innings;
using CSD.ViewModels.Match;
using Common.UI;

namespace CSD.ViewModels
{
    public class SelectedSeasonEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly UiGlobals fUiGlobals;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        private ICricketSeason fSelectedSeason;

        public ICricketSeason SelectedSeason
        {
            get => fSelectedSeason;
            set => SetAndNotify(ref fSelectedSeason, value, nameof(SelectedSeason));
        }

        private List<ICricketMatch> fSelectedMatches;
        public List<ICricketMatch> SelectedMatches
        {
            get => fSelectedMatches;
            set => SetAndNotify(ref fSelectedMatches, value, nameof(SelectedMatches));
        }

        private ICricketMatch fSelectedMatch;
        public ICricketMatch SelectedMatch
        {
            get => fSelectedMatch;
            set
            {
                SetAndNotify(ref fSelectedMatch, value, nameof(SelectedMatch));
                if (SelectedMatch != null)
                {
                    MatchInfoVM.UpdateData(fSelectedMatch.MatchData);
                }
            }
        }

        private MatchInfoViewModel fMatchInfoVM;
        public MatchInfoViewModel MatchInfoVM
        {
            get => fMatchInfoVM;
            set => SetAndNotify(ref fMatchInfoVM, value, nameof(MatchInfoVM));
        }


        public SelectedSeasonEditViewModel(ICricketSeason season, Action<Action<ICricketTeam>> updateTeam, UiGlobals uiGlobals)
            : base("Selected Season Edit")
        {
            fUiGlobals = uiGlobals;
            UpdateTeam = updateTeam;
            SelectedSeason = season;

            MatchInfoVM = new MatchInfoViewModel(SelectedMatch?.MatchData, null);

            AddMatchCommand = new RelayCommand(ExecuteAddMatch);
            DeleteMatchCommand = new RelayCommand(ExecuteDeleteMatch);
            EditInningsCommand = new RelayCommand<object>(ExecuteEditInnings);
        }

        public ICommand EditInningsCommand
        {
            get;
        }
        private void ExecuteEditInnings(object obj)
        {
            if (SelectedMatch != null)
            {
                CricketInnings innings;
                bool isSecondInnings = obj is string firstOrSecond && firstOrSecond.Equals("Second");
                if (isSecondInnings)
                {
                    innings = SelectedMatch.SecondInnings;
                }
                else
                {
                    innings = SelectedMatch.FirstInnings;
                }

                void updateBatting(CricketInnings innings)
                {
                    UpdateTeam(team =>
                        team.GetSeason(SelectedSeason.Year, SelectedSeason.Name)
                            .GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.HomeTeam, SelectedMatch.MatchData.AwayTeam)
                            .SetInnings(innings, !isSecondInnings));
                }

                fUiGlobals.DialogCreationService.DisplayCustomDialog(new EditInningsDialogViewModel(updateBatting, SelectedMatch.MatchData.HomeTeam, SelectedMatch.MatchData.AwayTeam, innings));
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
                void getName(MatchInfo info) => UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name)?.AddMatch(info));
                fUiGlobals.DialogCreationService.DisplayCustomDialog(new CreateMatchDialogViewModel(getName));
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
                UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name).RemoveMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.HomeTeam, SelectedMatch.MatchData.AwayTeam));
            }
        }

        public void UpdateSelected(ICricketSeason selectedSeason)
        {
            if (SelectedMatches != null)
            {
                int index = SelectedMatches.IndexOf(SelectedMatch);
                SelectedMatch = null;
                SelectedSeason = selectedSeason;
                SelectedMatches = selectedSeason?.Matches.ToList();
                if (index > 0 && index < SelectedMatches.Count)
                {
                    SelectedMatch = SelectedMatches[index];
                }
            }
            else
            {
                SelectedSeason = selectedSeason;
                SelectedMatches = selectedSeason?.Matches.ToList();
            }
        }
    }
}
