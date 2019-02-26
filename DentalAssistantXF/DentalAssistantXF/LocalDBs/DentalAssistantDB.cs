using DentalAssistantXF.Models;
using SQLite;
using System;
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
            _connection.CreateTableAsync<Appointment>().Wait();
        }

        #region patient
        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            var patients = await _connection.Table<Patient>().ToListAsync();
            return patients.OrderBy(p => p.LastName);
        }

        public async Task<List<Patient>>GetMatchingPatientsAsync(string filterText)
        {
            return await _connection.Table<Patient>().Where(p => p.FullName.StartsWith(filterText, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
        }

        public async Task<Patient> GetPatientAsync(int id)
        {
            return await _connection.GetAsync<Patient>(id); //QueryAsync<Patient>("select * from Patient where Id =?", id);            
        }

        public async Task<int> SavePatientAsync(Patient patient)
        {
            return await _connection.InsertAsync(patient);
        }

        public async Task<int> SaveAllPatientsAsync(IEnumerable<Patient> patients)
        {
            return await _connection.InsertAllAsync(patients);
        }

        public async Task<int> UpdatePatientAsync(Patient patient)
        {
            return await _connection.UpdateAsync(patient);
        }

        public async Task<int> DeletePatientAsync(Patient patient)
        {
            return await _connection.DeleteAsync(patient);
        }
        #endregion

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

        public async Task<int> DeleteProcedureAsync(PatientDentalProcedure procedure)
        {
            return await _connection.DeleteAsync(procedure);
        }
        #endregion

        #region FinTrades
        public async Task<List<FinTrade>> GetPatientFinTradesAsync(int patientId)
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

        public async Task<int> DeleteFinTradeAsync(FinTrade finTrade)
        {
            return await _connection.DeleteAsync(finTrade);
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

        public async Task<IEnumerable<GroupedFinTrade>> GetFinTradesAsync()
        {
            var finTrades = await _connection.QueryAsync<FinTradeDTO>("select P.FirstName, P.LastName, F.TradeType, F.Ammount " +
                                                                      "from FinTrade F inner join " +
                                                                      "Patient P on F.PatientId = P.Id");

            var groupedTrades = from f in finTrades
                                group f by f.FullName into g
                                select new GroupedFinTrade { FullName = g.Key, Sum = g.Sum(f => f.AmmountForSum) };

            return groupedTrades;
        }

        public async Task<AppointmentDTO> GetNextAppointment()
        {
            var appointments = await _connection.QueryAsync<AppointmentDTO>("select A.Id, P.Id as PatientId, P.FirstName, P.LastName, P.Phone, " +
                                                                            "A.AppointmentDate, A.AppointmentTime, A.subject " +
                                                                            "from Appointment A inner join " +
                                                                            "Patient P on A.PatientId = P.Id " +
                                                                            "where A.AppointmentDate >= ? ", DateTime.Now.Date);

            return appointments.Where(p => p.AppointmentDateAndTime >= DateTime.Now).OrderBy(p => p.AppointmentDateAndTime).FirstOrDefault();
        }
        #endregion

        #region appointments
        public async Task<List<AppointmentDTO>> GetAppointmentsListAsync(DateTime date)
        {
            return await _connection.QueryAsync<AppointmentDTO>("select A.Id, P.Id as PatientId, P.FirstName, P.LastName, P.Phone, " +
                                                                            "A.AppointmentDate, A.AppointmentTime, A.subject " +
                                                                            "from Appointment A inner join " +
                                                                            "Patient P on A.PatientId = P.Id " +
                                                                            "where A.AppointmentDate = ? " +
                                                                            "order by A.AppointmentTime", date);
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _connection.GetAsync<Appointment>(id);            
        }

        public async Task<int> SaveAppointmentAsync(Appointment appointment)
        {
            if (appointment.Id < 1)
            {
                return await _connection.InsertAsync(appointment);
            }
            else
            {
                return await _connection.UpdateAsync(appointment);
            }
        }

        public async Task<int> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _connection.GetAsync<Appointment>(appointmentId);
            return await _connection.DeleteAsync(appointment);
        }
        #endregion
    }
}
