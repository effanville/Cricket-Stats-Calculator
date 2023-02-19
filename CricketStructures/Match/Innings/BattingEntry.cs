﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System;

using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match.Innings
{
    /// <summary>
    /// Represents an entry on the batting part of a cricket scorecard.
    /// </summary>
    public sealed class BattingEntry : IValidity, IXmlSerializable, IEquatable<BattingEntry>
    {
        /// <summary>
        /// At what point in the innings did this batsman bat.
        /// This is the number the batsman went into bat.
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the player on this batting entry.
        /// </summary>
        public PlayerName Name
        {
            get;
            set;
        }

        /// <summary>
        /// How this batsman was out.
        /// </summary>
        public Wicket MethodOut
        {
            get;
            set;
        }

        /// <summary>
        /// Any possible fielder associated with the wicket.
        /// This could (and should) be null if not needed.
        /// </summary>
        public PlayerName Fielder
        {
            get;
            set;
        }

        /// <summary>
        /// Was the wicket taken by the keeper.
        /// </summary>
        public bool WasKeeper
        {
            get;
            set;
        }

        /// <summary>
        /// Any possible bowler associated with the wicket.
        /// This could (and should) be null if not needed.
        /// </summary>
        public PlayerName Bowler
        {
            get;
            set;
        }

        /// <summary>
        /// The runs the batsman scored.
        /// </summary>
        public int RunsScored
        {
            get;
            set;
        }

        /// <summary>
        /// The Wicket of the team this batsman fell at.
        /// </summary>
        public int WicketFellAt
        {
            get;
            set;
        }

        /// <summary>
        /// The number of runs the team had scored when the batsman was out.
        /// </summary>
        public int TeamScoreAtWicket
        {
            get;
            set;
        }

        /// <summary>
        /// Input the values into this batting entry.
        /// </summary>
        public void SetScores(Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null)
        {
            MethodOut = howOut;
            RunsScored = runs;
            Order = order;
            WicketFellAt = wicketFellAt;
            TeamScoreAtWicket = teamScoreAtWicket;
            Fielder = fielder;
            WasKeeper = wasKeeper;
            Bowler = bowler;
        }

        public BattingEntry(PlayerName name)
        {
            Name = name;
        }

        public BattingEntry()
        {
        }

        public override string ToString()
        {
            string fielderString = Fielder +(WasKeeper ? CricketConstants.WicketKeeperSymbol : "");
            return $"{Order}-{Name}-{MethodOut}-{fielderString}-{Bowler}-{RunsScored}-{WicketFellAt}-{TeamScoreAtWicket}";
        }

        /// <summary>
        /// Was this batsman out.
        /// </summary>
        public bool Out()
        {
            return MethodOut.IsOut();
        }

        /// <summary>
        /// Returns whether there are any errors with this entry.
        /// </summary>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <summary>
        /// Returns the errors with this entry.
        /// </summary>
        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(RunsScored, nameof(RunsScored), ToString()));
            if (MethodOut.DidNotBat())
            {
                results.AddIfNotNull(Validating.NotEqualTo(RunsScored, 0, nameof(RunsScored), ToString()));
            }

            if (MethodOut.IsBowlerWicket())
            {
                if (PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Bowler)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }
            else
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldntBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} should not be set with {MethodOut}.");
                    results.Add(bowlerShouldntBeSet);
                }
            }

            if (MethodOut.IsFielderWicket())
            {
                if (PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Fielder)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }
            else
            {
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult fielderShouldntBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} should not be set with {MethodOut}.");
                    results.Add(fielderShouldntBeSet);
                }
            }

            if (MethodOut.MustBeKeeper())
            {
                if (!WasKeeper)
                {
                    ValidationResult shouldBeKeeper = new ValidationResult(false, nameof(WasKeeper), ToString());
                    shouldBeKeeper.AddMessage($"{nameof(WasKeeper)} should be true with {MethodOut}.");
                    results.Add(shouldBeKeeper);
                }
            }

            return results;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXmlOld(XmlReader reader)
        {
            string order = reader.GetAttribute("O");

            reader.ReadStartElement("BattingEntry");

            string forename = reader.GetAttribute("F");
            string surname = reader.GetAttribute("S");
            reader.ReadStartElement("Name");

            reader.ReadStartElement("MethodOut");
            string methodOut = reader.ReadContentAsString();
            reader.ReadEndElement();

            string fielderforename = reader.GetAttribute("F");
            string fieldersurname = reader.GetAttribute("S");
            try
            {

                reader.ReadStartElement("Fielder");
            }
            catch (Exception)
            {
            }

            reader.ReadStartElement("WasKeeper");
            string wasKeeper = reader.ReadContentAsString();
            reader.ReadEndElement();

            string bowlerforename = reader.GetAttribute("F");
            string bowlersurname = reader.GetAttribute("S");
            try
            {

                reader.ReadStartElement("Bowler");
            }
            catch (System.Exception)
            {
            }
            reader.ReadStartElement("RunsScored");
            string runs = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("WicketFellAt");
            string wicket = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("TeamScoreAtWicket");
            string teamScore = reader.ReadContentAsString();
            reader.ReadEndElement();

            Order = int.Parse(order);
            Name = new PlayerName(surname, forename);

            MethodOut = (Wicket)Wicket.Parse(typeof(Wicket), methodOut);

            Fielder = new PlayerName(fieldersurname, fielderforename);

            WasKeeper = bool.Parse(wasKeeper);

            Bowler = new PlayerName(bowlersurname, bowlerforename);
            RunsScored = int.Parse(runs);
            WicketFellAt = int.Parse(wicket);
            TeamScoreAtWicket = int.Parse(teamScore);

            reader.ReadEndElement();
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            _ = reader.MoveToContent();
            string order = reader.GetAttribute("O");
            string name = reader.GetAttribute("N");
            string howOut = reader.GetAttribute("HO");
            string fielder = reader.GetAttribute("F");
            string wasKeeper = reader.GetAttribute("K");
            string bowler = reader.GetAttribute("B");
            string runs = reader.GetAttribute("R");
            string wicket = reader.GetAttribute("W");
            string teamScore = reader.GetAttribute("TS");

            try
            {
                Order = int.Parse(order);
                Name = PlayerName.FromString(name);
                MethodOut = (Wicket)Wicket.Parse(typeof(Wicket), howOut);
                Fielder = PlayerName.FromString(fielder);
                WasKeeper = bool.Parse(wasKeeper);
                Bowler = PlayerName.FromString(bowler);
                RunsScored = int.Parse(runs);
                WicketFellAt = int.Parse(wicket);
                TeamScoreAtWicket = int.Parse(teamScore);
            }
            catch (Exception)
            {
            }

            _ = reader.MoveToElement();
            reader.ReadStartElement();
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("O", Order.ToString());
            writer.WriteAttributeString("N", Name.ToString());
            writer.WriteAttributeString("HO", MethodOut.ToString());
            writer.WriteAttributeString("F", Fielder?.ToString() ?? " ");
            writer.WriteAttributeString("K", WasKeeper.ToString());
            writer.WriteAttributeString("B", Bowler?.ToString() ?? " ");
            writer.WriteAttributeString("R", RunsScored.ToString());
            writer.WriteAttributeString("W", WicketFellAt.ToString());
            writer.WriteAttributeString("TS", TeamScoreAtWicket.ToString());
        }

        /// <inheritdoc/>
        public bool Equals(BattingEntry other)
        {

            return Order.Equals(other?.Order)
                && (Name?.Equals(other?.Name) ?? other?.Name == null)
                && MethodOut.Equals(other?.MethodOut)
                && (Fielder?.Equals(other?.Fielder) ?? other?.Bowler == null)
                && WasKeeper.Equals(other?.WasKeeper)
                && (Bowler?.Equals(other?.Bowler) ?? other?.Bowler == null)
                && RunsScored.Equals(other?.RunsScored)
                && WicketFellAt.Equals(other?.WicketFellAt)
                && TeamScoreAtWicket.Equals(other?.TeamScoreAtWicket);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Order);
            hash.Add(Name);
            hash.Add(MethodOut);
            hash.Add(Fielder);
            hash.Add(WasKeeper);
            hash.Add(Bowler);
            hash.Add(RunsScored);
            hash.Add(WicketFellAt);
            hash.Add(TeamScoreAtWicket);
            return hash.ToHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as BattingEntry);
        }
    }
}
