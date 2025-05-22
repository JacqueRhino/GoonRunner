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
        public ICommand SanPhamViewCommand { get; set; }
        public ICommand PhieuNhapHangViewCommand { get; set; }
        public ICommand ChiTietPhieuNhapHangViewCommand { get; set; }
        public ICommand HoaDonViewCommand { get; set; }
        public ICommand ChiTietHoaDonViewCommand { get; set; }
        public ICommand TonKhoViewCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public OwnerHomeViewModel OwnerHomeVM { get; set; }
        public KhachHangViewModel KhachHangVM { get; set; }
        public NhanVienViewModel NhanVienVM { get; set; }
        public SanPhamViewModel SanPhamVM { get; set; }
        public PhieuNhapHangViewModel PhieuNhapHangVM { get; set; }
        public ChiTietPhieuNhapHangViewModel ChiTietPhieuNhapHangVM { get; set; }
        public HoaDonViewModel HoaDonVM { get; set; }
        public ChiTietHoaDonViewModel ChiTietHoaDonVM { get; set; }
        public TonKhoViewModel TonKhoVM { get; set; }
        public SidebarNhanVienViewModel SidebarNhanVienVM { get; set; }
        public SidebarKhachHangViewModel SidebarKhachHangVM { get; set; }
        public SidebarPhieuNhapHangViewModel SidebarPhieuNhapHangVM { get; set; }
        public SidebarChiTietPhieuNhapHangViewModel SidebarChiTietPhieuNhapHangVM { get; set; }
        public SidebarHoaDonViewModel SidebarHoaDonVM { get; set; }
        public SidebarChiTietHoaDonViewModel SidebarChiTietHoaDonVM { get; set; }
        
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
            OwnerHomeVM = new OwnerHomeViewModel();
            KhachHangVM = new KhachHangViewModel();
            NhanVienVM = new NhanVienViewModel();
            SanPhamVM = new SanPhamViewModel();
            PhieuNhapHangVM = new PhieuNhapHangViewModel();
            ChiTietPhieuNhapHangVM = new ChiTietPhieuNhapHangViewModel();
            HoaDonVM = new HoaDonViewModel();
            ChiTietHoaDonVM = new ChiTietHoaDonViewModel();
            TonKhoVM = new TonKhoViewModel();
            SidebarNhanVienVM = new SidebarNhanVienViewModel();
            SidebarKhachHangVM = new SidebarKhachHangViewModel();
            SidebarPhieuNhapHangVM = new SidebarPhieuNhapHangViewModel();
            SidebarChiTietPhieuNhapHangVM = new SidebarChiTietPhieuNhapHangViewModel();
            SidebarHoaDonVM = new SidebarHoaDonViewModel();
            SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel();
            if (Privilege == "Chủ cửa hàng")
                CurrentView = OwnerHomeVM;
            else 
                CurrentView = HomeVM;
            // DisableSidebar();

            //Change View

            HomeViewCommand = new RelayCommand<RadioButton>(o =>
            {
                if (Privilege == "Chủ cửa hàng")
                {
                    CurrentView = OwnerHomeVM;
                }
                else
                {
                    CurrentView = HomeVM;
                }
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

            SanPhamViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = SanPhamVM;
                DisableSidebar();
            });

            PhieuNhapHangViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = PhieuNhapHangVM;
                CurrentSidebarView = SidebarPhieuNhapHangVM;
                EnableSidebar();
            });

            ChiTietPhieuNhapHangViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = ChiTietPhieuNhapHangVM;
                CurrentSidebarView = SidebarChiTietPhieuNhapHangVM;
                EnableSidebar();
            });

            HoaDonViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = HoaDonVM;
                CurrentSidebarView = SidebarHoaDonVM;
                EnableSidebar();
            });

            ChiTietHoaDonViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = ChiTietHoaDonVM;
                CurrentSidebarView = SidebarChiTietHoaDonVM;
                EnableSidebar();
            });

            TonKhoViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = TonKhoVM;
                DisableSidebar();
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
