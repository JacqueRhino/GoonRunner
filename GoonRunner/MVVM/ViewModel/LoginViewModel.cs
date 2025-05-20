using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string _placeholder = "Nhập mật khẩu";
        public string Placeholder { get => _placeholder; set { _placeholder = value; OnPropertyChanged(); } }

        private string _errormessage;
        public string ErrorMassage { get => _errormessage; set { _errormessage = value; OnPropertyChanged(); } }

        private string _privilege = "DEVELOPER";
        public string Privilege { get => _privilege; set { _privilege = value; OnPropertyChanged(); } }

        private string _displayname = "BYPASS FOR DEVELOPER";
        public string DisplayName { get => _displayname; set { _displayname = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => true, Login);
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                if (string.IsNullOrEmpty(p.Password))
                {
                    Placeholder = "Nhập mật khẩu";
                }
                else
                    Placeholder = "";
                Password = p.Password;
            });

            ForgotPasswordCommand = new RelayCommand<Window>((p) => true, (p) => 
            {
                var forgotPasswordView = new ForgotPasswordView();
                forgotPasswordView.Show();
                p.Hide();
            });
        }

        private void LoginBYPASS(Window p)
        {
            IsLogin = true;
            p.Hide();
            MainWindowView mainwindow = new MainWindowView();
            mainwindow.Show();
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
                Placeholder = "Nhập mật khẩu";
                MainWindowView mainwindow = new MainWindowView();
                var MainVM = mainwindow.DataContext as MainViewModel;
                MainVM.DisplayName = DisplayName; // Gắn DisplayName qua bên MainWindow
                MainVM.Privilege = Privilege; // Gắn Privilege qua bên MainWindow

                // Xử lý ẩn hiện danh mục dựa vào quyền của user
                SetVisibilityByPrivilege(mainwindow);
                
                mainwindow.Show();
                p.Hide();
            }
            else
            {
                ErrorMassage = "Tên tài khoản hoặc mật khẩu không đúng.";
            }
        }
        
        private void SetVisibilityByPrivilege(MainWindowView mainwindow)
        {
            var hiddenControlsByRole = new Dictionary<string, List<UIElement>>
            {
                ["Nhân viên bán hàng"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên kế toán"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhachHangRadioButton,
                    mainwindow.SanPhamRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên chăm sóc khách hàng"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.DonHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên kiểm kho"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhachHangRadioButton,
                    mainwindow.DonHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên kỹ thuật"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhachHangRadioButton,
                    mainwindow.SanPhamRadioButton,
                    mainwindow.DonHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton
                },
                ["Admin"] = new List<UIElement>
                {
                    mainwindow.KhachHangRadioButton,
                    mainwindow.SanPhamRadioButton,
                    mainwindow.DonHangRadioButton,
                    mainwindow.BaoHanhRadioButton
                }
            };
        
            if (hiddenControlsByRole.TryGetValue(Privilege, out var controlsToHide))
            {
                foreach (var control in controlsToHide)
                {
                    control.Visibility = Visibility.Collapsed;
                }
            }
        }


        private bool CheckAccount()
        {
            string EncodedPass = MD5Hash(Password);
            if (!System.Data.Entity.Database.Exists("GoonRunnerEntities"))
            {
                ErrorMassage = "Cấu hình cơ sở dữ liệu không chính xác";
                return false;
            }

            using (var context = new GoonRunnerEntities())
            {
                if (!context.Database.Exists())
                {
                    ErrorMassage = "Không thể kết nối cơ sở dữ liệu";
                    return false;
                }

                
                // T xài stored procedure bên SQL Server cho hiểu xuất cao hơn
                // procedure đây:
                // CREATE PROCEDURE kiem_tra_login
                //     @UserName NVARCHAR(50),
                // @PasswordHash NVARCHAR(100)
                // AS
                //     BEGIN
                // SET NOCOUNT ON;
                //
                // SELECT 
                //     DisplayName,
                //     Quyen
                // FROM 
                //     ACCNHANVIEN
                // WHERE 
                //     UserName COLLATE Latin1_General_CS_AS = @UserName
                // AND Pass = @PasswordHash;
                // END                
                    
                // nếu thấy bất tiện thì t cũng có optimize cái cũ:
                // var userAccount = context.ACCNHANVIENs
                //     .Where(record => record.UserName == UserName && record.Pass == EncodedPass)
                //     .Select(record => new
                //     {
                //         record.DisplayName,
                //         record.Quyen
                //     })
                //     .FirstOrDefault();

                try
                {
                    var userAccount = context.Database.SqlQuery<UserAccountDTO>(
                        "EXEC kiem_tra_login @UserName, @PasswordHash",
                        new SqlParameter("@UserName", UserName),
                        new SqlParameter("@PasswordHash", EncodedPass)
                    ).FirstOrDefault();
                    
                    if (userAccount == null)
                        return false;

                    DisplayName = userAccount.DisplayName;
                    Privilege = userAccount.Quyen;
                    return true;
                }
                catch (SqlException exception)
                {
                    if (exception.Number == 2812)
                    {
                        ErrorMassage = "Stored procedure 'kiem_tra_login' không tồn tại trong cơ sở dữ liệu.";
                    }
                    else
                    {
                        ErrorMassage = $"Lỗi truy vấn cơ sở dữ liệu: {exception.Message}";
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    ErrorMassage = $"Lỗi không xác định: {ex.Message}";
                    return false;
                }

            }
        }
        
        public class UserAccountDTO
        {
            public string DisplayName { get; set; }
            public string Quyen { get; set; }
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
