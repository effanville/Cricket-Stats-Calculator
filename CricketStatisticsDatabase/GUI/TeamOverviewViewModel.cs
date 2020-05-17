using Cricket.Interfaces;
using System;
using System.Collections.Generic;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class TeamOverviewViewModel : ViewModelBase<ICricketTeam>
    {
        private List<ICricketPlayer> fPlayers;
        public List<ICricketPlayer> Players
        {
            get
            {
                return fPlayers;
            }
            set
            {
                fPlayers = value;
                OnPropertyChanged();
            }
        }

        private List<ICricketSeason> fSeasons;

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
            }
        }

        public TeamOverviewViewModel(Action<Action<ICricketTeam>> updateTeam, List<ICricketPlayer> players, List<ICricketSeason> seasons)
            : base("Team Overview")
        {
            Players = players;
            Seasons = seasons;
        }

        public override void UpdateData(ICricketTeam team)
        {
            Seasons = team.Seasons;
            Players = team.Players;
        }
    }
}
