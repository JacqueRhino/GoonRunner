using System.Collections.Generic;
using System.Linq;

namespace GoonRunner
{
    public static class Employee
    {
        public enum EmployeeRole
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

        public static class RoleNames
        {
            public static readonly Dictionary<string, EmployeeRole> StringToRole = new Dictionary<string, EmployeeRole>
            {
                ["Nhân viên bán hàng"] = EmployeeRole.NhanVienBanHang,
                ["Nhân viên kế toán"] = EmployeeRole.NhanVienKeToan,
                ["Nhân viên kỹ thuật"] = EmployeeRole.NhanVienKyThuat,
                ["Nhân viên Marketing"] = EmployeeRole.NhanVienMarketing,
                ["Nhân viên tạp vụ"] = EmployeeRole.NhanVienTapVu,
                ["Nhân viên kiểm kho"] = EmployeeRole.NhanVienKiemKho,
                ["Nhân viên bảo vệ"] = EmployeeRole.NhanVienBaoVe,
                ["Chăm sóc khách hàng"] = EmployeeRole.NhanVienChamSocKhachHang,
                ["Nhân viên giao hàng"] = EmployeeRole.NhanVienGiaoHang,
                ["Nhân viên quản trị mạng"] = EmployeeRole.NhanVienQuanTriMang,
                ["Admin"] = EmployeeRole.Admin,
                ["Chủ cửa hàng"] = EmployeeRole.ChuCuaHang
            };

            public static readonly Dictionary<EmployeeRole, string> RoleToString = StringToRole.ToDictionary(kv => kv.Value, kv => kv.Key);
        }
        
    }
}