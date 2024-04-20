using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAll();
        Task<Doctor> GetByIdAsync(string id);
        Task<Doctor> GetByIdAsyncNoTracking(string id);
        Task<IEnumerable<Doctor>> GetDoctorByNameOrCode(string name);
        Task<Doctor> GetByCode(string code);
        Task<IEnumerable<Doctor>> GetSpecialistAndResident();
        bool Add(Doctor doctor);
        bool Update(Doctor doctor);
        bool Delete(Doctor doctor);
        bool Save();
    }
}
