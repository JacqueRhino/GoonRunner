using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace GoonRunner.MVVM.ViewModel
{
    public class KhachHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _khachhanglist;
        public ObservableCollection<KHACHHANG> KhachHangList { get { return _khachhanglist; } set { _khachhanglist = value; OnPropertyChanged(); } }

        private KHACHHANG _selectedKHACHHANG;

        public KHACHHANG SelectedKHACHHANG { get { return _selectedKHACHHANG; } set { _selectedKHACHHANG = value; OnPropertyChanged(); }
        }
        public KhachHangViewModel()
        {
            LoadKhachHangList();
        }
        private void LoadKhachHangList()
        {
            KhachHangList = new ObservableCollection<KHACHHANG>();
            var DanhSachKhachHang = DataProvider.Ins.goonRunnerDB.KHACHHANGs;
            int i = 1;
            foreach (var item in DanhSachKhachHang)
            {
                KHACHHANG khachhang = new KHACHHANG();
                khachhang.MaKH = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where((n) => n.MaKH == i).Select(n => n.MaKH).FirstOrDefault();
                var HoKH = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where(n => n.MaKH == i).Select(n => n.HoKH).FirstOrDefault();
                var TenKH = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where(n => n.MaKH == i).Select(n => n.TenKH).FirstOrDefault();
                khachhang.TenKH = HoKH + " " + TenKH;
                khachhang.SdtKH = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where((n) => n.MaKH == i).Select(n => n.SdtKH).FirstOrDefault();
                khachhang.NgaySinh = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where((n) => n.MaKH == i).Select(n => n.NgaySinh).FirstOrDefault();
                khachhang.DiaChi = DataProvider.Ins.goonRunnerDB.KHACHHANGs.Where((n) => n.MaKH == i).Select(n => n.DiaChi).FirstOrDefault();
                KhachHangList.Add(khachhang);
                i++;
            }
        }
    }
}