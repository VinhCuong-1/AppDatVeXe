using Microsoft.EntityFrameworkCore;
using BusBookingAPI.Data;
using BusBookingAPI.Models;

namespace BusBookingAPI.Services
{
    public interface IBookingService
    {
        Task<ApiResponse<List<TripDto>>> SearchTripsAsync(SearchTripsRequest request);
        Task<ApiResponse<TripDto?>> GetTripDetailsAsync(int tripId);
        Task<ApiResponse<List<SeatDto>>> GetTripSeatsAsync(int tripId);
        Task<ApiResponse<BookingDto>> CreateBookingAsync(CreateBookingRequest request, string userId);
        Task<ApiResponse<BookingDto?>> GetBookingAsync(int bookingId);
        Task<ApiResponse<List<BookingDto>>> GetUserBookingsAsync(string userId);
        Task<ApiResponse<bool>> CancelBookingAsync(int bookingId, string userId);
        Task<ApiResponse<CheckinLogDto>> CheckinBookingAsync(CheckinRequest request, string staffId);
        Task<ApiResponse<BookingDto?>> VerifyBookingAsync(string qrToken);
    }

    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ApplicationDbContext context, ILogger<BookingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TripDto>>> SearchTripsAsync(SearchTripsRequest request)
        {
            try
            {
                // Ensure template trips are replicated to requested date if missing
                var matchedRoutes = await _context.Routes
                    .Where(r => r.Departure.Contains(request.From) && r.Destination.Contains(request.To))
                    .ToListAsync();

                foreach (var r in matchedRoutes)
                {
                    // Get all unique time slots (by TimeOfDay) for this route
                    var uniqueTimeSlots = await _context.Trips
                        .Where(t => t.RouteId == r.Id && t.Status == "Active")
                        .GroupBy(t => new { 
                            Hour = t.StartTime.Hour, 
                            Minute = t.StartTime.Minute,
                            t.BusName,
                            t.DriverName,
                            t.TotalSeats
                        })
                        .Select(g => new {
                            TimeOfDay = new TimeSpan(g.Key.Hour, g.Key.Minute, 0),
                            g.Key.BusName,
                            g.Key.DriverName,
                            g.Key.TotalSeats
                        })
                        .ToListAsync();

                    foreach (var slot in uniqueTimeSlots)
                    {
                        var targetStartTime = request.Date.Date.Add(slot.TimeOfDay);
                        
                        // Check if trip already exists for this exact time, bus, and driver
                        var existingTrip = await _context.Trips.AnyAsync(t => 
                            t.RouteId == r.Id && 
                            t.StartTime == targetStartTime && 
                            t.BusName == slot.BusName &&
                            (t.DriverName == slot.DriverName || (t.DriverName == null && slot.DriverName == null)) &&
                            t.Status == "Active");
                        
                        if (!existingTrip)
                        {
                            var newTrip = new Trip
                            {
                                RouteId = r.Id,
                                BusName = slot.BusName,
                                DriverName = slot.DriverName,
                                StartTime = targetStartTime,
                                TotalSeats = slot.TotalSeats,
                                Status = "Active",
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.Trips.Add(newTrip);
                            await _context.SaveChangesAsync();

                            // Create seats
                            var seats = new List<Seat>();
                            for (char row = 'A'; row <= 'J'; row++)
                            {
                                for (int num = 1; num <= 4; num++)
                                {
                                    seats.Add(new Seat
                                    {
                                        TripId = newTrip.Id,
                                        SeatNumber = $"{row}{num}",
                                        IsBooked = false
                                    });
                                }
                            }
                            _context.Seats.AddRange(seats);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                var trips = await _context.Trips
                    .Include(t => t.Route)
                    .Where(t => t.Route!.Departure.Contains(request.From) &&
                               t.Route!.Destination.Contains(request.To) &&
                               t.StartTime.Date == request.Date.Date &&
                               t.Status == "Active")
                    .OrderBy(t => t.StartTime)
                    .Select(t => new TripDto
                    {
                        Id = t.Id,
                        RouteId = t.RouteId,
                        BusName = t.BusName,
                        DriverName = t.DriverName,
                        StartTime = t.StartTime,
                        TotalSeats = t.TotalSeats,
                        AvailableSeats = _context.Seats.Count(s => s.TripId == t.Id && !s.IsBooked),
                        Status = t.Status,
                        Route = new RouteDto
                        {
                            Id = t.Route!.Id,
                            Departure = t.Route.Departure,
                            Destination = t.Route.Destination
                        }
                    })
                    .ToListAsync();

                return new ApiResponse<List<TripDto>>
                {
                    Success = true,
                    Message = "Tìm kiếm chuyến xe thành công",
                    Data = trips
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching trips from {From} to {To} on {Date}", 
                    request.From, request.To, request.Date);
                return new ApiResponse<List<TripDto>>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tìm kiếm chuyến xe",
                    Data = new List<TripDto>()
                };
            }
        }

        public async Task<ApiResponse<TripDto?>> GetTripDetailsAsync(int tripId)
        {
            try
            {
                var trip = await _context.Trips
                    .Include(t => t.Route)
                    .Where(t => t.Id == tripId)
                    .Select(t => new TripDto
                    {
                        Id = t.Id,
                        RouteId = t.RouteId,
                        BusName = t.BusName,
                        StartTime = t.StartTime,
                        TotalSeats = t.TotalSeats,
                        Status = t.Status,
                        Route = new RouteDto
                        {
                            Id = t.Route!.Id,
                            Departure = t.Route.Departure,
                            Destination = t.Route.Destination
                        }
                    })
                    .FirstOrDefaultAsync();

                return new ApiResponse<TripDto?>
                {
                    Success = true,
                    Message = trip != null ? "Lấy thông tin chuyến xe thành công" : "Không tìm thấy chuyến xe",
                    Data = trip
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trip details for tripId: {TripId}", tripId);
                return new ApiResponse<TripDto?>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy thông tin chuyến xe",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<SeatDto>>> GetTripSeatsAsync(int tripId)
        {
            try
            {
                var seats = await _context.Seats
                    .Where(s => s.TripId == tripId)
                    .OrderBy(s => s.SeatNumber)
                    .Select(s => new SeatDto
                    {
                        Id = s.Id,
                        TripId = s.TripId,
                        SeatNumber = s.SeatNumber,
                        IsBooked = s.IsBooked
                    })
                    .ToListAsync();

                return new ApiResponse<List<SeatDto>>
                {
                    Success = true,
                    Message = "Lấy danh sách ghế thành công",
                    Data = seats
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting seats for tripId: {TripId}", tripId);
                return new ApiResponse<List<SeatDto>>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy danh sách ghế",
                    Data = new List<SeatDto>()
                };
            }
        }

        public async Task<ApiResponse<BookingDto>> CreateBookingAsync(CreateBookingRequest request, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if seat is available
                var seat = await _context.Seats
                    .FirstOrDefaultAsync(s => s.TripId == request.TripId && s.SeatNumber == request.SeatNumber);

                if (seat == null)
                {
                    return new ApiResponse<BookingDto>
                    {
                        Success = false,
                        Message = "Không tìm thấy ghế"
                    };
                }

                if (seat.IsBooked)
                {
                    return new ApiResponse<BookingDto>
                    {
                        Success = false,
                        Message = "Ghế đã được đặt"
                    };
                }

                // Check if there's already a booking for this seat
                var existingBooking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.TripId == request.TripId && 
                                            b.SeatNumber == request.SeatNumber && 
                                            b.Status == "Reserved");

                if (existingBooking != null)
                {
                    return new ApiResponse<BookingDto>
                    {
                        Success = false,
                        Message = "Ghế đã được đặt"
                    };
                }

                // Create booking
                var qrToken = $"BOOK-{DateTime.UtcNow:yyyyMMddHHmmss}-{userId}-{Guid.NewGuid():N}";

                // Set expiry to the trip start time (xe xuất bến) theo yêu cầu
                var trip = await _context.Trips.FirstAsync(t => t.Id == request.TripId);

                var booking = new Booking
                {
                    TripId = request.TripId,
                    UserId = userId,
                    SeatNumber = request.SeatNumber,
                    HolderName = request.HolderName,
                    Phone = request.Phone,
                    Status = "Reserved",
                    PaymentStatus = "Unpaid",
                    QRToken = qrToken,
                    BookingTime = DateTime.UtcNow,
                    ExpiresAt = trip.StartTime, // hết hạn đúng giờ xe khởi hành
                    PickupPoint = request.PickupPoint
                };

                _context.Bookings.Add(booking);

                // Update seat status
                seat.IsBooked = true;

                // Create audit log
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = "CreateBooking",
                    Description = $"Tạo vé mới - BookingId: {booking.Id}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Get booking with trip details
                var bookingDto = await _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t!.Route)
                    .Where(b => b.Id == booking.Id)
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
                    .FirstAsync();

                return new ApiResponse<BookingDto>
                {
                    Success = true,
                    Message = "Đặt vé thành công",
                    Data = bookingDto
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating booking for tripId: {TripId}, seat: {SeatNumber}", 
                    request.TripId, request.SeatNumber);
                return new ApiResponse<BookingDto>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi đặt vé"
                };
            }
        }

        public async Task<ApiResponse<BookingDto?>> GetBookingAsync(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t!.Route)
                    .Where(b => b.Id == bookingId)
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
                    .FirstOrDefaultAsync();

                return new ApiResponse<BookingDto?>
                {
                    Success = true,
                    Message = booking != null ? "Lấy thông tin vé thành công" : "Không tìm thấy vé",
                    Data = booking
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking for bookingId: {BookingId}", bookingId);
                return new ApiResponse<BookingDto?>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy thông tin vé",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<BookingDto>>> GetUserBookingsAsync(string userId)
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t!.Route)
                    .Where(b => b.UserId == userId)
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

                return new ApiResponse<List<BookingDto>>
                {
                    Success = true,
                    Message = "Lấy danh sách vé thành công",
                    Data = bookings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user bookings for userId: {UserId}", userId);
                return new ApiResponse<List<BookingDto>>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi lấy danh sách vé",
                    Data = new List<BookingDto>()
                };
            }
        }

