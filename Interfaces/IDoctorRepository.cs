using Clinic.Models;

namespace Clinic.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAll();
        Task<Doctor> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetDoctorByNameOrCode(string name);
        Task<IEnumerable<Doctor>> GetSpecialistAndResident();
        bool Add(Doctor doctor);
        bool Update(Doctor doctor);
        bool Delete(Doctor doctor);
        bool Save();
    }
}
