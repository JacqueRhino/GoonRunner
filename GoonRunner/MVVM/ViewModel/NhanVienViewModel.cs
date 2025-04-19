using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace GoonRunner.MVVM.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NHANVIEN> _NhanVienList;
        public ObservableCollection<NHANVIEN> NhanVienList { get { return _NhanVienList; } set { _NhanVienList = value; OnPropertyChanged(); } }

        public NhanVienViewModel()
        {
            LoadNhanVienList();
        }
        private void LoadNhanVienList()
        {
            NhanVienList = new ObservableCollection<NHANVIEN>();
            var DanhSachNhanVien = DataProvider.Ins.goonRunnerDB.NHANVIENs;
            int i = 1;
            foreach (var item in DanhSachNhanVien)
            {
                NHANVIEN nhanvien = new NHANVIEN();
                nhanvien.MaNV = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where((n) => n.MaNV == i).Select(n => n.MaNV).FirstOrDefault();
                nhanvien.TenNV = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where(n => n.MaNV == i).Select(n => n.HoNV + " " + n.TenNV).FirstOrDefault();
                nhanvien.MaPB = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where(n => n.MaNV == i).Select(n => n.MaPB).FirstOrDefault();
                nhanvien.GioiTinh = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where((n) => n.MaNV == i).Select(n => n.GioiTinh).FirstOrDefault();
                nhanvien.SdtNV = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where((n) => n.MaNV == i).Select(n => n.SdtNV).FirstOrDefault();
                nhanvien.GioiTinh = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where((n) => n.MaNV == i).Select(n => n.GioiTinh).FirstOrDefault();
                nhanvien.DiaChiNV = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where((n) => n.MaNV == i).Select(n => n.DiaChiNV).FirstOrDefault();
                NhanVienList.Add(nhanvien);
                i++;
            }
        }
    }
}