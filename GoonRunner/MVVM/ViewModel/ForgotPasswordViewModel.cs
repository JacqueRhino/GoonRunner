using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;

namespace GoonRunner.MVVM.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private bool _isloading;
        public bool IsLoading { get => _isloading; set { _isloading = value; OnPropertyChanged(); } }
        
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        
        private string _placeholder = "Nhập mật khẩu mới";
        public string Placeholder { get => _placeholder; set { _placeholder = value; OnPropertyChanged(); } }
        
        private string _errormessage;
        public string ErrorMessage { get => _errormessage; set { _errormessage = value; OnPropertyChanged(); } }
        
        public ICommand ReturnCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        
        public ForgotPasswordViewModel()
        {
            ReturnCommand = new RelayCommand<Window>(ReturntoLoginPage);
            
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                Placeholder = string.IsNullOrEmpty(p.Password) ? "Nhập mật khẩu mới" : "";
                Password = p.Password;
            });
            
            ChangePasswordCommand = new RelayCommand<Window>( p => true, async p => await ChangePasswordAsync(p));
            
        }
        
        private async Task ChangePasswordAsync(Window p)
        {
            if (p == null)
                return;
            
            if (string.IsNullOrEmpty(Username))
            {
                ErrorMessage = "Hãy nhập tên người dùng";
                return;
            }
            
            if (Username.Length < 3)
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
            
            try
            {
                string hashedPassword = MD5Hash(Password);
                IsLoading = true;
                var storyboard = p.Resources["SpinnerStoryboard"] as System.Windows.Media.Animation.Storyboard;
                storyboard?.Begin();
                
                using (var context = new GoonRunnerEntities())
                {
                    var usernameParam = new SqlParameter("@UserName", Username);
                    var passwordParam = new SqlParameter("@NewPassword", hashedPassword);
                    var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    
                    await context.Database.ExecuteSqlCommandAsync(
                        "EXEC sp_ChangePassword @UserName, @NewPassword, @Result OUT",
                        usernameParam,
                        passwordParam,
                        resultParam
                    );                    
                    var result = (int)resultParam.Value;

                    storyboard?.Stop();
                    IsLoading = false;
                    switch (result)
                    {
                        case 0:
                            MessageBox.Show("Đổi mật khẩu thành công!");
                            
                            Password = "";
                            Username = "";
                            
                            ReturntoLoginPage(p);
                            break;
                        case 1:
                            ErrorMessage = "Không tìm thấy tài khoản với tên đăng nhập này!";
                            break;
                        case 2:
                            ErrorMessage = "Có lỗi xảy ra khi đổi mật khẩu!";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
            }
        }

        private void ReturntoLoginPage(Window p)
        {
            LogInView loginView = new LogInView();
            loginView.Show();
            p.Hide();
            Placeholder = "Nhập mật khẩu mới";
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