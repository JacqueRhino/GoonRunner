using System;
using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class KhachHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _khachhanglist;
        public ObservableCollection<KHACHHANG> KhachHangList { get => _khachhanglist; set { _khachhanglist = value; OnPropertyChanged(); } }
        private KHACHHANG _selectedKHACHHANG;
        public KHACHHANG SelectedKHACHHANG { get => _selectedKHACHHANG; set { _selectedKHACHHANG = value; OnPropertyChanged(); } }
        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredKhachHangList.Refresh(); } }
        private ICollectionView _filteredKhachHangList;
        public ICollectionView FilteredKhachHangList { get => _filteredKhachHangList; set { _filteredKhachHangList = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }

        public KhachHangViewModel()
        {
            LoadKhachHangList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                LoadKhachHangList();
                FilterText = string.Empty; 
            });
            FilteredKhachHangList = CollectionViewSource.GetDefaultView(KhachHangList);
            FilteredKhachHangList.Filter = FilterKhachHang;
        }

        public void LoadKhachHangList()
        {
            KhachHangList = new ObservableCollection<KHACHHANG>();
            var DanhSachKhachHang = DataProvider.Ins.goonRunnerDB.KHACHHANGs;
            foreach (var item in DanhSachKhachHang)
            {
                var khachhang = new KHACHHANG
                {
                    MaKH = item.MaKH,
                    TenKH = item.HoKH + " " + item.TenKH,
                    SdtKH = item.SdtKH,
                    NgaySinh = item.NgaySinh,
                    DiaChi = item.DiaChi
                };
                KhachHangList.Add(khachhang);
            }
        }

        private bool FilterKhachHang(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            if (!(item is KHACHHANG khachhang))
            {
                return false;
            }
            if (int.TryParse(FilterText, out var maKH))
            {
                if (khachhang.MaKH == maKH)
                    return true;
            }
            var fullName = $"{khachhang.HoKH ?? ""} {khachhang.TenKH ?? ""}"; 
            if ( fullName.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}