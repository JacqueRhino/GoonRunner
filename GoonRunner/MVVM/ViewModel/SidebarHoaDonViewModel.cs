using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarHoaDonViewModel : BaseViewModel
    {
        private UserSession CurrentSession { get; }

        private ObservableCollection<HOADON> _danhsachhoadon;
        public ObservableCollection<HOADON> DanhSachHoaDon
        {
            get => _danhsachhoadon;
            set { _danhsachhoadon = value; OnPropertyChanged(); }
        }

        private DateTime _selecteddate;
        public DateTime SelectedDate
        {
            get => _selecteddate;
            set
            {
                if (_selecteddate != value)
                {
                    _selecteddate = value;
                    NgayMuaHang = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _mahd;
        public int MaHd { get => _mahd; set { _mahd = value; OnPropertyChanged(); } }

        private int _makh;
        public int MaKh
        {
            get => _makh;
            set { _makh = value; OnPropertyChanged(); LoadKhachHangInfo(value); }
        }

        private string _hokh;
        public string HoKh { get => _hokh; set { _hokh = value; OnPropertyChanged(); } }

        private string _tenkh;
        public string TenKh { get => _tenkh; set { _tenkh = value; OnPropertyChanged(); } }

        private string _sdtkh;
        public string Sdtkh { get => _sdtkh; set { _sdtkh = value; OnPropertyChanged(); } }

        private string _diachi;
        public string DiaChi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }

        private int _manv;
        public int MaNv { get => _manv; set { _manv = value; OnPropertyChanged(); LoadNhanVienInfo(value); } }

        private string _hotennv;
        public string HoTenNv { get => _hotennv; set { _hotennv = value; OnPropertyChanged(); } }

        private string _honv;
        private string HoNv { get => _honv; set { _honv = value; OnPropertyChanged(); } }

        private string _tennv;
        private string TenNv { get => _tennv; set { _tennv = value; OnPropertyChanged(); } }

        private DateTime _ngaymuahang;
        public DateTime NgayMuaHang { get => _ngaymuahang; set { _ngaymuahang = value; OnPropertyChanged(); } }

        public ICommand AddHoaDonCommand { get; }
        public ICommand ClearFieldCommand { get; }

        public SidebarHoaDonViewModel(UserSession userSession, Action refreshHoaDonList = null)
        {
            CurrentSession = userSession ?? throw new ArgumentNullException(nameof(userSession));

            SelectedDate = DateTime.Now;
            DanhSachHoaDon = new ObservableCollection<HOADON>(DataProvider.Ins.goonRunnerDB.HOADONs);

            MaNv = CurrentSession.UserId;
            Messenger.HoaDonSelected += LoadHoaDonInfo;
            LoadNhanVienInfo(CurrentSession.UserId);
            AddHoaDonCommand = new RelayCommand<Button>(_ => true, _ =>
            {
                if (MaKh == 0 || string.IsNullOrEmpty(HoKh) || string.IsNullOrEmpty(TenKh) ||
                    string.IsNullOrEmpty(Sdtkh) || string.IsNullOrEmpty(DiaChi))
                {
                    MessageBox.Show("Hãy nhập đầy đủ thông tin khách hàng");
                    return;
                }

                if (!IsInSmallDateTimeRange(NgayMuaHang))
                {
                    MessageBox.Show("Ngày mua hàng không hợp lệ");
                    return;
                }

                var hoadon = new HOADON()
                {
                    MaKH = MaKh,
                    HoKH = HoKh,
                    TenKH = TenKh,
                    SdtKH = Sdtkh,
                    DiaChi = DiaChi,
                    MaNV = MaNv,
                    HoNV = HoNv,
                    TenNV = TenNv,
                    NgayMuaHang = NgayMuaHang
                };

                using (var transaction = DataProvider.Ins.goonRunnerDB.Database.BeginTransaction())
                {
                    try
                    {
                        DataProvider.Ins.goonRunnerDB.HOADONs.Add(hoadon);
                        DataProvider.Ins.goonRunnerDB.SaveChanges();

                        DanhSachHoaDon.Add(hoadon);

                        transaction.Commit();

                        MessageBox.Show("Thêm thành công!");
                        refreshHoaDonList?.Invoke(); 
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}");
                    }
                }
            });
            ClearFieldCommand = new RelayCommand<Button>(_ => true, _ => ClearFields());
        }

        public void LoadHoaDonInfo(int maHd)
        {
            try
            {
                var hoadon = DataProvider.Ins.goonRunnerDB.HOADONs.FirstOrDefault(hd => hd.MaHD == maHd);

                if (hoadon != null)
                {
                    MaHd = hoadon.MaHD;
                    MaKh = hoadon.MaKH;
                    HoKh =  hoadon.HoKH;
                    Sdtkh = hoadon.SdtKH;
                    HoTenNv = hoadon.HoNV + " " + hoadon.TenNV;
                    Debug.Assert(hoadon.NgayMuaHang != null, "hoadon.NgayMuaHang != null");
                    NgayMuaHang = (DateTime)hoadon.NgayMuaHang;
                    DiaChi = hoadon.DiaChi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhân viên: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            MaKh = 0;
            HoKh = string.Empty;
            TenKh = string.Empty;
            Sdtkh = string.Empty;
            DiaChi = string.Empty;
            SelectedDate = DateTime.Now;
        }

        private void LoadKhachHangInfo(int maKh)
        {
            try
            {
                var khachHang = DataProvider.Ins.goonRunnerDB.KHACHHANGs.FirstOrDefault(kh => kh.MaKH == maKh);
                if (khachHang != null)
                {
                    HoKh = khachHang.HoKH;
                    TenKh = khachHang.TenKH;
                    Sdtkh = khachHang.SdtKH;
                    DiaChi = khachHang.DiaChi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin khách hàng: {ex.Message}");
            }
        }

        private void LoadNhanVienInfo(int maNv)
        {
            try
            {
                var nhanVien = DataProvider.Ins.goonRunnerDB.NHANVIENs.FirstOrDefault(nv => nv.MaNV == maNv);
                if (nhanVien != null)
                {
                    HoNv = nhanVien.HoNV;
                    TenNv = nhanVien.TenNV;
                    HoTenNv = nhanVien.HoNV + " " + nhanVien.TenNV;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhân viên: {ex.Message}");
            }
        }

        private bool IsInSmallDateTimeRange(DateTime dateTime)
        {
            var minSmallDateTime = new DateTime(1900, 1, 1);
            var maxSmallDateTime = new DateTime(2079, 6, 6, 23, 59, 0);
            return dateTime >= minSmallDateTime && dateTime <= maxSmallDateTime;
        }
    }
}
