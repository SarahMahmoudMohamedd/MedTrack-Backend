using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace MedTrack
{
    public class ExceptionMiddleware
    {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionMiddleware> _logger;

            public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    // خلي الـ Request يكمل طريقه الطبيعي
                    await _next(context);
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ في الـ Console عندك عشان كديفيلوبر تشوفي السجل بالكامل
                    _logger.LogError(ex, $"🚨 حصلت مشكلة جوه السيستم: {ex.Message}");

                    // تحويل الخطأ لشكل شيك يروح للـ Front-end
                    await HandleExceptionAsync(context, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
                context.Response.ContentType = "application/json";

                // الحالة الافتراضية: خطأ سيرفر داخلي (500)
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "An unexpected error occurred on the server.";

                // 🛠️ هنا بقى بنقفش أخطاء قاعدة البيانات المحددة ونترجمها
                if (exception is DbUpdateException dbException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest; // تحويلها لـ 400 لأنها مشكلة مدخلات
                    message = "Database Error: Failed to save changes. Please check your data limits.";

                    // لو السبب إن النص طويل زي ما حصل معاكِ في الـ BloodType
                    if (dbException.InnerException != null && dbException.InnerException.Message.Contains("truncated"))
                    {
                        message = "Validation Error: One of the fields exceeds the allowed character length (e.g., BloodType max length is 3).";
                    }
                }
                // 💡 تقدري مستقبلاً تقفشي هنا أخطاء تانية زي الـ Unauthorized أو الـ KeyNotFound

                // تجهيز الـ JSON Object اللي هيرجع
                var response = new
                {
                    status = statusCode,
                    title = "An error occurred while processing your request.",
                    detail = message
                };

                context.Response.StatusCode = statusCode;

                // تحويل الكائن لنص JSON مع الحفاظ على صيغة الـ CamelCase (أول حرف صغير للفرونت إند)
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
