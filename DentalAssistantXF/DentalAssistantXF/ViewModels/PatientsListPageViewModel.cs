﻿using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
    public class PatientsListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private Patient _selectedPatient;
        private List<Patient> _patients = new List<Patient>();
        private string _filterText;
        private bool _isLoading;

        public PatientsListPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            Patients = new ObservableCollection<Patient>();

            AddPatientCommand = new DelegateCommand(AddPatient);
            NavigateToPatientDetailsCommand = new DelegateCommand(NavigateToPatientDetailsAsync);
            FilterPatientsCommand = new DelegateCommand(FilterPatients);

            //Subscribe to event only for the first appearance of the PatientsListPage
            MessagingCenter.Subscribe<PatientsListPage>(this, Constants.OnPatientsListPageAppearingMsg, (sender) => { GetPatientsAsync(); });

            //Refresh patients list after adding or editing patient
            MessagingCenter.Subscribe<EditPatientPageViewModel>(this, Constants.OnAddOrEditPatientMsg, (sender) => { GetPatientsAsync(); });
            MessagingCenter.Subscribe<EditPatientHistoryPageViewModel>(this, Constants.OnAddOrEditPatientMsg, (sender) => { GetPatientsAsync(); });
            MessagingCenter.Subscribe<PatientHistoryPageViewModel>(this, Constants.OnAddOrEditPatientMsg, (sender) => { GetPatientsAsync(); });
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { SetProperty(ref _selectedPatient, value); }
        }

        public ObservableCollection<Patient> Patients { get; set; }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                SetProperty(ref _filterText, value);
                CheckIfCleared();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public DelegateCommand NavigateToPatientDetailsCommand { get; }

        public DelegateCommand FilterPatientsCommand { get; }

        public DelegateCommand AddPatientCommand { get; }

        private async void GetPatientsAsync()
        {
            //FilterText = string.Empty;                         
            IsLoading = true;
            _patients.Clear();
            try
            {
                _patients = (await _databaseService.DentalAssistantDB.GetPatientsAsync()).ToList();

                Patients.Clear();
                foreach (var patient in _patients)
                {
                    patient.HasOpenCase = await _databaseService.DentalAssistantDB.HasOpenCasesAsync(patient.Id);
                    Patients.Add(patient);
                }


                //Get patients only on first appearance
                MessagingCenter.Unsubscribe<PatientsListPage>(this, Constants.OnPatientsListPageAppearingMsg);

                //To be removed in production
                if (Patients.Count == 0)
                {
                    GetPatients();
                }
                ///////////////////////
            }
            catch (Exception e)
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, e.Message);
            }
            finally
            {
                IsLoading = false;
            }            
        }

        private async void NavigateToPatientDetailsAsync()
        {
            if (SelectedPatient != null)
            {
                var navParams = new NavigationParameters();
                navParams.Add("PatientId", SelectedPatient.Id);
                await _navigationService.NavigateAsync("PatientProfilePage", navParams);
            }
        }

        private void FilterPatients()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                return;
            }

            var patients = _patients.Where(p => p.FullName.StartsWith(FilterText, true, null)).ToList();
            Patients.Clear();
            patients.ForEach(Patients.Add);
        }

        private void CheckIfCleared()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                Patients.Clear();
                _patients.ForEach(Patients.Add);
            }
        }

        private async void AddPatient()
        {
            var patient = new Patient();
            var navParams = new NavigationParameters();
            navParams.Add("Patient", patient);
            await _navigationService.NavigateAsync("EditPatientPage", navParams);
        }


        //Test data

        private async void GetPatients()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(PatientsListPageViewModel)).Assembly;

            Stream stream = assembly.GetManifestResourceStream("DentalAssistantXF.patients.json");
            string jsonInput = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                jsonInput = reader.ReadToEnd();
            }

            var patients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Patient>>(jsonInput);

            Patients.Clear();
            foreach (var patient in patients)
            {
                Patients.Add(patient);
            }

            await _databaseService.DentalAssistantDB.SaveAllPatientsAsync(patients);
        }
    }
}
