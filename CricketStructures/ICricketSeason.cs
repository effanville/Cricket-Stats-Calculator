using System;
using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Interfaces
{
    public interface ICricketSeason
    {
        int GamesPlayed
        {
            get;
        }

        int NumberWins
        {
            get;
        }

        int NumberLosses
        {
            get;
        }

        int NumberDraws
        {
            get;
        }

        int NumberTies
        {
            get;
        }

        void CalculateGamesPlayed(MatchType[] matchTypes);

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
        /// The players that played in this season.
        /// </summary>
        List<PlayerName> Players(string teamName);

        /// <summary>
        /// All matches played in this season.
        /// </summary>
        List<ICricketMatch> Matches
        {
            get;
        }

        void EditPlayerName(PlayerName oldName, PlayerName newName);

        void EditSeasonName(DateTime year, string name);

        /// <summary>
        /// Queries whether the given data is the same as this seasons.
        /// </summary>
        bool SameSeason(DateTime year, string name);

        /// <summary>
        /// Queries whether the given object is the same as this season.
        /// </summary>
        bool Equals(object obj);

        /// <summary>
        /// Adds a match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool AddMatch(MatchInfo info);

        /// <summary>
        /// Returns the match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        ICricketMatch GetMatch(DateTime date, string homeTeam, string awayTeam);

        /// <summary>
        /// Returns whether the match with a given identifier exists.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool ContainsMatch(DateTime date, string homeTeam, string awayTeam);

        /// <summary>
        /// Removes the match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool RemoveMatch(DateTime date, string homeTeam, string awayTeam);
    }
}
