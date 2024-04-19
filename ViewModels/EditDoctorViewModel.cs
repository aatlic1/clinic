using Clinic.Data.Enum;

namespace Clinic.ViewModels
{
    public class EditDoctorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Title Title { get; set; }
        public string Code { get; set; }
    }
}
