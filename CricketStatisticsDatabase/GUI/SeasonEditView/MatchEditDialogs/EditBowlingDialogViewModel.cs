using Cricket.Interfaces;
using Cricket.Match;
using GUISupport;
using GUISupport.ViewModels;
using System;
using System.Windows.Input;

namespace GUI.Dialogs.ViewModels
{
    public class EditBowlingDialogViewModel : ViewModelBase
    {
        Action<BowlingInnings> UpdateInnings;

        private BowlingInnings fInnings;
        public BowlingInnings Innings
        {
            get { return fInnings; }
            set { fInnings = value; OnPropertyChanged(); }
        }

        public EditBowlingDialogViewModel(Action<BowlingInnings> updateInnings, BowlingInnings innings)
            : base("Bowling Innings Edit")
        {
            UpdateInnings = updateInnings;
            Innings = innings;
            SubmitCommand = new BasicCommand(ExecuteSubmitCommand);
        }

        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(object obj)
        {
            UpdateInnings(Innings);
            if(obj is ICloseable window)
            { 
                window.Close();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
