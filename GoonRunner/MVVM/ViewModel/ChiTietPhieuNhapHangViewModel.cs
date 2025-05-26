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
    public class ChiTietPhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETPHIEUNHAPHANG> _chitietphieunhaphanglist;
        public ObservableCollection<CHITIETPHIEUNHAPHANG> ChiTietPhieuNhapHangList { get { return _chitietphieunhaphanglist; } set { _chitietphieunhaphanglist = value; OnPropertyChanged(); } }
        private int _mapnh;
        public int MaPNH { get { return _mapnh; } set { _mapnh = value; LoadChiTietPhieuNhapHangList(); OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ChiTietPhieuNhapHangViewModel()
        {
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadChiTietPhieuNhapHangList(); });
        }
        public ChiTietPhieuNhapHangViewModel(int maPNH)
        {
            MaPNH = maPNH;
            LoadChiTietPhieuNhapHangList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadChiTietPhieuNhapHangList(); });
            PreviousPageCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                MainViewModel.Instance.PhieuNhapHangVM = new PhieuNhapHangViewModel();

                MainViewModel.Instance.CurrentView = MainViewModel.Instance.PhieuNhapHangVM;
                MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarPhieuNhapHangVM;
            });
        }
        public void LoadChiTietPhieuNhapHangList()
        {
            ChiTietPhieuNhapHangList = new ObservableCollection<CHITIETPHIEUNHAPHANG>();
            int i = MaPNH;
            var DanhSachChiTietPhieuNhapHang = DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs.Where((n) => n.MaPNH == i).ToList();
            foreach (var item in DanhSachChiTietPhieuNhapHang)
            {
                CHITIETPHIEUNHAPHANG chitietphieunhaphang = new CHITIETPHIEUNHAPHANG();
                chitietphieunhaphang.MaSP = item.MaSP;
                chitietphieunhaphang.TenSP = item.TenSP;
                chitietphieunhaphang.SoLuongNhap = item.SoLuongNhap;
                chitietphieunhaphang.DonGia = item.DonGia;
                ChiTietPhieuNhapHangList.Add(chitietphieunhaphang);
            }
        }
    }
}
