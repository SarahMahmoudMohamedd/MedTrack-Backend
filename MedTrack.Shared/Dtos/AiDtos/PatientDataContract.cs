using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedTrack.Shared.Dtos.AiDtos
{
    public class PatientDataContract
    {
        [JsonPropertyName("patient")]
        public PatientAiDto Patient { get; set; } = new();

        [JsonPropertyName("conditions")]
        public List<ConditionAiDto> Conditions { get; set; } = new();

        [JsonPropertyName("allergies")]
        public List<AllergyAiDto> Allergies { get; set; } = new();

        [JsonPropertyName("visits")]
        public List<VisitAiDto> Visits { get; set; } = new();

        [JsonPropertyName("lab_tests")]
        public List<LabTestAiDto> Lab_Tests { get; set; } = new();

        [JsonPropertyName("appointments")]
        public List<AppointmentAiDto> Appointments { get; set; } = new();
    }

    public class PatientAiDto
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("FullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("DateOfBirth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [JsonPropertyName("Gender")]
        public int Gender { get; set; }

        [JsonPropertyName("BloodType")]
        public string BloodType { get; set; } = "A+";
    }

    public class ConditionAiDto
    {
        [JsonPropertyName("ConditionName")]
        public string ConditionName { get; set; } = string.Empty;
    }

    public class AllergyAiDto
    {
        [JsonPropertyName("AllergyName")]
        public string AllergyName { get; set; } = string.Empty;
    }

    public class VisitAiDto
    {
        [JsonPropertyName("VisitDate")]
        public string VisitDate { get; set; } = string.Empty;

        [JsonPropertyName("Diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;

        [JsonPropertyName("BloodPressure")]
        public string BloodPressure { get; set; } = string.Empty;

        [JsonPropertyName("HeartRate")]
        public int? HeartRate { get; set; }

        [JsonPropertyName("Temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("medications")]
        public List<MedicationAiDto> Medications { get; set; } = new();
    }

    public class MedicationAiDto
    {
        [JsonPropertyName("DrugName")]
        public string DrugName { get; set; } = string.Empty;

        [JsonPropertyName("Dosage")]
        public string Dosage { get; set; } = string.Empty;

        [JsonPropertyName("Frequency")]
        public string Frequency { get; set; } = string.Empty;
    }

    public class LabTestAiDto
    {
        [JsonPropertyName("TestName")]
        public string TestName { get; set; } = string.Empty;

        [JsonPropertyName("OrderDate")]
        public string OrderDate { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public int Status { get; set; } = 2;

        [JsonPropertyName("AdditionalNotes")]
        public string AdditionalNotes { get; set; } = string.Empty;
    }

    public class AppointmentAiDto
    {
        [JsonPropertyName("AppointmentDate")]
        public string AppointmentDate { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }
}
