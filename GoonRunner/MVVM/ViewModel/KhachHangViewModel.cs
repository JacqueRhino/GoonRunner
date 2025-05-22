using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class KhachHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _khachhanglist;
        public ObservableCollection<KHACHHANG> KhachHangList { get { return _khachhanglist; } set { _khachhanglist = value; OnPropertyChanged(); } }
        private KHACHHANG _selectedKHACHHANG;
        public KHACHHANG SelectedKHACHHANG { get { return _selectedKHACHHANG; } set { _selectedKHACHHANG = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public KhachHangViewModel()
        {
            LoadKhachHangList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadKhachHangList(); });
        }
        private void LoadKhachHangList()
        {
            KhachHangList = new ObservableCollection<KHACHHANG>();
            var DanhSachKhachHang = DataProvider.Ins.goonRunnerDB.KHACHHANGs;
            int i = 1;
            foreach (var item in DanhSachKhachHang)
            {
                KHACHHANG khachhang = new KHACHHANG();
                khachhang.MaKH = item.MaKH;
                khachhang.TenKH = item.HoKH + " " + item.TenKH;
                khachhang.SdtKH = item.SdtKH;
                khachhang.NgaySinh = item.NgaySinh;
                khachhang.DiaChi = item.DiaChi;
                KhachHangList.Add(khachhang);
                i++;
            }
        }
    }
}