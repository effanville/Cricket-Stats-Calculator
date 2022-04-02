using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;

using CSD.ViewModels.Dialogs;

namespace CSD.ViewModels
{
    public sealed class PlayerEditViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;

        private List<ICricketPlayer> fPlayers;
        public List<ICricketPlayer> Players
        {
            get => fPlayers;

            set => SetAndNotify(ref fPlayers, value, nameof(Players));
        }

        private ICricketPlayer fSelectedPlayer;
        public ICricketPlayer SelectedPlayer
        {
            get => fSelectedPlayer;

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
            get => fSelectedPlayerName;

            set => SetAndNotify(ref fSelectedPlayerName, value, nameof(SelectedPlayerName));
        }

        public PlayerEditViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Player Edit", team)
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Players = team.Players.ToList();
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
            foreach (var season in DataStore.Seasons)
            {
                foreach (var name in season.Players(DataStore.TeamName))
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
                    var existingName = SelectedPlayer.Name.Copy();
                    SelectedPlayer.EditName(array[0].ToString(), array[1].ToString());


                    UpdateTeam(team => team.EditPlayerName(existingName, SelectedPlayer.Name));
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
            Players = team.Players.ToList();
            base.UpdateData(team);
        }
    }
}
