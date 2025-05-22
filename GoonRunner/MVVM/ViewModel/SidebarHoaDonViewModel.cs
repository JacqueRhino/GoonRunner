using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarHoaDonViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> _danhsachhoadon;
        public ObservableCollection<HOADON> DanhSachHoaDon { get => _danhsachhoadon; set { _danhsachhoadon = value; OnPropertyChanged(); } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; FormattedDate = value.ToString("dd/MM/yyyy"); NgayMuaHang = value; OnPropertyChanged(); } } }
        private string _formattedDate;
        public string FormattedDate { get => _formattedDate; private set => _formattedDate = value; }
        private int _mahd;
        public int MaHD { get => _mahd; set { _mahd = value; OnPropertyChanged(); } }
        private int _makh;
        public int MaKH { get => _makh; set { _makh = value; OnPropertyChanged(); } }
        private string _hokh;
        public string HoKH { get => _hokh; set { _hokh = value; OnPropertyChanged(); } }
        private string _tenkh;
        public string TenKH { get => _tenkh; set { _tenkh = value; OnPropertyChanged(); } }
        private string _sdtkh;
        public string SDTKH { get => _sdtkh; set { _sdtkh = value; OnPropertyChanged(); } }
        private string _diachi;
        public string DiaChi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }
        private int _manv;
        public int MaNV { get => _manv; set { _manv = value; OnPropertyChanged(); } }
        private string _honv;
        public string HoNV { get => _honv; set { _honv = value; OnPropertyChanged(); } }
        private string _tennv;
        public string TenNV { get => _tennv; set { _tennv = value; OnPropertyChanged(); } }
        private DateTime _ngaymuahang;
        public DateTime NgayMuaHang { get => _ngaymuahang; set { _ngaymuahang = value; OnPropertyChanged(); } }
        public ICommand AddHoaDonCommand { get; set; }

        public SidebarHoaDonViewModel()
        {
            SelectedDate = DateTime.Now;
            DanhSachHoaDon = new ObservableCollection<HOADON>(DataProvider.Ins.goonRunnerDB.HOADONs);
            AddHoaDonCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaKH == 0)
                {
                    MessageBox.Show("Hãy nhập Mã khách hàng");
                    return;
                }

                if (string.IsNullOrEmpty(HoKH))
                {
                    MessageBox.Show("Hãy nhập Họ khách hàng");
                    return;
                }
                
                if (string.IsNullOrEmpty(TenKH))
                {
                    MessageBox.Show("Hãy nhập Tên khách hàng");
                    return;
                }
                
                if (string.IsNullOrEmpty(SDTKH))
                {
                    MessageBox.Show("Hãy nhập Số điện thoại khách hàng");
                    return;
                }
                
                if (string.IsNullOrEmpty(DiaChi))
                {
                    MessageBox.Show("Hãy nhập Địa chỉ khách hàng");
                    return;
                }

                if (MaNV == 0)
                {
                    MessageBox.Show("Hãy nhập Mã nhân viên");
                    return;
                }

                if (string.IsNullOrEmpty(HoNV))
                {
                    MessageBox.Show("Hãy nhập Họ nhân viên");
                    return;
                }

                if (string.IsNullOrEmpty(TenNV))
                {
                    MessageBox.Show("Hãy nhập Tên nhân viên");
                    return;
                }

                if (!IsInSmallDateTimeRange(NgayMuaHang))
                {
                    MessageBox.Show("Ngày mua hàng không hợp lệ");
                    return;
                }

                var hoadon = new HOADON() 
                { 
                    MaKH = MaKH, 
                    HoKH = HoKH, 
                    TenKH = TenKH,
                    SdtKH = SDTKH,
                    DiaChi = DiaChi,
                    MaNV = MaNV,
                    HoNV = HoNV,
                    TenNV = TenNV,
                    NgayMuaHang = NgayMuaHang
                };
                DataProvider.Ins.goonRunnerDB.HOADONs.Add(hoadon);
                DataProvider.Ins.goonRunnerDB.SaveChanges();
                DanhSachHoaDon.Add(hoadon);
                MessageBox.Show("Thêm thành công!");
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
