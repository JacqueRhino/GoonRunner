﻿using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarPhieuNhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<PHIEUNHAPHANG> _danhsachphieunhaphang;
        public ObservableCollection<PHIEUNHAPHANG> DanhSachPhieuNhapHang { get => _danhsachphieunhaphang; set { _danhsachphieunhaphang = value; OnPropertyChanged(); } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; NgayNhap = value; OnPropertyChanged(); } } }
        private int _mancc;
        public int MaNCC { get => _mancc; set { _mancc = value; LoadNhaCungCapInfo(value); OnPropertyChanged(); } }
        private string _tenncc;
        public string TenNCC { get => _tenncc; set { _tenncc = value; OnPropertyChanged(); } }
        private int _manv;
        public int MaNV { get => _manv; set { _manv = value; OnPropertyChanged(); } }
        private int _currentuser;
        public int CurrentUser { get => _currentuser; set { _currentuser = value; OnPropertyChanged(); } }
        private DateTime _ngaynhap;
        public DateTime NgayNhap { get => _ngaynhap; set { _ngaynhap = value; OnPropertyChanged(); } }
        public ICommand AddPhieuNhapHangCommand { get; set; }
        public ICommand ClearFieldCommand { get; set; }

        public SidebarPhieuNhapHangViewModel()
        {
            SelectedDate = DateTime.Now;
            DanhSachPhieuNhapHang = new ObservableCollection<PHIEUNHAPHANG>(DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs);
            AddPhieuNhapHangCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaNCC == 0)
                {
                    MessageBox.Show("Hãy nhập Mã nhà cung cấp");
                    return;
                }

                if (string.IsNullOrEmpty(TenNCC))
                {
                    MessageBox.Show("Hãy nhập Tên nhà cung cấp");
                    return;
                }

                if (!IsInSmallDateTimeRange(NgayNhap))
                {
                    MessageBox.Show("Ngày nhập hàng không hợp lệ");
                    return;
                }

                if (MaNV == 0)
                {
                    MessageBox.Show("Hãy nhập Mã nhân viên");
                    return;
                }

                var phieunhaphang = new PHIEUNHAPHANG() { MaNCC = MaNCC, TenNCC = TenNCC, NgayNhap = NgayNhap, MaNV = MaNV };
                DataProvider.Ins.goonRunnerDB.PHIEUNHAPHANGs.Add(phieunhaphang);
                DataProvider.Ins.goonRunnerDB.SaveChanges();
                DanhSachPhieuNhapHang.Add(phieunhaphang);
                MessageBox.Show("Thêm thành công!");
                MainViewModel.Instance?.PhieuNhapHangVM?.LoadPhieuNhapHangList();
            });

            ClearFieldCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                ClearFields();
            });
        }
        private void ClearFields()
        {
            MaNCC = 0;
            TenNCC = string.Empty;
            SelectedDate = DateTime.Now;
        }
        private void LoadNhaCungCapInfo(int maNCC)
        {
            try
            {
                // Assuming you have a KHACHHANG entity in your database
                var nhacungcap = DataProvider.Ins.goonRunnerDB.NHACUNGCAPs.FirstOrDefault(ncc => ncc.MaNCC == maNCC);

                if (nhacungcap != null)
                {
                    // Auto-fill customer information
                    TenNCC = nhacungcap.TenNCC;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhà cung cấp: {ex.Message}");
            }
        }
        public void LoadCurrentUserAsEmployee()
        {
            try
            {
                CurrentUser = MainViewModel.Instance.CurrentUser;
                MaNV = CurrentUser;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Lỗi khi tải thông tin nhân viên hiện tại: {ex.Message}");
            }
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
