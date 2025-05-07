using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarNhanVienViewModel : BaseViewModel
    {
        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string> 
        { 
            "Nhân viên bán hàng", 
            "Chăm sóc khách hàng", 
            "Nhân viên kiểm kho", 
            "Quản lý cửa hàng", 
            "Nhân viên kỹ thuật web", 
            "Nhân viên bảo hành",
            "Nhân viên Marketing",
            "Giao hàng", 
            "Bảo vệ",
            "Lao công"
        };
        public ObservableCollection<string> Gender { get; } = new ObservableCollection<string>
        {
            "Nam",
            "Nữ"
        };

        private ObservableCollection<NHANVIEN> _danhsachnhanvien;
        public ObservableCollection<NHANVIEN> DanhSachNhanVien { get => _danhsachnhanvien; set { _danhsachnhanvien = value; OnPropertyChanged(); } }
        private string _selectedrole;
        public string SelectedRole { get => _selectedrole; set { if (_selectedrole != value) { _selectedrole = value; ChucVu = value; OnPropertyChanged(); } } }
        private string _selectedgender;
        public string SelectedGender { get => _selectedgender; set { if (_selectedgender != value) { _selectedgender = value; GioiTinh = value; OnPropertyChanged(); } } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; NgaySinh = value; OnPropertyChanged(); } } }
        private string _honv;
        public string HoNV { get => _honv; set { _honv = value; OnPropertyChanged(); } }
        private string _tennv;
        public string TenNV { get => _tennv; set { _tennv = value; OnPropertyChanged(); } }
        private string _diachi;
        public string DiaChi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }
        private string _sdt;
        public string SDT { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }
        private string _cmnd;
        public string CMND { get => _cmnd; set { _cmnd = value; OnPropertyChanged(); } }
        private string _chucvu;
        public string ChucVu
        {
            get => _chucvu;
            set
            {
                if (_chucvu != value)
                {
                    _chucvu = value;
                    switch (_chucvu)
                    {
                        case "Nhân viên bán hàng":
                            _mapb = 1;
                            break;
                        case "Chăm sóc khách hàng":
                            _mapb = 2;
                            break;
                        case "Quản lý cửa hàng":
                            _mapb = 3;
                            break;
                        case "Nhân viên kỹ thuật web":
                            _mapb = 4;
                            break;
                        case "Nhân viên bảo hành":
                            _mapb = 5;
                            break;
                        case "Nhân viên Marketing":
                            _mapb = 6;
                            break;
                        case "Giao hàng":
                            _mapb = 7;
                            break;
                        case "Bảo vệ":
                            _mapb = 8;
                            break;
                        case "Lao công":
                            _mapb = 9;
                            break;
                        default:
                            _mapb = 0;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }
        private string _gioitinh;
        public string GioiTinh { get => _gioitinh; set { _gioitinh = value; OnPropertyChanged(); } }
        private int _mapb; 
        public int MaPB { get => _mapb; set { _mapb = value; OnPropertyChanged(); } }
        private DateTime _ngaysinh;
        public DateTime NgaySinh { get => _ngaysinh; set { _ngaysinh = value; OnPropertyChanged(); } }
        public ICommand AddNhanVienCommand { get; set; }
        public SidebarNhanVienViewModel()
        {
            
            DanhSachNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.goonRunnerDB.NHANVIENs);
            AddNhanVienCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(HoNV))
                {
                    MessageBox.Show("Hãy nhập Họ NV");
                    return;
                }                   

                if (string.IsNullOrEmpty(TenNV))
                {
                    MessageBox.Show("Hãy nhập Tên NV");
                    return;
                }                 

                if (string.IsNullOrEmpty(DiaChi))
                {
                    MessageBox.Show("Hãy nhập Địa chỉ NV");
                    return;
                }               

                if (string.IsNullOrEmpty(SDT))
                {
                    MessageBox.Show("Hãy nhập Số điện thoại");
                    return;
                }

                int existingCMND = DataProvider.Ins.goonRunnerDB.NHANVIENs.Where(nv => nv.CMND == CMND).Count();
                if (existingCMND > 0)
                {
                    MessageBox.Show("CMND đã tồn tại trong hệ thống. Vui lòng kiểm tra lại!", "Trùng CMND", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CMND = "";
                    return;
                } 

                if (!IsInSmallDateTimeRange(NgaySinh))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ");
                    return;
                }

                if (IsCurrentDateTime(NgaySinh))
                {
                    MessageBox.Show("Ngày sinh không thể là ngày hôm nay");
                    return;
                }

                

                var nhanvien = new NHANVIEN() { HoNV = HoNV, TenNV = TenNV, DiaChiNV = DiaChi, SdtNV = SDT, CMND = CMND, ChucVu = ChucVu, GioiTinh = GioiTinh, NgaySinh = NgaySinh, MaPB = MaPB};
                DataProvider.Ins.goonRunnerDB.NHANVIENs.Add(nhanvien);
                DataProvider.Ins.goonRunnerDB.SaveChanges();
                DanhSachNhanVien.Add(nhanvien);
            });

        }
        private bool IsInSmallDateTimeRange(DateTime dateTime)
        {
            // Define SMALLDATETIME boundaries
            DateTime minSmallDateTime = new DateTime(1900, 1, 1);
            DateTime maxSmallDateTime = new DateTime(2079, 6, 6, 23, 59, 0);

            // Check if the date is within range
            return dateTime >= minSmallDateTime && dateTime <= maxSmallDateTime;
        }
        private bool IsCurrentDateTime(DateTime dateTime)
        {
            if (dateTime.Date == DateTime.Today)
            {
                return true;
            }
            return false;
        }
    }
}