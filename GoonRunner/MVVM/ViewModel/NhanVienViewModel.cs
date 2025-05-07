using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NHANVIEN> _nhanvienlist;
        public ObservableCollection<NHANVIEN> NhanVienList { get { return _nhanvienlist; } set { _nhanvienlist = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public NhanVienViewModel()
        {
            LoadNhanVienList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadNhanVienList(); });
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