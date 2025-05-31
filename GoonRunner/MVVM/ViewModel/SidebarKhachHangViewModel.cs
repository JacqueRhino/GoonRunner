using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarKhachHangViewModel : BaseViewModel
    {
        #region getter and setter
        public ObservableCollection<KHACHHANG> DanhSachKhachHang { get => _danhsachkhachhang; set { _danhsachkhachhang = value; OnPropertyChanged(); } }
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; FormattedDate = value.ToString("dd/MM/yyyy"); NgaySinh = value; OnPropertyChanged(); } } }
        public string FormattedDate { get => _formattedDate; private set => _formattedDate = value; }
        public string TenKH { get => _tenkh; set { _tenkh = value; OnPropertyChanged(); } }
        public string HoKH { get => _hokh; set { _hokh = value; OnPropertyChanged(); } }
        public int MaKH
        {
            get => _makh; set
            {
                _makh = value; OnPropertyChanged();
            }
        }
        public string DiaChi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }
        public string SDT { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }
        public DateTime NgaySinh { get => _ngaysinh; set { _ngaysinh = value; OnPropertyChanged(); } }
        #endregion

        #region icommand
        public ICommand AddKhachHangCommand { get; set; }
        public ICommand UpdateKhachHangCommand { get; set; }
        public ICommand DeleteKhachHangCommand { get; set; }
        public ICommand ClearFieldCommand { get; set; }
        #endregion

        #region private property
        private ObservableCollection<KHACHHANG> _danhsachkhachhang;
        private DateTime _selecteddate;
        private string _formattedDate;
        private string _hokh;
        private string _tenkh;
        private int _makh;
        private string _diachi;
        private string _sdt;
        private DateTime _ngaysinh;
        #endregion
        public SidebarKhachHangViewModel()
        {
            SelectedDate = DateTime.Now;
            DanhSachKhachHang = new ObservableCollection<KHACHHANG>(DataProvider.Ins.goonRunnerDB.KHACHHANGs);
            UpdateKhachHangCommand = new RelayCommand<Button>(p => MaKH != 0, p =>
            {
                UpdateKhachHang();
            });
            AddKhachHangCommand = new RelayCommand<Button>( (p) => { AddKhachHang(); });
            DeleteKhachHangCommand = new RelayCommand<Button>((p) => MaKH != 0, (p) => { DeleteKhachHang(); });
            
            
            ClearFieldCommand = new RelayCommand<Button>((p) => true, (p) => { ClearFields(); });
        }

        public void LoadKhachHangInfo(int maKH)
        {
            try
            {
                var khachhang = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(kh => kh.MaKH == maKH);

                if (khachhang == null) return;
                HoKH = khachhang.HoKH;
                TenKH = khachhang.TenKH;
                DiaChi = khachhang.DiaChi;
                SDT = khachhang.SdtKH;
                
                Debug.Assert(khachhang.NgaySinh != null, "khachhang.NgaySinh != null");
                NgaySinh = (DateTime)khachhang.NgaySinh;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin Khách Hàng: {ex.Message}");
            }
        }
        
        private void AddKhachHang()
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
        }

        private void ClearFields()
        {
            MaKH = 0;
            HoKH = string.Empty;
            TenKH = string.Empty;
            DiaChi = string.Empty;
            SDT = string.Empty;
            SelectedDate = DateTime.Now;
            MainViewModel.Instance?.KhachHangVM?.ResetSelectedItem();
        }
        private static bool IsInSmallDateTimeRange(DateTime dateTime)
        {
            var minSmallDateTime = new DateTime(1900, 1, 1);
            var maxSmallDateTime = new DateTime(2079, 6, 6, 23, 59, 0);

            return dateTime >= minSmallDateTime && dateTime <= maxSmallDateTime;
        }
        private static bool IsCurrentDateTime(DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }
        
        private void DeleteKhachHang()
        {
            if (MaKH == 0)
            {
                MessageBox.Show("Vui lòng chọn Khách Hàng cần xóa!");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa Khách Hàng {HoKH} {TenKH}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;
            try
            {
                var khachhang = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(kh => kh.MaKH == MaKH);

                if (khachhang == null)
                    MessageBox.Show("Không tìm thấy Khách Hàng cần xóa!");

                if (khachhang != null)
                    DataProvider.Ins.goonRunnerDB.KHACHHANGs.Remove(khachhang);
                DataProvider.Ins.goonRunnerDB.SaveChanges();

                var itemToRemove = DanhSachKhachHang.FirstOrDefault(kh => kh.MaKH == MaKH);
                if (itemToRemove != null)
                    DanhSachKhachHang.Remove(itemToRemove);

                MainViewModel.Instance.KhachHangVM?.LoadKhachHangList();

                ClearFields();
            }
            
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessage = "Lỗi xác thực dữ liệu:\n";
                foreach (var validationError in ex.EntityValidationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        errorMessage += $"- {error.ErrorMessage}\n";
                    }
                }
                MessageBox.Show(errorMessage, "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa Khách Hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateKhachHang()
        {
                try
                {
                    if (MaKH == 0)
                    {
                        MessageBox.Show("Vui lòng chọn khách hàng để cập nhật");
                        return;
                    }

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

                    if (!IsInSmallDateTimeRange(NgaySinh) || NgaySinh >= DateTime.Now)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ");
                        return;
                    }


                    var existingCustomer = DataProvider.Ins.goonRunnerDB.KHACHHANGs
                        .FirstOrDefault(kh => kh.MaKH == MaKH);

                    if (existingCustomer == null)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng để cập nhật");
                        return;
                    }

                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn cập nhật thông tin khách hàng {existingCustomer.HoKH} {existingCustomer.TenKH}?",
                        "Xác nhận cập nhật",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    existingCustomer.HoKH = HoKH;
                    existingCustomer.TenKH = TenKH;
                    existingCustomer.DiaChi = DiaChi;
                    existingCustomer.SdtKH = SDT;
                    existingCustomer.NgaySinh = NgaySinh;

                    DataProvider.Ins.goonRunnerDB.SaveChanges();

                    var localCustomer = DanhSachKhachHang.FirstOrDefault(kh => kh.MaKH == MaKH);
                    if (localCustomer != null)
                    {
                        localCustomer.HoKH = HoKH;
                        localCustomer.TenKH = TenKH;
                        localCustomer.SdtKH = SDT;
                        localCustomer.NgaySinh = NgaySinh;
                    }

                    MainViewModel.Instance?.KhachHangVM?.LoadKhachHangList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", 
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }
    }
}