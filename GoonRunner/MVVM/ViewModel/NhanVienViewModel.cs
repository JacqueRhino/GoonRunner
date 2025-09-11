using GoonRunner.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NHANVIEN> _nhanvienlist;
        public ObservableCollection<NHANVIEN> NhanVienList { get { return _nhanvienlist; } set { _nhanvienlist = value; OnPropertyChanged(); } }
        private bool _isUpdatingSelection;
        private NHANVIEN _selectedItem;
        public NHANVIEN SelectedItem
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
                    Messenger.NotifyNhanVienSelected(_selectedItem?.MaNV ?? 0);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }
        }
        private int _manv;
        public int MaNV { get => _manv; set { _manv  = value; OnPropertyChanged(); } }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand LoadToSidebar { get; set; }
        public ICommand RefreshCommand { get; set; }
        public NhanVienViewModel()
        {
            LoadNhanVienList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadNhanVienList(); });
            Messenger.NhanVienChanged += nv =>
            {
                LoadNhanVienList();
            };
            
            Messenger.NhanVienSelected += manv =>
            {
                SelectedItem = NhanVienList.FirstOrDefault(nv =>  nv.MaNV== manv);
            };

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