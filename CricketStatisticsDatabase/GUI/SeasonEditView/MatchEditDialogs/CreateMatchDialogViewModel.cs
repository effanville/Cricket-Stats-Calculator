﻿using Cricket.Interfaces;
using Cricket.Match;
using GUISupport;
using GUISupport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GUI.Dialogs.ViewModels
{
    public class CreateMatchDialogViewModel :ViewModelBase
    {
        private string fOpposition;
        public string Opposition
        {
            get
            {
                return fOpposition;
            }
            set
            {
                fOpposition = value;
                OnPropertyChanged();
            }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(); }
        }

        private string fPlace;
        public string Place
        {
            get { return fPlace; }
            set { fPlace = value; OnPropertyChanged(); }
        }

        private MatchType fType;
        public MatchType Type
        {
            get { return fType; }
            set { fType = value; OnPropertyChanged(nameof(Type)); }
        }

        public List<MatchType> MatchTypes
        {
            get
            {
                return Enum.GetValues(typeof(MatchType)).Cast<MatchType>().ToList();
            }
        }

        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(object obj)
        {
            bool dateParse = DateTime.TryParse(date, out DateTime result);
            if (dateParse)
            {
                var info = new MatchInfo(Opposition, result, Place, Type);
                AddMatch(info);

                if (obj is ICloseable window)
                {
                    window.Close();
                }
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }

        private readonly Action<MatchInfo> AddMatch;
        public CreateMatchDialogViewModel(Action<MatchInfo> addMatch) : base("Create New Match")
        {
            AddMatch = addMatch;
            SubmitCommand = new BasicCommand(ExecuteSubmitCommand);
        }
    }
}