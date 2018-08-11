using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
    public class AppointmentsListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private DateTime _filterDate;
        private AppointmentDTO _selectedAppointment;

        public AppointmentsListPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            Appointments = new ObservableCollection<AppointmentDTO>();
            MessagingCenter.Subscribe<AppointmentsListPage>(this, Constants.OnAppointmentsListPageAppearingMsg, (sender) => { FilterDate = DateTime.Today; });

            AddAppointmentCommand = new DelegateCommand(AddAppointment);
        }

        public DateTime FilterDate
        {
            get { return _filterDate; }
            set
            {
                SetProperty(ref _filterDate, value);
                GetAppointmentsAsync();
            }
        }

        public AppointmentDTO SelectedAppointment
        {
            get { return _selectedAppointment; }
            set { SetProperty(ref _selectedAppointment, value); }
        }

        public ObservableCollection<AppointmentDTO> Appointments { get; }

        public DelegateCommand AddAppointmentCommand { get; }

        private async void GetAppointmentsAsync()
        {
            var appointments = await _databaseService.DentalAssistantDB.GetAppointmentsListAsync(FilterDate);

            Appointments.Clear();
            appointments.ForEach(Appointments.Add);
        }

        private async void AddAppointment()
        {
            var appointment = new Appointment { AppointmentDate = DateTime.Today, PatientId = 1 }; // PatientId = 1    to be deleted......!!!!!!

            var navParams = new NavigationParameters();
            navParams.Add("Appointment", appointment);
            await _navigationService.NavigateAsync("EditAppointmentPage", navParams);
        }
    }
}
