using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusBookingAPI.Models
{
    public class LoginRequest
    {
        [Required]
        public string Phone { get; set; } = string.Empty; // Có thể là Email hoặc Phone
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class SendOtpRequest
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }

    public class VerifyOtpRequest
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class OtpResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = "Customer";
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
        public string? Token { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }

    public class SearchTripsRequest
    {
        [Required]
        public string From { get; set; } = string.Empty;
        
        [Required]
        public string To { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
    }

    public class CreateTripRequest
    {
        [Required]
        public int RouteId { get; set; }
        
        [Required]
        public string BusName { get; set; } = string.Empty;
        
        public string? DriverName { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Range(1, 100)]
        [DefaultValue(40)]
        public int TotalSeats { get; set; } = 40; // Mặc định 40 ghế
    }

    public class TripDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string? DriverName { get; set; } // Tên tài xế
        public DateTime StartTime { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; } // Số ghế còn trống
        public string Status { get; set; } = string.Empty;
        public RouteDto? Route { get; set; }
    }

    public class RouteDto
    {
        public int Id { get; set; }
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }

    public class SeatDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
    }

    public class CreateBookingRequest
    {
        [Required]
        public int TripId { get; set; }
        
        [Required]
        public string SeatNumber { get; set; } = string.Empty;
        
        [Required]
        public string HolderName { get; set; } = string.Empty;
        
        [Required]
        public string Phone { get; set; } = string.Empty;
        
        public string? PickupPoint { get; set; } // Optional: "Dọc tuyến đường" or "Bến xe miền đông"
    }

    public class BookingDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string QRToken { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? PickupPoint { get; set; }
        public TripDto? Trip { get; set; }
    }

    public class CheckinRequest
    {
        [Required]
        public string QRToken { get; set; } = string.Empty;
        
        [Required]
        public string CheckinPoint { get; set; } = string.Empty;
    }

    public class CheckinLogDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string StaffId { get; set; } = string.Empty;
        public string CheckinPoint { get; set; } = string.Empty;
        public DateTime CheckinTime { get; set; }
    }

    public class CreateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
    }

    public class UpdateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
