using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class DashboardPageViewModel : BindableBase
	{
        private readonly IDatabaseService _databaseService;
        private decimal _totalDebt;

        public DashboardPageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            GroupedOpenDentalProcedures = new ObservableCollection<GroupedOpenDentalProcedure>();
            GroupedFinTrades = new ObservableCollection<GroupedFinTrade>();

            MessagingCenter.Subscribe<DashboardPage>(this, Constants.OnDashboardPageAppearingMsg, (sender) => { DashboardProceduresAsync(); });
            MessagingCenter.Subscribe<EditPatientFinTradePageViewModel>(this, Constants.OnDasboardDataChangeMsg, (sender) => { DashboardProceduresAsync(); });
            MessagingCenter.Subscribe<EditPatientHistoryPageViewModel>(this, Constants.OnDasboardDataChangeMsg, (sender) => { DashboardProceduresAsync(); });
        }        

        public decimal TotalDebt
        {
            get { return _totalDebt; }
            set { SetProperty(ref _totalDebt, value); }
        }

        public ObservableCollection<GroupedOpenDentalProcedure> GroupedOpenDentalProcedures { get; }    
        
        public ObservableCollection<GroupedFinTrade> GroupedFinTrades { get; }

        private async void DashboardProceduresAsync()
        {
            var openProcedures = await _databaseService.DentalAssistantDB.GetGroupedOpenDentalProceduresAsync();

            GroupedOpenDentalProcedures.Clear();
            foreach(var groupedProcedure in openProcedures)
            {
                GroupedOpenDentalProcedures.Add(groupedProcedure);
            }

            var groupedTrades = await _databaseService.DentalAssistantDB.GetFinTradesAsync();
            TotalDebt = groupedTrades.Sum(g => g.Sum);

            var maxSumGroupedTrades = groupedTrades.OrderByDescending(q => q.Sum).Take(groupedTrades.Count() > 2 ? 3 : groupedTrades.Count());

            GroupedFinTrades.Clear();
            foreach (var gTrade in maxSumGroupedTrades)
            {
                GroupedFinTrades.Add(gTrade);
            }

            MessagingCenter.Unsubscribe<DashboardPage>(this, Constants.OnDashboardPageAppearingMsg);
        }
    }
}
