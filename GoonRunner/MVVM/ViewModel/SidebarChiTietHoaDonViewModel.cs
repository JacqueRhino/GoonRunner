using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarChiTietHoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETHOADON> _danhsachchitiethoadon;
        public ObservableCollection<CHITIETHOADON> DanhSachChiTietHoaDon { get => _danhsachchitiethoadon; set { _danhsachchitiethoadon = value; OnPropertyChanged(); } }
        private int _mahd;
        public int MaHD { get => _mahd; set { _mahd = value; OnPropertyChanged(); } }

        private int _masp;
        public int MaSP { get => _masp; set { _masp = value; LoadSanPhamInfo(value); OnPropertyChanged(); } }
        private string _tensp;
        public string TenSP { get => _tensp; set { _tensp = value; OnPropertyChanged(); } }
        private int _soluongban;
        public int SoLuongBan { get => _soluongban; set { _soluongban = value; OnPropertyChanged(); } }
        private int _dongia;
        public int DonGia { get => _dongia; set { _dongia = value; OnPropertyChanged(); } }
        private int _thanhtien;
        public int ThanhTien { get => _thanhtien; set { _thanhtien = value; OnPropertyChanged(); } }
        private int _tongtien;
        public int TongTien { get => _tongtien; set { _tongtien = value; OnPropertyChanged(); } }
        public ICommand AddChiTietHoaDonCommand { get; set; }
        public SidebarChiTietHoaDonViewModel() { }
        public SidebarChiTietHoaDonViewModel(int maHD)
        {
            MaHD = maHD;
            DanhSachChiTietHoaDon = new ObservableCollection<CHITIETHOADON>(DataProvider.Ins.goonRunnerDB.CHITIETHOADONs);
            AddChiTietHoaDonCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaHD == 0)
                {
                    MessageBox.Show("Hãy nhập Mã hóa đơn");
                    return;
                }

                if (MaSP == 0)
                {
                    MessageBox.Show("Hãy nhập Mã sản phẩm");
                    return;
                }

                if (string.IsNullOrEmpty(TenSP))
                {
                    MessageBox.Show("Hãy nhập Tên sản phẩm");
                    return;
                }

                if (SoLuongBan == 0)
                {
                    MessageBox.Show("Hãy nhập số lượng bán");
                    return;
                }

                if (DonGia == 0)
                {
                    MessageBox.Show("Hãy nhập đơn giá");
                    return;
                }

                var chitiethoadon = new CHITIETHOADON() { MaHD = MaHD, MaSP = MaSP, TenSP = TenSP, SoLuongDat = SoLuongBan, DonGia = DonGia };
                DataProvider.Ins.goonRunnerDB.CHITIETHOADONs.Add(chitiethoadon);
                try
                {
                    DataProvider.Ins.goonRunnerDB.SaveChanges();
                    DanhSachChiTietHoaDon.Add(chitiethoadon);
                    MessageBox.Show("Thêm thành công!");
                    MainViewModel.Instance?.ChiTietHoaDonVM?.LoadChiTietHoaDonList();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    MessageBox.Show("Không có Mã hóa đơn hàng này");
                }
            });
        }
        private void LoadSanPhamInfo(int maSP)
        {
            try
            {
                var sanpham = DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanpham != null)
                {
                    TenSP = sanpham.TenSP;
                    DonGia = (int)sanpham.DonGia;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin khách hàng: {ex.Message}");
            }
        }
    }
}
