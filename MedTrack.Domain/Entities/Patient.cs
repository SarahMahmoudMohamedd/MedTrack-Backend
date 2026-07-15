using MedTrack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Patient : BaseEntity<Guid>
    {
        public string NationalId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; } 
        public string BloodType { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Emergency Info
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyRelationship { get; set; } = string.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<PatientCondition> ChronicConditions { get; set; } = new List<PatientCondition>();
        public ICollection<PatientAllergy> Allergies { get; set; } = new List<PatientAllergy>();
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();
    }

    public class PatientCondition : BaseEntity<Guid>
    {
        public Guid PatientId { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public Patient? Patient { get; set; }
    }

    public class PatientAllergy : BaseEntity<Guid>
    {
        public Guid PatientId { get; set; }
        public string AllergyName { get; set; } = string.Empty;
        public Patient? Patient { get; set; }
    }
}