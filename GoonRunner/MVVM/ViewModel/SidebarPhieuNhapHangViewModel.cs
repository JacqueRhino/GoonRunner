using GoonRunner.MVVM.Model;
using GoonRunner.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarPhieuNhapHangViewModel : BaseViewModel
    {
        #region Public Properties

        public ObservableCollection<PHIEUNHAPHANG> DanhSachPhieuNhapHang
        {
            get => _danhsachphieunhaphang;
            set { _danhsachphieunhaphang = value; OnPropertyChanged(); }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    NgayNhap = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaNcc
        {
            get => _maNcc;
            set { _maNcc = value; LoadNhaCungCapInfo(value); OnPropertyChanged(); }
        }

        public string TenNcc
        {
            get => _tenNcc;
            set { _tenNcc = value; OnPropertyChanged(); }
        }

        public int MaNv
        {
            get => _maNv;
            set { _maNv = value; OnPropertyChanged(); }
        }

        public DateTime NgayNhap
        {
            get => _ngayNhap;
            set { _ngayNhap = value; OnPropertyChanged(); }
        }

        #endregion

        #region Public Commands

        public ICommand AddPhieuNhapHangCommand { get; }
        public ICommand ClearFieldCommand { get; }

        #endregion

        #region Constructor

        public SidebarPhieuNhapHangViewModel(UserSession userSession)
        {
            if (userSession == null) throw new ArgumentNullException(nameof(userSession));

            MaNv = userSession.UserId;

            Messenger.PhieuNhapHangSelected += LoadPnh;
            SelectedDate = DateTime.Now;

            DanhSachPhieuNhapHang = new ObservableCollection<PHIEUNHAPHANG>(
                DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs
            );

            AddPhieuNhapHangCommand = new RelayCommand<Button>(_ => true, _ => AddPhieuNhapHang(userSession));
            ClearFieldCommand = new RelayCommand<Button>(_ => true, _ => ClearFields());
        }

        #endregion

        #region Private Fields

        private ObservableCollection<PHIEUNHAPHANG> _danhsachphieunhaphang;
        private DateTime _selectedDate;
        private int _maNcc;
        private string _tenNcc;
        private int _maNv;
        private DateTime _ngayNhap;

        #endregion

        #region Private Helpers

        private void AddPhieuNhapHang(UserSession user)
        {
            if (MaNcc == 0)
            {
                MessageBox.Show("Hãy nhập Mã nhà cung cấp");
                return;
            }

            if (string.IsNullOrEmpty(TenNcc))
            {
                MessageBox.Show("Hãy nhập Tên nhà cung cấp");
                return;
            }

            if (!IsInSmallDateTimeRange(NgayNhap))
            {
                MessageBox.Show("Ngày nhập hàng không hợp lệ");
                return;
            }

            if (user.UserId == 0)
            {
                MessageBox.Show("Không tìm thấy nhân viên hiện tại");
                return;
            }

            var phieuNhapHang = new PHIEUNHAPHANG
            {
                MaNCC = MaNcc,
                TenNCC = TenNcc,
                NgayNhap = NgayNhap,
                MaNV = user.UserId
            };

            DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs.Add(phieuNhapHang);
            DataProvider.Ins.goonRunnerDB.SaveChanges();
            DanhSachPhieuNhapHang.Add(phieuNhapHang);

            MessageBox.Show("Thêm thành công!");
            Messenger.NotifyPhieuNhapHangChanged(phieuNhapHang);
        }

        private void ClearFields()
        {
            MaNcc = 0;
            TenNcc = string.Empty;
            SelectedDate = DateTime.Now;
        }

        private void LoadPnh(int maPnh)
        {
            try
            {
                var phieuNhapHang = DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs
                    .FirstOrDefault(p => p.MaPNH == maPnh);

                if (phieuNhapHang == null) return;

                TenNcc = phieuNhapHang.TenNCC;
                MaNcc = phieuNhapHang.MaNCC;

                Debug.Assert(phieuNhapHang.NgayNhap != null, "phieuNhapHang.NgayNhap != null");
                NgayNhap = (DateTime)phieuNhapHang.NgayNhap;
                SelectedDate = (DateTime)phieuNhapHang.NgayNhap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin phiếu nhập hàng: {ex.Message}");
                throw;
            }
        }

        private void LoadNhaCungCapInfo(int maNcc)
        {
            try
            {
                var nhacungcap = DataProvider.Ins.goonRunnerDB.NHACUNGCAPs
                    .FirstOrDefault(ncc => ncc.MaNCC == maNcc);

                if (nhacungcap != null)
                {
                    TenNcc = nhacungcap.TenNCC;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhà cung cấp: {ex.Message}");
            }
        }

        private static bool IsInSmallDateTimeRange(DateTime dateTime)
        {
            var minSmallDateTime = new DateTime(1900, 1, 1);
            var maxSmallDateTime = new DateTime(2079, 6, 6, 23, 59, 0);
            return dateTime >= minSmallDateTime && dateTime <= maxSmallDateTime;
        }

        #endregion
    }
}
