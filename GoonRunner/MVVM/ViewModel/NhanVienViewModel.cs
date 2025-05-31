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
        public ICommand LoadToSidebar { get; set; }
        public ICommand RefreshCommand { get; set; }
        public NhanVienViewModel()
        {
            LoadNhanVienList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadNhanVienList(); });
            LoadToSidebar = new RelayCommand<object>((p) => SelectedItem != null, (p) =>
            {
                MainViewModel.Instance.SidebarNhanVienVM.MaNV = SelectedItem.MaNV;
                MainViewModel.Instance.SidebarNhanVienVM.LoadNhanVienInfo(SelectedItem.MaNV);
            });
        }
        public void LoadNhanVienList()
        {
            NhanVienList = new ObservableCollection<NHANVIEN>();
            var danhSachNhanVien = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where(n => n.MaNV > 0);
            
            foreach (var item in danhSachNhanVien)
            {
                var nhanvien = new NHANVIEN
                {
                    MaNV = item.MaNV,
                    TenNV = item.HoNV + " " + item.TenNV,
                    MaPB = item.MaPB,
                    GioiTinh = item.GioiTinh,
                    SdtNV = item.SdtNV,
                    DiaChiNV = item.DiaChiNV
                };
                NhanVienList.Add(nhanvien);
            }
        }
    }
}