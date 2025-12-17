using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusBookingAPI.Models;

namespace BusBookingAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for business entities
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<CheckinLog> CheckinLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Trip>()
                .HasOne(t => t.Route)
                .WithMany()
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Seat>()
                .HasOne(s => s.Trip)
                .WithMany()
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .HasOne(b => b.Trip)
                .WithMany()
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CheckinLog>()
                .HasOne(c => c.Booking)
                .WithMany()
                .HasForeignKey(c => c.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CheckinLog>()
                .HasOne(c => c.Staff)
                .WithMany()
                .HasForeignKey(c => c.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes
            builder.Entity<Models.Route>()
                .HasIndex(r => new { r.Departure, r.Destination });

            builder.Entity<Trip>()
                .HasIndex(t => t.StartTime);

            builder.Entity<Booking>()
                .HasIndex(b => b.QRToken)
                .IsUnique();

            builder.Entity<Booking>()
                .HasIndex(b => new { b.TripId, b.SeatNumber });

            builder.Entity<Seat>()
                .HasIndex(s => new { s.TripId, s.SeatNumber })
                .IsUnique();

            builder.Entity<OtpCode>()
                .HasIndex(o => o.Phone);

            builder.Entity<OtpCode>()
                .HasIndex(o => new { o.Phone, o.ExpiresAt });

            // Seed data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var today = DateTime.UtcNow.Date;
            
            // Seed Routes (6 locations -> create both directions for each pair)
            var locations = new [] { "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Lâm Đồng", "Hải Phòng", "Phú Yên" };
            var routeId = 1;
            var routes = new List<Models.Route>();
            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = 0; j < locations.Length; j++)
                {
                    if (i == j) continue;
                    routes.Add(new Models.Route { Id = routeId++, Departure = locations[i], Destination = locations[j], CreatedAt = seedDate });
                }
            }
            builder.Entity<Models.Route>().HasData(routes);

            // Seed Trips: for each route, 2 trips per time slot (morning/afternoon/evening)
            var trips = new List<Trip>();
            int tripId = 1;
            var morning1 = new TimeSpan(6, 30, 0); // 06:30
            var morning2 = new TimeSpan(8, 15, 0); // 08:15
            var afternoon1 = new TimeSpan(12, 0, 0); // 12:00
            var afternoon2 = new TimeSpan(14, 30, 0); // 14:30
            var evening1 = new TimeSpan(18, 0, 0); // 18:00
            var evening2 = new TimeSpan(19, 45, 0); // 19:45

            foreach (var r in routes)
            {
                // Morning trips
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 1", StartTime = today.Add(morning1), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 2", StartTime = today.Add(morning2), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
                
                // Afternoon trips
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 3", StartTime = today.Add(afternoon1), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 4", StartTime = today.Add(afternoon2), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
                
                // Evening trips
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 1", StartTime = today.Add(evening1), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
                trips.Add(new Trip { Id = tripId++, RouteId = r.Id, BusName = "Ngũ An", DriverName = "Tài 2", StartTime = today.Add(evening2), TotalSeats = 40, Status = "Active", CreatedAt = seedDate });
            }
            builder.Entity<Trip>().HasData(trips);

            // Seed Seats for each trip
            var seatId = 1;
            for (int tId = 1; tId <= trips.Count; tId++)
            {
                for (char row = 'A'; row <= 'J'; row++)
                {
                    for (int seatNum = 1; seatNum <= 4; seatNum++)
                    {
                        builder.Entity<Seat>().HasData(
                            new Seat
                            {
                                Id = seatId++,
                                TripId = tId,
                                SeatNumber = $"{row}{seatNum}",
                                IsBooked = false
                            }
                        );
                    }
                }
            }
        }
    }
}
