using System;
using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Player;
using Common.Structure.Validation;

namespace CricketStructures.Season
{
    public interface ICricketSeason : IValidity
    {
        /// <summary>
        /// The name associated to this season.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// The year this season was held in.
        /// </summary>
        DateTime Year
        {
            get;
        }

        /// <summary>
        /// All matches played in this season.
        /// </summary>
        IReadOnlyList<ICricketMatch> Matches
        {
            get;
        }

        /// <summary>
        /// The players that played in this season.
        /// </summary>
        IReadOnlyList<PlayerName> Players(string teamName, MatchType[] matchTypes);

        /// <summary>
        /// Did the player play in this season.
        /// </summary>
        bool Played(string teamName, MatchType[] matchTypes, PlayerName player);

        /// <summary>
        /// Calculates what games have been played in the season.
        /// </summary>
        SeasonRecord CalculateGamesPlayed(MatchType[] matchTypes);

        /// <summary>
        /// Alters the player name in all matches in the season.
        /// </summary>
        void EditPlayerName(PlayerName oldName, PlayerName newName);

        /// <summary>
        /// Alters the information about the season.
        /// </summary>
        void EditSeasonName(DateTime year, string name);

        /// <summary>
        /// Queries whether the given data is the same as this seasons.
        /// </summary>
        bool SameSeason(DateTime year, string name);

        /// <summary>
        /// Adds a match in the list with a given identifier. Returns false if 
        /// a match with the same information already exists.
        /// </summary>
        bool AddMatch(MatchInfo info);

        /// <summary>
        /// Returns the match in the list with a given identifier.
        /// </summary>
        ICricketMatch GetMatch(DateTime date, string homeTeam, string awayTeam);

        /// <summary>
        /// Returns whether the match with a given identifier exists.
        /// </summary>
        bool ContainsMatch(DateTime date, string homeTeam, string awayTeam);

        /// <summary>
        /// Removes the match in the list with a given identifier.
        /// </summary>
        bool RemoveMatch(DateTime date, string homeTeam, string awayTeam);
    }
}
