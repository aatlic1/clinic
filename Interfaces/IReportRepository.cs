using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IReportRepository
    {
        bool Add(Report report);
        bool Save();
    }
}
