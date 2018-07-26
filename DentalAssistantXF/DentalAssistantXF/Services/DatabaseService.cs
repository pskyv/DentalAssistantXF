using DentalAssistantXF.Services;
using System;
using Xamarin.Forms;
using XFPrismDemo.LocalDBs;

namespace DentalAssistantXF.Services
{
    public class DatabaseService : IDatabaseService
    {
        private DentalAssistantDB _dentalAssistantDB;

        public DentalAssistantDB DentalAssistantDB
        {
            get
            {
                if (_dentalAssistantDB == null)
                {
                    _dentalAssistantDB = new DentalAssistantDB(DependencyService.Get<ISQLiteConnection>().GetConnection());
                }
                    
                return _dentalAssistantDB;
            }
        }
    }
}
