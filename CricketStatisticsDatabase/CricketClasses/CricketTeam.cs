using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Cricket.Interfaces;
using Cricket.Player;
using StructureCommon.Validation;

namespace Cricket.Team
{
    public class CricketTeam : ICricketTeam, IValidity
    {
        private void OnPlayerAdded(object obj, EventArgs args)
        {
            if (obj is PlayerName name)
            {
                if (!ContainsPlayer(name))
                {
                    TeamPlayers.Add(new CricketPlayer(name));
                }
            }
        }

        public void SetupEventListening()
        {
            foreach (var season in TeamSeasons)
            {
                season.PlayerAdded += OnPlayerAdded;
                season.SetupEventListening();
            }
        }

        public override string ToString()
        {
            return TeamName;
        }

        public string TeamName
        {
            get;
            set;
        } = "MyCricketTeam";

        public void SetTeamName(string name)
        {
            TeamName = name;
        }

        public string HomeLocation
        {
            get;
            set;
        } = string.Empty;

        public void SetTeamHome(string home)
        {
            HomeLocation = home;
        }

        public List<CricketPlayer> TeamPlayers
        {
            get;
            set;
        } = new List<CricketPlayer>();

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public List<ICricketPlayer> Players
        {
            get
            {
                TeamPlayers.Sort((player1, player2) => player1.Name.CompareTo(player2.Name));
                return TeamPlayers.Select(player => (ICricketPlayer)player).ToList();
            }
        }

        public List<CricketSeason> TeamSeasons
        {
            get;
            set;
        } = new List<CricketSeason>();

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public List<ICricketSeason> Seasons
        {
            get
            {
                TeamSeasons.Sort((season1, season2) => season1.Year.CompareTo(season2.Year));
                return TeamSeasons.Select(season => (ICricketSeason)season).ToList();
            }
        }

        public CricketTeam()
        {
        }

        /// <inheritdoc/>
        public bool AddPlayer(PlayerName name)
        {
            if (!ContainsPlayer(name))
            {
                TeamPlayers.Add(new CricketPlayer(name));
                TeamPlayers.Sort((player1, player2) => player1.Name.CompareTo(player2.Name));
                return true;
            }

            return false;
        }

        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            TeamSeasons.ForEach(season => season.EditPlayerName(oldName, newName));
        }

        /// <inheritdoc/>
        public bool ContainsPlayer(PlayerName name)
        {
            return TeamPlayers.Any(player => player.Name.Equals(name));
        }

        /// <inheritdoc/>
        public ICricketPlayer GetPlayer(PlayerName name)
        {
            if (ContainsPlayer(name))
            {
                return TeamPlayers.First(player => player.Name.Equals(name));
            }

            return null;
        }

        /// <inheritdoc/>
        public bool RemovePlayer(PlayerName name)
        {
            int removed = TeamPlayers.RemoveAll(player => player.Name.Equals(name));
            if (removed == 1)
            {
                return true;
            }
            if (removed == 0)
            {
                return false;
            }

            throw new Exception($"Had {removed} players with name {name}, but should have at most 1.");
        }

        public bool AddSeason(DateTime year, string name)
        {
            if (!ContainsSeason(year, name))
            {
                var season = new CricketSeason(year, name);
                season.PlayerAdded += OnPlayerAdded;
                TeamSeasons.Add(season);
                TeamSeasons.Sort((season1, season2) => season1.Year.CompareTo(season2.Year));
                return true;
            }

            return false;
        }

        public bool ContainsSeason(DateTime year, string name)
        {
            return TeamSeasons.Any(season => season.SameSeason(year, name));
        }

        public ICricketSeason GetSeason(DateTime year, string name)
        {
            if (ContainsSeason(year, name))
            {
                return TeamSeasons.First(season => season.SameSeason(year, name));
            }

            return null;
        }

        public bool RemoveSeason(DateTime year, string name)
        {
            int removed = TeamSeasons.RemoveAll(season => season.SameSeason(year, name));
            if (removed == 1)
            {
                return true;
            }
            if (removed == 0)
            {
                return false;
            }

            throw new Exception($"Had {removed} seasons with year {year} and name {name}, but should have at most 1.");
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            foreach (var player in TeamPlayers)
            {
                results.AddRange(player.Validation());
            }
            foreach (var season in TeamSeasons)
            {
                results.AddRange(season.Validation());
            }

            return results;
        }
    }
}
