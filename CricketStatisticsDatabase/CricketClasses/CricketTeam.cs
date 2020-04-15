﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Team
{
    public class CricketTeam : ICricketTeam
    {
        public List<CricketPlayer> TeamPlayers
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<ICricketPlayer> Players
        {
            get 
            { 
                return TeamPlayers.Select(player => (ICricketPlayer)player).ToList(); 
            }
        }

        public List<CricketSeason> TeamSeasons
        {
            get; 
            set; 
        }

        /// <inheritdoc/>
        public List<ICricketSeason> Seasons
        {
            get 
            { 
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
                return true;
            }

            return false;
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
                TeamSeasons.Add(new CricketSeason(year, name));
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

            throw new Exception($"Had {removed} seasons with name {name}, but should have at most 1.");
        }
    }
}