using System.Text;
using System.Text.Json;

namespace BusBookingAPI.Services
{
    public interface ISmsService
    {
        Task<(bool Success, string Message)> SendOtpAsync(string phone, string otpCode);
    }

    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;

        public SmsService(HttpClient httpClient, IConfiguration configuration, ILogger<SmsService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> SendOtpAsync(string phone, string otpCode)
        {
            try
            {
                var apiKey = _configuration["eSMS:ApiKey"];
                var secretKey = _configuration["eSMS:SecretKey"];
                var brandName = _configuration["eSMS:BrandName"];
                var enableSms = _configuration.GetValue<bool>("eSMS:EnableSms");
                var testPhoneNumbers = _configuration.GetSection("eSMS:TestPhoneNumbers").Get<List<string>>() ?? new List<string>();

                // Kiểm tra xem có phải số test không
                var isTestPhone = testPhoneNumbers.Contains(phone);

                // Nếu không bật SMS thật HOẶC là số test, chỉ log và trả về thành công
                if (!enableSms || isTestPhone)
                {
                    var modeLabel = isTestPhone ? "TEST PHONE" : "TEST MODE";
                    _logger.LogInformation($"[{modeLabel}] SMS OTP to {phone}: {otpCode}");
                    return (true, $"[TEST MODE: {otpCode}] Mã OTP đã được gửi");
                }

                // Chuẩn hóa số điện thoại (thêm +84 nếu cần)
                var normalizedPhone = NormalizePhoneNumber(phone);

                // Nội dung tin nhắn (loại bỏ dấu tiếng Việt để tránh lỗi)
                var smsContent = $"Ma xac thuc OTP cua ban la: {otpCode}. Ma nay co hieu luc trong 5 phut.";

                // Tạo request body cho eSMS API
                // Dùng SmsType = 6 (OTP) và KHÔNG cần Brandname
                // SMS sẽ được gửi qua đầu số ngắn mặc định của eSMS
                var requestBody = new
                {
                    ApiKey = apiKey,
                    SecretKey = secretKey,
                    Phone = normalizedPhone,
                    Content = smsContent,
                    SmsType = "8" // 6 = OTP (không cần Brandname)
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi request đến eSMS API
                var response = await _httpClient.PostAsync("http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_post_json/", httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"eSMS Response: {responseContent}");

                // Parse response từ eSMS
                var result = JsonSerializer.Deserialize<EsmsResponse>(responseContent);

                if (result?.CodeResult == "100")
                {
                    _logger.LogInformation($"SMS OTP sent successfully to {phone}");
                    return (true, "Mã OTP đã được gửi đến số điện thoại của bạn");
                }
                else
                {
                    _logger.LogError($"Failed to send SMS to {phone}. Error: {result?.ErrorMessage}");
                    return (false, $"Gửi SMS thất bại: {result?.ErrorMessage ?? "Unknown error"}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending SMS to {phone}");
                return (false, "Có lỗi xảy ra khi gửi SMS");
            }
        }

        private string NormalizePhoneNumber(string phone)
        {
            // Loại bỏ khoảng trắng và ký tự đặc biệt
            phone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Chuyển đổi 0xxx thành 84xxx
            if (phone.StartsWith("0"))
            {
                phone = "84" + phone.Substring(1);
            }

            // Đảm bảo có 84 ở đầu
            if (!phone.StartsWith("84"))
            {
                phone = "84" + phone;
            }

            return phone;
        }
    }

    // Response model từ eSMS API
    public class EsmsResponse
    {
        public string? CodeResult { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SMSID { get; set; }
    }
}

