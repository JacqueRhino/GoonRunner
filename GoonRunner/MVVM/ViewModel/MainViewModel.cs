using GoonRunner.MVVM.View;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
// ReSharper disable InconsistentNaming

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
        private int _currentuser;
        public int CurrentUser { get => _currentuser; set { _currentuser = value; OnPropertyChanged(); } }
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
        private object _currentSidebarView;

        public object CurrentSidebarView
        {
            get { return _currentSidebarView; }
            set
            {
                _currentSidebarView = value;
                OnPropertyChanged();
            }
        }
        
        private bool _sidebarButtonEnabled;

        public bool SidebarButtonEnabled
        {
            get { return _sidebarButtonEnabled; }
            set
            {
                _sidebarButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _sidebarLeftGapWidth;

        public int SidebarLeftGapWidth
        {
            get { return _sidebarLeftGapWidth; }
            set
            {
                _sidebarLeftGapWidth = value;
                OnPropertyChanged();
            }
        }

        private int _sidebarWidth;

        public int SidebarWidth
        {
            get { return _sidebarWidth; }
            set
            {
                _sidebarWidth = value;
                OnPropertyChanged();
            }
        }

        private string _split2Enabled;

        public string Split2Enabled
        {
            get { return _split2Enabled; }
            set
            {
                _split2Enabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsHomeVisible { get => _ishomevisible; set { _ishomevisible = value; OnPropertyChanged(); } }
        public bool IsNhanvienVisible {get => _isnhanvienvisible; set { _isnhanvienvisible = value; OnPropertyChanged();}}
        public bool IsSanPhamVisible {get => _issanphamvisible; set { _issanphamvisible = value; OnPropertyChanged();}}
        public bool IsHoaDonVisible {get => _ishoadonvisible; set { _ishoadonvisible = value; OnPropertyChanged();}}
        public bool IsPhieuNhapVisible {get => _isphieunhapvisible; set { _isphieunhapvisible = value; OnPropertyChanged();}}
        public bool IsTonKhoVisible {get => _istonkhovisible; set { _istonkhovisible = value; OnPropertyChanged();}}
        public bool IsKhachHangVisible {get => _iskhachhangvisible; set { _iskhachhangvisible = value; OnPropertyChanged();}}
        public bool IsKhuyenMaiVisible {get => _iskhuyenmaivisible; set { _iskhuyenmaivisible = value; OnPropertyChanged();}}
        public bool IsBaoHanhVisible {get => _isbaohanhvisible; set { _isbaohanhvisible = value; OnPropertyChanged();}}
        private bool _ishomevisible;
        private bool _isnhanvienvisible;
        private bool _issanphamvisible;
        private bool _ishoadonvisible;
        private bool _isphieunhapvisible;
        private bool _istonkhovisible;
        private bool _iskhachhangvisible;
        private bool _iskhuyenmaivisible;
        private bool _isbaohanhvisible;

        public static MainViewModel Instance { get; private set; }
        
        public MainViewModel()
        {
            Instance = this;
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
                SidebarPhieuNhapHangVM?.LoadCurrentUserAsEmployee();
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
                SidebarHoaDonVM?.LoadCurrentUserAsEmployee();
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
                var messageResult = MessageBox.Show("Bạn có muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo);
                if (messageResult == MessageBoxResult.Yes)
                    SignOut(p);
            });
        }
        
        public void SetUpCurrentUser(int maNV,string privilige, string displayname)
        {
            CurrentUser = maNV;
            Privilege = privilige;
            DisplayName = displayname;
        }
        
        public void SetAuthorization(Employee.EmployeeRole Role)
        {
            switch (Role)
            {
                //TODO:ADD KHUYENMAI VIEWMODEL
                case Employee.EmployeeRole.Admin:
                    IsHomeVisible = true;
                    IsNhanvienVisible = true;
                    IsKhachHangVisible = true;
                    IsKhuyenMaiVisible = true;
                    
                    HomeVM = new HomeViewModel();
                    NhanVienVM = new NhanVienViewModel();
                    KhachHangVM = new KhachHangViewModel();
                    
                    SidebarNhanVienVM = new SidebarNhanVienViewModel();
                    SidebarKhachHangVM = new SidebarKhachHangViewModel();
                    
                    CurrentView = HomeVM;
                    break;

                case Employee.EmployeeRole.NhanVienBanHang:
                    IsHomeVisible = true;
                    IsHoaDonVisible = true;
                    IsKhachHangVisible = true;
                    IsKhuyenMaiVisible = true;
                    IsSanPhamVisible = true;
                    IsTonKhoVisible = true;
                    
                    HomeVM = new HomeViewModel();
                    HoaDonVM = new HoaDonViewModel();
                    ChiTietHoaDonVM = new ChiTietHoaDonViewModel();
                    KhachHangVM = new KhachHangViewModel();
                    SanPhamVM = new SanPhamViewModel();
                    TonKhoVM = new TonKhoViewModel();
                    
                    SidebarNhanVienVM = new SidebarNhanVienViewModel();
                    SidebarKhachHangVM = new SidebarKhachHangViewModel();
                    SidebarHoaDonVM = new SidebarHoaDonViewModel();
                    SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel();

                    CurrentView = HomeVM;
                    break;
                
                case Employee.EmployeeRole.NhanVienKeToan:
                    IsHomeVisible = true;
                    IsNhanvienVisible = true;
                    IsHoaDonVisible = true;
                    
                    NhanVienVM = new NhanVienViewModel();
                    HoaDonVM = new HoaDonViewModel();
                    ChiTietHoaDonVM = new ChiTietHoaDonViewModel();
                    
                    SidebarNhanVienVM = new SidebarNhanVienViewModel();
                    SidebarHoaDonVM = new SidebarHoaDonViewModel();
                    SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel();
 
                    CurrentView = HomeVM;
                    break;
                
                case Employee.EmployeeRole.NhanVienChamSocKhachHang:
                    IsHomeVisible = true;
                    IsKhachHangVisible = true;
                    IsHoaDonVisible = true;
                    
                    HomeVM = new HomeViewModel();
                    KhachHangVM = new KhachHangViewModel();
                    HoaDonVM = new HoaDonViewModel();
                    
                    ChiTietHoaDonVM = new ChiTietHoaDonViewModel();
                    SidebarKhachHangVM = new SidebarKhachHangViewModel();
                    SidebarHoaDonVM = new SidebarHoaDonViewModel();
                    SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel();

                    CurrentView = HomeVM;
                    break;
                
                case Employee.EmployeeRole.NhanVienKiemKho:
                    IsHomeVisible = true;
                    IsTonKhoVisible = true;
                    IsSanPhamVisible = true;
                    IsPhieuNhapVisible = true;
                    
                    HomeVM = new HomeViewModel();
                    TonKhoVM = new TonKhoViewModel();
                    SanPhamVM = new SanPhamViewModel();
                    PhieuNhapHangVM = new PhieuNhapHangViewModel();
                    ChiTietPhieuNhapHangVM = new ChiTietPhieuNhapHangViewModel();
                    
                    SidebarPhieuNhapHangVM = new SidebarPhieuNhapHangViewModel();
                    SidebarChiTietPhieuNhapHangVM = new SidebarChiTietPhieuNhapHangViewModel();
 
                    CurrentView = HomeVM;
                    break;
                
                case Employee.EmployeeRole.NhanVienKyThuat:
                    IsHomeVisible = true;
                    IsKhachHangVisible = true;
                    IsSanPhamVisible = true;
                    IsBaoHanhVisible = true;
                    break;
                case Employee.EmployeeRole.ChuCuaHang:
                    IsHomeVisible = true;
                    IsNhanvienVisible = true;
                    IsSanPhamVisible = true;
                    
                    OwnerHomeVM = new OwnerHomeViewModel();
                    NhanVienVM = new NhanVienViewModel();
                    SanPhamVM = new SanPhamViewModel();
                    
                    SidebarNhanVienVM = new SidebarNhanVienViewModel();
                    CurrentView = OwnerHomeVM;
                    break;
            }
        }


        private static void SignOut(Window p)
        {
            var loginWindow = new LogInView();
            var loginVM = loginWindow.DataContext as LoginViewModel; 
            loginVM?.ResetLogin();
           
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
