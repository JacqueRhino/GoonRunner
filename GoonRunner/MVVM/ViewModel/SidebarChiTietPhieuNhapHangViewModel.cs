using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarChiTietPhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETPHIEUNHAPHANG> _danhsachchitietphieunhaphang;
        public ObservableCollection<CHITIETPHIEUNHAPHANG> DanhSachPhieuNhapHang { get => _danhsachchitietphieunhaphang; set { _danhsachchitietphieunhaphang = value; OnPropertyChanged(); } }
        private int _mapnh;
        public int MaPNH { get => _mapnh; set { _mapnh = value; OnPropertyChanged(); } }
        
        private int _masp;
        public int MaSP { get => _masp; set { _masp = value; LoadSanPhamInfo(value); OnPropertyChanged(); } }
        private string _tensp;
        public string TenSP { get => _tensp; set { _tensp = value; OnPropertyChanged(); } }
        private int _soluongnhap;
        public int SoLuongNhap { get => _soluongnhap; set { _soluongnhap = value; OnPropertyChanged(); } } 
        private int _dongia;
        public int DonGia { get => _dongia; set { _dongia = value; OnPropertyChanged(); } }
        public ICommand AddChiTietPhieuNhapHangCommand { get; set; }
        public ICommand ClearFieldCommand { get; set; }
        public SidebarChiTietPhieuNhapHangViewModel() { }
        public SidebarChiTietPhieuNhapHangViewModel(int maPNH)
        {
            MaPNH = maPNH;
            //ChiTietPhieuNhapHangViewModel chiTietPhieuNhapHangViewModel = new ChiTietPhieuNhapHangViewModel();
            //MaPNH = chiTietPhieuNhapHangViewModel.MaPNH;
            DanhSachPhieuNhapHang = new ObservableCollection<CHITIETPHIEUNHAPHANG>(DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs);
            AddChiTietPhieuNhapHangCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaPNH == 0)
                {
                    MessageBox.Show("Hãy nhập Mã phiếu nhập hàng");
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

                if (SoLuongNhap == 0)
                {
                    MessageBox.Show("Hãy nhập số lượng");
                    return;
                }
                
                if (DonGia == 0)
                {
                    MessageBox.Show("Hãy nhập đơn giá");
                    return;
                }

                var chitietphieunhaphang = new CHITIETPHIEUNHAPHANG() { MaPNH = MaPNH, MaSP = MaSP, TenSP = TenSP, SoLuongNhap = SoLuongNhap, DonGia = DonGia };
                DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs.Add(chitietphieunhaphang);
                try
                { 
                    DataProvider.Ins.goonRunnerDB.SaveChanges();
                    DanhSachPhieuNhapHang.Add(chitietphieunhaphang);
                    MessageBox.Show("Thêm thành công!");
                    MainViewModel.Instance?.ChiTietPhieuNhapHangVM?.LoadChiTietPhieuNhapHangList();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    MessageBox.Show("Không có Mã phiếu nhập hàng này");
                }
            });

            ClearFieldCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                ClearFields();
            });
        }
        private void ClearFields()
        {
            MaSP = 0;
            TenSP = string.Empty;
            SoLuongNhap = 0;
            DonGia = 0;
        }

        private void LoadSanPhamInfo(int maSP)
        {
            try
            {
                var sanpham = DataProvider.Ins.goonRunnerDB.SANPHAMs.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanpham != null)
                {
                    TenSP = sanpham.TenSP;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin khách hàng: {ex.Message}");
            }
        }
    }
}
