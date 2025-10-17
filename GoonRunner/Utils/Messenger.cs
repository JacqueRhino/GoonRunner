using GoonRunner.MVVM.Model;
using System;

namespace GoonRunner.Utils
{
    /// <summary>
    /// Provides a simple messaging system for notifying view models or views
    /// about changes or selections of entities.
    /// </summary>
    public static class Messenger
    {
        #region KHACHHANG
        public static event Action<KHACHHANG> KhachHangChanged;
        public static void NotifyKhachHangChanged(KHACHHANG kh) => KhachHangChanged?.Invoke(kh);

        public static event Action<int> KhachHangSelected;
        public static void NotifyKhachHangSelected(int maKh) => KhachHangSelected?.Invoke(maKh);
        #endregion

        #region NHANVIEN
        public static event Action<NHANVIEN> NhanVienChanged;
        public static void NotifyNhanVienChanged(NHANVIEN nhanvien) => NhanVienChanged?.Invoke(nhanvien);

        public static event Action<int> NhanVienSelected;
        public static void NotifyNhanVienSelected(int maNv) => NhanVienSelected?.Invoke(maNv);
        #endregion

        #region ACCNHANVIEN
        public static event Action<ACCNHANVIEN> AccNhanVienChanged;
        public static void NotifyAccNhanVienChanged(ACCNHANVIEN accnhanvien) => AccNhanVienChanged?.Invoke(accnhanvien);

        public static event Action<int> AccNhanVienSelected;
        public static void NotifyAccNhanVienSelected(int maNv) => AccNhanVienSelected?.Invoke(maNv);
        #endregion

        #region HOADON
        public static event Action<HOADON> HoaDonChanged;
        public static void NotifyHoaDonChanged(HOADON hd) => HoaDonChanged?.Invoke(hd);

        public static event Action<int> HoaDonSelected;
        public static void NotifyHoaDonSelectedChanged(int maHd) => HoaDonSelected?.Invoke(maHd);

        public static event Action<CHITIETHOADON> ChiTietHoaDonChanged;
        public static void NotifyChiTietHoaDonChanged(CHITIETHOADON cthd) => ChiTietHoaDonChanged?.Invoke(cthd);
        #endregion

        #region PHIEUNHAPHANG
        public static event Action<PHIEUNHAPHANG> PhieuNhapHangChanged;
        public static void NotifyPhieuNhapHangChanged(PHIEUNHAPHANG pnh) => PhieuNhapHangChanged?.Invoke(pnh);

        public static event Action<int> PhieuNhapHangSelected;
        public static void NotifyPhieuNhapHangSelected(int maPnh) => PhieuNhapHangSelected?.Invoke(maPnh);

        public static event Action<CHITIETPHIEUNHAPHANG> ChiTietPhieuNhapHangChanged;
        public static void NotifyChiTietPhieuNhapHangChanged(CHITIETPHIEUNHAPHANG ctpnh)
            => ChiTietPhieuNhapHangChanged?.Invoke(ctpnh);

        public static event Action<int> ChiTietPhieuNhapHangSelected;
        public static void NotifyChiTietPhieuNhapHangSelected(int maCtpnh)
            => ChiTietPhieuNhapHangSelected?.Invoke(maCtpnh);
        #endregion
    }
}
