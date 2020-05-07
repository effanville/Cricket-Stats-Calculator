using Cricket.Interfaces;
using Cricket.Match;
using GUISupport;
using GUISupport.ViewModels;
using System;
using System.Windows.Input;
using System.Collections.Generic;

namespace GUI.Dialogs.ViewModels
{
    public class EditBattingDialogViewModel : ViewModelBase
    {
        Action<BattingInnings> UpdateInnings;

        private BattingInnings fInnings;
        public BattingInnings Innings
        {
            get { return fInnings; }
            set { fInnings = value; OnPropertyChanged(nameof(Innings)); }
        }

        private List<BattingEntry> fInfo;
        public List<BattingEntry> Info
        {
            get { return fInfo; }
            set { fInfo = value; OnPropertyChanged(nameof(Info)); }
        }

        private BattingEntry fSelectedEntry;
        public BattingEntry SelectedEntry
        {
            get { return fSelectedEntry; }
            set { fSelectedEntry = value; OnPropertyChanged(); }
        }

        public EditBattingDialogViewModel(Action<BattingInnings> updateInnings, BattingInnings innings)
            : base("Batting Innings Edit")
        {
            UpdateInnings = updateInnings;
            Innings = innings;
            Info = Innings.BattingInfo;
            SubmitCommand = new BasicCommand(ExecuteSubmitCommand);
            MoveUpCommand = new BasicCommand(ExecuteMoveUp);
            MoveDownCommand = new BasicCommand(ExecuteMoveDown);
        }

        public ICommand MoveUpCommand { get; }
        private void ExecuteMoveUp(object obj)
        {
            var newList = new List<BattingEntry>();
            newList.AddRange(Info);
            int index = newList.IndexOf(SelectedEntry);
            if (index == 0)
            {
                return;
            }
            newList.RemoveAt(index);
            newList.Insert(index - 1, SelectedEntry);
            Info = newList;
        }

        public ICommand MoveDownCommand { get; }
        private void ExecuteMoveDown(object obj)
        {
            var newList = new List<BattingEntry>();
            newList.AddRange(Info);
            int index = newList.IndexOf(SelectedEntry);
            if (index == Innings.BattingInfo.Count -1)
            {
                return;
            }
            newList.RemoveAt(index);
            newList.Insert(index + 1, SelectedEntry);
            Info = newList;
        }

        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(object obj)
        {
            var newInnings = new BattingInnings();
            newInnings.BattingInfo = Info;
            newInnings.Extras = Innings.Extras;
            UpdateInnings(newInnings);
            if (obj is ICloseable window)
            {
                window.Close();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
