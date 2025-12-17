using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusBookingAPI.Models;
using BusBookingAPI.Services;

namespace BusBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckinController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<CheckinController> _logger;

        public CheckinController(IBookingService bookingService, ILogger<CheckinController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CheckinLogDto>>> Checkin([FromBody] CheckinRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<CheckinLogDto>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ"
                });
            }

            // Kiểm tra quyền Admin
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền check-in" });
            }

            var staffId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CheckinBookingAsync(request, staffId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("verify")]
        public async Task<ActionResult<ApiResponse<BookingDto?>>> VerifyBooking([FromQuery] string token)
        {
            // Kiểm tra quyền Admin
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền xác thực vé" });
            }

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new ApiResponse<BookingDto?>
                {
                    Success = false,
                    Message = "Token không được để trống"
                });
            }

            var result = await _bookingService.VerifyBookingAsync(token);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Data == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
