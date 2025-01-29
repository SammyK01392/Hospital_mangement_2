using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace Hospital_mangement_2.Models
{
    public class Hospital
    {
        public int HospitalId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public string? Email { get; set; }

        public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

        public virtual ICollection<Practitioner> Practitioners { get; set; } = new List<Practitioner>();

    }
}
