using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Input;

using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.ReportWriting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics;
using CricketStructures.Statistics.Implementation.Collection;

namespace CSD.ViewModels
{
    public class StatsViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly UiGlobals fUiGlobals;
        private IFileInteractionService fFileService => fUiGlobals.FileInteractionService;

        public List<StatCollection> StatisticTypes => Enum.GetValues(typeof(StatCollection)).Cast<StatCollection>().ToList();

        private StatCollection fSelectedStatsType;
        public StatCollection SelectedStatsType
        {
            get => fSelectedStatsType;
            set
            {
                SelectedStats = null;
                fSelectedStatsType = value;
                OnPropertyChanged(nameof(SelectedStatsType));
                OnPropertyChanged(nameof(SeasonStatsSelected));

                var matchTypesToUse = MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray();

                SelectedStats = StatsCollectionBuilder.StandardStat(
                    value,
                    MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray(),
                    team: DataStore,
                    teamName: DataStore.TeamName,
                    season: SelectedSeason);
            }
        }

        private List<Selectable<MatchType>> fMatchTypeNames = new List<Selectable<MatchType>>();

        public List<Selectable<MatchType>> MatchTypeNames
        {
            get => fMatchTypeNames;
            set
            {
                fMatchTypeNames = value;
                OnPropertyChanged();
            }
        }

        public bool SeasonStatsSelected => SelectedStatsType == StatCollection.SeasonBrief;


        private bool fSeasonStatsSet;
        public bool SeasonStatsSet
        {
            get => fSeasonStatsSet;
            set
            {
                fSeasonStatsSet = value;
                OnPropertyChanged(nameof(SeasonStatsSet));
            }
        }

        private object fSelectedStats;
        public object SelectedStats
        {
            get => fSelectedStats;
            set
            {
                fSelectedStats = value;
                OnPropertyChanged(nameof(SelectedStats));
            }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get => fSelectedSeason;
            set
            {
                fSelectedSeason = value;
                OnPropertyChanged(nameof(SelectedSeason));
                SelectedStats = StatsCollectionBuilder.StandardStat(
                    StatCollection.SeasonBrief,
                    MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray(),
                    teamName: DataStore.TeamName,
                    season: value);
            }
        }

        private PlayerName fSelectedPlayer;
        public PlayerName SelectedPlayer
        {
            get => fSelectedPlayer;
            set
            {
                fSelectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private bool fPlayerStatsSet;
        public bool PlayerStatsSet
        {
            get => fPlayerStatsSet;
            set
            {
                fPlayerStatsSet = value;
                OnPropertyChanged(nameof(PlayerStatsSet));
            }
        }
        private PlayerBriefStatistics fSelectedPlayerStats;
        public PlayerBriefStatistics SelectedPlayerStats
        {
            get => fSelectedPlayerStats;
            set
            {
                fSelectedPlayerStats = value;
                OnPropertyChanged(nameof(SelectedPlayerStats));
                PlayerStatsSet = value == null ? false : true;
            }
        }

        public StatsViewModel(ICricketTeam team, UiGlobals uiGlobals)
            : base("Statistics", team)
        {
            fUiGlobals = uiGlobals;
            MatchTypeNames = new List<Selectable<MatchType>>();
            foreach (var name in MatchHelpers.AllMatchTypes)
            {
                MatchTypeNames.Add(new Selectable<MatchType>(name, true));
            }

            ExportStatsCommand = new RelayCommand(ExecuteExportStatsCommand);
        }

        public ICommand ExportStatsCommand
        {
            get;
        }

        private void ExecuteExportStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success)
            {
                var allTimeStats = StatsCollectionBuilder.StandardStat(
                    SelectedStatsType,
                    MatchTypeNames.Where(n => n.Selected).Select(name => name.Instance).ToArray(),
                    DataStore,
                    teamName: DataStore.TeamName,
                    season: SelectedSeason);
                string extension = new FileSystem().Path.GetExtension(gotFile.FilePath).Trim('.');
                DocumentType type = extension.ToEnum<DocumentType>();
                allTimeStats.ExportStats(new FileSystem(), gotFile.FilePath, type);
            }
        }

        public override void UpdateData(ICricketTeam cricketTeam)
        {
            base.UpdateData(cricketTeam);
            OnPropertyChanged();
        }
    }
}
