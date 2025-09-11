using System;
using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class KhachHangViewModel : BaseViewModel
    {
        public ObservableCollection<KHACHHANG> KhachHangList { get; set; } = new ObservableCollection<KHACHHANG>();

        private bool _isUpdatingSelection;
        private KHACHHANG _selectedItem;
        public KHACHHANG SelectedItem
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
                    Messenger.NotifyKhachHangSelected(_selectedItem?.MaKH ?? 0);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }
        }
        

        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredKhachHangList.Refresh(); } }

        private ICollectionView FilteredKhachHangList { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand LoadToSidebar { get; set; }
        

        public KhachHangViewModel()
        {
            Init();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { RefreshList(); }); 
            Messenger.KhachHangChanged += kh =>
            {
                LoadKhachHangList();
            };
            
            Messenger.KhachHangSelected += maKh =>
            {
                SelectedItem = KhachHangList.FirstOrDefault(kh => kh.MaKH == maKh);
            };
        }

        public void ResetSelectedItem()
        {
            SelectedItem = null;
        }

        private void LoadKhachHangList()
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
        
        private bool FilterKhachHang(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText)) return true;

            if (!(item is KHACHHANG khachhang)) return false;

            if (!int.TryParse(FilterText, out var maKh))
                return khachhang.TenKH.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
            
            if (khachhang.MaKH == maKh)
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