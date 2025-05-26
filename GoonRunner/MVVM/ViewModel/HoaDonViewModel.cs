using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class HoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> _hoadonlist;
        public ObservableCollection<HOADON> HoaDonList { get { return _hoadonlist; } set { _hoadonlist = value; OnPropertyChanged(); } }
        private HOADON _selectedItem;
        public HOADON SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public HoaDonViewModel()
        {
            LoadHoaDonList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadHoaDonList(); });
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                MainViewModel.Instance.ChiTietHoaDonVM = new ChiTietHoaDonViewModel(SelectedItem.MaHD);
                MainViewModel.Instance.SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel(SelectedItem.MaHD);

                MainViewModel.Instance.CurrentView = MainViewModel.Instance.ChiTietHoaDonVM;
                MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarChiTietHoaDonVM;
            });
        }
        public void LoadHoaDonList()
        {
            HoaDonList = new ObservableCollection<HOADON>();
            var DanhSachHoaDon = DataProvider.Ins.goonRunnerDB.HOADONs;
            int i = 1;
            foreach (var item in DanhSachHoaDon)
            {
                HOADON hoadon = new HOADON();
                hoadon.MaHD = item.MaHD;
                hoadon.MaKH = item.MaKH;
                hoadon.TenKH = item.HoKH + " " + item.TenKH;
                hoadon.SdtKH = item.SdtKH;
                hoadon.DiaChi = item.DiaChi;
                hoadon.MaNV = item.MaNV;
                hoadon.TenNV = item.HoNV + " " + item.TenNV;
                hoadon.NgayMuaHang = item.NgayMuaHang;
                HoaDonList.Add(hoadon);
                i++;
            }
        }
    }
}
