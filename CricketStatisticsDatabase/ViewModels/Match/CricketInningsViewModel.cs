using System;
using System.Collections.Generic;

using Common.UI.ViewModelBases;

using CricketStructures.Match.Innings;

namespace CSD.ViewModels.Match
{
    public sealed class CricketInningsViewModel : ViewModelBase<CricketInnings>
    {
        private readonly Action<CricketInnings> fUpdateInningsAction;
        private string fBattingTeam;
        private string fFieldingTeam;
        private List<BattingEntry> fBatting;
        private List<BowlingEntry> fBowling;
        private Extras fInningsExtras;

        public string BattingTeam
        {
            get => fBattingTeam;
            set => SetAndNotify(ref fBattingTeam, value, nameof(BattingTeam));
        }

        public string FieldingTeam
        {
            get => fFieldingTeam;
            set => SetAndNotify(ref fFieldingTeam, value, nameof(FieldingTeam));
        }

        public List<BattingEntry> Batting
        {
            get => fBatting;
            set => SetAndNotify(ref fBatting, value, nameof(Batting));
        }

        private BattingEntry fSelectedBatting;
        public BattingEntry SelectedBatting
        {
            get => fSelectedBatting;
            set => SetAndNotify(ref fSelectedBatting, value, nameof(SelectedBatting));
        }

        public List<BowlingEntry> Bowling
        {
            get => fBowling;
            set => SetAndNotify(ref fBowling, value, nameof(Bowling));
        }

        private BowlingEntry fSelectedBowling;
        public BowlingEntry SelectedBowling
        {
            get => fSelectedBowling;
            set => SetAndNotify(ref fSelectedBowling, value, nameof(SelectedBowling));
        }

        public Extras InningsExtras
        {
            get => fInningsExtras;
            set => SetAndNotify(ref fInningsExtras, value, nameof(InningsExtras));
        }

        public CricketInningsViewModel(CricketInnings database, Action<CricketInnings> updateInningsAction)
            : base("Cricket Innings", database)
        {
            fUpdateInningsAction = updateInningsAction;
            UpdateData(database);
        }

        public override void UpdateData(CricketInnings dataToDisplay)
        {
            base.UpdateData(dataToDisplay);
            if (dataToDisplay != null)
            {
                BattingTeam = dataToDisplay.BattingTeam;
                FieldingTeam = dataToDisplay.FieldingTeam;
                Batting = dataToDisplay.Batting;
                Bowling = dataToDisplay.Bowling;
                InningsExtras = dataToDisplay.InningsExtras;
            }
            else
            {
                Batting = new List<BattingEntry>();
                Bowling = new List<BowlingEntry>();
            }
        }

        public void ExecuteMoveBatsmanUp()
        {
            var newList = new List<BattingEntry>();
            newList.AddRange(Batting);
            int index = newList.IndexOf(SelectedBatting);
            if (index == 0)
            {
                return;
            }
            newList.RemoveAt(index);
            newList.Insert(index - 1, SelectedBatting);
            Batting = newList;
        }

        public void ExecuteMoveBatsmanDown()
        {
            var newList = new List<BattingEntry>();
            newList.AddRange(Batting);
            int index = newList.IndexOf(SelectedBatting);
            if (index == Batting.Count - 1)
            {
                return;
            }
            newList.RemoveAt(index);
            newList.Insert(index + 1, SelectedBatting);
            Batting = newList;
        }
    }
}
