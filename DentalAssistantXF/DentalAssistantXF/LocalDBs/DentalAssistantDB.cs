using DentalAssistantXF.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
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
            _connection.CreateTableAsync<FinTrade>().Wait();
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

        #region DentalProcedures
        public async Task<List<PatientDentalProcedure>> GetPatientDentalProceduresAsync(int patientId)
        {
            return await _connection.QueryAsync<PatientDentalProcedure>("select * from PatientDentalProcedure where PatientId =? order by StartDate desc", patientId);
        }

        public async Task<int> GetPatientDentalProceduresCountAsync(int patientId)
        {
            return await _connection.Table<PatientDentalProcedure>().Where(p => p.PatientId == patientId).CountAsync();
        }

        public async Task<bool> HasOpenCasesAsync(int patientId)
        {
            return await _connection.Table<PatientDentalProcedure>().Where(p => p.PatientId == patientId && p.IsCompleted == false).CountAsync() > 0;
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
        #endregion

        #region FinTrades
        public async Task<IEnumerable<FinTrade>> GetPatientFinTradesAsync(int patientId)
        {
            return await _connection.QueryAsync<FinTrade>("select * from FinTrade where PatientId =? order by TradeDate desc", patientId);
        }

        public async Task<decimal> GetPatientBalanceAsync(int patientId)
        {
            var trades = await _connection.Table<FinTrade>().Where(f => f.PatientId == patientId).ToListAsync();
            return trades.Sum(t => t.AmmountForSum);
        }

        public async Task<int> SavePatientFinTradeAsync(FinTrade finTrade)
        {
            if (finTrade.Id < 1)
            {
                return await _connection.InsertAsync(finTrade);
            }
            else
            {
                return await _connection.UpdateAsync(finTrade);
            }
        }
        #endregion

        #region dashboard
        public async Task<IEnumerable<GroupedOpenDentalProcedure>> GetGroupedOpenDentalProceduresAsync()
        {
            var openProcedures = await _connection.QueryAsync<PatientDentalProcedure>("select * from PatientDentalProcedure where IsCompleted =?", false); //Table<PatientDentalProcedure>().Where(d => d.IsCompleted == false);
            var query = from d in openProcedures
                        group d by d.DentalProcedure into g
                        select new GroupedOpenDentalProcedure { DentalProcedureType = g.Key, Count = g.Count() };

            return query;
        }

        public async Task<IEnumerable<FinTradeDTO>> GetFinTradesAsync()
        {
            return await _connection.QueryAsync<FinTradeDTO>("select P.FirstName, P.LastName, F.TradeType, F.Ammount " +
                                                             "from FinTrade F inner join " +
                                                             "Patient P on F.PatientId = P.Id");
        }
        #endregion
    }
}
