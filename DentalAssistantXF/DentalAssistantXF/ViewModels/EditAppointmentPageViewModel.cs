using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
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
    public class EditAppointmentPageViewModel : BindableBase, INavigatingAware
    {
        private readonly IDatabaseService _databaseService;
        private Appointment _appointment;
        private Patient _selectedPatient;
        private Patient _tempPatient;
        private string _title;

        public EditAppointmentPageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            SaveAppointmentCommand = new DelegateCommand(SaveAppointment);
            Patients = new ObservableCollection<Patient>();
        }        

        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetProperty(ref _appointment, value); }
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { SetProperty(ref _selectedPatient, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            _tempPatient = null;

            if (parameters != null)
            {
                Appointment = (Appointment)parameters["Appointment"];
                Title = Appointment.Id < 1 ? "Add new appointment" : "Edit appointment";                

                if (parameters["Patient"] != null)
                {
                    _tempPatient = (Patient)parameters["Patient"];
                }

                GetPatientsAsync();
            }            
        }        

        public ObservableCollection<Patient> Patients { get; }

        public DelegateCommand SaveAppointmentCommand { get; }

        private async void GetPatientsAsync()
        {
            var patients = await _databaseService.DentalAssistantDB.GetPatientsAsync();
            Patients.Clear();
            patients.ToList().ForEach(Patients.Add);
            if (_tempPatient != null)
            {
                SelectedPatient = Patients.Where(p => p.Id == _tempPatient.Id).FirstOrDefault();
            }
        }

        private async void SaveAppointment()
        {
            Appointment.PatientId = SelectedPatient.Id;
            try
            {
                if (await _databaseService.DentalAssistantDB.SaveAppointmentAsync(Appointment) > 0)
                {
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Appointment saved successfully");
                    MessagingCenter.Send(this, Constants.OnAddOrEditAppointmentMsg);
                    MessagingCenter.Send(this, Constants.OnDashboardDataChangeMsg);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
