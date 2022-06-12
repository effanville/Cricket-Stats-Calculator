using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;
using CSD.ViewModels.Dialogs;
using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Match.Innings;
using CSD.ViewModels.Match;

namespace CSD.ViewModels
{
    public class SelectedSeasonEditViewModel : ViewModelBase<ICricketTeam>
    {
        private string fTeamName;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        private ICricketSeason fSelectedSeason;

        private string fTeamHomeLocation;

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


        public SelectedSeasonEditViewModel(string teamName, ICricketSeason season, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService, string homeLocation)
            : base("Selected Season Edit")
        {
            fTeamHomeLocation = homeLocation;
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            SelectedSeason = season;

            MatchInfoVM = new MatchInfoViewModel(SelectedMatch?.MatchData, null);

            AddMatchCommand = new RelayCommand(ExecuteAddMatch);
            EditMatchCommand = new RelayCommand<object[]>(ExecuteEditMatch);
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

                Action<CricketInnings> updateBatting = (innings) => 
                {
                    UpdateTeam(team => 
                        team.GetSeason(SelectedSeason.Year, SelectedSeason.Name)
                            .GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.HomeTeam, SelectedMatch.MatchData.AwayTeam)
                            .SetInnings(innings, !isSecondInnings));
                };
                fDialogService.DisplayCustomDialog(new EditInningsDialogViewModel(updateBatting, innings));
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
                // TODO: Implement this properly
                UpdateTeam(team => team.GetSeason(SelectedSeason.Year, SelectedSeason.Name)
                    .GetMatch(SelectedMatch.MatchData.Date, SelectedMatch.MatchData.HomeTeam, SelectedMatch.MatchData.AwayTeam)
                    .EditInfo(array[0].ToString()));
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

        public void UpdateSelected(ICricketSeason selectedSeason, string homeLocation)
        {
            fTeamHomeLocation = homeLocation;
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
