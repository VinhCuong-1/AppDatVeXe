using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusBookingAPI.Data;
using BusBookingAPI.Models;

namespace BusBookingAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                _logger.LogInformation($"Login attempt for: {request.Phone}");
                
                // Tìm user bằng Email hoặc Phone
                var user = await _userManager.FindByEmailAsync(request.Phone) ?? 
                          await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.Phone);
                
                if (user == null)
                {
                    _logger.LogWarning($"User not found for: {request.Phone}");
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email/Số điện thoại hoặc mật khẩu không đúng"
                    };
                }

                _logger.LogInformation($"User found: {user.Email}, Phone: {user.PhoneNumber}, Role: {user.Role}");
                
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Password check failed for user: {user.Email}");
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Số điện thoại hoặc mật khẩu không đúng"
                    };
                }

                var token = await GenerateJwtTokenAsync(user);
                var userDto = MapToUserDto(user);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    User = userDto,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for phone: {Phone}", request.Phone);
                return new AuthResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi đăng nhập"
                };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Validate role
                var validRoles = new[] { "Customer", "Admin" };
                if (!validRoles.Contains(request.Role))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Role không hợp lệ. Chỉ chấp nhận: Customer, Admin"
                    };
                }

                var existingUser = await _userManager.FindByNameAsync(request.Phone);
                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Số điện thoại đã được sử dụng"
                    };
                }

                var existingEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email đã được sử dụng"
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email, // Sử dụng Email làm UserName
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    FullName = request.FullName,
                    Role = request.Role,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Có lỗi xảy ra khi tạo tài khoản",
                        Errors = errors
                    };
                }

                var token = await GenerateJwtTokenAsync(user);
                var userDto = MapToUserDto(user);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Đăng ký thành công",
                    User = userDto,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for phone: {Phone}", request.Phone);
                return new AuthResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi đăng ký"
                };
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                return user != null ? MapToUserDto(user) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", userId);
                return null;
            }
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expireHours = int.Parse(jwtSettings["ExpireHours"]!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new("FullName", user.FullName),
                new(ClaimTypes.Role, user.Role), // ⬅️ Sửa thành ClaimTypes.Role
                new("Phone", user.PhoneNumber!)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"Reset password attempt for: {request.Phone}");
                
                // Tìm user bằng Phone
                var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.Phone);
                
                if (user == null)
                {
                    _logger.LogWarning($"User not found for: {request.Phone}");
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Số điện thoại không tồn tại"
                    };
                }

                // Verify OTP (cần inject OtpService hoặc check trực tiếp từ database)
                var otpRecord = await _context.OtpCodes
                    .Where(o => o.Phone == request.Phone && o.Code == request.Otp)
                    .OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync();

                if (otpRecord == null || otpRecord.ExpiresAt < DateTime.UtcNow || otpRecord.IsUsed)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Mã OTP không hợp lệ hoặc đã hết hạn"
                    };
                }

                // Mark OTP as used
                otpRecord.IsUsed = true;
                await _context.SaveChangesAsync();

                // Reset password using UserManager
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không thể đặt lại mật khẩu",
                        Data = errors
                    };
                }

                _logger.LogInformation($"Password reset successful for: {request.Phone}");
                
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập lại."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for phone: {Phone}", request.Phone);
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi đặt lại mật khẩu"
                };
            }
        }

        private static UserDto MapToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Phone = user.PhoneNumber!,
                Email = user.Email!,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
