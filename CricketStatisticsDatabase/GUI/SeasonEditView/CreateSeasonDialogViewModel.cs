using Cricket.Interfaces;
using System;
using System.Windows.Input;
using UICommon.Commands;
using UICommon.Interfaces;
using UICommon.ViewModelBases;

namespace GUI.Dialogs.ViewModels
{
    public class CreateSeasonDialogViewModel : ViewModelBase<ICricketTeam>
    {
        private string fYear;
        private string fName;

        public string Year
        {
            get { return fYear; }
            set { fYear = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return fName; }
            set { fName = value; OnPropertyChanged(); }
        }
        public ICommand SubmitCommand { get; }
        private void ExecuteSubmitCommand(ICloseable window)
        {
            bool dateParse = int.TryParse(Year, out int result);
            if (dateParse && result > 1850)
            {
                ReportName(new DateTime(result, 1, 1), Name);
                window.Close();
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
        }

        private readonly Action<DateTime, string> ReportName;

        public CreateSeasonDialogViewModel(Action<DateTime, string> reportNameBack)
            : base("Create New Player")
        {
            ReportName = reportNameBack;
            SubmitCommand = new RelayCommand<ICloseable>(ExecuteSubmitCommand);
        }
    }
}
