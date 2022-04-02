using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Common.UI.Commands;
using Common.UI.Interfaces;
using Common.UI.ViewModelBases;
using CricketStructures;
using CricketStructures.Player;

namespace CSD.ViewModels.Dialogs
{
    public class CreatePlayerDialogViewModel : ViewModelBase<ICricketTeam>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> fErrorsByPropertyName = new Dictionary<string, List<string>>();


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private string fSurname;
        private string fFirstName;

        public string Surname
        {
            get
            {
                return fSurname;
            }
            set
            {
                fSurname = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Forename
        {
            get
            {
                return fFirstName;
            }
            set
            {
                fFirstName = value;
                OnPropertyChanged();
                Validate();
            }
        }
        public ICommand SubmitCommand
        {
            get;
        }
        private void ExecuteSubmitCommand(ICloseable window)
        {
            if (!HasErrors)
            {
                ReportName(new PlayerName(Surname, Forename));
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
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
            Validate();
        }

        public bool HasErrors => fErrorsByPropertyName.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return fErrorsByPropertyName.SelectMany(pair => pair.Value);
            }
            else
            {
                return fErrorsByPropertyName.ContainsKey(propertyName) ? fErrorsByPropertyName[propertyName] : null;
            }
        }

        private void Validate()
        {
            ClearErrors(nameof(Surname));
            ClearErrors(nameof(Forename));
            var possible = new PlayerName(Surname, Forename);
            var validationResults = possible.Validation();
            foreach (var result in validationResults)
            {
                AddError(result.PropertyName, result.GetMessage());
            }
        }

        private void AddError(string propertyName, string error)
        {
            if (!fErrorsByPropertyName.ContainsKey(propertyName))
            {
                fErrorsByPropertyName[propertyName] = new List<string>();
            }

            if (!fErrorsByPropertyName[propertyName].Contains(error))
            {
                fErrorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (fErrorsByPropertyName.ContainsKey(propertyName))
            {
                _ = fErrorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
