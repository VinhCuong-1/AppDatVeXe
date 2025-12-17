using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusBookingAPI.Models;
using BusBookingAPI.Services;

namespace BusBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IBookingService bookingService, ILogger<TripsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("search")]
        public async Task<ActionResult<ApiResponse<List<TripDto>>>> SearchTrips([FromBody] SearchTripsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<List<TripDto>>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = new List<TripDto>()
                });
            }

            var result = await _bookingService.SearchTripsAsync(request);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TripDto?>>> GetTripDetails(int id)
        {
            var result = await _bookingService.GetTripDetailsAsync(id);
            
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

        [HttpGet("{id}/seats")]
        public async Task<ActionResult<ApiResponse<List<SeatDto>>>> GetTripSeats(int id)
        {
            var result = await _bookingService.GetTripSeatsAsync(id);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
