using System;
using System.Windows.Input;
using CricketStructures.Match;
using Common.UI.Commands;
using Common.UI.Interfaces;
using Common.UI.ViewModelBases;
using CSD.ViewModels.Match;

namespace CSD.ViewModels.Dialogs
{
    public sealed class CreateMatchDialogViewModel : ViewModelBase<MatchInfo>
    {
        private MatchInfo fMatchInfo;

        private MatchInfoViewModel fMatchInfoVM;
        public MatchInfoViewModel MatchInfoVM
        {
            get => fMatchInfoVM;
            set => SetAndNotify(ref fMatchInfoVM, value, nameof(MatchInfoVM));
        }

        private readonly Action<MatchInfo> AddMatch;
        public CreateMatchDialogViewModel(Action<MatchInfo> addMatch)
            : base("Create New Match")
        {
            AddMatch = addMatch;
            MatchInfoVM = new MatchInfoViewModel(new MatchInfo(), UpdateMatchInfo);
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
        }

        public ICommand SubmitCommand
        {
            get;
        }

        private void ExecuteSubmitCommand(ICloseable window)
        {
            AddMatch(fMatchInfo);
            window.Close();
        }

        private void UpdateMatchInfo(MatchInfo matchInfo)
        {
            fMatchInfo = matchInfo;
        }
    }
}
