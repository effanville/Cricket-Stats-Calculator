using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Match;
using UICommon.Commands;
using UICommon.Interfaces;
using UICommon.ViewModelBases;

namespace GUI.Dialogs.ViewModels
{
    public class EditBattingDialogViewModel : ViewModelBase<ICricketTeam>
    {
        private Action<BattingInnings> UpdateInnings;

        private BattingInnings fInnings;
        public BattingInnings Innings
        {
            get
            {
                return fInnings;
            }
            set
            {
                fInnings = value;
                OnPropertyChanged(nameof(Innings));
            }
        }

        private List<BattingEntry> fInfo;
        public List<BattingEntry> Info
        {
            get
            {
                return fInfo;
            }
            set
            {
                fInfo = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        private BattingEntry fSelectedEntry;
        public BattingEntry SelectedEntry
        {
            get
            {
                return fSelectedEntry;
            }
            set
            {
                fSelectedEntry = value;
                OnPropertyChanged();
            }
        }

        public EditBattingDialogViewModel(Action<BattingInnings> updateInnings, BattingInnings innings)
            : base("Batting Innings Edit")
        {
            UpdateInnings = updateInnings;
            Innings = innings;
            Info = Innings.BattingInfo;
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
            MoveUpCommand = new RelayCommand(ExecuteMoveUp);
            MoveDownCommand = new RelayCommand(ExecuteMoveDown);
        }

        public ICommand MoveUpCommand
        {
            get;
        }
        private void ExecuteMoveUp()
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

        public ICommand MoveDownCommand
        {
            get;
        }
        private void ExecuteMoveDown()
        {
            var newList = new List<BattingEntry>();
            newList.AddRange(Info);
            int index = newList.IndexOf(SelectedEntry);
            if (index == Innings.BattingInfo.Count - 1)
            {
                return;
            }
            newList.RemoveAt(index);
            newList.Insert(index + 1, SelectedEntry);
            Info = newList;
        }

        public ICommand SubmitCommand
        {
            get;
        }
        private void ExecuteSubmitCommand(ICloseable window)
        {
            var newInnings = new BattingInnings
            {
                BattingInfo = Info,
                Extras = Innings.Extras
            };
            UpdateInnings(newInnings);
            window.Close();
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
