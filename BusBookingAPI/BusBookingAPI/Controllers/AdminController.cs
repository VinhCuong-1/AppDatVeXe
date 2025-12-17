using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusBookingAPI.Models;
using BusBookingAPI.Services;
using Microsoft.EntityFrameworkCore;
using BusBookingAPI.Data;

namespace BusBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IBookingService bookingService, 
            ApplicationDbContext context,
            ILogger<AdminController> logger)
        {
            _bookingService = bookingService;
            _context = context;
            _logger = logger;
        }

        // Kiểm tra quyền Admin
        private bool IsAdmin()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            return role == "Admin";
        }

        // GET: api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<ApiResponse<object>>> GetDashboard()
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var totalBookings = await _context.Bookings.CountAsync();
                var totalTrips = await _context.Trips.CountAsync();
                var totalUsers = await _context.Users.CountAsync();
                var todayBookings = await _context.Bookings
                    .Where(b => b.BookingTime.Date == DateTime.Today)
                    .CountAsync();

                var dashboard = new
                {
                    TotalBookings = totalBookings,
                    TotalTrips = totalTrips,
                    TotalUsers = totalUsers,
                    TodayBookings = todayBookings,
                    LastUpdated = DateTime.UtcNow
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Lấy thông tin dashboard thành công",
                    Data = dashboard
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy thông tin dashboard"
                });
            }
        }

        // GET: api/admin/trips (Returns unique trip templates)
        [HttpGet("trips")]
        public async Task<ActionResult<ApiResponse<List<TripDto>>>> GetAllTrips()
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                // Get all active/paused trips from database
                var allTrips = await _context.Trips
                    .Include(t => t.Route)
                    .Where(t => t.Status == "Active" || t.Status == "Paused")
                    .ToListAsync();

                // Group trips in memory to get unique templates
                var tripTemplates = allTrips
                    .GroupBy(t => new
                    {
                        t.RouteId,
                        t.BusName,
                        DriverName = t.DriverName ?? "",
                        t.TotalSeats,
                        Hour = t.StartTime.Hour,
                        Minute = t.StartTime.Minute,
                        t.Status
                    })
                    .Select(g => g.OrderByDescending(t => t.StartTime).First())
                    .OrderBy(t => t.StartTime.Hour)
                    .ThenBy(t => t.StartTime.Minute)
                    .ToList();

                // Build DTOs with available seats
                var trips = new List<TripDto>();
                foreach (var t in tripTemplates)
                {
                    var availableSeats = await _context.Seats
                        .Where(s => s.TripId == t.Id && !s.IsBooked)
                        .CountAsync();

                    trips.Add(new TripDto
                    {
                        Id = t.Id,
                        RouteId = t.RouteId,
                        BusName = t.BusName,
                        DriverName = t.DriverName,
                        StartTime = t.StartTime,
                        TotalSeats = t.TotalSeats,
                        AvailableSeats = availableSeats,
                        Status = t.Status,
                        Route = new RouteDto
                        {
                            Id = t.Route!.Id,
                            Departure = t.Route.Departure,
                            Destination = t.Route.Destination
                        }
                    });
                }

                return Ok(new ApiResponse<List<TripDto>>
                {
                    Success = true,
                    Message = "Lấy danh sách chuyến xe thành công",
                    Data = trips
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all trips: {Message}", ex.Message);
                return BadRequest(new ApiResponse<List<TripDto>>
                {
                    Success = false,
                    Message = $"Có lỗi xảy ra: {ex.Message}",
                    Data = new List<TripDto>()
                });
            }
        }

        // POST: api/admin/trips
        [HttpPost("trips")]
        public async Task<ActionResult<ApiResponse<TripDto>>> CreateTrip([FromBody] CreateTripRequest request)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                // Validate route exists
                var route = await _context.Routes.FindAsync(request.RouteId);
                if (route == null)
                {
                    return BadRequest(new ApiResponse<TripDto>
                    {
                        Success = false,
                        Message = "Tuyến đường không tồn tại"
                    });
                }

                var trip = new Trip
                {
                    RouteId = request.RouteId,
                    BusName = request.BusName,
                    DriverName = request.DriverName ?? "Chưa phân công", // Default if not provided
                    StartTime = request.StartTime,
                    TotalSeats = request.TotalSeats,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();

                // Create seats for the trip
                var seats = new List<Seat>();
                int seatsPerRow = 4; // 4 seats per row (A1-A4, B1-B4, etc.)
                int totalRows = (int)Math.Ceiling((double)request.TotalSeats / seatsPerRow);
                
                for (int row = 0; row < totalRows && seats.Count < request.TotalSeats; row++)
                {
                    char rowLetter = (char)('A' + row);
                    for (int num = 1; num <= seatsPerRow && seats.Count < request.TotalSeats; num++)
                    {
                        seats.Add(new Seat
                        {
                            TripId = trip.Id,
                            SeatNumber = $"{rowLetter}{num}",
                            IsBooked = false
                        });
                    }
                }
                
                _context.Seats.AddRange(seats);
                await _context.SaveChangesAsync();

                var tripDto = new TripDto
                {
                    Id = trip.Id,
                    RouteId = trip.RouteId,
                    BusName = trip.BusName,
                    DriverName = trip.DriverName,
                    StartTime = trip.StartTime,
                    TotalSeats = trip.TotalSeats,
                    AvailableSeats = seats.Count, // All seats are available initially
                    Status = trip.Status,
                    Route = new RouteDto
                    {
                        Id = route.Id,
                        Departure = route.Departure,
                        Destination = route.Destination
                    }
                };

                return Ok(new ApiResponse<TripDto>
                {
                    Success = true,
                    Message = "Thêm chuyến xe thành công",
                    Data = tripDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trip");
                return BadRequest(new ApiResponse<TripDto>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi thêm chuyến xe"
                });
            }
        }

        // PUT: api/admin/trips/{id}/pause (pauses all trips with same template)
        [HttpPut("trips/{id}/pause")]
        public async Task<ActionResult<ApiResponse<bool>>> PauseTrip(int id)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy chuyến xe"
                    });
                }

                // Pause all trips with same template (same hour, minute, route, bus, driver)
                var tripsToUpdate = await _context.Trips
                    .Where(t => t.RouteId == trip.RouteId &&
                               t.BusName == trip.BusName &&
                               t.DriverName == trip.DriverName &&
                               t.TotalSeats == trip.TotalSeats &&
                               t.StartTime.Hour == trip.StartTime.Hour &&
                               t.StartTime.Minute == trip.StartTime.Minute &&
                               t.Status == "Active")
                    .ToListAsync();

                foreach (var t in tripsToUpdate)
                {
                    t.Status = "Paused";
                }

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = $"Tạm ngưng {tripsToUpdate.Count} chuyến xe thành công",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pausing trip");
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tạm ngưng chuyến xe"
                });
            }
        }

        // PUT: api/admin/trips/{id}/resume (resumes all trips with same template)
        [HttpPut("trips/{id}/resume")]
        public async Task<ActionResult<ApiResponse<bool>>> ResumeTrip(int id)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy chuyến xe"
                    });
                }

                // Resume all trips with same template
                var tripsToUpdate = await _context.Trips
                    .Where(t => t.RouteId == trip.RouteId &&
                               t.BusName == trip.BusName &&
                               t.DriverName == trip.DriverName &&
                               t.TotalSeats == trip.TotalSeats &&
                               t.StartTime.Hour == trip.StartTime.Hour &&
                               t.StartTime.Minute == trip.StartTime.Minute &&
                               t.Status == "Paused")
                    .ToListAsync();

                foreach (var t in tripsToUpdate)
                {
                    t.Status = "Active";
                }

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = $"Hủy tạm ngưng {tripsToUpdate.Count} chuyến xe thành công",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resuming trip");
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi hủy tạm ngưng chuyến xe"
                });
            }
        }

        // PUT: api/admin/trips/{id} (updates all trips with same template)
        [HttpPut("trips/{id}")]
        public async Task<ActionResult<ApiResponse<TripDto>>> UpdateTrip(int id, [FromBody] CreateTripRequest request)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound(new ApiResponse<TripDto>
                    {
                        Success = false,
                        Message = "Không tìm thấy chuyến xe"
                    });
                }

                // Validate route exists
                var route = await _context.Routes.FindAsync(request.RouteId);
                if (route == null)
                {
                    return BadRequest(new ApiResponse<TripDto>
                    {
                        Success = false,
                        Message = "Tuyến đường không tồn tại"
                    });
                }

                // Find all trips with same OLD template
                var oldTrips = await _context.Trips
                    .Where(t => t.RouteId == trip.RouteId &&
                               t.BusName == trip.BusName &&
                               t.DriverName == trip.DriverName &&
                               t.TotalSeats == trip.TotalSeats &&
                               t.StartTime.Hour == trip.StartTime.Hour &&
                               t.StartTime.Minute == trip.StartTime.Minute)
                    .ToListAsync();

                // Update all matching trips
                foreach (var t in oldTrips)
                {
                    // Preserve the date, only update the time
                    var newStartTime = new DateTime(
                        t.StartTime.Year,
                        t.StartTime.Month,
                        t.StartTime.Day,
                        request.StartTime.Hour,
                        request.StartTime.Minute,
                        0
                    );

                    t.RouteId = request.RouteId;
                    t.BusName = request.BusName;
                    t.DriverName = request.DriverName ?? "Chưa phân công";
                    t.StartTime = newStartTime;
                    t.TotalSeats = request.TotalSeats;
                }

                await _context.SaveChangesAsync();

                var tripDto = new TripDto
                {
                    Id = trip.Id,
                    RouteId = trip.RouteId,
                    BusName = trip.BusName,
                    DriverName = trip.DriverName,
                    StartTime = trip.StartTime,
                    TotalSeats = trip.TotalSeats,
                    AvailableSeats = await _context.Seats.CountAsync(s => s.TripId == trip.Id && !s.IsBooked),
                    Status = trip.Status,
                    Route = new RouteDto
                    {
                        Id = route.Id,
                        Departure = route.Departure,
                        Destination = route.Destination
                    }
                };

                return Ok(new ApiResponse<TripDto>
                {
                    Success = true,
                    Message = "Cập nhật chuyến xe thành công",
                    Data = tripDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating trip");
                return BadRequest(new ApiResponse<TripDto>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi cập nhật chuyến xe"
                });
            }
        }

        // DELETE (Hard Delete): api/admin/trips/{id}/delete (deletes all trips with same template)
        [HttpDelete("trips/{id}/delete")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTrip(int id)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy chuyến xe"
                    });
                }

                // Find all trips with same template
                var tripsToDelete = await _context.Trips
                    .Where(t => t.RouteId == trip.RouteId &&
                               t.BusName == trip.BusName &&
                               t.DriverName == trip.DriverName &&
                               t.TotalSeats == trip.TotalSeats &&
                               t.StartTime.Hour == trip.StartTime.Hour &&
                               t.StartTime.Minute == trip.StartTime.Minute)
                    .ToListAsync();

                // Check if any of these trips have active bookings
                var tripIds = tripsToDelete.Select(t => t.Id).ToList();
                var hasBookings = await _context.Bookings
                    .AnyAsync(b => tripIds.Contains(b.TripId) && b.Status != "Cancelled");

                if (hasBookings)
                {
                    return BadRequest(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không thể xóa chuyến xe đã có người đặt vé"
                    });
                }

                // Delete seats for all trips
                var seats = await _context.Seats.Where(s => tripIds.Contains(s.TripId)).ToListAsync();
                _context.Seats.RemoveRange(seats);

                // Delete all trips
                _context.Trips.RemoveRange(tripsToDelete);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = $"Xóa {tripsToDelete.Count} chuyến xe thành công",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting trip");
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi xóa chuyến xe"
                });
            }
        }

        // GET: api/admin/bookings
        [HttpGet("bookings")]
        public async Task<ActionResult<ApiResponse<List<BookingDto>>>> GetAllBookings([FromQuery] string? phone = null)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var query = _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t!.Route)
                    .Include(b => b.User)
                    .AsQueryable();

                // Filter by phone if provided
                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(b => b.Phone.Contains(phone));
                }

                var bookings = await query
                    .OrderByDescending(b => b.BookingTime)
                    .Select(b => new BookingDto
                    {
                        Id = b.Id,
                        TripId = b.TripId,
                        UserId = b.UserId,
                        SeatNumber = b.SeatNumber,
                        HolderName = b.HolderName,
                        Phone = b.Phone,
                        Status = b.Status,
                        PaymentStatus = b.PaymentStatus,
                        QRToken = b.QRToken,
                        BookingTime = b.BookingTime,
                        ExpiresAt = b.ExpiresAt,
                        PickupPoint = b.PickupPoint,
                        Trip = new TripDto
                        {
                            Id = b.Trip!.Id,
                            RouteId = b.Trip.RouteId,
                            BusName = b.Trip.BusName,
                            StartTime = b.Trip.StartTime,
                            TotalSeats = b.Trip.TotalSeats,
                            Status = b.Trip.Status,
                            Route = new RouteDto
                            {
                                Id = b.Trip.Route!.Id,
                                Departure = b.Trip.Route.Departure,
                                Destination = b.Trip.Route.Destination
                            }
                        }
                    })
                    .ToListAsync();

                return Ok(new ApiResponse<List<BookingDto>>
                {
                    Success = true,
                    Message = "Lấy danh sách đặt vé thành công",
                    Data = bookings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all bookings");
                return BadRequest(new ApiResponse<List<BookingDto>>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy danh sách đặt vé",
                    Data = new List<BookingDto>()
                });
            }
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền truy cập" });
            }

            try
            {
                var users = await _context.Users
                    .OrderBy(u => u.CreatedAt)
                    .Select(u => new UserDto
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        Phone = u.PhoneNumber!,
                        Email = u.Email!,
                        Role = u.Role,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(new ApiResponse<List<UserDto>>
                {
                    Success = true,
                    Message = "Lấy danh sách người dùng thành công",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return BadRequest(new ApiResponse<List<UserDto>>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy danh sách người dùng",
                    Data = new List<UserDto>()
                });
            }
        }

        // POST: api/admin/checkin
        [HttpPost("checkin")]
        public async Task<ActionResult<ApiResponse<CheckinLogDto>>> CheckinBooking([FromBody] CheckinRequest request)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền check-in" });
            }

            var adminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CheckinBookingAsync(request, adminId);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET: api/admin/verify
        [HttpGet("verify")]
        public async Task<ActionResult<ApiResponse<BookingDto?>>> VerifyBooking([FromQuery] string token)
        {
            if (!IsAdmin())
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

        // DELETE: api/admin/bookings/{id}
        [HttpDelete("bookings/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelBooking(int id)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền hủy vé" });
            }

            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy vé"
                    });
                }

                // Update booking status
                booking.Status = "Cancelled";

                // Update seat status
                var seat = await _context.Seats
                    .FirstOrDefaultAsync(s => s.TripId == booking.TripId && s.SeatNumber == booking.SeatNumber);
                if (seat != null)
                {
                    seat.IsBooked = false;
                }

                // Create audit log
                var adminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var auditLog = new AuditLog
                {
                    UserId = adminId!,
                    Action = "AdminCancelBooking",
                    Description = $"Admin hủy vé - BookingId: {id}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Hủy vé thành công",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling booking {BookingId}", id);
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi hủy vé",
                    Data = false
                });
            }
        }

        // DELETE HARD: api/admin/bookings/{id}/hard
        [HttpDelete("bookings/{id}/hard")]
        public async Task<ActionResult<ApiResponse<bool>>> HardDeleteBooking(int id)
        {
            if (!IsAdmin())
            {
                return StatusCode(403, new { message = "Chỉ Admin mới có quyền xóa vé" });
            }

            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy vé"
                    });
                }

                // Xóa các CheckinLogs liên quan (nếu có)
                var checkinLogs = await _context.CheckinLogs.Where(c => c.BookingId == id).ToListAsync();
                if (checkinLogs.Any())
                {
                    _context.CheckinLogs.RemoveRange(checkinLogs);
                }

                // Trả ghế về trống trước khi xóa
                var seat = await _context.Seats.FirstOrDefaultAsync(s => s.TripId == booking.TripId && s.SeatNumber == booking.SeatNumber);
                if (seat != null)
                {
                    seat.IsBooked = false;
                }

                _context.Bookings.Remove(booking);

                var adminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = adminId!,
                    Action = "AdminHardDeleteBooking",
                    Description = $"Admin xóa vé (hard) - BookingId: {id}",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Xóa vé thành công",
                    Data = true
                });
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi xóa vé",
                    Data = false
                });
            }
        }
    }
}
