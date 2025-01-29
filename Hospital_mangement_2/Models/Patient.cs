using System.Diagnostics.Metrics;

namespace Hospital_mangement_2.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public int? HospitalId { get; set; }
        public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

        public virtual Hospital? Hospital { get; set; }
    }
}
