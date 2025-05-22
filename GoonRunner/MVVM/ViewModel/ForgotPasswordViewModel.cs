using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoonRunner.MVVM.View;

namespace GoonRunner.MVVM.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string _placeholder = "Nhập mật khẩu mới";
        public string Placeholder { get => _placeholder; set { _placeholder = value; OnPropertyChanged(); } }
        public ICommand ReturnCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            ReturnCommand = new RelayCommand<Window>(p =>
            {
                LogInView loginView = new LogInView();
                loginView.Show();
                p.Hide();
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                Placeholder = string.IsNullOrEmpty(p.Password) ? "Nhập mật khẩu mới" : "";
                Password = p.Password;
            });
        }
    }
}