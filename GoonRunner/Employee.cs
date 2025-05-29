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
            NhanVienChamSocKhachHang,
            NhanVienKiemKho,
            NhanVienKyThuat,
            Admin,
            ChuCuaHang
        }

        public static class RoleNames
        {
            public static readonly Dictionary<string, EmployeeRole> StringToRole = new Dictionary<string, EmployeeRole>
            {
                ["Nhân viên bán hàng"] = EmployeeRole.NhanVienBanHang,
                ["Nhân viên kế toán"] = EmployeeRole.NhanVienKeToan,
                ["Nhân viên chăm sóc khách hàng"] = EmployeeRole.NhanVienChamSocKhachHang,
                ["Nhân viên kiểm kho"] = EmployeeRole.NhanVienKiemKho,
                ["Nhân viên kỹ thuật"] = EmployeeRole.NhanVienKyThuat,
                ["Admin"] = EmployeeRole.Admin,
                ["Chủ cửa hàng"] = EmployeeRole.ChuCuaHang
            };
    
            public static readonly Dictionary<EmployeeRole, string> RoleToString = StringToRole.ToDictionary(kv => kv.Value, kv => kv.Key);
            
        }    
    }
}