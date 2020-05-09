using Cricket.Interfaces;
using Cricket.Match;
using System;
using System.Windows.Input;
using UICommon.Commands;
using UICommon.Interfaces;
using UICommon.ViewModelBases;

namespace GUI.Dialogs.ViewModels
{
    public class EditFieldingDialogViewModel : ViewModelBase<ICricketTeam>
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
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
        }

        public ICommand SubmitCommand { get; }
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
