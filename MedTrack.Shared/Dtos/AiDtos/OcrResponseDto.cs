using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedTrack.Shared.Dtos.AiDtos
{
    // Phase A - OCR Response
    public class OcrResponseDto
    {
        public List<string> Medications { get; set; } = new();
        public double Confidence { get; set; }
    }

    // Phase B - Chat Request & Response
    public class ChatRequestDto
    {
        [JsonPropertyName("patient_data")]
        public PatientDataContract Patient_Data { get; set; } = new();

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("history")]
        public List<Dictionary<string, string>> History { get; set; } = new();
    }

    public class ChatResponseDto
    {
        [JsonPropertyName("reply")]
        public string Reply { get; set; } = string.Empty;

        [JsonPropertyName("history")]
        public List<Dictionary<string, string>> History { get; set; } = new();
    }

    // Phase B - Lab Summary Response
    public class LabSummaryResponseDto
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
    }
}