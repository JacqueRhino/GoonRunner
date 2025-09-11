using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class ChiTietHoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<CHITIETHOADON> _chitiethoadonlist;
        public ObservableCollection<CHITIETHOADON> ChiTietHoaDonList
        {
            get => _chitiethoadonlist;
            set { _chitiethoadonlist = value; OnPropertyChanged(); }
        }

        private int _mahd;
        public int MaHD
        {
            get => _mahd;
            set { _mahd = value; LoadChiTietHoaDonList(); OnPropertyChanged(); }
        }

        private int _tongtien;
        public int TongTien
        {
            get => _tongtien;
            set { _tongtien = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private readonly System.Action _navigateBack;

        public ChiTietHoaDonViewModel(System.Action navigateBack = null)
        {
            _navigateBack = navigateBack;
            RefreshCommand = new RelayCommand<Button>(_ => true, _ => LoadChiTietHoaDonList());
            PreviousPageCommand = new RelayCommand<object>(_ => true, _ => _navigateBack?.Invoke());
            Messenger.ChiTietHoaDonChanged += cthd =>
            {
                LoadChiTietHoaDonList();
            };
        }

        public ChiTietHoaDonViewModel(int maHD, System.Action navigateBack = null) : this(navigateBack)
        {
            MaHD = maHD;
            LoadChiTietHoaDonList();
        }

        public void LoadChiTietHoaDonList()
        {
            ChiTietHoaDonList = new ObservableCollection<CHITIETHOADON>();
            int temp = 0;
            var danhSachChiTietHoaDon = DataProvider.Ins.goonRunnerDB.CHITIETHOADONs
                .Where(n => n.MaHD == MaHD)
                .ToList();

            foreach (var item in danhSachChiTietHoaDon)
            {
                var chitiethoadon = new CHITIETHOADON
                {
                    MaSP = item.MaSP,
                    TenSP = item.TenSP,
                    SoLuongDat = item.SoLuongDat,
                    DonGia = item.DonGia,
                    ThanhTien = item.SoLuongDat * item.DonGia
                };
                temp += chitiethoadon.ThanhTien ?? 0;
                ChiTietHoaDonList.Add(chitiethoadon);
            }

            TongTien = temp;
        }
    }
}
