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
            _connection.CreateTableAsync<PatientDentalProcedure>().Wait();
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

        public async Task<List<PatientDentalProcedure>> GetPatientDentalProcedures(int patientId)
        {
            return await _connection.QueryAsync<PatientDentalProcedure>("select * from PatientDentalProcedure where PatientId =? order by StartDate desc", patientId);
        }

        public async Task<int> SavePatientDentalprocedureAsync(PatientDentalProcedure procedure)
        {
            if (procedure.Id < 1)
            {
                return await _connection.InsertAsync(procedure);
            }
            else
            {
                return await _connection.UpdateAsync(procedure);
            }
        }
    }
}
