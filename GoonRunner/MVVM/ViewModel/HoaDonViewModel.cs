using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
// ReSharper disable InconsistentNaming

namespace GoonRunner.MVVM.ViewModel
{
    public class HoaDonViewModel : BaseViewModel
    {
        public ObservableCollection<HOADON> HoaDonList { get; set; } = new ObservableCollection<HOADON>();
        private HOADON _selectedItem;
        public HOADON SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredHoaDonList.Refresh(); } }
        private ICollectionView _filteredhoadonlist;
        public ICollectionView FilteredHoaDonList { get => _filteredhoadonlist; set { _filteredhoadonlist = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public HoaDonViewModel()
        {
            Init();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                FilterText = string.Empty;
                LoadHoaDonList();
            });
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                NavigateToChiTietHoaDon(SelectedItem.MaHD);
            });
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
                    TenNV = item.HoNV + " " + item.TenNV,
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

            var fullName = $"{hoadon.HoKH ?? ""} {hoadon.TenKH ?? ""}";
            return fullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        
        private void NavigateToChiTietHoaDon(int maHD)
        {
            MainViewModel.Instance.ChiTietHoaDonVM = new ChiTietHoaDonViewModel(maHD);
            MainViewModel.Instance.SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel(maHD);

            MainViewModel.Instance.CurrentView = MainViewModel.Instance.ChiTietHoaDonVM;
            MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarChiTietHoaDonVM;
        }

        private void Init()
        {
            LoadHoaDonList();
            FilteredHoaDonList = CollectionViewSource.GetDefaultView(HoaDonList);
            FilteredHoaDonList.Filter = FilterHoaDon;
        }
        
    }
}
