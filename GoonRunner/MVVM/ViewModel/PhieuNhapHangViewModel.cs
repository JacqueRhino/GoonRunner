using System;
using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class PhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<PHIEUNHAPHANG> _phieunhaphanglist;
        public ObservableCollection<PHIEUNHAPHANG> PhieuNhapHangList { get { return _phieunhaphanglist; } set { _phieunhaphanglist = value; OnPropertyChanged(); } }
        private bool _isUpdatingSelection;
        private PHIEUNHAPHANG _selectedItem;
        public PHIEUNHAPHANG SelectedItem
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
                    Messenger.NotifyPhieuNhapHangSelected(_selectedItem?.MaPNH ?? 0);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }
        }

        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredPhieuNhapHangList.Refresh(); } }
        private ICollectionView _filteredphieunhaphanglist;
        public ICollectionView FilteredPhieuNhapHangList { get => _filteredphieunhaphanglist; set { _filteredphieunhaphanglist = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private readonly Action<int> _navigateToChiTietPhieuNhap;
        public PhieuNhapHangViewModel( Action<int> navigateToChiTietPhieuNhap)
        {
            _navigateToChiTietPhieuNhap = navigateToChiTietPhieuNhap ?? throw new ArgumentNullException(nameof(navigateToChiTietPhieuNhap));
            LoadPhieuNhapHangList();
            Messenger.PhieuNhapHangChanged += pnh =>
            {
                LoadPhieuNhapHangList();
            };
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                LoadPhieuNhapHangList();
                FilterText = string.Empty;
            });
            FilteredPhieuNhapHangList = CollectionViewSource.GetDefaultView(PhieuNhapHangList);
            FilteredPhieuNhapHangList.Filter = FilterPhieuNhapHang;
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                _navigateToChiTietPhieuNhap?.Invoke(SelectedItem.MaPNH);
            });
        }
        public void LoadPhieuNhapHangList()
        {
            PhieuNhapHangList = new ObservableCollection<PHIEUNHAPHANG>();
            var DanhSachPhieuNhapHang = DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs;
            foreach (var item in DanhSachPhieuNhapHang)
            {
                PHIEUNHAPHANG phieunhaphang = new PHIEUNHAPHANG();
                phieunhaphang.MaPNH = item.MaPNH;
                phieunhaphang.MaNCC = item.MaNCC;
                phieunhaphang.TenNCC = item.TenNCC;
                phieunhaphang.NgayNhap = item.NgayNhap;
                phieunhaphang.MaNV = item.MaNV;
                PhieuNhapHangList.Add(phieunhaphang);
            }
        }
        
        private bool FilterPhieuNhapHang(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            if (!(item is PHIEUNHAPHANG phieunhaphang))
            {
                return false;
            }

            if (int.TryParse(FilterText, out var maPNH ))
            {
                if (phieunhaphang.MaPNH == maPNH)
                    return true;
            }
            
            if ( phieunhaphang.TenNCC?.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
