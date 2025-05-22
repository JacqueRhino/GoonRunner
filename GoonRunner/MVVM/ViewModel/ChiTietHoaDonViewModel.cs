using GoonRunner.CustomControl;
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
        public ICommand RefreshCommand { get; set; }
        public ChiTietHoaDonViewModel()
        {
            LoadChiTietHoaDonList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadChiTietHoaDonList(); });
        }
        private void LoadChiTietHoaDonList()
        {
            ChiTietHoaDonList = new ObservableCollection<CHITIETHOADON>();
            int i = MaHD;
            var DanhSachChiTietHoaDon = DataProvider.Ins.goonRunnerDB.CHITIETHOADONs.Where((n) => n.MaHD == i).ToList();
            foreach (var item in DanhSachChiTietHoaDon)
            {
                CHITIETHOADON chitiethoadonn = new CHITIETHOADON();
                chitiethoadonn.MaSP = item.MaSP;
                chitiethoadonn.TenSP = item.TenSP;
                chitiethoadonn.SoLuongDat = item.SoLuongDat;
                chitiethoadonn.DonGia = item.DonGia;
                chitiethoadonn.ThanhTien = item.SoLuongDat * item.DonGia;
                ChiTietHoaDonList.Add(chitiethoadonn);
            }
        }
    }
}
