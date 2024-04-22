using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsByPatientId(int id);
        bool Add(Report report);
        bool Save();
    }
}
