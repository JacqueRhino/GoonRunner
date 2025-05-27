using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class HoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> _hoadonlist;
        public ObservableCollection<HOADON> HoaDonList { get { return _hoadonlist; } set { _hoadonlist = value; OnPropertyChanged(); } }
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
            LoadHoaDonList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                LoadHoaDonList();
                FilterText = string.Empty;
            });
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                MainViewModel.Instance.ChiTietHoaDonVM = new ChiTietHoaDonViewModel(SelectedItem.MaHD);
                MainViewModel.Instance.SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel(SelectedItem.MaHD);

                MainViewModel.Instance.CurrentView = MainViewModel.Instance.ChiTietHoaDonVM;
                MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarChiTietHoaDonVM;
            });
            FilteredHoaDonList = CollectionViewSource.GetDefaultView(HoaDonList);
            FilteredHoaDonList.Filter = FilterHoaDon;
        }
        public void LoadHoaDonList()
        {
            HoaDonList = new ObservableCollection<HOADON>();
            var danhSachHoaDon = DataProvider.Ins.goonRunnerDB.HOADONs;
            foreach (var item in danhSachHoaDon)
            {
                var hoadon = new HOADON
                {
                    MaHD = item.MaHD,
                    MaKH = item.MaKH,
                    TenKH = item.HoKH + " " + item.TenKH,
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
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            if (!(item is HOADON hoadon))
            {
                return false;
            }

            if (int.TryParse(FilterText, out var maHD))
            {
                if (hoadon.MaHD == maHD)
                    return true;
            }
            
            var fullName = $"{hoadon.HoKH ?? ""} {hoadon.TenKH ?? ""}"; 
            if ( fullName.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
