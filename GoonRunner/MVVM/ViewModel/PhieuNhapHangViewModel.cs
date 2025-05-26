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
        private PHIEUNHAPHANG _selectedItem;
        public PHIEUNHAPHANG SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public PhieuNhapHangViewModel()
        {
            LoadPhieuNhapHangList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadPhieuNhapHangList(); });
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                MainViewModel.Instance.ChiTietPhieuNhapHangVM = new ChiTietPhieuNhapHangViewModel(SelectedItem.MaPNH);
                MainViewModel.Instance.SidebarChiTietPhieuNhapHangVM = new SidebarChiTietPhieuNhapHangViewModel(SelectedItem.MaPNH);

                MainViewModel.Instance.CurrentView = MainViewModel.Instance.ChiTietPhieuNhapHangVM;
                MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarChiTietPhieuNhapHangVM;
            });
        }
        public void LoadPhieuNhapHangList()
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
