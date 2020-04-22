using Cricket.Interfaces;
using Cricket.Player;
using GUISupport;
using GUISupport.ViewModels;
using System;
using System.Windows.Input;

namespace GUI.Dialogs.ViewModels
{
    public class CreatePlayerDialogViewModel : ViewModelBase
    {
        private string fSurname;
        private string fFirstName;

        public string Surname
        {
            get { return fSurname; }
            set { fSurname = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get { return fFirstName; }
            set { fFirstName = value; OnPropertyChanged(); }
        }
        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(object obj)
        {
            ReportName(new PlayerName(Surname, FirstName));

            if (obj is ICloseable window)
            {
                window.Close();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
            throw new NotImplementedException();
        }

        private readonly Action<PlayerName> ReportName;

        public CreatePlayerDialogViewModel(Action<PlayerName> reportNameBack)
            : base("Create New Player")
        {
            ReportName = reportNameBack;
            SubmitCommand = new BasicCommand(ExecuteSubmitCommand);
        }
    }
}
