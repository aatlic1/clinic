using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsByPatientId(int id);
        Task<Report> GetReportById(int id);
        Task<Report> GetReportByReception(Reception reception);
        bool Add(Report report);
        bool Save();
    }
}
