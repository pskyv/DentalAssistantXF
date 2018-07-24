using DentalAssistantXF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DentalAssistantXF.ViewModels
{
	public class PatientsListPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private Patient _selectedPatient;
        private string _filterText;

        public PatientsListPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Patients = new ObservableCollection<Patient>();
            GetPatients();
        }        

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { SetProperty(ref _selectedPatient, value); }
        }

        public ObservableCollection<Patient> Patients { get; }

        public string FilterText
        {
            get { return _filterText; }
            set { SetProperty(ref _filterText, value); }
        }

        public DelegateCommand NavigateToPatientDetailsCommand => new DelegateCommand( async () => { await _navigationService.NavigateAsync("PatientProfilePage"); });

        public DelegateCommand FilterPatientsCommand = new DelegateCommand(FilterPatients);

        private void GetPatients()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(PatientsListPageViewModel)).Assembly;

            Stream stream = assembly.GetManifestResourceStream("DentalAssistantXF.patientsList.json");
            string jsonInput = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                jsonInput = reader.ReadToEnd();
            }

            var patients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Patient>>(jsonInput);
            foreach(var patient in patients)
            {
                Patients.Add(patient);
            }
        }

        private static void FilterPatients()
        {
            
        }
    }
}
