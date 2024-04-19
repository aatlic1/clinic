using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient> GetByIdAsync(int id);
        Task<Patient> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Patient>> GetPatientByName(string name);
        bool Add(Patient patient);
        bool Update(Patient patient);
        bool Delete(Patient patient);
        bool Save();
    }
}
