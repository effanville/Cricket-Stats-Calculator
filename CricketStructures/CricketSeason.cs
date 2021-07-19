﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace CricketStructures
{
    public class CricketSeason : ICricketSeason, IValidity
    {
        /// <inheritdoc/>
        public string Name
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public DateTime Year
        {
            get;
            set;
        }

        public List<CricketMatch> SeasonsMatches
        {
            get;
            set;
        } = new List<CricketMatch>();

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public IReadOnlyList<ICricketMatch> Matches => SeasonsMatches.Select(match => (ICricketMatch)match).ToList();

        /// <inheritdoc/>
        public SeasonGames CalculateGamesPlayed(MatchType[] matchTypes)
        {
            int gamesPlayed = 0;
            int numberWins = 0;
            int numberLosses = 0;
            int numberDraws = 0;
            int numberTies = 0;
            foreach (var match in Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    gamesPlayed++;

                    if (match.Result == ResultType.Win)
                    {
                        numberWins++;
                    }
                    if (match.Result == ResultType.Loss)
                    {
                        numberLosses++;
                    }
                    if (match.Result == ResultType.Draw)
                    {
                        numberDraws++;
                    }
                    if (match.Result == ResultType.Tie)
                    {
                        numberTies++;
                    }
                }
            }

            return new SeasonGames(gamesPlayed, numberWins, numberLosses, numberDraws, numberTies);
        }

        public event EventHandler PlayerAdded;

        private void OnPlayerAdded(object obj, EventArgs args)
        {
            PlayerAdded?.Invoke(obj, args);
        }

        public void SetupEventListening()
        {
            foreach (var match in SeasonsMatches)
            {
                match.PlayerAdded += OnPlayerAdded;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CricketSeason season)
            {
                if (string.IsNullOrEmpty(Name))
                {
                    if (string.IsNullOrEmpty(season.Name))
                    {
                        return Year.Equals(season.Year);
                    }

                    return false;
                }

                return Name.Equals(season.Name) && Year.Equals(season.Year);
            }

            return false;
        }

        /// <inheritdoc/>
        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            SeasonsMatches.ForEach(match => match.EditPlayerName(oldName, newName));
        }

        /// <inheritdoc/>
        public bool SameSeason(DateTime year, string name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Year.Equals(year);
                }

                return false;
            }

            return Year.Equals(year) && Name.Equals(name);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Year.Year.ToString() + " " + Name;
        }

        /// <inheritdoc/>
        public IReadOnlyList<PlayerName> Players(string teamName)
        {
            return SeasonsMatches.SelectMany(match => match.Players(teamName)).Distinct().ToList();
        }

        /// <inheritdoc/>
        public void EditSeasonName(DateTime year, string name)
        {
            Year = year;
            Name = name;
        }

        /// <inheritdoc/>
        /// This is currently not implemented.
        public ICricketMatch GetMatch(DateTime date, string homeTeam, string awayTeam)
        {
            if (ContainsMatch(date, homeTeam, awayTeam))
            {
                return SeasonsMatches.First(match => match.SameMatch(date, homeTeam, awayTeam));
            }

            return null;
        }

        /// <inheritdoc/>
        public bool AddMatch(MatchInfo info)
        {
            if (!ContainsMatch(info.Date, info.HomeTeam, info.AwayTeam))
            {
                var match = new CricketMatch(info);
                match.PlayerAdded += OnPlayerAdded;
                SeasonsMatches.Add(match);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool ContainsMatch(DateTime date, string homeTeam, string awayTeam)
        {
            return SeasonsMatches.Any(match => match.SameMatch(date, homeTeam, awayTeam));
        }

        /// <inheritdoc/>
        public bool RemoveMatch(DateTime date, string homeTeam, string awayTeam)
        {
            int removed = SeasonsMatches.RemoveAll(match => match.SameMatch(date, homeTeam, awayTeam));
            if (removed == 1)
            {
                return true;
            }
            if (removed == 0)
            {
                return false;
            }

            throw new Exception($"Had {removed} matches with info {date} and {homeTeam}-{awayTeam}, but should have at most 1.");
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            foreach (var match in SeasonsMatches)
            {
                results.AddValidations(match.Validation(), ToString());
            }
            if (Year.Equals(new DateTime()))
            {
                var yearNotSet = new ValidationResult
                {
                    IsValid = false,
                    PropertyName = nameof(Year)
                };
                yearNotSet.AddMessage($"{nameof(Year)} must be set.");
            }
            return results;
        }

        public CricketSeason()
        {
        }

        public CricketSeason(DateTime year, string name)
        {
            Name = name;
            Year = year;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
