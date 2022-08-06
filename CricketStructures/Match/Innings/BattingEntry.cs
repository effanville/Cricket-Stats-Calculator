using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System;

namespace CricketStructures.Match.Innings
{
    /// <summary>
    /// Represents an entry on the batting part of a cricket scorecard.
    /// </summary>
    public class BattingEntry : IValidity, IXmlSerializable
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
            if (Name != null)
            {
                return "Batsman-" + Name.ToString();
            }

            return "Batsman: No Name";
        }

        /// <summary>
        /// Was this batsman out.
        /// </summary>
        public bool Out()
        {
            return !(MethodOut == Wicket.NotOut || MethodOut == Wicket.DidNotBat);
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
            if (MethodOut == Wicket.DidNotBat)
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldntBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} cannot be set with DidnotBat.");
                    results.Add(bowlerShouldntBeSet);
                }
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult fielderShouldntBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} cannot be set with DidnotBat.");
                    results.Add(fielderShouldntBeSet);
                }
                results.AddIfNotNull(Validating.NotEqualTo(RunsScored, 0, nameof(RunsScored), ToString()));
            }

            if (MethodOut == Wicket.Bowled || MethodOut == Wicket.LBW || MethodOut == Wicket.Caught || MethodOut == Wicket.HitWicket || MethodOut == Wicket.Stumped)
            {
                if (PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Bowler)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            if (MethodOut == Wicket.Caught || MethodOut == Wicket.RunOut || MethodOut == Wicket.Stumped)
            {
                if (PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Fielder)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            if (MethodOut == Wicket.Stumped)
            {
                if (!WasKeeper)
                {
                    ValidationResult shouldBeKeeper = new ValidationResult(false, nameof(WasKeeper), ToString());
                    shouldBeKeeper.AddMessage($"{nameof(WasKeeper)} should be true with {MethodOut}.");
                    results.Add(shouldBeKeeper);
                }
            }

            if (MethodOut == Wicket.RunOut || MethodOut == Wicket.NotOut)
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldntBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} should not be set with {MethodOut}.");
                    results.Add(bowlerShouldntBeSet);
                }
            }

            if (MethodOut == Wicket.Bowled || MethodOut == Wicket.NotOut || MethodOut == Wicket.HitWicket || MethodOut == Wicket.LBW)
            {
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult fielderShouldntBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} should not be set with {MethodOut}.");
                    results.Add(fielderShouldntBeSet);
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
    }
}
