using System;
using System.Collections.Generic;

using Common.Structure.ReportWriting;

using CricketStructures.Match.Innings;
using CricketStructures.Match.Result;
using CricketStructures.Player;

namespace CricketStructures.Match
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
        List<PlayerName> MenOfMatch
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
        HashSet<PlayerName> Players(string team);

        /// <summary>
        /// Query to determine whether a player played or not for this team.
        /// </summary>
        bool Played(string team, PlayerName name);

        /// <summary>
        /// Alters a players name in the record.
        /// </summary>
        void EditPlayerName(PlayerName oldName, PlayerName newName);

        /// <summary>
        /// Edit the ancillary information for the match.
        /// </summary>
        void EditInfo(string homeTeam = null, string awayTeam = null, DateTime? date = null, string location = null, MatchType? typeOfMatch = null, ResultType? result = null);

        /// <summary>
        /// Sets the outcome of the toss as to who bats first. Uses the current home and 
        /// away team names.
        /// </summary>
        /// <param name="isHomeTeam">Whether the home team is batting first.</param>
        void SetBattingFirst(bool isHomeTeam);

        /// <summary>
        /// Get the innings for the team specified.
        /// </summary>
        CricketInnings GetInnings(string team, bool batting);

        void SetInnings(CricketInnings innings, bool first);

        /// <summary>
        /// Gets the score of the team specified.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        InningsScore Score(string team);

        /// <summary>
        /// The result of the match, detailing the winning margin.
        /// </summary>
        MatchResult MatchResult();

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

        /// <summary>
        /// Gets all fielding entries for the team specified.
        /// </summary>
        IReadOnlyList<FieldingEntry> GetAllFielding(string team);

        void SetBatting(string team, PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null);

        bool DeleteBattingEntry(string team, PlayerName player);

        void SetBowling(string team, PlayerName player, Over overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0);

        bool DeleteBowlingEntry(string team, PlayerName player);

        List<Partnership> Partnerships(string team = null);
        ReportBuilder SerializeToString(DocumentType exportType);
    }
}
