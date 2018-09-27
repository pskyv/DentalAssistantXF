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
        private AppointmentDTO _nextAppointment;
        private decimal _totalDebt;
        private int _totalOpen;
        private bool _hasNextAppointment;

        public DashboardPageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            GroupedOpenDentalProcedures = new ObservableCollection<GroupedOpenDentalProcedure>();
            GroupedFinTrades = new ObservableCollection<GroupedFinTrade>();
            NextAppointments = new ObservableCollection<AppointmentDTO>();

            MessagingCenter.Subscribe<DashboardPage>(this, Constants.OnDashboardPageAppearingMsg, (sender) => { LoadDashboardData(); });
            MessagingCenter.Subscribe<PatientFinTradesPageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadFinTradeDataAsync(); });
            MessagingCenter.Subscribe<EditPatientFinTradePageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadFinTradeDataAsync(); });            
            MessagingCenter.Subscribe<PatientHistoryPageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadOpenProceduresDataAsync(); });
            MessagingCenter.Subscribe<EditPatientHistoryPageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadOpenProceduresDataAsync(); });
            MessagingCenter.Subscribe<EditAppointmentPageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadSchedulerDataAsync(); });
            MessagingCenter.Subscribe<AppointmentsListPageViewModel>(this, Constants.OnDashboardDataChangeMsg, (sender) => { LoadSchedulerDataAsync(); });
        }        

        public decimal TotalDebt
        {
            get { return _totalDebt; }
            set { SetProperty(ref _totalDebt, value); }
        }

        public int TotalOpen
        {
            get { return _totalOpen; }
            set { SetProperty(ref _totalOpen, value); }
        }

        public AppointmentDTO NextAppointment
        {
            get { return _nextAppointment; }
            set { SetProperty(ref _nextAppointment, value); }
        }

        public bool HasNextAppointment
        {
            get { return _hasNextAppointment; }
            set { SetProperty(ref _hasNextAppointment, value); }
        }

        public ObservableCollection<GroupedOpenDentalProcedure> GroupedOpenDentalProcedures { get; }    
        
        public ObservableCollection<GroupedFinTrade> GroupedFinTrades { get; }

        public ObservableCollection<AppointmentDTO> NextAppointments { get; }

        private void LoadDashboardData()
        {
            LoadOpenProceduresDataAsync();
            LoadFinTradeDataAsync();
            LoadSchedulerDataAsync();

            MessagingCenter.Unsubscribe<DashboardPage>(this, Constants.OnDashboardPageAppearingMsg);
        }

        private async void LoadOpenProceduresDataAsync()
        {
            var openProcedures = await _databaseService.DentalAssistantDB.GetGroupedOpenDentalProceduresAsync();

            TotalOpen = openProcedures.Sum(p => p.Count);

            GroupedOpenDentalProcedures.Clear();
            foreach (var groupedProcedure in openProcedures)
            {
                GroupedOpenDentalProcedures.Add(groupedProcedure);
            }
        }

        private async void LoadFinTradeDataAsync()
        {
            var groupedTrades = await _databaseService.DentalAssistantDB.GetFinTradesAsync();
            TotalDebt = groupedTrades.Sum(g => g.Sum);

            var maxSumGroupedTrades = groupedTrades.OrderByDescending(q => q.Sum).Take(groupedTrades.Count() > 2 ? 3 : groupedTrades.Count());

            GroupedFinTrades.Clear();
            foreach (var gTrade in maxSumGroupedTrades)
            {
                GroupedFinTrades.Add(gTrade);
            }
        }

        private async void LoadSchedulerDataAsync()
        {
            NextAppointment = await _databaseService.DentalAssistantDB.GetNextAppointment();
            HasNextAppointment = !(NextAppointment == null);
            NextAppointments.Clear();
            NextAppointments.Add(NextAppointment);
        }
    }
}
