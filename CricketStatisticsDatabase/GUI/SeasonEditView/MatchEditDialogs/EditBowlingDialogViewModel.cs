using System;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Match;
using Common.UI.Commands;
using Common.UI.Interfaces;
using Common.UI.ViewModelBases;

namespace GUI.Dialogs.ViewModels
{
    public class EditBowlingDialogViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly Action<BowlingInnings> UpdateInnings;

        private BowlingInnings fInnings;
        public BowlingInnings Innings
        {
            get
            {
                return fInnings;
            }
            set
            {
                fInnings = value;
                OnPropertyChanged();
            }
        }

        public EditBowlingDialogViewModel(Action<BowlingInnings> updateInnings, BowlingInnings innings)
            : base("Bowling Innings Edit")
        {
            UpdateInnings = updateInnings;
            Innings = innings;
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
        }

        public ICommand SubmitCommand
        {
            get;
        }
        private void ExecuteSubmitCommand(ICloseable window)
        {
            UpdateInnings(Innings);
            window.Close();
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
