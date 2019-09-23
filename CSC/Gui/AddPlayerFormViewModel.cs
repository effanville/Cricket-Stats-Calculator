using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cricket;
using ReportingStructures;
using CricketDatabaseEditing;
using Command;
using System.Windows;

namespace CricketStatsCalc
{
    class AddPlayerFormViewModel : INotifyPropertyChanged
    {
        public RelayCommand<Window> AddThenCloseWindowCommand { get; private set; }

        private void AddthenCloseWindow(Window window)
        {
            if (NewName != String.Empty)
            {
                if (CricketDatabaseEditingFunctions.AddPlayer(NewName))
                {
                    if (window != null)
                    {
                        CloseWindowAction();
                    }
                }
            }
            else
            {
                ErrorReports.AddError("User has not specified a name for new player.");
            }

            if (ErrorReports.GetErrors().Count != 0)
            {
                ShowErrorsAction();
            }
        }

        private Action CloseWindowAction;

        private Action ShowErrorsAction;

        private string newName;

        public string NewName
        {
            get
            {
                return newName;
            }
            set
            {
                newName = value;
                OnPropertyChanged();
            }
        }

        public AddPlayerFormViewModel(Action CloseWindow, Action ShowErrors)
        {
            CloseWindowAction = CloseWindow;
            ShowErrorsAction = ShowErrors;

            NewName = String.Empty;
            AddThenCloseWindowCommand = new RelayCommand<Window>(this.AddthenCloseWindow);
        }


        public AddPlayerFormViewModel()
        { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
