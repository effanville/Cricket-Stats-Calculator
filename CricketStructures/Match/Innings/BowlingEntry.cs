﻿using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System;

namespace CricketStructures.Match.Innings
{
    public sealed class BowlingEntry : IValidity, IXmlSerializable, IEquatable<BowlingEntry>
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public Over OversBowled
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public int Wides
        {
            get;
            set;
        }

        public int NoBalls
        {
            get;
            set;
        }

        public BowlingEntry(PlayerName name)
        {
            Name = name;
        }

        public BowlingEntry()
        {
        }

        public override string ToString()
        {
            if (Name != null)
            {
                return "Bowler-" + Name.ToString();
            }

            return "Bowler: No name";
        }

        public void SetBowling(Over overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            OversBowled = overs;
            Maidens = maidens;
            RunsConceded = runsConceded;
            Wickets = wickets;
            Wides = wides;
            NoBalls = noBalls;
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = Name.Validation();
            results.AddIfNotNull(Validating.NotLessThan(OversBowled, Over.Min, nameof(OversBowled), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Maidens, nameof(Maidens), ToString()));
            results.AddIfNotNull(Validating.NotNegative(RunsConceded, nameof(RunsConceded), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Wickets, nameof(Wickets), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(Wickets, 10, nameof(Wickets), ToString()));
            return results;
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXmlOld(XmlReader reader)
        {
            reader.ReadStartElement("BowlingEntry");

            string forename = reader.GetAttribute("F");
            string surname = reader.GetAttribute("S");
            reader.ReadStartElement("Name");

            reader.ReadStartElement("OversBowled");
            string overs = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("Maidens");
            string maidens = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("RunsConceded");
            string runs = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("Wickets");
            string wicket = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("Wides");
            string wides = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("NoBalls");
            string nb = reader.ReadContentAsString();
            reader.ReadEndElement();

            OversBowled = (Over)double.Parse(overs);
            Name = new PlayerName(surname, forename);
            Maidens = int.Parse(maidens);
            RunsConceded = int.Parse(runs);
            Wickets = int.Parse(wicket);
            Wides = int.Parse(wides);
            NoBalls = int.Parse(nb);

            reader.ReadEndElement();
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            _ = reader.MoveToContent();
            string name = reader.GetAttribute("N");
            string overs = reader.GetAttribute("O");
            string m = reader.GetAttribute("M");
            string r = reader.GetAttribute("R");
            string w = reader.GetAttribute("W");
            string wd = reader.GetAttribute("WD");

            string nb = reader.GetAttribute("NB");
            try
            {
                Name = PlayerName.FromString(name);
                OversBowled = (Over)overs;
                Maidens = int.Parse(m);
                RunsConceded = int.Parse(r);
                Wickets = int.Parse(w);
                Wides = int.Parse(wd);
                NoBalls = int.Parse(nb);
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
            writer.WriteAttributeString("N", Name.ToString());
            writer.WriteAttributeString("O", OversBowled.ToString());
            writer.WriteAttributeString("M", Maidens.ToString());
            writer.WriteAttributeString("R", RunsConceded.ToString());
            writer.WriteAttributeString("W", Wickets.ToString());
            writer.WriteAttributeString("WD", Wides.ToString());
            writer.WriteAttributeString("NB", NoBalls.ToString());
        }

        /// <inheritdoc/>
        public bool Equals(BowlingEntry other)
        {
            return Name.Equals(other.Name)
                && OversBowled.Equals(other.OversBowled)
                && Maidens.Equals(other.Maidens)
                && RunsConceded.Equals(other.RunsConceded)
                && Wickets.Equals(other.Wickets)
                && Wides.Equals(other.Wides)
                && NoBalls.Equals(other.NoBalls);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as BowlingEntry);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, OversBowled, Maidens, RunsConceded, Wickets, Wides, NoBalls);
        }
    }
}
