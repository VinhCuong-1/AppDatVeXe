using BusBookingAPI.Models;
using BusBookingAPI.Services;
using BusBookingAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;
        private readonly IAuthService _authService;
        private readonly ILogger<OtpController> _logger;

        public OtpController(
            OtpService otpService,
            IAuthService authService,
            ILogger<OtpController> logger)
        {
            _otpService = otpService;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// POST: api/otp/send
        /// Gửi mã OTP đến số điện thoại
        /// </summary>
        [HttpPost("send")]
        public async Task<ActionResult<OtpResponse>> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                var (success, message, expiresAt) = await _otpService.SendOtpAsync(request.Phone);

                if (!success)
                {
                    return BadRequest(new OtpResponse
                    {
                        Success = false,
                        Message = message
                    });
                }

                return Ok(new OtpResponse
                {
                    Success = true,
                    Message = message,
                    ExpiresAt = expiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendOtp");
                return StatusCode(500, new OtpResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi gửi mã OTP"
                });
            }
        }

        /// <summary>
        /// POST: api/otp/verify
        /// Xác thực mã OTP và đăng nhập
        /// </summary>
        [HttpPost("verify")]
        public async Task<ActionResult<AuthResponse>> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                var (success, message, user) = await _otpService.VerifyOtpAsync(request.Phone, request.Otp);

                if (!success || user == null)
                {
                    return BadRequest(new AuthResponse
                    {
                        Success = false,
                        Message = message
                    });
                }

                // Tạo token trực tiếp từ AuthService
                var token = await _authService.GenerateJwtTokenAsync(user);
                
                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công bằng OTP",
                    User = new UserDto
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Phone = user.PhoneNumber!,
                        Email = user.Email!,
                        Role = user.Role,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt
                    },
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyOtp");
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi xác thực mã OTP"
                });
            }
        }
    }
}

