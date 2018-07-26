using DentalAssistantXF.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XFPrismDemo.LocalDBs
{
    public class DentalAssistantDB
    {
        private readonly SQLiteAsyncConnection _connection;

        public DentalAssistantDB(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            _connection.CreateTableAsync<Patient>().Wait();
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            return await _connection.Table<Patient>().ToListAsync();
        }

        public async Task<Patient> GetPatientAsync(int id)
        {
            return await _connection.GetAsync<Patient>(id); //QueryAsync<Patient>("select * from Patient where Id =?", id);            
        }

        public async Task<int> SavePatientAsync(Patient patient)
        {
            return await _connection.InsertAsync(patient);
        }

        public async Task<int> UpdatePatientAsync(Patient patient)
        {
            return await _connection.UpdateAsync(patient);
        }

        public async Task<int> DeletePatientAsync(Patient patient)
        {
            return await _connection.DeleteAsync(patient);
        }
    }
}
