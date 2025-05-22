using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SanPhamViewModel : BaseViewModel
    {
        private ObservableCollection<SANPHAM> _sanphamlist;
        public ObservableCollection<SANPHAM> SanPhamList { get { return _sanphamlist; } set { _sanphamlist = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public SanPhamViewModel()
        {
            LoadSanPhamList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadSanPhamList(); });
        }
        private void LoadSanPhamList()
        {
            SanPhamList = new ObservableCollection<SANPHAM>();
            var DanhSachSanPham = DataProvider.Ins.goonRunnerDB.SANPHAMs;
            int i = 1;
            foreach (var item in DanhSachSanPham)
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
                i++;
            }
        }
    }
}
