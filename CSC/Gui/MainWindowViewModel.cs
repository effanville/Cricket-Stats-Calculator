using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CricketDatabaseEditing;

namespace CricketStatsCalc
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> fViewingData;

        public ObservableCollection<string> ViewingData
        {
            get { return fViewingData; }
            set
            {
                fViewingData = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> fPlayerList;

        public ObservableCollection<string> PlayerList
        {
            get { return fPlayerList; }
            set
            {
                fPlayerList = value;
                OnPropertyChanged();
            }
        }

        private ICommand fOnActivatedCommand;

        public ICommand OnActivatedCommand
        {
            get { return fOnActivatedCommand; }
            set { fOnActivatedCommand = value; }
        }

        public void Handle(object o)
        {
            //do something...
        }

        public MainWindowViewModel(List<string> MatchesToDisplay, List<string> PlayersToDisplay)
        {
            ViewingData = new ObservableCollection<string>(MatchesToDisplay);
            PlayerList = new ObservableCollection<string>(PlayersToDisplay);
            OnActivatedCommand = new BasicCommand(UpdateData);
        }

        public void UpdateData(object obj)
        {
           ViewingData = new ObservableCollection<string>(CricketDatabaseEditingFunctions.GetMatchesOppoDate());
           PlayerList = new ObservableCollection<string>(CricketDatabaseEditingFunctions.GetPlayers);
        }

        public MainWindowViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class BasicCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public BasicCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public BasicCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
