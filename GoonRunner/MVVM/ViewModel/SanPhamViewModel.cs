using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SanPhamViewModel : BaseViewModel
    {
        private ObservableCollection<SANPHAM> _sanphamlist;
        public ObservableCollection<SANPHAM> SanPhamList { get { return _sanphamlist; } set { _sanphamlist = value; OnPropertyChanged(); } }
        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredSanPhamList.Refresh(); } }
        private ICollectionView _filteredsanphamlist;
        public ICollectionView FilteredSanPhamList { get => _filteredsanphamlist; set { _filteredsanphamlist = value; OnPropertyChanged(); } }
        
        public ICommand RefreshCommand { get; set; }
        public SanPhamViewModel()
        {
            LoadSanPhamList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                LoadSanPhamList();
                FilterText = string.Empty;
            });
            FilteredSanPhamList = CollectionViewSource.GetDefaultView(SanPhamList);
            FilteredSanPhamList.Filter = FilterSanPham;
        }
        private void LoadSanPhamList()
        {
            SanPhamList = new ObservableCollection<SANPHAM>();
            var danhSachSanPham = DataProvider.Ins.goonRunnerDB.SANPHAMs;
            foreach (var item in danhSachSanPham)
            {
                SANPHAM sanpham = new SANPHAM();
                sanpham.MaSP = item.MaSP;
                sanpham.TenSP = item.TenSP;
                sanpham.LoaiSP = item.LoaiSP;
                sanpham.ThoiGianBH = item.ThoiGianBH;
                sanpham.GiaSP = item.GiaSP;
                sanpham.NhaSX = item.NhaSX;
                sanpham.CoKhuyenMai = item.CoKhuyenMai;
                SanPhamList.Add(sanpham);
            }
        }
        
        private bool FilterSanPham(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            if (!(item is SANPHAM sanpham))
            {
                return false;
            }

            if (int.TryParse(FilterText, out var maSP))
            {
                if (sanpham.MaSP == maSP)
                    return true;
            }
            
            if ( sanpham.TenSP.ToLower().Contains(FilterText.ToLower()) )
            {
                return true;
            }
            return false;
        }
    }
}
