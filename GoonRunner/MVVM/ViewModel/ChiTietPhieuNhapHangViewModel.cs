using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class ChiTietPhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETPHIEUNHAPHANG> _chitietphieunhaphanglist;
        public ObservableCollection<CHITIETPHIEUNHAPHANG> ChiTietPhieuNhapHangList
        {
            get => _chitietphieunhaphanglist;
            set { _chitietphieunhaphanglist = value; OnPropertyChanged(); }
        }

        private int _mapnh;
        public int MaPNH
        {
            get => _mapnh;
            set { _mapnh = value; LoadChiTietPhieuNhapHangList(); OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private readonly System.Action _navigateBack;

        public ChiTietPhieuNhapHangViewModel(System.Action navigateBack = null)
        {
            _navigateBack = navigateBack;
            RefreshCommand = new RelayCommand<Button>(_ => true, _ => LoadChiTietPhieuNhapHangList());
            PreviousPageCommand = new RelayCommand<object>(_ => true, _ => _navigateBack?.Invoke());
            Messenger.ChiTietHoaDonChanged += cthd =>
            {
                LoadChiTietPhieuNhapHangList();
            };
        }

        public ChiTietPhieuNhapHangViewModel(int maPNH, System.Action navigateBack = null) : this(navigateBack)
        {
            MaPNH = maPNH;
            LoadChiTietPhieuNhapHangList();
        }

        public void LoadChiTietPhieuNhapHangList()
        {
            ChiTietPhieuNhapHangList = new ObservableCollection<CHITIETPHIEUNHAPHANG>();

            var danhSachChiTietPhieuNhapHang = DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs
                .Where(n => n.MaPNH == MaPNH)
                .ToList();

            foreach (var item in danhSachChiTietPhieuNhapHang)
            {
                ChiTietPhieuNhapHangList.Add(new CHITIETPHIEUNHAPHANG
                {
                    MaSP = item.MaSP,
                    TenSP = item.TenSP,
                    SoLuongNhap = item.SoLuongNhap,
                    DonGia = item.DonGia
                });
            }
        }
    }
}
