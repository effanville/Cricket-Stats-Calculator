using Cricket.Team;
using GUISupport.Services;

namespace GUI.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;

        internal CricketTeam TeamToPlayWith;

        public MainWindowViewModel(IFileInteractionService fileService, IDialogCreationService dialogService)
        {
            fFileService = fileService;
            fDialogService = dialogService;
            TeamToPlayWith = new CricketTeam();
        }
    }
}
