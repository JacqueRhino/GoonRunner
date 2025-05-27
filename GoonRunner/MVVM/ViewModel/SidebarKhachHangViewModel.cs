using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarKhachHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _danhsachkhachhang;
        public ObservableCollection<KHACHHANG> DanhSachKhachHang { get => _danhsachkhachhang; set { _danhsachkhachhang = value; OnPropertyChanged(); } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; FormattedDate = value.ToString("dd/MM/yyyy"); NgaySinh = value; OnPropertyChanged(); } } }
        private string _formattedDate;
        public string FormattedDate { get => _formattedDate; private set => _formattedDate = value; }
        private string _hokh;
        public string HoKH { get => _hokh; set { _hokh = value; OnPropertyChanged(); } }
        private string _tenkh;
        public string TenKH { get => _tenkh; set { _tenkh = value; OnPropertyChanged(); } }
        private string _diachi;
        public string DiaChi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }
        private string _sdt;
        public string SDT { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }
        private DateTime _ngaysinh;
        public DateTime NgaySinh { get => _ngaysinh; set { _ngaysinh = value; OnPropertyChanged(); } }
        public ICommand AddKhachHangCommand { get; set; }
        public ICommand ClearFieldCommand { get; set; }
        public SidebarKhachHangViewModel()
        {
            SelectedDate = DateTime.Now;
            DanhSachKhachHang = new ObservableCollection<KHACHHANG>(DataProvider.Ins.goonRunnerDB.KHACHHANGs);
            AddKhachHangCommand = new RelayCommand<Button>((p) => { return true; }, (p) => 
            {
                if (string.IsNullOrEmpty(HoKH))
                {
                    MessageBox.Show("Hãy nhập Họ KH");
                    return;
                }

                if (string.IsNullOrEmpty(TenKH))
                {
                    MessageBox.Show("Hãy nhập Tên KH");
                    return;
                }

                if (string.IsNullOrEmpty(DiaChi))
                {
                    MessageBox.Show("Hãy nhập Địa chỉ KH");
                    return;
                }

                if (string.IsNullOrEmpty(SDT))
                {
                    MessageBox.Show("Hãy nhập Số điện thoại");
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

                var khachhang = new KHACHHANG() { HoKH = HoKH, TenKH = TenKH, DiaChi = DiaChi, SdtKH = SDT, NgaySinh = NgaySinh };
                DataProvider.Ins.goonRunnerDB.KHACHHANGs.Add(khachhang);
                DataProvider.Ins.goonRunnerDB.SaveChanges();
                DanhSachKhachHang.Add(khachhang);
                MessageBox.Show("Thêm thành công!");
                MainViewModel.Instance?.KhachHangVM?.LoadKhachHangList();
            });

            ClearFieldCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                ClearFields();
            });
        }
        private void ClearFields()
        {
            HoKH = string.Empty;
            TenKH = string.Empty;
            DiaChi = string.Empty;
            SDT = string.Empty;
            SelectedDate = DateTime.Now;
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