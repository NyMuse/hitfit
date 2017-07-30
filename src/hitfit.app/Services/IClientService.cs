using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;

namespace hitfit.app.Services
{
    public interface IClientService
    {
        Task<int> AddClientProgramAsync(UserProgram userProgram);

        Task<UserProgram> GetClientProgramAsync(int userProgramId);

        Task<List<UserProgram>> GetClientProgramsAsync(int? userId = null, int? programId = null);
        
        Task<List<UserProgram>> GetClientProgramsAsync(DateTime? startedOnFrom = null, DateTime? startedOnTo = null);

        Task UpdateClientProgramAsync(UserProgram userProgram);



        Task<int> AddClientMeasurementsAsync(UserMeasurements measurements);

        Task<UserMeasurements> GetClientMeasurementsAsync(int measurementsId);

        Task<List<UserMeasurements>> GetClientMeasurementsAsync(int? userId = null, int? userProgramId = null, string measurementsType = null);

        Task UpdateUserMeasurements(UserMeasurements userMeasurements);



        Task<int> AddClientReportAsync(Report report);

        Task<Report> GetClientReportAsync(int reportId);

        Task<List<Report>> GetClientReportsAsync(int? userId = null, int? userProgramId = null);
    }
}
