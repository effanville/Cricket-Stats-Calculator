using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Player;
using GUI.Dialogs.ViewModels;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class PlayerEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        private ICricketTeam teamHere;
        private List<ICricketPlayer> fPlayers;
        public List<ICricketPlayer> Players
        {
            get
            {
                return fPlayers;
            }
            set
            {
                fPlayers = value;
                OnPropertyChanged();
            }
        }
        private ICricketPlayer fSelectedPlayer;
        public ICricketPlayer SelectedPlayer
        {
            get
            {
                return fSelectedPlayer;
            }
            set
            {
                fSelectedPlayer = value;
                if (fSelectedPlayer != null)
                {
                    var name = fSelectedPlayer.Name.Copy();
                    SelectedPlayerName = name;
                }
                else
                {
                    SelectedPlayerName = new PlayerName("", "");
                }
                OnPropertyChanged();
            }
        }

        private PlayerName fSelectedPlayerName;

        public PlayerName SelectedPlayerName
        {
            get
            {
                return fSelectedPlayerName;
            }
            set
            {
                fSelectedPlayerName = value;
                OnPropertyChanged();
            }
        }

        public PlayerEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Player Edit")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Players = team.Players;
            teamHere = team;
            AddPlayerCommand = new RelayCommand(ExecuteAddPlayer);
            EditPlayerCommand = new RelayCommand<object[]>(ExecuteEditPlayer);
            DeletePlayerCommand = new RelayCommand(ExecuteDeletePlayer);

            AddFromTeamPlayerCommand = new RelayCommand(Execute);
        }
        public ICommand AddFromTeamPlayerCommand
        {
            get;
        }
        private void Execute()
        {
            foreach (var season in teamHere.Seasons)
            {
                foreach (var name in season.Players)
                {
                    UpdateTeam(team => team.AddPlayer(name));
                }
            }
        }

        public ICommand AddPlayerCommand
        {
            get;
        }
        private void ExecuteAddPlayer()
        {
            Action<PlayerName> getName = (name) => UpdateTeam(team => team.AddPlayer(name));
            fDialogService.DisplayCustomDialog(new CreatePlayerDialogViewModel(getName));
        }

        public ICommand EditPlayerCommand
        {
            get;
        }
        private void ExecuteEditPlayer(object[] array)
        {
            if (SelectedPlayer != null)
            {
                if (array.Length == 2)
                {
                    SelectedPlayer.EditName(array[0].ToString(), array[1].ToString());
                }
            }
        }

        public ICommand DeletePlayerCommand
        {
            get;
        }

        private void ExecuteDeletePlayer()
        {
            if (SelectedPlayer != null)
            {
                UpdateTeam(team => team.RemovePlayer(SelectedPlayer.Name));
            }
        }

        public override void UpdateData(ICricketTeam team)
        {
            Players = team.Players;
            teamHere = null;
            teamHere = team;
        }
    }
}
