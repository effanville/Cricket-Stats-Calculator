using Cricket.Interfaces;
using Cricket.Match;
using GUISupport;
using GUISupport.ViewModels;
using System;
using System.Windows.Input;

namespace GUI.Dialogs.ViewModels
{
    public class EditFieldingDialogViewModel : ViewModelBase
    {
        Action<Fielding> UpdateInnings;

        private Fielding fInnings;
        public Fielding Innings
        {
            get { return fInnings; }
            set { fInnings = value; OnPropertyChanged(); }
        }
        public EditFieldingDialogViewModel(Action<Fielding> updateInnings, Fielding innings)
               : base("Fielding Edit")
        {
            UpdateInnings = updateInnings;
            Innings = innings;
            SubmitCommand = new BasicCommand(ExecuteSubmitCommand);
        }

        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(object obj)
        {
            UpdateInnings(Innings);
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
