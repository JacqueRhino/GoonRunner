﻿using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GoonRunner.MVVM.Model;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using GoonRunner.MVVM.View;

namespace GoonRunner.MVVM.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        private string _errormessage;
        public string ErrorMassage { get { return _errormessage; } set { _errormessage = value; OnPropertyChanged(); } }

        private string _privilege;
        public string Privilege { get { return _privilege; } set { _privilege = value; OnPropertyChanged(); } }

        private string _displayname;
        public string DisplayName { get { return _displayname; } set { _displayname = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => true, (p) => Login(p));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            ForgotPasswordCommand = new RelayCommand<Window>((p) => true, (p) => 
            {
                var forgotPasswordView = new ForgotPasswordView();
                forgotPasswordView.Show();
                p.Hide();
            });
        }
        private void Login(Window p)
        {
            if (p == null)
                return;

            
            if (string.IsNullOrEmpty(UserName))
            {
                ErrorMassage = "Hãy nhập tên người dùng";
                return;
            }

            if (UserName.Length < 3)
            {
                ErrorMassage = "Tên người dùng phải dài ít nhất là 3 ký tự";
                return;
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                ErrorMassage = "Hãy nhập mật khẩu";
                return;
            }
            
            if (Password.Length < 3)
            {
                ErrorMassage = "Mật khẩu phải dài ít nhất là 3 ký tự";
                return;
            }

            // Kiểm tra tài khoản
            if (CheckAccount())
            {
                IsLogin = true;
                MainWindowView mainwindowview = new MainWindowView();
                var MainVM = mainwindowview.DataContext as MainViewModel;
                MainVM.DisplayName = DisplayName; // Gắn DisplayName qua bên MainWindow
                MainVM.Privilege = Privilege; // Gắn Privilege qua bên MainWindow

                // Xử lý ẩn hiện danh mục dựa vào quyền của user
                switch (Privilege)
                {
                    case "Nhân viên bán hàng":
                        mainwindowview.NhanVienRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhuyenMaiRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.BaoHanhRadioButton.Visibility = Visibility.Collapsed;
                        break;
                    case "Nhân viên kế toán":
                        mainwindowview.NhanVienRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhachHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.SanPhamRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhuyenMaiRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.BaoHanhRadioButton.Visibility = Visibility.Collapsed;
                        break;
                    case "Nhân viên chăm sóc khách hàng":
                        mainwindowview.NhanVienRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.DonHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhuyenMaiRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.BaoHanhRadioButton.Visibility = Visibility.Collapsed;
                        break;
                    case "Nhân viên kiểm kho":
                        mainwindowview.NhanVienRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhachHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.DonHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhuyenMaiRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.BaoHanhRadioButton.Visibility = Visibility.Collapsed;
                        break;
                    case "Nhân viên kỹ thuật":
                        mainwindowview.NhanVienRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhachHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.SanPhamRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.DonHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.KhuyenMaiRadioButton.Visibility = Visibility.Collapsed;
                        break;
                    case "Admin":
                        mainwindowview.KhachHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.SanPhamRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.DonHangRadioButton.Visibility = Visibility.Collapsed;
                        mainwindowview.BaoHanhRadioButton.Visibility = Visibility.Collapsed;
                        break;
                }
                mainwindowview.Show();
                p.Hide();
            }
            else
            {
                ErrorMassage = "Tên tài khoản hoặc mật khẩu không đúng.";
            }
        }

        private bool CheckAccount()
        {
            string EncodedPass = MD5Hash(Password);
            using (var context = new GoonRunnerEntities())
            {
                int accCount = context.ACCNHANVIENs.Count(record => record.UserName == UserName && record.Pass == EncodedPass); // Kiểm tra có tài khoản hay không
                DisplayName = context.ACCNHANVIENs.Where(a => a.UserName == UserName && a.Pass == EncodedPass)
                                                  .Select(a => a.DisplayName).FirstOrDefault(); // Lấy dữ liệu từ cột DisplayName bên CSDL qua
                Privilege = context.ACCNHANVIENs.Where(record => record.UserName == UserName && record.Pass == EncodedPass)
                                                .Select(record => record.Quyen).FirstOrDefault(); // Lấy dữ liệu từ cột Quyen bên CSDL qua
                if (accCount > 0)
                    return true;
                else
                    return false;
            }
        }
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
