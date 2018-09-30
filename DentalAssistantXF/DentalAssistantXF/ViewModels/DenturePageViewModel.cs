using DentalAssistantXF.Models;
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
	public class DenturePageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;

        public DenturePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            TeethList = new ObservableCollection<ToothState>();
            Initialization();
        }        

        public ObservableCollection<ToothState> TeethList { get; set; }

        public DelegateCommand ReturnTeethNumbersCommand => new DelegateCommand(async () => 
        {
            var teethList = TeethList.Where(t => t.Checked == true).Select(t => t.Index.ToString()).ToList();
            var teethNumbers = string.Join(", ", teethList);
            //MessagingCenter.Send(this, Constants.TeethNumbersMsg, teethNumbers);
            var navParams = new NavigationParameters();
            navParams.Add("TeethNumbers", teethNumbers);
            await _navigationService.GoBackAsync(navParams);
        });

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if(parameters != null)
            {
                var teethList = (List<int>)parameters["TeethList"];
                foreach(var tooth in teethList)
                {
                    TeethList[tooth-1].Checked = true;
                }
            }
        }

        private void Initialization()
        {
            TeethList.Add(new ToothState { Checked = false, Index = 1 });
            TeethList.Add(new ToothState { Checked = false, Index = 2 });
            TeethList.Add(new ToothState { Checked = false, Index = 3 });
            TeethList.Add(new ToothState { Checked = false, Index = 4 });
            TeethList.Add(new ToothState { Checked = false, Index = 5 });
            TeethList.Add(new ToothState { Checked = false, Index = 6 });
            TeethList.Add(new ToothState { Checked = false, Index = 7 });
            TeethList.Add(new ToothState { Checked = false, Index = 8 });
            TeethList.Add(new ToothState { Checked = false, Index = 9 });
            TeethList.Add(new ToothState { Checked = false, Index = 10 });
            TeethList.Add(new ToothState { Checked = false, Index = 11 });
            TeethList.Add(new ToothState { Checked = false, Index = 12 });
            TeethList.Add(new ToothState { Checked = false, Index = 13 });
            TeethList.Add(new ToothState { Checked = false, Index = 14 });
            TeethList.Add(new ToothState { Checked = false, Index = 15 });
            TeethList.Add(new ToothState { Checked = false, Index = 16 });
            TeethList.Add(new ToothState { Checked = false, Index = 17 });
            TeethList.Add(new ToothState { Checked = false, Index = 18 });
            TeethList.Add(new ToothState { Checked = false, Index = 19 });
            TeethList.Add(new ToothState { Checked = false, Index = 20 });
            TeethList.Add(new ToothState { Checked = false, Index = 21 });
            TeethList.Add(new ToothState { Checked = false, Index = 22 });
            TeethList.Add(new ToothState { Checked = false, Index = 23 });
            TeethList.Add(new ToothState { Checked = false, Index = 24 });
            TeethList.Add(new ToothState { Checked = false, Index = 25 });
            TeethList.Add(new ToothState { Checked = false, Index = 26 });
            TeethList.Add(new ToothState { Checked = false, Index = 27 });
            TeethList.Add(new ToothState { Checked = false, Index = 28 });
            TeethList.Add(new ToothState { Checked = false, Index = 29 });
            TeethList.Add(new ToothState { Checked = false, Index = 30 });
            TeethList.Add(new ToothState { Checked = false, Index = 31 });
            TeethList.Add(new ToothState { Checked = false, Index = 32 });
        }
    }
}
