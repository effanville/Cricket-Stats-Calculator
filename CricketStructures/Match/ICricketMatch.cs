using System;
using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Match.Innings;

namespace CricketStructures.Interfaces
{
    public interface ICricketMatch
    {
        /// <summary>
        /// Ancillary data on which teams played and where.
        /// </summary>
        MatchInfo MatchData
        {
            get;
        }

        /// <summary>
        /// The result of the match.
        /// </summary>
        ResultType Result
        {
            get;
        }

        /// <summary>
        /// The MVPs of this match.
        /// </summary>
        PlayerName[] MenOfMatch
        {
            get;
        }

        /// <summary>
        /// The first innings of the match.
        /// </summary>
        CricketInnings FirstInnings
        {
            get;
        }

        /// <summary>
        /// The second innings of the match.
        /// </summary>
        CricketInnings SecondInnings
        {
            get;
        }

        /// <summary>
        /// The players for the team who played for the team specified.
        /// A null team defaults to a team specified by the atHome flag.
        /// </summary>
        List<PlayerName> Players(string team = null);

        /// <summary>
        /// Query to determine whether a player played or not for the team.
        /// </summary>
        bool PlayNotPlay(string team, PlayerName name);

        /// <summary>
        /// Alters a players name in the record.
        /// </summary>
        void EditPlayerName(PlayerName oldName, PlayerName newName);

        /// <summary>
        /// Edit the ancillary information for the match.
        /// </summary>
        void EditInfo(string homeTeam = null, string awayTeam = null, DateTime? date = null, string location = null, MatchType? typeOfMatch = null, ResultType? result = null);

        /// <summary>
        /// Edit the man of the match for the game.
        /// </summary>
        bool EditManOfMatch(PlayerName[] player);

        /// <summary>
        /// Get the innings for the team specified.
        /// </summary>
        CricketInnings GetInnings(string team, bool batting);

        /// <summary>
        /// Gets the score of the team specified.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        InningsScore Score(string team);

        /// <summary>
        /// Returns whether the team with name team batted first.
        /// TODO make sure this is correct.
        /// </summary>
        bool BattedFirst(string team);

        /// <summary>
        /// Get the batting for the player.
        /// </summary>
        BattingEntry GetBatting(string team, PlayerName player);

        /// <summary>
        /// Get the bowling for the player.
        /// </summary>
        BowlingEntry GetBowling(string team, PlayerName player);

        /// <summary>
        /// Get the fielding for the player.
        /// </summary>
        FieldingEntry GetFielding(string team, PlayerName player);

        IReadOnlyList<FieldingEntry> GetAllFielding(string team);

        void SetBatting(string team, PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null);

        bool DeleteBattingEntry(string team, PlayerName player);

        void SetBowling(string team, PlayerName player, double overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0);

        bool DeleteBowlingEntry(string team, PlayerName player);

        List<Partnership> Partnerships(string team = null);
    }
}
