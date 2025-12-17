using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusBookingAPI.Data;
using BusBookingAPI.Models;

namespace BusBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Select(u => new UserDto
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        Email = u.Email ?? string.Empty,
                        Phone = u.PhoneNumber ?? string.Empty,
                        Role = u.Role,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách người dùng" });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng" });
                }

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    Phone = user.PhoneNumber ?? string.Empty,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", id);
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin người dùng" });
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                // Check if phone already exists
                var existingUserByPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.Phone);
                if (existingUserByPhone != null)
                {
                    return BadRequest(new { message = "Số điện thoại đã tồn tại" });
                }

                // Check if email already exists
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
                    if (existingUserByEmail != null)
                    {
                        return BadRequest(new { message = "Email đã tồn tại" });
                    }
                }

                var user = new ApplicationUser
                {
                    UserName = request.Phone, // Use phone as username
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    Role = request.Role ?? "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { message = $"Lỗi khi tạo người dùng: {errors}" });
                }

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    Phone = user.PhoneNumber ?? string.Empty,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                };

                _logger.LogInformation("Created new user: {Phone}", user.PhoneNumber);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { message = "Lỗi khi tạo người dùng" });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng" });
                }

                // Check if phone is being changed and already exists
                if (request.Phone != user.PhoneNumber)
                {
                    var existingUserByPhone = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.PhoneNumber == request.Phone && u.Id != id);
                    if (existingUserByPhone != null)
                    {
                        return BadRequest(new { message = "Số điện thoại đã tồn tại" });
                    }
                }

                // Check if email is being changed and already exists
                if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
                {
                    var existingUserByEmail = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Email == request.Email && u.Id != id);
                    if (existingUserByEmail != null)
                    {
                        return BadRequest(new { message = "Email đã tồn tại" });
                    }
                }

                user.FullName = request.FullName;
                user.Email = request.Email;
                user.PhoneNumber = request.Phone;
                user.UserName = request.Phone; // Keep username in sync with phone
                user.Role = request.Role ?? user.Role;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    return BadRequest(new { message = $"Lỗi khi cập nhật người dùng: {errors}" });
                }

                // Only update password if provided
                if (!string.IsNullOrEmpty(request.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                    if (!passwordResult.Succeeded)
                    {
                        var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                        return BadRequest(new { message = $"Lỗi khi đổi mật khẩu: {errors}" });
                    }
                }

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    Phone = user.PhoneNumber ?? string.Empty,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                };

                _logger.LogInformation("Updated user: {UserId}", id);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, new { message = "Lỗi khi cập nhật người dùng" });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng" });
                }

                // Check if user has bookings
                var hasBookings = await _context.Bookings.AnyAsync(b => b.UserId == id);
                if (hasBookings)
                {
                    // Deactivate instead of delete
                    user.IsActive = false;
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("Deactivated user with bookings: {UserId}", id);
                    return Ok(new { message = "Đã vô hiệu hóa người dùng (có lịch sử đặt vé)" });
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { message = $"Lỗi khi xóa người dùng: {errors}" });
                }

                _logger.LogInformation("Deleted user: {UserId}", id);
                return Ok(new { message = "Đã xóa người dùng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, new { message = "Lỗi khi xóa người dùng" });
            }
        }

        // PUT: api/users/{id}/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng" });
                }

                user.IsActive = !user.IsActive;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { message = $"Lỗi khi thay đổi trạng thái: {errors}" });
                }

                _logger.LogInformation("Toggled status for user: {UserId} to {IsActive}", id, user.IsActive);
                return Ok(new { message = user.IsActive ? "Đã kích hoạt người dùng" : "Đã vô hiệu hóa người dùng", isActive = user.IsActive });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling user status {UserId}", id);
                return StatusCode(500, new { message = "Lỗi khi thay đổi trạng thái người dùng" });
            }
        }
    }
}
