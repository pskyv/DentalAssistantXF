using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.SfAutoComplete.XForms;
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
        private List<Patient> _patients;
        private string _title;
        private string _filterText;
        private SuggestionBoxPlacement _isSuggestionListOpen;

        public EditAppointmentPageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            SaveAppointmentCommand = new DelegateCommand(SaveAppointment);
            Patients = new ObservableCollection<Patient>();
            IsSuggestionListOpen = SuggestionBoxPlacement.None;
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

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                SetProperty(ref _filterText, value);
                if (FilterText != null)
                {
                    IsSuggestionListOpen = FilterText.Length < 3 ? SuggestionBoxPlacement.None : SuggestionBoxPlacement.Bottom;
                }
                FilterPatients();
            }
        }

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) => values
        .Where(x => x.ToLower().StartsWith(text.ToLower()))
        .OrderBy(x => x)
        .ToList();

        public SuggestionBoxPlacement IsSuggestionListOpen
        {
            get { return _isSuggestionListOpen; }
            set { SetProperty(ref _isSuggestionListOpen, value); }
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
                    Patients.Add(_tempPatient);
                    SelectedPatient = Patients.Where(p => p.Id == _tempPatient.Id).FirstOrDefault();
                    FilterText = SelectedPatient.FullName;
                }                
            }            
        }        

        public ObservableCollection<Patient> Patients { get; }

        public DelegateCommand SaveAppointmentCommand { get; }

        private async void FilterPatients()
        {
            if (string.IsNullOrEmpty(FilterText) || FilterText.Length < 3)
            {
                SelectedPatient = null;
                Patients.Clear();
                return;
            }

            if (SelectedPatient != null)
            {
                return;
            }

            Patients.Clear();
            //var patients = _patients.Where(p => p.FullName.StartsWith(FilterText, StringComparison.CurrentCultureIgnoreCase));
            var patients = await _databaseService.DentalAssistantDB.GetMatchingPatientsAsync(FilterText);
            patients.ForEach(Patients.Add);
        }

        private async void GetPatientAsync()
        {
            //var patients = await _databaseService.DentalAssistantDB.GetPatientsAsync();
            //_patients = patients.ToList();
            //Patients.Clear();
            //patients.ToList().ForEach(Patients.Add);
            var patient = await _databaseService.DentalAssistantDB.GetPatientAsync(_tempPatient.Id);
            if (_tempPatient != null)
            {
                SelectedPatient = Patients.Where(p => p.Id == _tempPatient.Id).FirstOrDefault();
            }
        }

        private async void SaveAppointment()
        {
            if(SelectedPatient == null)
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, "You have to select a patient first");
                return;
            }

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
