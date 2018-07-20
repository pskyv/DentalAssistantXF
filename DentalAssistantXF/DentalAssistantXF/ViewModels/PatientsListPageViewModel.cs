using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
	public class PatientsListPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;

        public PatientsListPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand NavigateToPatientDetailsCommand => new DelegateCommand( async () => { await _navigationService.NavigateAsync("PatientProfilePage"); });

    }
}
