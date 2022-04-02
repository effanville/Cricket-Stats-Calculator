using System;
using System.Collections.Generic;
using System.Linq;
using Common.UI.ViewModelBases;
using CricketStructures;
using CricketStructures.Player.Interfaces;
using CricketStructures.Season;

namespace CSD.ViewModels
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
                        team.HomeLocation = fTeamHome;
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
                        team.TeamName = fTeamName;
                    }
                });
                OnPropertyChanged(nameof(TeamName));
            }
        }

        private IReadOnlyList<ICricketPlayer> fPlayers;
        public IReadOnlyList<ICricketPlayer> Players
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

        private IReadOnlyList<ICricketSeason> fSeasons;

        public IReadOnlyList<ICricketSeason> Seasons
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

        public TeamOverviewViewModel(Action<Action<ICricketTeam>> updateTeam, IReadOnlyList<ICricketPlayer> players, IReadOnlyList<ICricketSeason> seasons)
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

            Seasons = team.Seasons.ToList();
            Players = team.Players.ToList();
        }
    }
}
