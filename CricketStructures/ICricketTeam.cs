using System;
using System.Collections.Generic;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using StructureCommon.Validation;

namespace CricketStructures.Interfaces
{
    public interface ICricketTeam : IValidity
    {
        /// <summary>
        /// The name of the team in question.
        /// </summary>
        string TeamName
        {
            get;
            set;
        }

        /// <summary>
        /// The location of all home games.
        /// </summary>
        string HomeLocation
        {
            get;
            set;
        }

        /// <summary>
        /// The players associated to this team.
        /// </summary>
        IReadOnlyList<ICricketPlayer> Players
        {
            get;
        }

        /// <summary>
        /// The seasons for which this team has played games.
        /// </summary>
        IReadOnlyList<ICricketSeason> Seasons
        {
            get;
        }

        /// <summary>
        /// Adds a player to the teams player list.
        /// </summary>
        bool AddPlayer(PlayerName name);

        /// <summary>
        /// Alters the player name in all matches in every season.
        /// </summary>
        void EditPlayerName(PlayerName oldName, PlayerName newName);

        /// <summary>
        /// Queries whether a player with the specified name is a member of the team.
        /// </summary>
        bool ContainsPlayer(PlayerName name);

        /// <summary>
        /// Returns a player with the name desired, or null if the player doesnt exist.
        /// </summary>
        ICricketPlayer GetPlayer(PlayerName name);

        /// <summary>
        /// Removes the player with the specified name.
        /// </summary>
        /// <exception cref="Exception"> Thrown if removes too many players.</exception>
        bool RemovePlayer(PlayerName name);

        /// <summary>
        /// Adds a season to the teams season list.
        /// </summary>
        bool AddSeason(DateTime year, string name);

        /// <summary>
        /// Queries whether a season with the specified name is a member of the team.
        /// </summary>
        bool ContainsSeason(DateTime year, string name);

        /// <summary>
        /// Returns a season with the name desired, or null if the season doesnt exist.
        /// </summary>
        ICricketSeason GetSeason(DateTime year, string name);

        /// <summary>
        /// Removes the season with the specified name and year.
        /// </summary>
        int RemoveSeason(DateTime year, string name);
    }
}
