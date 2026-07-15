using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.AiDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class MedicalAiService : IMedicalAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _ocrBaseUrl;
        private readonly string _chatBaseUrl;

       
        public MedicalAiService(HttpClient httpClient, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _ocrBaseUrl = configuration["AiServices:OcrBaseUrl"] ?? throw new ArgumentNullException("OcrBaseUrl is missing in appsettings.");
            _chatBaseUrl = configuration["AiServices:ChatBaseUrl"] ?? throw new ArgumentNullException("ChatBaseUrl is missing in appsettings.");
        }

      
        public async Task<OcrResponseDto?> ProcessPrescriptionOcrAsync(IFormFile imageFile)
        {
            using var content = new MultipartFormDataContent();
            using var stream = imageFile.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileContent, "image", imageFile.FileName);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");

            var response = await _httpClient.PostAsync($"{_ocrBaseUrl}/predict", content);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<OcrResponseDto>();
        }

 
        public async Task<ChatResponseDto?> AskChatbotWithContextAsync(string nationalId, MessageDto messageDto)
        {
            
            var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await patientRepo.FindAsync(p => p.NationalId.Trim() == nationalId.Trim());
            var patient = patients.FirstOrDefault();
            if (patient == null) return null;

            var patientData = new PatientDataContract
            {
                Patient = new PatientAiDto
                {
                    Id = patient.Id.ToString(),
                    FullName = patient.FullName,
                    DateOfBirth = patient.DateOfBirth.ToString("yyyy-MM-dd"), 
                    Gender = (int)patient.Gender, // 0 = Male, 1 = Female
                    BloodType = "A+"
                }
            };

        
            var conditionRepo = _unitOfWork.GetRepository<PatientCondition, Guid>();
            var conditions = await conditionRepo.FindAsync(c => c.PatientId == patient.Id);
            patientData.Conditions = conditions.Select(c => new ConditionAiDto { ConditionName = c.ConditionName }).ToList();

        
            var allergyRepo = _unitOfWork.GetRepository<PatientAllergy, Guid>();
            var allergies = await allergyRepo.FindAsync(a => a.PatientId == patient.Id);
            patientData.Allergies = allergies.Select(a => new AllergyAiDto { AllergyName = a.AllergyName }).ToList();

        
            var visitRepo = _unitOfWork.GetRepository<Visit, Guid>();
            var visits = await visitRepo.FindAsync(v => v.PatientId == patient.Id);
            foreach (var v in visits)
            {
                patientData.Visits.Add(new VisitAiDto
                {
                    VisitDate = v.VisitDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Diagnosis = v.Diagnosis,
                    BloodPressure = v.BloodPressure,
                    HeartRate = v.HeartRate ?? 0,
                    Temperature = (double)(v.Temperature ?? 0),
                    Medications = v.Medications?.Select(m => new MedicationAiDto
                    {
                        DrugName = m.DrugName,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency
                    }).ToList() ?? new List<MedicationAiDto>()
                });
            }

           
            var labRepo = _unitOfWork.GetRepository<LabTest, Guid>();
            var labs = await labRepo.FindAsync(l => l.PatientId == patient.Id && (int)l.Status == 2);
            patientData.Lab_Tests = labs.Select(l => new LabTestAiDto
            {
                TestName = l.TestName,
                OrderDate = l.OrderDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Status = (int)l.Status,
                AdditionalNotes = l.AdditionalNotes
            }).ToList();

       
            var appointmentRepo = _unitOfWork.GetRepository<Appointment, Guid>();
            var appointments = await appointmentRepo.FindAsync(a => a.PatientId == patient.Id);
            patientData.Appointments = appointments.Select(a => new AppointmentAiDto
            {
                AppointmentDate = a.AppointmentDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Type = a.Type.ToString(),
                Status = (int)a.Status
            }).ToList();

           
            var chatRequest = new ChatRequestDto
            {
                Patient_Data = patientData,
                Message = messageDto.Message,
                History = messageDto.History ?? new List<Dictionary<string, string>>()
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");

            var response = await _httpClient.PostAsJsonAsync($"{_chatBaseUrl}/chat", chatRequest);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ChatResponseDto>();
        }

       
        public async Task<string?> SummarizeLabPdfAsync(IFormFile pdfFile)
        {
            using var content = new MultipartFormDataContent();
            using var stream = pdfFile.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(pdfFile.ContentType);
            content.Add(fileContent, "file", pdfFile.FileName);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");

            var response = await _httpClient.PostAsync($"{_chatBaseUrl}/labs/summarize", content);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<LabSummaryResponseDto>();
            return result?.Summary;
        }

        public async Task<object?> GetComparisonGraphDataAsync(LabComparisonRequestDto requestDto)
        {
            if (requestDto == null) return null;

            var chartData = new List<object>();

            
            if (requestDto.CurrentLab != null && !string.IsNullOrEmpty(requestDto.CurrentLab.TestName))
            {
                chartData.Add(new
                {
                    period = "Current Lab",
                    testName = requestDto.CurrentLab.TestName,
                    date = requestDto.CurrentLab.OrderDate,
                    metrics = ParseAdditionalNotes(requestDto.CurrentLab.AdditionalNotes)
                });
            }

         
            if (requestDto.PreviousLab != null && !string.IsNullOrEmpty(requestDto.PreviousLab.TestName))
            {
                chartData.Add(new
                {
                    period = "Previous Lab",
                    testName = requestDto.PreviousLab.TestName,
                    date = requestDto.PreviousLab.OrderDate,
                    metrics = ParseAdditionalNotes(requestDto.PreviousLab.AdditionalNotes)
                });
            }

            return new
            {
                status = "success",
                chartType = "BarChart",
                data = chartData
            };
        }

    
        private Dictionary<string, double> ParseAdditionalNotes(string notes)
        {
            var metrics = new Dictionary<string, double>();
            if (string.IsNullOrWhiteSpace(notes)) return metrics;

            var parts = notes.Split(',');
            foreach (var part in parts)
            {
                if (part.Contains(':'))
                {
                    var keyValue = part.Split(':');
                    if (keyValue.Length == 2 && double.TryParse(keyValue[1].Trim(), out double val))
                    {
                        metrics[keyValue[0].Trim().ToLower()] = val;
                    }
                }
            }
            return metrics;
        }
    }
}