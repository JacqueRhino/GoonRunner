using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;

namespace GoonRunner.MVVM.ViewModel
{
    class LoginViewModel : BaseViewModel
    {     
        public bool IsLogin { get; set; }
        private bool _isloading;
        public bool IsLoading {get => _isloading; set { _isloading = value; OnPropertyChanged();}}
        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string _placeholder = "Nhập mật khẩu";
        public string Placeholder { get => _placeholder; set { _placeholder = value; OnPropertyChanged(); } }

        private string _errormessage;
        public string ErrorMessage { get => _errormessage; set { _errormessage = value; OnPropertyChanged(); } }

        private string _privilege = "DEVELOPER";
        public string Privilege { get => _privilege; set { _privilege = value; OnPropertyChanged(); } }

        private string _displayname = "BYPASS FOR DEVELOPER";
        public string DisplayName { get => _displayname; set { _displayname = value; OnPropertyChanged(); } }
        private int _manv;
        public int MaNV { get => _manv; set { _manv = value; OnPropertyChanged(); } }
        private UserAccountDTO _useraccount;
        public UserAccountDTO UserAccount { get => _useraccount; set { _useraccount = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => true, async (p) => await LoginAsync(p));
            PasswordChangedCommand = new RelayCommand<System.Windows.Controls.PasswordBox>((p) => true, (p) =>
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

        private async Task LoginAsync(Window p)
        {
            if (p == null)
                return;
            
            ErrorMessage = "";
            
            if (string.IsNullOrEmpty(UserName))
            {
                ErrorMessage = "Hãy nhập tên người dùng";
                return;
            }

            if (UserName.Length < 3)
            {
                ErrorMessage = "Tên người dùng phải dài ít nhất là 3 ký tự";
                return;
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Hãy nhập mật khẩu";
                return;
            }
            
            if (Password.Length < 3)
            {
                ErrorMessage = "Mật khẩu phải dài ít nhất là 3 ký tự";
                return;
            }

            IsLoading = true;
            var storyboard = p.Resources["SpinnerStoryboard"] as System.Windows.Media.Animation.Storyboard;
            storyboard?.Begin();

            var isAccountValid = await Task.Run(CheckAccount);
            IsLoading = false;
            storyboard?.Stop();
            if (!isAccountValid)
            {
                ErrorMessage = "Tên tài khoản hoặc mật khẩu không đúng.";
                return;
            }
            
            IsLogin = true;
            Placeholder = "Nhập mật khẩu";
            MainWindowView mainwindow = new MainWindowView();
            SidebarHoaDonView sidebarHoaDonView = new SidebarHoaDonView();
            var SidebarHoaDonVM = sidebarHoaDonView.DataContext as SidebarHoaDonViewModel;
            
            //TODO: Thêm error handling
            if (mainwindow.DataContext is MainViewModel MainVM)
            {
                MainVM.DisplayName = DisplayName;
                MainVM.Privilege = Privilege;
                if (Privilege == "Chủ cửa hàng")
                {
                    MainVM.CurrentView = MainVM.OwnerHomeVM;
                }
                else
                {
                    MainVM.CurrentView = MainVM.HomeVM;
                }
            }
            MainViewModel.Instance.CurrentUser = MaNV;
            SetVisibilityByPrivilege(mainwindow);
            
            mainwindow.Show();
            p.Hide();
        }

        private void SetVisibilityByPrivilege(MainWindowView mainwindow)
        {
            var hiddenControlsByRole = new Dictionary<string, List<UIElement>>
            {
                ["Nhân viên bán hàng"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên kế toán"] = new List<UIElement>
                {
                    mainwindow.KhachHangRadioButton,
                    mainwindow.SanPhamRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton,
                    mainwindow.TonKhoRadioButton
                },
                ["Nhân viên chăm sóc khách hàng"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton,
                    mainwindow.TonKhoRadioButton
                },
                ["Nhân viên kiểm kho"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.KhachHangRadioButton,
                    mainwindow.HoaDonRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.KhuyenMaiRadioButton,
                    mainwindow.BaoHanhRadioButton
                },
                ["Nhân viên kỹ thuật"] = new List<UIElement>
                {
                    mainwindow.NhanVienRadioButton,
                    mainwindow.HoaDonRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    mainwindow.TonKhoRadioButton
                },
                ["Admin"] = new List<UIElement>
                {
                    mainwindow.KhachHangRadioButton,
                    mainwindow.HoaDonRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.BaoHanhRadioButton,
                    mainwindow.TonKhoRadioButton
                },
                ["Chủ cửa hàng"] = new List<UIElement>
                {
                    mainwindow.KhachHangRadioButton,
                    mainwindow.TonKhoRadioButton,
                    mainwindow.HoaDonRadioButton,
                    //mainwindow.ChiTietHoaDonRadioButton,
                    mainwindow.PhieuNhapHangRadioButton,
                    //mainwindow.ChiTietPhieuNhapHangRadioButton,
                    mainwindow.BaoHanhRadioButton,
                    mainwindow.TonKhoRadioButton
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
            string encodedPass = MD5Hash(Password);
            if (!System.Data.Entity.Database.Exists("GoonRunnerEntities"))
            {
                ErrorMessage = "Cấu hình cơ sở dữ liệu không chính xác";
                return false;
            }

            using (var context = new GoonRunnerEntities())
            {
                if (!context.Database.Exists())
                {
                    ErrorMessage = "Không thể kết nối cơ sở dữ liệu";
                    return false;
                }
                
                try
                {
                    var UserAccount = context.Database.SqlQuery<UserAccountDTO>(
                        "EXEC kiem_tra_login @UserName, @PasswordHash",
                        new SqlParameter("@UserName", UserName),
                        new SqlParameter("@PasswordHash", encodedPass)
                    ).FirstOrDefault();
                    
                    if (UserAccount == null)
                        return false;

                    DisplayName = UserAccount.DisplayName;
                    Privilege = UserAccount.Quyen;
                    MaNV = UserAccount.MaNV;
                    return true;
                }
                catch (SqlException exception)
                {
                    if (exception.Number == 2812)
                    {
                        ErrorMessage = "Stored procedure 'kiem_tra_login' không tồn tại trong cơ sở dữ liệu.";
                    }
                    else
                    {
                        ErrorMessage = $"Lỗi truy vấn cơ sở dữ liệu: {exception.Message}";
                    }
                    return false;
                }
                catch (System.Exception ex)
                {
                    ErrorMessage = $"Lỗi không xác định: {ex.Message}";
                    return false;
                }

            }
        }
        
        public class UserAccountDTO
        {
            public string DisplayName { get; set; }
            public string Quyen { get; set; }
            public int MaNV { get; set; }
        }
        
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5CryptoServiceProvider md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