        public async Task<ApiResponse<bool>> CancelBookingAsync(int bookingId, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId && b.Status == "Reserved");

                if (booking == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy vé hoặc vé không thể hủy"
                    };
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
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = "CancelBooking",
                    Description = $"Hủy vé - BookingId: {bookingId}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Hủy vé thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error canceling booking for bookingId: {BookingId}", bookingId);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi hủy vé",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<CheckinLogDto>> CheckinBookingAsync(CheckinRequest request, string staffId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.QRToken == request.QRToken && b.Status == "Reserved");

                if (booking == null)
                {
                    return new ApiResponse<CheckinLogDto>
                    {
                        Success = false,
                        Message = "Không tìm thấy vé hoặc vé không hợp lệ"
                    };
                }

                // Update booking status
                booking.Status = "CheckedIn";

                // Create checkin log
                var checkinLog = new CheckinLog
                {
                    BookingId = booking.Id,
                    StaffId = staffId,
                    CheckinPoint = request.CheckinPoint,
                    CheckinTime = DateTime.UtcNow
                };
                _context.CheckinLogs.Add(checkinLog);

                // Create audit log
                var auditLog = new AuditLog
                {
                    UserId = staffId,
                    Action = "Checkin",
                    Description = $"Check-in vé - BookingId: {booking.Id}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var checkinLogDto = new CheckinLogDto
                {
                    Id = checkinLog.Id,
                    BookingId = checkinLog.BookingId,
                    StaffId = checkinLog.StaffId,
                    CheckinPoint = checkinLog.CheckinPoint,
                    CheckinTime = checkinLog.CheckinTime
                };

                return new ApiResponse<CheckinLogDto>
                {
                    Success = true,
                    Message = "Check-in thành công",
                    Data = checkinLogDto
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error checking in booking with QRToken: {QRToken}", request.QRToken);
                return new ApiResponse<CheckinLogDto>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi check-in"
                };
            }
        }

        public async Task<ApiResponse<BookingDto?>> VerifyBookingAsync(string qrToken)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t!.Route)
                    .Where(b => b.QRToken == qrToken)
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
                    .FirstOrDefaultAsync();

                return new ApiResponse<BookingDto?>
                {
                    Success = true,
                    Message = booking != null ? "Xác thực vé thành công" : "Không tìm thấy vé",
                    Data = booking
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying booking with QRToken: {QRToken}", qrToken);
                return new ApiResponse<BookingDto?>
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi xác thực vé",
                    Data = null
                };
            }
        }
    }
}
