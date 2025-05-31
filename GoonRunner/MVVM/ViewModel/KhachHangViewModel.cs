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
        public ObservableCollection<KHACHHANG> KhachHangList { get; set; } = new ObservableCollection<KHACHHANG>();

        private KHACHHANG _selectedItem;
        public KHACHHANG SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }

        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredKhachHangList.Refresh(); } }

        private ICollectionView FilteredKhachHangList { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand LoadToSidebar { get; set; }
        

        public KhachHangViewModel()
        {
            Init();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { RefreshList(); }); 
            LoadToSidebar = new RelayCommand<object>((p) => SelectedItem != null, (p) => { LoadDataToSidebar(); });
        }

        public void ResetSelectedItem()
        {
            SelectedItem = null;
        }

        public void LoadKhachHangList()
        {
            KhachHangList.Clear();
            var danhSachKhachHang = DataProvider.Ins.goonRunnerDB.KHACHHANGs;
            foreach (var item in danhSachKhachHang)
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

            FilteredKhachHangList?.Refresh();
        }
        
        private void LoadDataToSidebar()
        {
            MainViewModel.Instance.SidebarKhachHangVM.MaKH = SelectedItem.MaKH;
            MainViewModel.Instance.SidebarKhachHangVM.LoadKhachHangInfo(SelectedItem.MaKH);
        }
        
        private bool FilterKhachHang(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText)) return true;

            if (!(item is KHACHHANG khachhang)) return false;

            if (!int.TryParse(FilterText, out var maKH))
                return khachhang.TenKH.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
            
            if (khachhang.MaKH == maKH)
                return true;

            return khachhang.TenKH.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void RefreshList()
        {
            LoadKhachHangList();
            FilterText = string.Empty;
            FilteredKhachHangList.Refresh();
        }

        private void Init()
        {
            LoadKhachHangList();
            FilteredKhachHangList = CollectionViewSource.GetDefaultView(KhachHangList);
            FilteredKhachHangList.Filter = FilterKhachHang;
        }
        
    }
}