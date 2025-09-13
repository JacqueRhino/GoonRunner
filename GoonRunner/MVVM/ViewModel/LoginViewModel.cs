using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;
using GoonRunner.Utils;

// ReSharper disable InconsistentNaming

namespace GoonRunner.MVVM.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {     
        private const string DefaultPasswordPlaceholder = "Nhập mật khẩu";
        private bool _isloading;
        public bool IsLoading {get => _isloading; set { _isloading = value; OnPropertyChanged();}}
        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        private string _placeholder = "Nhập mật khẩu";
        public string Placeholder { get => _placeholder;
            private set { _placeholder = value; OnPropertyChanged(); } }

        private string _errormessage;
        public string ErrorMessage { get => _errormessage;
            private set { _errormessage = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { _ = LoginAsync(p);});           
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                Placeholder = string.IsNullOrEmpty(p.Password) ? DefaultPasswordPlaceholder : "";
            });

            ForgotPasswordCommand = new RelayCommand<Window>((p) => true, (p) => 
            {
                var forgotPasswordView = new ForgotPasswordView();
                forgotPasswordView.Show();
                Placeholder = DefaultPasswordPlaceholder;
                p.Hide();
            });
        }


        private class UserAccountDTO
        {
            //Disable nó tại vì setter có sử dụng qua reflection
            //nhưng resharper không đọc được
            //Resharper disable UnusedAutoPropertyAccessor.Local
            public string DisplayName { get; set; }
            public string Quyen { get; set; }
            public int MaNV { get; set; }
            //Resharper restore UnusedAutoPropertyAccessor.Local
        }

        public void ResetLogin()
        {
            UserName = "";
            ErrorMessage = "";
        }

        private async Task LoginAsync(Window p)
        {
            try
            {

                if (p == null)
                    return;
            
                ErrorMessage = "";

                if (!(p.FindName("Password") is PasswordBox passwordBox))
                {
                    Console.WriteLine(@"Không tìm thấy passwordbox");
                    return;
                }
                var password = passwordBox.Password;

                var userValidation = ValidateLoginInput(UserName, password);
                if (!userValidation.IsSuccess)
                {
                    ErrorMessage = userValidation.Error;
                    return;
                }

                IsLoading = true;
                var storyboard = p.Resources["SpinnerStoryboard"] as System.Windows.Media.Animation.Storyboard;
                try
                {
                    storyboard?.Begin();

                    var userAccount = await Task.Run(() => CheckAccount(UserName, password));

                    if (!userAccount.IsSuccess)
                    {
                        ErrorMessage = userAccount.Error;
                        return;
                    }

                    if (!EmployeeRoles.RoleNames.StringToRole.TryGetValue(userAccount.Value.Quyen, out var employeeRole))
                    {
                        ErrorMessage = "Không phải quyền hợp lệ";
                        return;
                    }

                    EnterMainView(p, userAccount.Value.MaNV, userAccount.Value.Quyen, userAccount.Value.DisplayName, employeeRole);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                finally
                {
                    IsLoading = false;
                    storyboard?.Stop();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private static Result<bool, string> ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return Result<bool, string>.Failure("Hãy nhập tên người dùng");
            

            if (username.Length < 3)
                return Result<bool, string>.Failure("Tên người dùng phải dài ít nhất là 3 ký tự");

            if (string.IsNullOrEmpty(password))
                return Result<bool, string>.Failure("Hãy nhập mật khẩu");

            if (password.Length < 3)
                return Result<bool, string>.Failure("Mật khẩu phải dài ít nhất là 3 ký tự");

            return Result<bool, string>.Success(true);
        } 
        
        private void EnterMainView(Window p,int MaNV, string RoleInString, string DisplayName, EmployeeRoles.Roles Role)
        {
            Placeholder = DefaultPasswordPlaceholder;
            var mainwindow = new MainWindowView();
            var mainVM = new MainViewModel();
            mainwindow.DataContext = mainVM;
            mainVM.SetCurrentSession(MaNV, RoleInString,Role,DisplayName);
            mainVM.SetAuthorization(Role);
            mainwindow.Show();
            p.Hide();
        }

        private static Result<UserAccountDTO,string> CheckAccount(string username, string password)
        {
            var encodedPass = MD5Hash(password);

            using (var context = new GoonRunnerEntities())
            {
                if (!context.Database.Exists())
                {
                    return Result<UserAccountDTO, string>.Failure("Không thể kết nối cơ sở dữ liệu");
                }

                try
                {
                    var userAccount = context.Database.SqlQuery<UserAccountDTO>(
                        "EXEC kiem_tra_login @UserName, @PasswordHash",
                        new SqlParameter("@UserName", username),
                        new SqlParameter("@PasswordHash", encodedPass)
                    ).FirstOrDefault();

                    if (userAccount == null)
                        return Result<UserAccountDTO, string>.Failure("Tên tài khoản hoặc mật khẩu không đúng.");

                    return Result<UserAccountDTO, string>.Success(userAccount);
                }
                catch (SqlException ex)
                {
                    var error = ex.Number == 2812
                        ? "Stored procedure 'kiem_tra_login' không tồn tại trong cơ sở dữ liệu."
                        : $"Lỗi truy vấn cơ sở dữ liệu: {ex.Message}";
                    return Result<UserAccountDTO, string>.Failure(error);
                }
                catch (Exception ex)
                {
                    return Result<UserAccountDTO, string>.Failure(ex.Message);
                }
            }
        }
        
        private static string MD5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
