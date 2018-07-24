using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
	public class PatientProfilePageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;

        public PatientProfilePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand NavigateBackCommand => new DelegateCommand(async () => { await _navigationService.GoBackAsync(); } );

    }
}
