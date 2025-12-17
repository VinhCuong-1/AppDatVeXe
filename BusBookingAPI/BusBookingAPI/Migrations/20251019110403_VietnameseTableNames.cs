using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class VietnameseTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    XacNhanEmail = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XacNhanSoDienThoai = table.Column<bool>(type: "bit", nullable: false),
                    XacThucHaiYeuTo = table.Column<bool>(type: "bit", nullable: false),
                    KhoaDen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ChoPhepKhoa = table.Column<bool>(type: "bit", nullable: false),
                    SoLanTruyCapThatBai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TuyenDuong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiemDi = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemDen = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyenDuong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungDangNhap",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungDangNhap", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_NguoiDungDangNhap_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungQuyen",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungQuyen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NguoiDungQuyen_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungToken",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_NguoiDungToken_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HanhDong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhatKyAudit_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChuyenXe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTuyenDuong = table.Column<int>(type: "int", nullable: false),
                    TenXe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioKhoiHanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongSoGhe = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenXe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChuyenXe_TuyenDuong_MaTuyenDuong",
                        column: x => x.MaTuyenDuong,
                        principalTable: "TuyenDuong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungVaiTro",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungVaiTro", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_NguoiDungVaiTro_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDungVaiTro_VaiTro_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "VaiTro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaiTroQuyen",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTroQuyen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaiTroQuyen_VaiTro_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "VaiTro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatVe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChuyenXe = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoGhe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNguoiDat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaQR = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThoiGianDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HetHanLuc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatVe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatVe_ChuyenXe_MaChuyenXe",
                        column: x => x.MaChuyenXe,
                        principalTable: "ChuyenXe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatVe_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GheNgoi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChuyenXe = table.Column<int>(type: "int", nullable: false),
                    SoGhe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DaDat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GheNgoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GheNgoi_ChuyenXe_MaChuyenXe",
                        column: x => x.MaChuyenXe,
                        principalTable: "ChuyenXe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyCheckIn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDatVe = table.Column<int>(type: "int", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemCheckIn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianCheckIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyCheckIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhatKyCheckIn_DatVe_MaDatVe",
                        column: x => x.MaDatVe,
                        principalTable: "DatVe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhatKyCheckIn_NguoiDung_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalSchema: "identity",
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TuyenDuong",
                columns: new[] { "Id", "NgayTao", "DiemDi", "DiemDen" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hà Nội", "Hồ Chí Minh" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hà Nội", "Đà Nẵng" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hà Nội", "Hải Phòng" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hồ Chí Minh", "Đà Lạt" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hồ Chí Minh", "Cần Thơ" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đà Nẵng", "Huế" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hà Nội", "Nghệ An" }
                });

            migrationBuilder.InsertData(
                table: "ChuyenXe",
                columns: new[] { "Id", "TenXe", "NgayTao", "MaTuyenDuong", "GioKhoiHanh", "TrangThai", "TongSoGhe" },
                values: new object[,]
                {
                    { 1, "Xe Khách Phương Trang", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 1, 2, 8, 0, 0, 0, DateTimeKind.Utc), "Active", 40 },
                    { 2, "Xe Khách Hoàng Long", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 1, 2, 14, 0, 0, 0, DateTimeKind.Utc), "Active", 40 },
                    { 3, "Xe Khách Mai Linh", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 1, 2, 20, 0, 0, 0, DateTimeKind.Utc), "Active", 40 },
                    { 4, "Xe Khách Phương Trang", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2024, 1, 2, 9, 0, 0, 0, DateTimeKind.Utc), "Active", 40 },
                    { 5, "Xe Khách Hoàng Long", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2024, 1, 2, 15, 0, 0, 0, DateTimeKind.Utc), "Active", 40 },
                    { 6, "Xe Khách Mai Linh", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2024, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc), "Active", 40 }
                });

            migrationBuilder.InsertData(
                table: "GheNgoi",
                columns: new[] { "Id", "DaDat", "SoGhe", "MaChuyenXe" },
                values: new object[,]
                {
                    { 1, false, "A1", 1 },
                    { 2, false, "A2", 1 },
                    { 3, false, "A3", 1 },
                    { 4, false, "A4", 1 },
                    { 5, false, "B1", 1 },
                    { 6, false, "B2", 1 },
                    { 7, false, "B3", 1 },
                    { 8, false, "B4", 1 },
                    { 9, false, "C1", 1 },
                    { 10, false, "C2", 1 },
                    { 11, false, "C3", 1 },
                    { 12, false, "C4", 1 },
                    { 13, false, "D1", 1 },
                    { 14, false, "D2", 1 },
                    { 15, false, "D3", 1 },
                    { 16, false, "D4", 1 },
                    { 17, false, "E1", 1 },
                    { 18, false, "E2", 1 },
                    { 19, false, "E3", 1 },
                    { 20, false, "E4", 1 },
                    { 21, false, "F1", 1 },
                    { 22, false, "F2", 1 },
                    { 23, false, "F3", 1 },
                    { 24, false, "F4", 1 },
                    { 25, false, "G1", 1 },
                    { 26, false, "G2", 1 },
                    { 27, false, "G3", 1 },
                    { 28, false, "G4", 1 },
                    { 29, false, "H1", 1 },
                    { 30, false, "H2", 1 },
                    { 31, false, "H3", 1 },
                    { 32, false, "H4", 1 },
                    { 33, false, "I1", 1 },
                    { 34, false, "I2", 1 },
                    { 35, false, "I3", 1 },
                    { 36, false, "I4", 1 },
                    { 37, false, "J1", 1 },
                    { 38, false, "J2", 1 },
                    { 39, false, "J3", 1 },
                    { 40, false, "J4", 1 },
                    { 41, false, "A1", 2 },
                    { 42, false, "A2", 2 },
                    { 43, false, "A3", 2 },
                    { 44, false, "A4", 2 },
                    { 45, false, "B1", 2 },
                    { 46, false, "B2", 2 },
                    { 47, false, "B3", 2 },
                    { 48, false, "B4", 2 },
                    { 49, false, "C1", 2 },
                    { 50, false, "C2", 2 },
                    { 51, false, "C3", 2 },
                    { 52, false, "C4", 2 },
                    { 53, false, "D1", 2 },
                    { 54, false, "D2", 2 },
                    { 55, false, "D3", 2 },
                    { 56, false, "D4", 2 },
                    { 57, false, "E1", 2 },
                    { 58, false, "E2", 2 },
                    { 59, false, "E3", 2 },
                    { 60, false, "E4", 2 },
                    { 61, false, "F1", 2 },
                    { 62, false, "F2", 2 },
                    { 63, false, "F3", 2 },
                    { 64, false, "F4", 2 },
                    { 65, false, "G1", 2 },
                    { 66, false, "G2", 2 },
                    { 67, false, "G3", 2 },
                    { 68, false, "G4", 2 },
                    { 69, false, "H1", 2 },
                    { 70, false, "H2", 2 },
                    { 71, false, "H3", 2 },
                    { 72, false, "H4", 2 },
                    { 73, false, "I1", 2 },
                    { 74, false, "I2", 2 },
                    { 75, false, "I3", 2 },
                    { 76, false, "I4", 2 },
                    { 77, false, "J1", 2 },
                    { 78, false, "J2", 2 },
                    { 79, false, "J3", 2 },
                    { 80, false, "J4", 2 },
                    { 81, false, "A1", 3 },
                    { 82, false, "A2", 3 },
                    { 83, false, "A3", 3 },
                    { 84, false, "A4", 3 },
                    { 85, false, "B1", 3 },
                    { 86, false, "B2", 3 },
                    { 87, false, "B3", 3 },
                    { 88, false, "B4", 3 },
                    { 89, false, "C1", 3 },
                    { 90, false, "C2", 3 },
                    { 91, false, "C3", 3 },
                    { 92, false, "C4", 3 },
                    { 93, false, "D1", 3 },
                    { 94, false, "D2", 3 },
                    { 95, false, "D3", 3 },
                    { 96, false, "D4", 3 },
                    { 97, false, "E1", 3 },
                    { 98, false, "E2", 3 },
                    { 99, false, "E3", 3 },
                    { 100, false, "E4", 3 },
                    { 101, false, "F1", 3 },
                    { 102, false, "F2", 3 },
                    { 103, false, "F3", 3 },
                    { 104, false, "F4", 3 },
                    { 105, false, "G1", 3 },
                    { 106, false, "G2", 3 },
                    { 107, false, "G3", 3 },
                    { 108, false, "G4", 3 },
                    { 109, false, "H1", 3 },
                    { 110, false, "H2", 3 },
                    { 111, false, "H3", 3 },
                    { 112, false, "H4", 3 },
                    { 113, false, "I1", 3 },
                    { 114, false, "I2", 3 },
                    { 115, false, "I3", 3 },
                    { 116, false, "I4", 3 },
                    { 117, false, "J1", 3 },
                    { 118, false, "J2", 3 },
                    { 119, false, "J3", 3 },
                    { 120, false, "J4", 3 },
                    { 121, false, "A1", 4 },
                    { 122, false, "A2", 4 },
                    { 123, false, "A3", 4 },
                    { 124, false, "A4", 4 },
                    { 125, false, "B1", 4 },
                    { 126, false, "B2", 4 },
                    { 127, false, "B3", 4 },
                    { 128, false, "B4", 4 },
                    { 129, false, "C1", 4 },
                    { 130, false, "C2", 4 },
                    { 131, false, "C3", 4 },
                    { 132, false, "C4", 4 },
                    { 133, false, "D1", 4 },
                    { 134, false, "D2", 4 },
                    { 135, false, "D3", 4 },
                    { 136, false, "D4", 4 },
                    { 137, false, "E1", 4 },
                    { 138, false, "E2", 4 },
                    { 139, false, "E3", 4 },
                    { 140, false, "E4", 4 },
                    { 141, false, "F1", 4 },
                    { 142, false, "F2", 4 },
                    { 143, false, "F3", 4 },
                    { 144, false, "F4", 4 },
                    { 145, false, "G1", 4 },
                    { 146, false, "G2", 4 },
                    { 147, false, "G3", 4 },
                    { 148, false, "G4", 4 },
                    { 149, false, "H1", 4 },
                    { 150, false, "H2", 4 },
                    { 151, false, "H3", 4 },
                    { 152, false, "H4", 4 },
                    { 153, false, "I1", 4 },
                    { 154, false, "I2", 4 },
                    { 155, false, "I3", 4 },
                    { 156, false, "I4", 4 },
                    { 157, false, "J1", 4 },
                    { 158, false, "J2", 4 },
                    { 159, false, "J3", 4 },
                    { 160, false, "J4", 4 },
                    { 161, false, "A1", 5 },
                    { 162, false, "A2", 5 },
                    { 163, false, "A3", 5 },
                    { 164, false, "A4", 5 },
                    { 165, false, "B1", 5 },
                    { 166, false, "B2", 5 },
                    { 167, false, "B3", 5 },
                    { 168, false, "B4", 5 },
                    { 169, false, "C1", 5 },
                    { 170, false, "C2", 5 },
                    { 171, false, "C3", 5 },
                    { 172, false, "C4", 5 },
                    { 173, false, "D1", 5 },
                    { 174, false, "D2", 5 },
                    { 175, false, "D3", 5 },
                    { 176, false, "D4", 5 },
                    { 177, false, "E1", 5 },
                    { 178, false, "E2", 5 },
                    { 179, false, "E3", 5 },
                    { 180, false, "E4", 5 },
                    { 181, false, "F1", 5 },
                    { 182, false, "F2", 5 },
                    { 183, false, "F3", 5 },
                    { 184, false, "F4", 5 },
                    { 185, false, "G1", 5 },
                    { 186, false, "G2", 5 },
                    { 187, false, "G3", 5 },
                    { 188, false, "G4", 5 },
                    { 189, false, "H1", 5 },
                    { 190, false, "H2", 5 },
                    { 191, false, "H3", 5 },
                    { 192, false, "H4", 5 },
                    { 193, false, "I1", 5 },
                    { 194, false, "I2", 5 },
                    { 195, false, "I3", 5 },
                    { 196, false, "I4", 5 },
                    { 197, false, "J1", 5 },
                    { 198, false, "J2", 5 },
                    { 199, false, "J3", 5 },
                    { 200, false, "J4", 5 },
                    { 201, false, "A1", 6 },
                    { 202, false, "A2", 6 },
                    { 203, false, "A3", 6 },
                    { 204, false, "A4", 6 },
                    { 205, false, "B1", 6 },
                    { 206, false, "B2", 6 },
                    { 207, false, "B3", 6 },
                    { 208, false, "B4", 6 },
                    { 209, false, "C1", 6 },
                    { 210, false, "C2", 6 },
                    { 211, false, "C3", 6 },
                    { 212, false, "C4", 6 },
                    { 213, false, "D1", 6 },
                    { 214, false, "D2", 6 },
                    { 215, false, "D3", 6 },
                    { 216, false, "D4", 6 },
                    { 217, false, "E1", 6 },
                    { 218, false, "E2", 6 },
                    { 219, false, "E3", 6 },
                    { 220, false, "E4", 6 },
                    { 221, false, "F1", 6 },
                    { 222, false, "F2", 6 },
                    { 223, false, "F3", 6 },
                    { 224, false, "F4", 6 },
                    { 225, false, "G1", 6 },
                    { 226, false, "G2", 6 },
                    { 227, false, "G3", 6 },
                    { 228, false, "G4", 6 },
                    { 229, false, "H1", 6 },
                    { 230, false, "H2", 6 },
                    { 231, false, "H3", 6 },
                    { 232, false, "H4", 6 },
                    { 233, false, "I1", 6 },
                    { 234, false, "I2", 6 },
                    { 235, false, "I3", 6 },
                    { 236, false, "I4", 6 },
                    { 237, false, "J1", 6 },
                    { 238, false, "J2", 6 },
                    { 239, false, "J3", 6 },
                    { 240, false, "J4", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_GioKhoiHanh",
                table: "ChuyenXe",
                column: "GioKhoiHanh");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenXe_MaTuyenDuong",
                table: "ChuyenXe",
                column: "MaTuyenDuong");

            migrationBuilder.CreateIndex(
                name: "IX_DatVe_MaChuyenXe_SoGhe",
                table: "DatVe",
                columns: new[] { "MaChuyenXe", "SoGhe" });

            migrationBuilder.CreateIndex(
                name: "IX_DatVe_MaNguoiDung",
                table: "DatVe",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DatVe_MaQR",
                table: "DatVe",
                column: "MaQR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GheNgoi_MaChuyenXe_SoGhe",
                table: "GheNgoi",
                columns: new[] { "MaChuyenXe", "SoGhe" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "NguoiDung",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "NguoiDung",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungDangNhap_UserId",
                schema: "identity",
                table: "NguoiDungDangNhap",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungQuyen_UserId",
                schema: "identity",
                table: "NguoiDungQuyen",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungVaiTro_RoleId",
                schema: "identity",
                table: "NguoiDungVaiTro",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyAudit_MaNguoiDung",
                table: "NhatKyAudit",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyCheckIn_MaDatVe",
                table: "NhatKyCheckIn",
                column: "MaDatVe");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyCheckIn_MaNhanVien",
                table: "NhatKyCheckIn",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_TuyenDuong_DiemDi_DiemDen",
                table: "TuyenDuong",
                columns: new[] { "DiemDi", "DiemDen" });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "VaiTro",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VaiTroQuyen_RoleId",
                schema: "identity",
                table: "VaiTroQuyen",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GheNgoi");

            migrationBuilder.DropTable(
                name: "NguoiDungDangNhap",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "NguoiDungQuyen",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "NguoiDungToken",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "NguoiDungVaiTro",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "NhatKyAudit");

            migrationBuilder.DropTable(
                name: "NhatKyCheckIn");

            migrationBuilder.DropTable(
                name: "VaiTroQuyen",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "DatVe");

            migrationBuilder.DropTable(
                name: "VaiTro",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ChuyenXe");

            migrationBuilder.DropTable(
                name: "NguoiDung",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "TuyenDuong");
        }
    }
}
