using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using CricketStructures.Season;
using Common.Structure.Validation;

namespace CricketStructures
{
    public class CricketTeam : ICricketTeam, IValidity
    {
        /// <inheritdoc/>
        [XmlAttribute]
        public string TeamName
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlAttribute]
        public string HomeLocation
        {
            get;
            set;
        } = string.Empty;

        [XmlArray(Order = 1)]
        public List<CricketPlayer> TeamPlayers
        {
            get;
            set;
        } = new List<CricketPlayer>();

        [XmlArray(Order = 2)]
        public List<CricketSeason> TeamSeasons
        {
            get;
            set;
        } = new List<CricketSeason>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<ICricketSeason> Seasons => TeamSeasons.Select(season => (ICricketSeason)season).ToList();

        public CricketTeam()
        {
        }

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

        /// <inheritdoc/>
        public IReadOnlyList<ICricketPlayer> Players()
        {
            var playersCached = TeamPlayers.Select(player => (ICricketPlayer)player).ToList();
            foreach (var season in Seasons)
            {
                foreach (var name in season.Players(TeamName, MatchHelpers.AllMatchTypes))
                {
                    bool added = AddPlayer(name);
                    if (added)
                    {
                        playersCached.Add(new CricketPlayer(name));
                    }
                }
            }

            return playersCached;
        }

        /// <inheritdoc/>
        public bool AddPlayer(PlayerName name)
        {
            if (!ContainsPlayer(name))
            {
                TeamPlayers.Add(new CricketPlayer(name));
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

        public int RemoveSeason(DateTime year, string name)
        {
            return TeamSeasons.RemoveAll(season => season.SameSeason(year, name));
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
