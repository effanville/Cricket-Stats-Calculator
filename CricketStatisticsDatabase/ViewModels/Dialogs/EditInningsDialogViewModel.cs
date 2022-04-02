using System;
using System.Windows.Input;
using CricketStructures;
using CricketStructures.Match.Innings;
using Common.UI.Commands;
using Common.UI.Interfaces;
using Common.UI.ViewModelBases;
using CSD.ViewModels.Match;

namespace CSD.ViewModels.Dialogs
{
    public class EditInningsDialogViewModel : ViewModelBase<ICricketTeam>
    {
        private CricketInnings fCricketInnings;
        private Action<CricketInnings> UpdateInnings;

        private CricketInningsViewModel fInnings;
        public CricketInningsViewModel Innings
        {
            get => fInnings;
            set => SetAndNotify(ref fInnings, value, nameof(Innings));
        }

        public EditInningsDialogViewModel(Action<CricketInnings> updateInnings, CricketInnings innings)
            : base("Batting Innings Edit")
        {
            fCricketInnings = innings;
            UpdateInnings = updateInnings;
            Innings = new CricketInningsViewModel(innings, UpdateCricketInnings);
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
            Innings.ExecuteMoveBatsmanUp();
        }

        public ICommand MoveDownCommand
        {
            get;
        }

        private void ExecuteMoveDown()
        {
            Innings.ExecuteMoveBatsmanDown();
        }

        public ICommand SubmitCommand
        {
            get;
        }

        private void ExecuteSubmitCommand(ICloseable window)
        {
            UpdateInnings(fCricketInnings);
            window.Close();
        }

        private void UpdateCricketInnings(CricketInnings innings)
        {
            fCricketInnings = innings;
        }
    }
}
