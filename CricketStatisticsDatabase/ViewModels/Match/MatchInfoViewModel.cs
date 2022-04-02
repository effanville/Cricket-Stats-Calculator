using System;
using System.ComponentModel;

using Common.UI.ViewModelBases;

using CricketStructures.Match;

namespace CSD.ViewModels.Match
{
    public sealed class MatchInfoViewModel : ViewModelBase<MatchInfo>
    {
        private readonly Action<MatchInfo> fUpdateModel;
        private string fHomeTeam;
        private string fAwayTeam;
        private string fLocation;
        private DateTime fDate;
        private MatchType fMatchType;

        public string HomeTeam
        {
            get => fHomeTeam;
            set => SetAndNotify(ref fHomeTeam, value);
        }

        public string AwayTeam
        {
            get => fAwayTeam;
            set => SetAndNotify(ref fAwayTeam, value);
        }

        public string Location
        {
            get => fLocation;
            set => SetAndNotify(ref fLocation, value);
        }

        public DateTime Date
        {
            get => fDate;
            set => SetAndNotify(ref fDate, value);
        }

        public MatchType Type
        {
            get => fMatchType;
            set => SetAndNotify(ref fMatchType, value, nameof(Type));
        }

        public MatchInfoViewModel(MatchInfo matchInfo, Action<MatchInfo> UpdateModel)
            : base("MatchInfoEditor", matchInfo)
        {
            fUpdateModel = UpdateModel;
            PropertyChanged += EnactUpdateMatchInfoAction;
            HomeTeam = matchInfo?.HomeTeam;
            AwayTeam = matchInfo?.AwayTeam;
            Location = matchInfo?.Location;
            Date = matchInfo?.Date ?? DateTime.Today;
            Type = matchInfo?.Type ?? MatchType.League;
        }

        private void EnactUpdateMatchInfoAction(object sender, PropertyChangedEventArgs e)
        {
            if (DataStore != null)
            {

                if (e.PropertyName == nameof(HomeTeam))
                {
                    DataStore.HomeTeam = HomeTeam;
                }
                if (e.PropertyName == nameof(AwayTeam))
                {
                    DataStore.AwayTeam = AwayTeam;
                }
                if (e.PropertyName == nameof(Location))
                {
                    DataStore.Location = Location;
                }
                if (e.PropertyName == nameof(Date))
                {
                    DataStore.Date = Date;
                }
                if (e.PropertyName == nameof(Type))
                {
                    DataStore.Type = Type;
                }

                fUpdateModel?.Invoke(DataStore);
            }
        }

        public override void UpdateData(MatchInfo matchInfo)
        {
            base.UpdateData(matchInfo);
            HomeTeam = matchInfo.HomeTeam;
            AwayTeam = matchInfo.AwayTeam;
            Location = matchInfo.Location;
            Date = matchInfo.Date;
            Type = matchInfo.Type;
        }
    }
}
