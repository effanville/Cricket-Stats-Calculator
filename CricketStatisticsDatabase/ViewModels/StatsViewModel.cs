using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.ReportWriting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures.Player;

using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Statistics;

namespace CSD.ViewModels
{
    public class StatsViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly UiGlobals fUiGlobals;
        private IFileInteractionService fFileService => fUiGlobals.FileInteractionService;

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

                if (SelectedSeason != null)
                {
                    SelectedStats = StatsCollectionBuilder.StandardStat(
                        value,
                        MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray(),
                        team: DataStore,
                        teamName: DataStore.TeamName,
                        season: SelectedSeason);
                }
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

        public bool SeasonStatsSelected => SelectedStatsType.IsSeasonStat();


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

        public StatsViewModel(ICricketTeam team, UiGlobals uiGlobals)
            : base("Statistics", team)
        {
            fUiGlobals = uiGlobals;
            MatchTypeNames = new List<Selectable<MatchType>>();
            foreach (var name in MatchHelpers.AllMatchTypes)
            {
                MatchTypeNames.Add(new Selectable<MatchType>(name, false));
            }

            ExportStatsCommand = new RelayCommand(ExecuteExportStatsCommand);
        }

        public ICommand ExportStatsCommand
        {
            get;
        }

        private void ExecuteExportStatsCommand()
        {
            if (SelectedStatsType.IsPlayerStat())
            {
                SaveAllPlayerStats();
            }
            else
            {
                FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
                if (gotFile.Success)
                {
                    var allTimeStats = StatsCollectionBuilder.StandardStat(
                        SelectedStatsType,
                        MatchTypeNames.Where(n => n.Selected).Select(name => name.Instance).ToArray(),
                        team: DataStore,
                        teamName: DataStore.TeamName,
                        season: SelectedSeason);
                    string extension = fUiGlobals.CurrentFileSystem.Path.GetExtension(gotFile.FilePath).Trim('.');
                    DocumentType type = extension.ToEnum<DocumentType>();
                    allTimeStats.ExportStats(fUiGlobals.CurrentFileSystem, gotFile.FilePath, type);
                }
            }
        }

        private void SaveAllPlayerStats()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("", "", filter: "Html Files|*.html|All Files|*.*");
            if (gotFile.Success)
            {
                string extension = fUiGlobals.CurrentFileSystem.Path.GetExtension(gotFile.FilePath).Trim('.');
                string location = fUiGlobals.CurrentFileSystem.Path.GetDirectoryName(gotFile.FilePath);
                var matchTypes = MatchTypeNames.Where(n => n.Selected).Select(name => name.Instance).ToArray();
                IReadOnlyList<PlayerName> players = SelectedStatsType == StatCollection.PlayerBrief
                    ? DataStore.Players().Select(player => player.Name).ToList()
                    : SelectedStatsType == StatCollection.PlayerSeason
                        ? SelectedSeason.Players(DataStore.TeamName, matchTypes)
                        : new List<PlayerName>();

                DocumentType type = DocumentType.Html;
                foreach (var playerName in players)
                {
                    string filePath = $"{location}\\{playerName}.{type}";
                    var allTimeStats = StatsCollectionBuilder.StandardStat(
                        SelectedStatsType,
                        matchTypes,
                        team: DataStore,
                        teamName: DataStore.TeamName,
                        season: SelectedSeason,
                        playerName: playerName);
                    allTimeStats.ExportStats(fUiGlobals.CurrentFileSystem, filePath, type);

                }

            }
        }

        public override void UpdateData(ICricketTeam cricketTeam)
        {
            base.UpdateData(cricketTeam);
            OnPropertyChanged();
        }
    }
}
