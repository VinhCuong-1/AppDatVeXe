using System.ComponentModel.DataAnnotations;
using BusBookingAPI.Data;

namespace BusBookingAPI.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Trip
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string? DriverName { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalSeats { get; set; } = 40;
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Route? Route { get; set; }
    }

    public class Seat
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsBooked { get; set; } = false;

        // Navigation properties
        public Trip? Trip { get; set; }
    }

    public class Booking
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "Reserved";
        public string PaymentStatus { get; set; } = "Unpaid";
        public string QRToken { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public string? PickupPoint { get; set; } // "Dọc tuyến đường" or "Bến xe miền đông"

        // Navigation properties
        public Trip? Trip { get; set; }
        public ApplicationUser? User { get; set; }
    }

    public class CheckinLog
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string StaffId { get; set; } = string.Empty;
        public string CheckinPoint { get; set; } = string.Empty;
        public DateTime CheckinTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Booking? Booking { get; set; }
        public ApplicationUser? Staff { get; set; }
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
    }

    public class OtpCode
    {
        [Key]
        public int Id { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public int AttemptCount { get; set; } = 0;
    }
}
