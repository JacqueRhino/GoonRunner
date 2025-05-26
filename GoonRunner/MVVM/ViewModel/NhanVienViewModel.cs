using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NHANVIEN> _nhanvienlist;
        public ObservableCollection<NHANVIEN> NhanVienList { get { return _nhanvienlist; } set { _nhanvienlist = value; OnPropertyChanged(); } }
        private NHANVIEN _selectedItem;
        public NHANVIEN SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(); } }
        private int _manv;
        public int MaNV { get => _manv; set { _manv  = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public NhanVienViewModel()
        {
            LoadNhanVienList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadNhanVienList(); });
            DoubleClickCommand = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                MainViewModel.Instance.SidebarNhanVienVM = new SidebarNhanVienViewModel(SelectedItem.MaNV);

                //MainViewModel.Instance.CurrentView = MainViewModel.Instance.ChiTietHoaDonVM;
                //MainViewModel.Instance.CurrentSidebarView = MainViewModel.Instance.SidebarNhanVienVM;
            });
        }
        public void LoadNhanVienList()
        {
            NhanVienList = new ObservableCollection<NHANVIEN>();
            var DanhSachNhanVien = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where(n => n.MaNV > 0);
            
            foreach (var item in DanhSachNhanVien)
            {
                NHANVIEN nhanvien = new NHANVIEN();
                nhanvien.MaNV = item.MaNV;
                nhanvien.TenNV = item.HoNV + " " + item.TenNV;
                nhanvien.MaPB = item.MaPB;
                nhanvien.GioiTinh = item.GioiTinh;
                nhanvien.SdtNV = item.SdtNV;
                nhanvien.DiaChiNV = item.DiaChiNV;
                NhanVienList.Add(nhanvien);
            }
        }
    }
}