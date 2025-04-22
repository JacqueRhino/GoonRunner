using GoonRunner.MVVM.View;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace GoonRunner.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand HomeViewCommand { get; set; }
        public ICommand NhanVienViewCommand { get; set; }
        public ICommand KhachHangViewCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public KhachHangViewModel KhachHangVM { get; set; }
        public NhanVienViewModel NhanVienVM { get; set; }
        public SidebarNhanVienViewModel SidebarNhanVienVM { get; set; }
        public SidebarKhachHangViewModel SidebarKhachHangVM { get; set; }
        private object _currentView;
        private string _displayname;
        public string DisplayName { get => _displayname; set { _displayname = value; OnPropertyChanged(); } }
        private string _privilege = "DEVELOPER";
        public string Privilege { get => _privilege; set { _privilege = value; OnPropertyChanged(); } }
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }
        
        private object _currentSidebarView;

        public object CurrentSidebarView
        {
            get { return _currentSidebarView; }
            set
            {
                _currentSidebarView = value;
                OnPropertyChanged("CurrentSidebarView");
            }
        }
        
        private bool _sidebarButtonEnabled;

        public bool SidebarButtonEnabled
        {
            get { return _sidebarButtonEnabled; }
            set
            {
                _sidebarButtonEnabled = value;
                OnPropertyChanged("SidebarButtonEnabled");
            }
        }

        private int _sidebarLeftGapWidth;

        public int SidebarLeftGapWidth
        {
            get { return _sidebarLeftGapWidth; }
            set
            {
                _sidebarLeftGapWidth = value;
                OnPropertyChanged("SidebarLeftGapWidth");
            }
        }

        private int _sidebarWidth;

        public int SidebarWidth
        {
            get { return _sidebarWidth; }
            set
            {
                _sidebarWidth = value;
                OnPropertyChanged("SidebarWidth");
            }
        }

        private string _split2Enabled;

        public string Split2Enabled
        {
            get { return _split2Enabled; }
            set
            {
                _split2Enabled = value;
                OnPropertyChanged("Split2Enabled");
            }
        }

        public MainViewModel()
        {
            LogInView loginWindow = new LogInView();
            var loginVM = loginWindow.DataContext as LoginViewModel; // Gọi LoginViewModel
            DisplayName = loginVM.DisplayName; // Lấy UserName
            Privilege = loginVM.Privilege; // Lấy Privilege
            HomeVM = new HomeViewModel();
            KhachHangVM = new KhachHangViewModel();
            NhanVienVM = new NhanVienViewModel();
            SidebarNhanVienVM = new SidebarNhanVienViewModel();
            SidebarKhachHangVM = new SidebarKhachHangViewModel();
            CurrentView = HomeVM;
            // DisableSidebar();
            
            //Change View

            HomeViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = HomeVM;
                DisableSidebar();
            });
            
            NhanVienViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = NhanVienVM;
                CurrentSidebarView = SidebarNhanVienVM;
                EnableSidebar();
            });
            
            KhachHangViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = KhachHangVM;
                CurrentSidebarView = SidebarKhachHangVM;
                EnableSidebar();
            });

            SignOutCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                MessageBoxResult MessageResult = MessageBox.Show("Bạn có muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo);
                if (MessageResult == MessageBoxResult.Yes)
                    SignOut(p);
                else
                    return;
            });
        }
        private void SignOut(Window p)
        {
            LogInView loginWindow = new LogInView();
            var loginVM = loginWindow.DataContext as LoginViewModel; // Gọi LoginViewModel
            loginVM.UserName = "";
            loginVM.Password = ""; // Khi thực hiện đăng xuất sẽ reset lại ô username và password
            loginVM.ErrorMassage = "";
            CurrentView = HomeVM;
            loginWindow.Show();
            p.Hide();
        }
        private void EnableSidebar()
        {
            SidebarButtonEnabled = true;
        }
        private void DisableSidebar()
        {
            SidebarButtonEnabled = false;
        }
    }
}
