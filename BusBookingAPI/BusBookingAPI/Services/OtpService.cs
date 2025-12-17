using BusBookingAPI.Data;
using BusBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BusBookingAPI.Services
{
    public class OtpService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISmsService _smsService;
        private readonly ILogger<OtpService> _logger;
        private const int OTP_EXPIRY_MINUTES = 5;
        private const int MAX_ATTEMPTS = 3;

        public OtpService(ApplicationDbContext context, ISmsService smsService, ILogger<OtpService> logger)
        {
            _context = context;
            _smsService = smsService;
            _logger = logger;
        }

        public async Task<(bool Success, string Message, DateTime? ExpiresAt)> SendOtpAsync(string phone)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
                if (user == null)
                {
                    return (false, "Số điện thoại không tồn tại trong hệ thống", null);
                }

                var oldOtps = await _context.OtpCodes.Where(o => o.Phone == phone).ToListAsync();
                _context.OtpCodes.RemoveRange(oldOtps);

                var random = new Random();
                var otpCode = random.Next(100000, 999999).ToString();
                var expiresAt = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES);

                var otp = new OtpCode
                {
                    Phone = phone,
                    Code = otpCode,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false,
                    AttemptCount = 0
                };

                _context.OtpCodes.Add(otp);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"OTP cho {phone}: {otpCode} (Hết hạn: {expiresAt})");

                // Gửi SMS qua eSMS
                var (smsSuccess, smsMessage) = await _smsService.SendOtpAsync(phone, otpCode);
                
                if (!smsSuccess)
                {
                    _logger.LogError($"Failed to send SMS to {phone}: {smsMessage}");
                    // Vẫn trả về success = true vì OTP đã được tạo, chỉ SMS fail
                    // Trong trường hợp này, người dùng có thể thử lại hoặc dùng phương thức khác
                }

                return (true, smsMessage, expiresAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending OTP to {phone}");
                return (false, "Có lỗi xảy ra khi gửi mã OTP", null);
            }
        }

        public async Task<(bool Success, string Message, ApplicationUser? User)> VerifyOtpAsync(string phone, string otpCode)
        {
            try
            {
                var otp = await _context.OtpCodes
                    .Where(o => o.Phone == phone && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
                    .OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync();

                if (otp == null)
                {
                    return (false, "Mã OTP không hợp lệ hoặc đã hết hạn", null);
                }

                otp.AttemptCount++;

                if (otp.AttemptCount > MAX_ATTEMPTS)
                {
                    otp.IsUsed = true;
                    await _context.SaveChangesAsync();
                    return (false, "Bạn đã nhập sai quá số lần cho phép", null);
                }

                if (otp.Code != otpCode)
                {
                    await _context.SaveChangesAsync();
                    return (false, $"Mã OTP không chính xác. Còn {MAX_ATTEMPTS - otp.AttemptCount} lần thử", null);
                }

                otp.IsUsed = true;
                await _context.SaveChangesAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
                if (user == null)
                {
                    return (false, "Không tìm thấy người dùng", null);
                }

                return (true, "Xác thực thành công", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying OTP for {phone}");
                return (false, "Có lỗi xảy ra khi xác thực mã OTP", null);
            }
        }
    }
}

