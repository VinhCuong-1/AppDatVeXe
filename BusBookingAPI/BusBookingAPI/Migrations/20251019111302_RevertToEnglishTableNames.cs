using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class RevertToEnglishTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenXe_TuyenDuong_MaTuyenDuong",
                table: "ChuyenXe");

            migrationBuilder.DropForeignKey(
                name: "FK_DatVe_ChuyenXe_MaChuyenXe",
                table: "DatVe");

            migrationBuilder.DropForeignKey(
                name: "FK_DatVe_NguoiDung_MaNguoiDung",
                table: "DatVe");

            migrationBuilder.DropForeignKey(
                name: "FK_GheNgoi_ChuyenXe_MaChuyenXe",
                table: "GheNgoi");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungDangNhap_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungDangNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungQuyen_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungQuyen");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungToken_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungToken");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungVaiTro_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungVaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungVaiTro_VaiTro_RoleId",
                schema: "identity",
                table: "NguoiDungVaiTro");

            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyAudit_NguoiDung_MaNguoiDung",
                table: "NhatKyAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyCheckIn_DatVe_MaDatVe",
                table: "NhatKyCheckIn");

            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyCheckIn_NguoiDung_MaNhanVien",
                table: "NhatKyCheckIn");

            migrationBuilder.DropForeignKey(
                name: "FK_VaiTroQuyen_VaiTro_RoleId",
                schema: "identity",
                table: "VaiTroQuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTroQuyen",
                schema: "identity",
                table: "VaiTroQuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VaiTro",
                schema: "identity",
                table: "VaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TuyenDuong",
                table: "TuyenDuong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhatKyCheckIn",
                table: "NhatKyCheckIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhatKyAudit",
                table: "NhatKyAudit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungVaiTro",
                schema: "identity",
                table: "NguoiDungVaiTro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungToken",
                schema: "identity",
                table: "NguoiDungToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungQuyen",
                schema: "identity",
                table: "NguoiDungQuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDungDangNhap",
                schema: "identity",
                table: "NguoiDungDangNhap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung",
                schema: "identity",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GheNgoi",
                table: "GheNgoi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatVe",
                table: "DatVe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChuyenXe",
                table: "ChuyenXe");

            migrationBuilder.RenameTable(
                name: "VaiTroQuyen",
                schema: "identity",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "VaiTro",
                schema: "identity",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "TuyenDuong",
                newName: "Routes");

            migrationBuilder.RenameTable(
                name: "NhatKyCheckIn",
                newName: "CheckinLogs");

            migrationBuilder.RenameTable(
                name: "NhatKyAudit",
                newName: "AuditLogs");

            migrationBuilder.RenameTable(
                name: "NguoiDungVaiTro",
                schema: "identity",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "NguoiDungToken",
                schema: "identity",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "NguoiDungQuyen",
                schema: "identity",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "NguoiDungDangNhap",
                schema: "identity",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "NguoiDung",
                schema: "identity",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "GheNgoi",
                newName: "Seats");

            migrationBuilder.RenameTable(
                name: "DatVe",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "ChuyenXe",
                newName: "Trips");

            migrationBuilder.RenameIndex(
                name: "IX_VaiTroQuyen_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "Routes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DiemDi",
                table: "Routes",
                newName: "Departure");

            migrationBuilder.RenameColumn(
                name: "DiemDen",
                table: "Routes",
                newName: "Destination");

            migrationBuilder.RenameIndex(
                name: "IX_TuyenDuong_DiemDi_DiemDen",
                table: "Routes",
                newName: "IX_Routes_Departure_Destination");

            migrationBuilder.RenameColumn(
                name: "ThoiGianCheckIn",
                table: "CheckinLogs",
                newName: "CheckinTime");

            migrationBuilder.RenameColumn(
                name: "MaNhanVien",
                table: "CheckinLogs",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "MaDatVe",
                table: "CheckinLogs",
                newName: "BookingId");

            migrationBuilder.RenameColumn(
                name: "DiemCheckIn",
                table: "CheckinLogs",
                newName: "CheckinPoint");

            migrationBuilder.RenameIndex(
                name: "IX_NhatKyCheckIn_MaNhanVien",
                table: "CheckinLogs",
                newName: "IX_CheckinLogs_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_NhatKyCheckIn_MaDatVe",
                table: "CheckinLogs",
                newName: "IX_CheckinLogs_BookingId");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "AuditLogs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "AuditLogs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "AuditLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "HanhDong",
                table: "AuditLogs",
                newName: "Action");

            migrationBuilder.RenameIndex(
                name: "IX_NhatKyAudit_MaNguoiDung",
                table: "AuditLogs",
                newName: "IX_AuditLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDungVaiTro_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDungQuyen_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NguoiDungDangNhap_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "XacThucHaiYeuTo",
                table: "AspNetUsers",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "XacNhanSoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "XacNhanEmail",
                table: "AspNetUsers",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "VaiTro",
                table: "AspNetUsers",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "TenDangNhap",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "SoLanTruyCapThatBai",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "SoDienThoai",
                table: "AspNetUsers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "KhoaDen",
                table: "AspNetUsers",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "HoTen",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "ChoPhepKhoa",
                table: "AspNetUsers",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "SoGhe",
                table: "Seats",
                newName: "SeatNumber");

            migrationBuilder.RenameColumn(
                name: "MaChuyenXe",
                table: "Seats",
                newName: "TripId");

            migrationBuilder.RenameColumn(
                name: "DaDat",
                table: "Seats",
                newName: "IsBooked");

            migrationBuilder.RenameIndex(
                name: "IX_GheNgoi_MaChuyenXe_SoGhe",
                table: "Seats",
                newName: "IX_Seats_TripId_SeatNumber");

            migrationBuilder.RenameColumn(
                name: "TrangThaiThanhToan",
                table: "Bookings",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "TrangThai",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ThoiGianDat",
                table: "Bookings",
                newName: "BookingTime");

            migrationBuilder.RenameColumn(
                name: "TenNguoiDat",
                table: "Bookings",
                newName: "HolderName");

            migrationBuilder.RenameColumn(
                name: "SoGhe",
                table: "Bookings",
                newName: "SeatNumber");

            migrationBuilder.RenameColumn(
                name: "SoDienThoai",
                table: "Bookings",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "MaQR",
                table: "Bookings",
                newName: "QRToken");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "MaChuyenXe",
                table: "Bookings",
                newName: "TripId");

            migrationBuilder.RenameColumn(
                name: "HetHanLuc",
                table: "Bookings",
                newName: "ExpiresAt");

            migrationBuilder.RenameIndex(
                name: "IX_DatVe_MaQR",
                table: "Bookings",
                newName: "IX_Bookings_QRToken");

            migrationBuilder.RenameIndex(
                name: "IX_DatVe_MaNguoiDung",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DatVe_MaChuyenXe_SoGhe",
                table: "Bookings",
                newName: "IX_Bookings_TripId_SeatNumber");

            migrationBuilder.RenameColumn(
                name: "TrangThai",
                table: "Trips",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "TongSoGhe",
                table: "Trips",
                newName: "TotalSeats");

            migrationBuilder.RenameColumn(
                name: "TenXe",
                table: "Trips",
                newName: "BusName");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "Trips",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "MaTuyenDuong",
                table: "Trips",
                newName: "RouteId");

            migrationBuilder.RenameColumn(
                name: "GioKhoiHanh",
                table: "Trips",
                newName: "StartTime");

            migrationBuilder.RenameIndex(
                name: "IX_ChuyenXe_MaTuyenDuong",
                table: "Trips",
                newName: "IX_Trips_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_ChuyenXe_GioKhoiHanh",
                table: "Trips",
                newName: "IX_Trips_StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Routes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckinLogs",
                table: "CheckinLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Trips_TripId",
                table: "Bookings",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckinLogs_AspNetUsers_StaffId",
                table: "CheckinLogs",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckinLogs_Bookings_BookingId",
                table: "CheckinLogs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Routes_RouteId",
                table: "Trips",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Trips_TripId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckinLogs_AspNetUsers_StaffId",
                table: "CheckinLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckinLogs_Bookings_BookingId",
                table: "CheckinLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Routes_RouteId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckinLogs",
                table: "CheckinLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "ChuyenXe");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "GheNgoi");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "TuyenDuong");

            migrationBuilder.RenameTable(
                name: "CheckinLogs",
                newName: "NhatKyCheckIn");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "DatVe");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "NhatKyAudit");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "NguoiDungToken",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "NguoiDung",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "NguoiDungVaiTro",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "NguoiDungDangNhap",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "NguoiDungQuyen",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "VaiTro",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "VaiTroQuyen",
                newSchema: "identity");

            migrationBuilder.RenameColumn(
                name: "TotalSeats",
                table: "ChuyenXe",
                newName: "TongSoGhe");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ChuyenXe",
                newName: "TrangThai");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ChuyenXe",
                newName: "GioKhoiHanh");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "ChuyenXe",
                newName: "MaTuyenDuong");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ChuyenXe",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "BusName",
                table: "ChuyenXe",
                newName: "TenXe");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_StartTime",
                table: "ChuyenXe",
                newName: "IX_ChuyenXe_GioKhoiHanh");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_RouteId",
                table: "ChuyenXe",
                newName: "IX_ChuyenXe_MaTuyenDuong");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "GheNgoi",
                newName: "MaChuyenXe");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "GheNgoi",
                newName: "SoGhe");

            migrationBuilder.RenameColumn(
                name: "IsBooked",
                table: "GheNgoi",
                newName: "DaDat");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_TripId_SeatNumber",
                table: "GheNgoi",
                newName: "IX_GheNgoi_MaChuyenXe_SoGhe");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "TuyenDuong",
                newName: "DiemDen");

            migrationBuilder.RenameColumn(
                name: "Departure",
                table: "TuyenDuong",
                newName: "DiemDi");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TuyenDuong",
                newName: "NgayTao");

            migrationBuilder.RenameIndex(
                name: "IX_Routes_Departure_Destination",
                table: "TuyenDuong",
                newName: "IX_TuyenDuong_DiemDi_DiemDen");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "NhatKyCheckIn",
                newName: "MaNhanVien");

            migrationBuilder.RenameColumn(
                name: "CheckinTime",
                table: "NhatKyCheckIn",
                newName: "ThoiGianCheckIn");

            migrationBuilder.RenameColumn(
                name: "CheckinPoint",
                table: "NhatKyCheckIn",
                newName: "DiemCheckIn");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "NhatKyCheckIn",
                newName: "MaDatVe");

            migrationBuilder.RenameIndex(
                name: "IX_CheckinLogs_StaffId",
                table: "NhatKyCheckIn",
                newName: "IX_NhatKyCheckIn_MaNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_CheckinLogs_BookingId",
                table: "NhatKyCheckIn",
                newName: "IX_NhatKyCheckIn_MaDatVe");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DatVe",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "DatVe",
                newName: "MaChuyenXe");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "DatVe",
                newName: "TrangThai");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "DatVe",
                newName: "SoGhe");

            migrationBuilder.RenameColumn(
                name: "QRToken",
                table: "DatVe",
                newName: "MaQR");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "DatVe",
                newName: "SoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "DatVe",
                newName: "TrangThaiThanhToan");

            migrationBuilder.RenameColumn(
                name: "HolderName",
                table: "DatVe",
                newName: "TenNguoiDat");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "DatVe",
                newName: "HetHanLuc");

            migrationBuilder.RenameColumn(
                name: "BookingTime",
                table: "DatVe",
                newName: "ThoiGianDat");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "DatVe",
                newName: "IX_DatVe_MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_TripId_SeatNumber",
                table: "DatVe",
                newName: "IX_DatVe_MaChuyenXe_SoGhe");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_QRToken",
                table: "DatVe",
                newName: "IX_DatVe_MaQR");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NhatKyAudit",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "NhatKyAudit",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NhatKyAudit",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "NhatKyAudit",
                newName: "HanhDong");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_UserId",
                table: "NhatKyAudit",
                newName: "IX_NhatKyAudit_MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "identity",
                table: "NguoiDung",
                newName: "TenDangNhap");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                schema: "identity",
                table: "NguoiDung",
                newName: "XacThucHaiYeuTo");

            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "identity",
                table: "NguoiDung",
                newName: "VaiTro");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                schema: "identity",
                table: "NguoiDung",
                newName: "XacNhanSoDienThoai");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "identity",
                table: "NguoiDung",
                newName: "SoDienThoai");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                schema: "identity",
                table: "NguoiDung",
                newName: "KhoaDen");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                schema: "identity",
                table: "NguoiDung",
                newName: "ChoPhepKhoa");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "identity",
                table: "NguoiDung",
                newName: "HoTen");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                schema: "identity",
                table: "NguoiDung",
                newName: "XacNhanEmail");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "identity",
                table: "NguoiDung",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                schema: "identity",
                table: "NguoiDung",
                newName: "SoLanTruyCapThatBai");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "identity",
                table: "NguoiDungVaiTro",
                newName: "IX_NguoiDungVaiTro_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "identity",
                table: "NguoiDungDangNhap",
                newName: "IX_NguoiDungDangNhap_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "identity",
                table: "NguoiDungQuyen",
                newName: "IX_NguoiDungQuyen_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "identity",
                table: "VaiTroQuyen",
                newName: "IX_VaiTroQuyen_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChuyenXe",
                table: "ChuyenXe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GheNgoi",
                table: "GheNgoi",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TuyenDuong",
                table: "TuyenDuong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhatKyCheckIn",
                table: "NhatKyCheckIn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatVe",
                table: "DatVe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhatKyAudit",
                table: "NhatKyAudit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungToken",
                schema: "identity",
                table: "NguoiDungToken",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung",
                schema: "identity",
                table: "NguoiDung",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungVaiTro",
                schema: "identity",
                table: "NguoiDungVaiTro",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungDangNhap",
                schema: "identity",
                table: "NguoiDungDangNhap",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDungQuyen",
                schema: "identity",
                table: "NguoiDungQuyen",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTro",
                schema: "identity",
                table: "VaiTro",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaiTroQuyen",
                schema: "identity",
                table: "VaiTroQuyen",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenXe_TuyenDuong_MaTuyenDuong",
                table: "ChuyenXe",
                column: "MaTuyenDuong",
                principalTable: "TuyenDuong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatVe_ChuyenXe_MaChuyenXe",
                table: "DatVe",
                column: "MaChuyenXe",
                principalTable: "ChuyenXe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatVe_NguoiDung_MaNguoiDung",
                table: "DatVe",
                column: "MaNguoiDung",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GheNgoi_ChuyenXe_MaChuyenXe",
                table: "GheNgoi",
                column: "MaChuyenXe",
                principalTable: "ChuyenXe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungDangNhap_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungDangNhap",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungQuyen_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungQuyen",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungToken_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungToken",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungVaiTro_NguoiDung_UserId",
                schema: "identity",
                table: "NguoiDungVaiTro",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungVaiTro_VaiTro_RoleId",
                schema: "identity",
                table: "NguoiDungVaiTro",
                column: "RoleId",
                principalSchema: "identity",
                principalTable: "VaiTro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyAudit_NguoiDung_MaNguoiDung",
                table: "NhatKyAudit",
                column: "MaNguoiDung",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyCheckIn_DatVe_MaDatVe",
                table: "NhatKyCheckIn",
                column: "MaDatVe",
                principalTable: "DatVe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyCheckIn_NguoiDung_MaNhanVien",
                table: "NhatKyCheckIn",
                column: "MaNhanVien",
                principalSchema: "identity",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VaiTroQuyen_VaiTro_RoleId",
                schema: "identity",
                table: "VaiTroQuyen",
                column: "RoleId",
                principalSchema: "identity",
                principalTable: "VaiTro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
