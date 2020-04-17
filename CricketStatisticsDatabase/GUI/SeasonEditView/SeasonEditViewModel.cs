using Cricket.Interfaces;
using GUISupport.Services;
using GUISupport.ViewModels;
using System;

namespace GUI.ViewModels
{
    public class SeasonEditViewModel : ViewModelBase
    {
        public SeasonEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Season Edit")
        { }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }
    }
}
