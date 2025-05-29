using GoonRunner.Utils;
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
    public class ChiTietHoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETHOADON> _chitiethoadonlist;
        public ObservableCollection<CHITIETHOADON> ChiTietHoaDonList { get { return _chitiethoadonlist; } set { _chitiethoadonlist = value; OnPropertyChanged(); } }
        private int _mahd;
        public int MaHD { get { return _mahd; } set { _mahd = value; LoadChiTietHoaDonList(); OnPropertyChanged(); } }
        private int _tongtien;
        public int TongTien { get { return _tongtien; } set { _tongtien = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ChiTietHoaDonViewModel()
        {
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadChiTietHoaDonList(); });
        }
        public ChiTietHoaDonViewModel(int maHD)
        {
            MaHD = maHD;
            LoadChiTietHoaDonList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadChiTietHoaDonList(); });
            PreviousPageCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                MainViewModel.Instance.HoaDonVM = new HoaDonViewModel();

                MainViewModel.Instance.CurrentView = MainViewModel.Instance.HoaDonVM;
                MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarHoaDonVM;
            });
        }
        public void LoadChiTietHoaDonList()
        {
            ChiTietHoaDonList = new ObservableCollection<CHITIETHOADON>();
            int i = MaHD;
            int temp = 0;
            var DanhSachChiTietHoaDon = DataProvider.Ins.goonRunnerDB.CHITIETHOADONs.Where((n) => n.MaHD == i);
            foreach (var item in DanhSachChiTietHoaDon)
            {
                CHITIETHOADON chitiethoadon = new CHITIETHOADON();
                chitiethoadon.MaSP = item.MaSP;
                chitiethoadon.TenSP = item.TenSP;
                chitiethoadon.SoLuongDat = item.SoLuongDat;
                chitiethoadon.DonGia = item.DonGia;
                chitiethoadon.ThanhTien = item.SoLuongDat * item.DonGia;
                temp += chitiethoadon.ThanhTien ?? 0;
                ChiTietHoaDonList.Add(chitiethoadon);
            }
            TongTien = temp;
        }
    }
}