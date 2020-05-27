using Cricket.Interfaces;
using System;
using System.Collections.Generic;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class TeamOverviewViewModel : ViewModelBase<ICricketTeam>
    {
        private string fTeamHome;
        public string TeamHome
        {
            get
            {
                return fTeamHome;
            }
            set
            {
                fTeamHome = value;
                fUpdateTeam(team =>
                {
                    if (team.HomeLocation != fTeamHome)
                    {
                        team.SetTeamHome(fTeamHome);
                    }
                });
                OnPropertyChanged(nameof(TeamHome));
            }
        }

        private string fTeamName;
        public string TeamName
        {
            get
            {
                return fTeamName;
            }
            set
            {
                fTeamName = value;
                fUpdateTeam(team =>
                {
                    if (team.TeamName != fTeamName)
                    {
                        team.SetTeamName(fTeamName);
                    }
                });
                OnPropertyChanged(nameof(TeamName));
            }
        }

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

        private readonly Action<Action<ICricketTeam>> fUpdateTeam;

        public TeamOverviewViewModel(Action<Action<ICricketTeam>> updateTeam, List<ICricketPlayer> players, List<ICricketSeason> seasons)
            : base("Team Overview")
        {
            fUpdateTeam = updateTeam;
            Players = players;
            Seasons = seasons;
        }

        public override void UpdateData(ICricketTeam team)
        {
            if (TeamName != team.TeamName)
            {
                TeamName = team.TeamName;
            }
            if (TeamHome != team.HomeLocation)
            {
                TeamHome = team.HomeLocation;
            }

            Seasons = team.Seasons;
            Players = team.Players;
        }
    }
}
