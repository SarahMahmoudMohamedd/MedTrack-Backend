using MedTrack.Shared.Dtos.AiDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
        public interface IMedicalAiService
        {
            Task<OcrResponseDto?> ProcessPrescriptionOcrAsync(IFormFile imageFile);
            Task<ChatResponseDto?> AskChatbotWithContextAsync(string nationalId, MessageDto messageDto);
            Task<string?> SummarizeLabPdfAsync(IFormFile pdfFile);
        Task<object?> GetComparisonGraphDataAsync(LabComparisonRequestDto requestDto);
    }
    }
