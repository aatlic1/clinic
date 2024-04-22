using Clinic.Data;
using Clinic.Interfaces;
using Clinic.Models;

namespace Clinic.Repository
{
    public class ReportRepository: IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Report report)
        {
            _context.Add(report);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
