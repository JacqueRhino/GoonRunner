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
        public ICommand RefreshCommand { get; set; }
        public HoaDonViewModel()
        {
            LoadHoaDonList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadHoaDonList(); });
        }
        private void LoadHoaDonList()
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
