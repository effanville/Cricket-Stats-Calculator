using Cricket.Interfaces;
using GUISupport;
using GUISupport.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using Validation;

namespace GUI.ViewModels
{
    public class ReportingViewModel : ViewModelBase
    {
        private ICricketTeam team;
        public ICricketTeam TeamToPlayWith
        {
            get { return team; }
            set {team = value; OnPropertyChanged(); }
        }
        private List<ValidationResult> fValidations = new List<ValidationResult>();
        public List<ValidationResult> Validations
        {
            get { return fValidations; }
            set { fValidations = value; OnPropertyChanged(); }
        }

        public ICommand ValidateCommand { get; }
        private void ExecuteValidateCommand(object obj)
        {
            if (TeamToPlayWith != null)
            {
                Validations = TeamToPlayWith.Validation();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
            TeamToPlayWith = portfolio;
        }

        public ReportingViewModel(ICricketTeam team)
            : base("ReportingView")
        {
            TeamToPlayWith = team;
            ValidateCommand = new BasicCommand(ExecuteValidateCommand);
        }
    }
}
