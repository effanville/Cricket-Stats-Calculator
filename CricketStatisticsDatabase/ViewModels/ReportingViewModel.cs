using System.Collections.Generic;
using System.Windows.Input;
using Common.Structure.Validation;
using Common.UI.Commands;
using Common.UI.ViewModelBases;
using CricketStructures;

namespace CSD.ViewModels
{
    public sealed class ReportingViewModel : ViewModelBase<ICricketTeam>
    {
        private ICricketTeam team;
        public ICricketTeam TeamToPlayWith
        {
            get
            {
                return team;
            }
            set
            {
                team = value;
                OnPropertyChanged();
            }
        }
        private List<ValidationResult> fValidations = new List<ValidationResult>();
        public List<ValidationResult> Validations
        {
            get
            {
                return fValidations;
            }
            set
            {
                fValidations = value;
                OnPropertyChanged();
            }
        }

        public ICommand ValidateCommand
        {
            get;
        }
        private void ExecuteValidateCommand()
        {
            if (TeamToPlayWith != null)
            {
                Validations = TeamToPlayWith.Validation();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
            TeamToPlayWith = portfolio;
            Validations = null;
            Validations = new List<ValidationResult>();
        }

        public ReportingViewModel(ICricketTeam team)
            : base("ReportingView")
        {
            TeamToPlayWith = team;
            ValidateCommand = new RelayCommand(ExecuteValidateCommand);
        }
    }
}
