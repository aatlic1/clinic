using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Patient patient)
        {
            _context.Add(patient);
            return Save();
        }

        public bool Delete(Patient patient)
        {
            _context.Remove(patient);
            return Save();
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Patient>> GetPatientByName(string name)
        {
            return await _context.Patients.Where(p => p.Name.Contains(name) || p.Surname.Contains(name)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Patient patient)
        {
            _context.Update(patient);
            return Save();
        }
    }
}
