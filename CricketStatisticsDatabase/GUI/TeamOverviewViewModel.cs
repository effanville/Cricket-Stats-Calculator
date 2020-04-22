using Cricket.Interfaces;
using GUISupport.ViewModels;
using System;
using System.Collections.Generic;

namespace GUI.ViewModels
{
    public class TeamOverviewViewModel : ViewModelBase
    {
        List<ICricketPlayer> fPlayers;
        public List<ICricketPlayer> Players
        {
            get { return fPlayers; }
            set { fPlayers = value; OnPropertyChanged(); }
        }

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
            set { fSelectedSeason = value; OnPropertyChanged(); }
        }

        public TeamOverviewViewModel(Action<Action<ICricketTeam>> updateTeam, List<ICricketPlayer> players, List<ICricketSeason> seasons)
            : base ("Team Overview")
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
