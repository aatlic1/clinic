using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IReceptionRepository
    {
        Task<IEnumerable<Reception>> GetAll();
        Task<Reception> GetByIdAsync(int id);
        Task<IEnumerable<Reception>> GetReceptionsByDates(DateTime startDate, DateTime endDate);
        bool Add(Reception reception);
        bool Update(Reception reception);
        bool Delete(Reception reception);
        bool Save();
    }
}
