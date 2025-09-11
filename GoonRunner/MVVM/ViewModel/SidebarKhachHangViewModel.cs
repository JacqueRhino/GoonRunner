using GoonRunner.MVVM.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarKhachHangViewModel : BaseViewModel
    {
        #region Properties

        private int _makh;
        public int MaKh
        {
            get => _makh;
            set { _makh = value; OnPropertyChanged(); }
        }

        private string _hokh;
        public string HoKh
        {
            get => _hokh;
            set { _hokh = value; OnPropertyChanged(); }
        }

        private string _tenkh;
        public string TenKh
        {
            get => _tenkh;
            set { _tenkh = value; OnPropertyChanged(); }
        }

        private string _diachi;
        public string DiaChi
        {
            get => _diachi;
            set { _diachi = value; OnPropertyChanged(); }
        }

        private string _sdt;
        public string Sdt
        {
            get => _sdt;
            set { _sdt = value; OnPropertyChanged(); }
        }

        private DateTime _ngaysinh;

        private DateTime NgaySinh
        {
            get => _ngaysinh;
            set { _ngaysinh = value; OnPropertyChanged(); }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    NgaySinh = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands
        public ICommand AddKhachHangCommand { get; }
        public ICommand UpdateKhachHangCommand { get; }
        public ICommand DeleteKhachHangCommand { get; }
        public ICommand ClearFieldCommand { get; }
        #endregion
        
        

        public SidebarKhachHangViewModel()
        {
            SelectedDate = DateTime.Now;
            
            Messenger.KhachHangSelected += LoadKhachHangInfo;


            AddKhachHangCommand = new RelayCommand<Button>(_ => true, _ => AddKhachHang());
            UpdateKhachHangCommand = new RelayCommand<Button>(_ => MaKh != 0, _ => UpdateKhachHang());
            DeleteKhachHangCommand = new RelayCommand<Button>(_ => MaKh != 0, _ => DeleteKhachHang());
            ClearFieldCommand = new RelayCommand<Button>(_ => true, _ => ClearFields());
        }

        #region Methods
        private bool _isUpdatingSelection;

        private void LoadKhachHangInfo(int maKh)
        {
            if (_isUpdatingSelection) return;

            try
            {
                _isUpdatingSelection = true;

                var kh = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(k => k.MaKH == maKh);
                if (kh == null) return;

                HoKh = kh.HoKH;
                TenKh = kh.TenKH;
                DiaChi = kh.DiaChi;
                Sdt = kh.SdtKH;
                NgaySinh = kh.NgaySinh ?? DateTime.Now;

                MaKh = kh.MaKH;
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }
        private void AddKhachHang()
        {
            if (!ValidateFields()) return;

            var kh = new KHACHHANG
            {
                HoKH = HoKh,
                TenKH = TenKh,
                DiaChi = DiaChi,
                SdtKH = Sdt,
                NgaySinh = NgaySinh
            };

            DataProvider.Ins.goonRunnerDB.KHACHHANGs.Add(kh);
            DataProvider.Ins.goonRunnerDB.SaveChanges();

            Messenger.NotifyKhachHangChanged(kh);

            MessageBox.Show("Thêm thành công!");
            ClearFields();
        }

        private void UpdateKhachHang()
        {
            if (MaKh == 0 || !ValidateFields()) return;

            var kh = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(k => k.MaKH == MaKh);
            if (kh == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng để cập nhật!");
                return;
            }

            kh.HoKH = HoKh;
            kh.TenKH = TenKh;
            kh.DiaChi = DiaChi;
            kh.SdtKH = Sdt;
            kh.NgaySinh = NgaySinh;

            DataProvider.Ins.goonRunnerDB.SaveChanges();
            Messenger.NotifyKhachHangChanged(kh);

            MessageBox.Show("Cập nhật thành công!");
        }

        private void DeleteKhachHang()
        {
            if (MaKh == 0) return;

            var kh = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(k => k.MaKH == MaKh);
            if (kh == null) return;

            DataProvider.Ins.goonRunnerDB.KHACHHANGs.Remove(kh);
            DataProvider.Ins.goonRunnerDB.SaveChanges();

            Messenger.NotifyKhachHangChanged(kh);

            ClearFields();
        }

        private void ClearFields()
        {
            MaKh = 0;
            HoKh = TenKh = DiaChi = Sdt = string.Empty;
            SelectedDate = DateTime.Now;
            Messenger.NotifyKhachHangSelected(0);
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(HoKh) || string.IsNullOrWhiteSpace(TenKh)
                || string.IsNullOrWhiteSpace(DiaChi) || string.IsNullOrWhiteSpace(Sdt))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return false;
            }

            if (!IsInSmallDateTimeRange(NgaySinh) || IsCurrentDateTime(NgaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ!");
                return false;
            }

            return true;
        }

        private static bool IsInSmallDateTimeRange(DateTime dateTime)
        {
            var min = new DateTime(1900, 1, 1);
            var max = new DateTime(2079, 6, 6, 23, 59, 0);
            return dateTime >= min && dateTime <= max;
        }

        private static bool IsCurrentDateTime(DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }
        #endregion
    }
}
