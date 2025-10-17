using System.Collections.Generic;
using System.Linq;

namespace GoonRunner.Utils
{
    public static class EmployeeRoles
    {
        public enum Roles
        {
            NhanVienBanHang,
            NhanVienKeToan,
            NhanVienKyThuat,
            NhanVienMarketing,
            NhanVienTapVu,
            NhanVienKiemKho,
            NhanVienBaoVe,
            NhanVienChamSocKhachHang,
            NhanVienGiaoHang,
            NhanVienQuanTriMang,
            Admin,
            ChuCuaHang
        }
        public enum Permission
        {
            Home,
            NhanVien,
            AccNhanVien,
            KhachHang,
            SanPham,
            HoaDon,
            PhieuNhap,
            TonKho,
            KhuyenMai,
            BaoHanh
        }
        public static class RoleNames
        {
            public static readonly Dictionary<string, Roles> StringToRole = new Dictionary<string, Roles>
            {
                ["Nhân viên bán hàng"] = Roles.NhanVienBanHang,
                ["Nhân viên kế toán"] = Roles.NhanVienKeToan,
                ["Nhân viên kỹ thuật"] = Roles.NhanVienKyThuat,
                ["Nhân viên Marketing"] = Roles.NhanVienMarketing,
                ["Nhân viên tạp vụ"] = Roles.NhanVienTapVu,
                ["Nhân viên kiểm kho"] = Roles.NhanVienKiemKho,
                ["Nhân viên bảo vệ"] = Roles.NhanVienBaoVe,
                ["Chăm sóc khách hàng"] = Roles.NhanVienChamSocKhachHang,
                ["Nhân viên giao hàng"] = Roles.NhanVienGiaoHang,
                ["Nhân viên quản trị mạng"] = Roles.NhanVienQuanTriMang,
                ["Admin"] = Roles.Admin,
                ["Chủ cửa hàng"] = Roles.ChuCuaHang
            };

            public static readonly Dictionary<Roles, string> RoleToString = StringToRole.ToDictionary(kv => kv.Value, kv => kv.Key);
        }
        public static class Permissions
        {
            public static readonly Dictionary<Roles, Permission[]> Map =
                new Dictionary<Roles, Permission[]>
                {
                    [Roles.Admin] = new[]
                    {
                        Permission.Home, 
                        Permission.NhanVien,
                        Permission.AccNhanVien,
                        Permission.KhachHang, 
                        Permission.KhuyenMai
                    },
                    [Roles.NhanVienBanHang] = new[]
                    {
                        Permission.Home,
                        Permission.HoaDon, 
                        Permission.KhachHang, 
                        Permission.KhuyenMai, 
                        Permission.SanPham, 
                        Permission.TonKho
                    },
                    [Roles.NhanVienKeToan] = new[]
                    {
                        Permission.Home,
                        Permission.NhanVien, 
                        Permission.HoaDon
                    },
                    [Roles.NhanVienChamSocKhachHang] = new[]
                    {
                        Permission.Home,
                        Permission.KhachHang, 
                        Permission.HoaDon
                    },
                    [Roles.NhanVienKiemKho] = new[]
                    {
                        Permission.Home,
                        Permission.TonKho, 
                        Permission.SanPham, 
                        Permission.PhieuNhap
                    },
                    [Roles.NhanVienKyThuat] = new[]
                    {
                        Permission.Home,
                        Permission.KhachHang, 
                        Permission.SanPham
                    },
                    [Roles.ChuCuaHang] = new[]
                    {
                        Permission.Home,
                        Permission.NhanVien,
                        Permission.SanPham
                    }
                };
        }
        
    }
}