using Clinic.Data;
using Clinic.Data.Enum;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Doctor doctor)
        {
            _context.Add(doctor);
            return Save();
        }

        public bool Delete(Doctor doctor)
        {
            _context.Remove(doctor);
            return Save();
        }

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _context.Doctors.Where(d => d.Code != null && d.Title != null).ToListAsync();
        }

        public async Task<Doctor> GetByCode(string code)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<Doctor> GetByIdAsync(string id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Doctor> GetByIdAsyncNoTracking(string id)
        {
            return await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorByNameOrCode(string name)
        {
            return await _context.Doctors.Where(d => d.Name.Contains(name) || d.Surname.Contains(name) || d.Code.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetSpecialistAndResident()
        {
            return await _context.Doctors.Where(d => d.Title == Title.Specialist || d.Title == Title.Resident).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Doctor doctor)
        {
            _context.Update(doctor);
            return Save();
        }
    }
}
