using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class PhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<PHIEUNHAPHANG> _phieunhaphanglist;
        public ObservableCollection<PHIEUNHAPHANG> PhieuNhapHangList { get { return _phieunhaphanglist; } set { _phieunhaphanglist = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public PhieuNhapHangViewModel()
        {
            LoadPhieuNhapHangList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadPhieuNhapHangList(); });
        }
        private void LoadPhieuNhapHangList()
        {
            PhieuNhapHangList = new ObservableCollection<PHIEUNHAPHANG>();
            var DanhSachPhieuNhapHang = DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs;
            int i = 1;
            foreach (var item in DanhSachPhieuNhapHang)
            {
                PHIEUNHAPHANG phieunhaphang = new PHIEUNHAPHANG();
                phieunhaphang.MaPNH = item.MaPNH;
                phieunhaphang.MaNCC = item.MaNCC;
                phieunhaphang.TenNCC = item.TenNCC;
                phieunhaphang.NgayNhap = item.NgayNhap;
                phieunhaphang.MaNV = item.MaNV;
                PhieuNhapHangList.Add(phieunhaphang);
                i++;
            }
        }
    }
}
