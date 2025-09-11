using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class HoaDonViewModel : BaseViewModel
    {
        public ObservableCollection<HOADON> HoaDonList { get; set; } = new ObservableCollection<HOADON>();
        private bool _isUpdatingSelection;
        private HOADON _selectedItem;
        public HOADON SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged();

                if (_isUpdatingSelection) return;

                try
                {
                    _isUpdatingSelection = true;
                    Messenger.NotifyHoaDonSelectedChanged(_selectedItem?.MaHD ?? 0);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set { _filterText = value; OnPropertyChanged(); FilteredHoaDonList.Refresh(); }
        }

        private ICollectionView _filteredhoadonlist;
        public ICollectionView FilteredHoaDonList
        {
            get => _filteredhoadonlist;
            set { _filteredhoadonlist = value; OnPropertyChanged(); }
        }

        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private readonly Action<int> _navigateToChiTietHoaDon;

        // Inject navigation callback in constructor
        public HoaDonViewModel(Action<int> navigateToChiTietHoaDon)
        {
            _navigateToChiTietHoaDon = navigateToChiTietHoaDon ?? throw new ArgumentNullException(nameof(navigateToChiTietHoaDon));

            Init();

            RefreshCommand = new RelayCommand<Button>(_ => true, _ =>
            {
                FilterText = string.Empty;
                LoadHoaDonList();
            });

            DoubleClickCommand = new RelayCommand<object>(_ => SelectedItem != null, _ =>
            {
                _navigateToChiTietHoaDon?.Invoke(SelectedItem.MaHD);
            });
            
            
            Messenger.HoaDonChanged += hd =>
            {
                LoadHoaDonList();
            };
            
            Messenger.HoaDonSelected += mahd =>
            {
                SelectedItem = HoaDonList.FirstOrDefault(hd =>  hd.MaHD==mahd);
            };
        }

        public void Refresh()
        {
            LoadHoaDonList();
            FilteredHoaDonList.Refresh();
        }

        public void LoadHoaDonList()
        {
            HoaDonList.Clear();
            var danhSachHoaDon = DataProvider.Ins.goonRunnerDB.HOADONs;
            foreach (var item in danhSachHoaDon)
            {
                var hoadon = new HOADON
                {
                    MaHD = item.MaHD,
                    MaKH = item.MaKH,
                    TenKH = $"{item.HoKH} {item.TenKH}",
                    SdtKH = item.SdtKH,
                    DiaChi = item.DiaChi,
                    MaNV = item.MaNV,
                    TenNV = $"{item.HoNV} {item.TenNV}",
                    NgayMuaHang = item.NgayMuaHang
                };
                HoaDonList.Add(hoadon);
            }
        }

        private bool FilterHoaDon(object item)
        {
            var filter = FilterText?.Trim();
            if (string.IsNullOrWhiteSpace(filter))
                return true;

            if (!(item is HOADON hoadon))
                return false;

            if (int.TryParse(filter, out var maHD) && hoadon.MaHD == maHD)
                return true;

            var fullName = $"{hoadon.TenKH}";
            return fullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void Init()
        {
            LoadHoaDonList();
            FilteredHoaDonList = CollectionViewSource.GetDefaultView(HoaDonList);
            FilteredHoaDonList.Filter = FilterHoaDon;
        }
    }
}
