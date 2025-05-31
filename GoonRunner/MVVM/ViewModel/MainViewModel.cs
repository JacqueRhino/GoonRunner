using System;
using GoonRunner.MVVM.View;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
// ReSharper disable InconsistentNaming

namespace GoonRunner.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region ICommand
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

        #endregion
        #region ViewModel
        public KhachHangViewModel KhachHangVM { get; private set; }
        public NhanVienViewModel NhanVienVM { get; private set; }
        private SanPhamViewModel SanPhamVM { get; set; }
        public PhieuNhapHangViewModel PhieuNhapHangVM { get; set; }
        public ChiTietPhieuNhapHangViewModel ChiTietPhieuNhapHangVM { get; set; }
        public HoaDonViewModel HoaDonVM { get; set; }
        public ChiTietHoaDonViewModel ChiTietHoaDonVM { get; set; }
        public TonKhoViewModel TonKhoVM { get; set; }
        private HomeViewModel HomeVM { get; set; }
        private OwnerHomeViewModel OwnerHomeVM { get; set; }
        #endregion
        #region SidebarViewModel
        public SidebarNhanVienViewModel SidebarNhanVienVM { get; private set; }
        private SidebarKhachHangViewModel SidebarKhachHangVM { get; set; }
        public SidebarPhieuNhapHangViewModel SidebarPhieuNhapHangVM { get; private set; }
        public SidebarChiTietPhieuNhapHangViewModel SidebarChiTietPhieuNhapHangVM { get; set; }
        public SidebarHoaDonViewModel SidebarHoaDonVM { get; private set; }
        public SidebarChiTietHoaDonViewModel SidebarChiTietHoaDonVM { get; set; }
        #endregion
        
        public string DisplayName
        { get => _displayname;
            private set 
            { _displayname = value;
              OnPropertyChanged(); 
            } 
        }

        public Employee.EmployeeRole Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged();
            }
            
        }
        
        public string Privilege {
            get => _privilege;
            private set
            {
                _privilege = value;
                OnPropertyChanged();
            } 
        }
        
        public int CurrentUser 
        {
            get => _currentuser;
            private set
            {
                _currentuser = value;
                OnPropertyChanged();
            } 
        }
        
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public object CurrentSidebarView
        {
            get => _currentSidebarView;
            set
            {
                _currentSidebarView = value;
                OnPropertyChanged();
            }
        }

        
        public bool SidebarButtonEnabled
        {
            get => _sidebarButtonEnabled;
            set
            {
                _sidebarButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        
        public int SidebarLeftGapWidth
        {
            get => _sidebarLeftGapWidth;
            set
            {
                _sidebarLeftGapWidth = value;
                OnPropertyChanged();
            }
        }
        
        public int SidebarWidth
        {
            get => _sidebarWidth;
            set
            {
                _sidebarWidth = value;
                OnPropertyChanged();
            }
        }
        
        public string Split2Enabled
        {
            get => _split2Enabled;
            set
            {
                _split2Enabled = value;
                OnPropertyChanged();
            }
        }
        private string _displayname;
        private Employee.EmployeeRole _role;
        private string _privilege;
        private int _currentuser;
        private object _currentView;
        private object _currentSidebarView;
        private bool _sidebarButtonEnabled;
        private int _sidebarLeftGapWidth;
        private int _sidebarWidth;
        private string _split2Enabled;


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
                if (Role == Employee.EmployeeRole.ChuCuaHang)
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
        
        public void SetAuthorization(Employee.EmployeeRole role)
        {
            ResetVisiblity();
            InitializeHomeViewModel(role);
            switch (role)
            {
                //TODO:ADD KHUYENMAI VIEWMODEL
                case Employee.EmployeeRole.Admin:
                    SetAdminPriviliges();
                    break;

                case Employee.EmployeeRole.NhanVienBanHang:
                    SetSalesEmployeePriviliges();
                    CurrentView = HomeVM;
                    break;
                
                case Employee.EmployeeRole.NhanVienKeToan:
                    SetAccountantPriviliges();
                    break;
                
                case Employee.EmployeeRole.NhanVienChamSocKhachHang:
                    SetCustomerServicePriviliges();
                    break;
                
                case Employee.EmployeeRole.NhanVienKiemKho:
                    SetInventoryPriviliges();
                    break;
                
                case Employee.EmployeeRole.NhanVienKyThuat:
                    SetTechnicalPriviliges();
                    break;
                case Employee.EmployeeRole.ChuCuaHang:
                    SetOwnerPriviliges();
                    break;
                
                case Employee.EmployeeRole.NhanVienMarketing:
                case Employee.EmployeeRole.NhanVienTapVu:
                case Employee.EmployeeRole.NhanVienBaoVe:
                case Employee.EmployeeRole.NhanVienGiaoHang:
                case Employee.EmployeeRole.NhanVienQuanTriMang:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
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

        private void ResetVisiblity()
        {
            IsHomeVisible = false;
            IsNhanvienVisible = false;
            IsSanPhamVisible = false;
            IsHoaDonVisible = false;
            IsPhieuNhapVisible = false;
            IsTonKhoVisible = false;
            IsKhachHangVisible = false;
            IsKhuyenMaiVisible = false;
            IsBaoHanhVisible = false; 
        }
        
        private void EnableSidebar()
        {
            SidebarButtonEnabled = true;
        }
        private void DisableSidebar()
        {
            SidebarButtonEnabled = false;
        }
        
        private void InitializeHomeViewModel(Employee.EmployeeRole role)
        {
            IsHomeVisible = true;

            if (role == Employee.EmployeeRole.ChuCuaHang)
            {
                if (OwnerHomeVM == null)
                    OwnerHomeVM = new OwnerHomeViewModel();

                CurrentView = OwnerHomeVM;
                return;
            }

            if (HomeVM == null)
                HomeVM = new HomeViewModel();

            CurrentView = HomeVM;
        }
        private void SetAdminPriviliges()
        {
            IsNhanvienVisible = true;
            IsKhachHangVisible = true;
            IsKhuyenMaiVisible = true;
            
            InitializeViewModels(
                nhanVien: true,
                khachHang: true
            );
        }

        private void SetSalesEmployeePriviliges()
        {
            IsHoaDonVisible = true;
            IsKhachHangVisible = true;
            IsKhuyenMaiVisible = true;
            IsSanPhamVisible = true;
            IsTonKhoVisible = true;
            
            InitializeViewModels(
                hoaDon: true,
                khachHang: true,
                sanPham: true,
                tonKho: true
            );
        }

        private void SetAccountantPriviliges()
        {
            IsNhanvienVisible = true;
            IsHoaDonVisible = true;
            
            InitializeViewModels(
                nhanVien: true,
                hoaDon: true
            );
        }

        private void SetCustomerServicePriviliges()
        {
            IsKhachHangVisible = true;
            IsHoaDonVisible = true;

            InitializeViewModels(
                khachHang: true,
                hoaDon: true
            );
        }

        private void SetInventoryPriviliges()
        {
            InitializeViewModels(
                tonKho: true,
                sanPham: true,
                phieuNhapHang: true
            );
        }

        //TODO:Setup KyThuat
        private void SetTechnicalPriviliges()
        {
            InitializeViewModels(
                khachHang:true,
                sanPham:true
                );
        }

        private void SetOwnerPriviliges()
        {
            IsNhanvienVisible = true;
            IsSanPhamVisible = true;
            
            InitializeViewModels(
                nhanVien: true,
                sanPham: true
            );
        }
        //TODO:MISSING BAOHANH VIEWMODEL
        private void InitializeViewModels(
            bool nhanVien = false,
            bool khachHang = false,
            bool sanPham = false,
            bool hoaDon = false,
            bool phieuNhapHang = false,
            bool tonKho = false
        )
        {
            IsHomeVisible = true;
            if (nhanVien)
            {
                if (NhanVienVM == null)
                    NhanVienVM = new NhanVienViewModel();

                if (SidebarNhanVienVM == null)
                    SidebarNhanVienVM = new SidebarNhanVienViewModel();                
                
                IsNhanvienVisible = true;
            }

            if (khachHang)
            {
                if (KhachHangVM == null)
                    KhachHangVM = new KhachHangViewModel();

                if (SidebarKhachHangVM == null)
                    SidebarKhachHangVM = new SidebarKhachHangViewModel(); 
                IsKhachHangVisible = true;
            }
            if (sanPham && SanPhamVM == null) SanPhamVM = new SanPhamViewModel();
            if (hoaDon)
            {
                if (HoaDonVM == null)
                    HoaDonVM = new HoaDonViewModel();

                if (ChiTietHoaDonVM == null)
                    ChiTietHoaDonVM = new ChiTietHoaDonViewModel();

                if (SidebarHoaDonVM == null)
                    SidebarHoaDonVM = new SidebarHoaDonViewModel();

                if (SidebarChiTietHoaDonVM == null)
                    SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel();                
                
                IsHoaDonVisible = true;
            } 
            
            if (phieuNhapHang)
            {
                if (PhieuNhapHangVM == null)
                    PhieuNhapHangVM = new PhieuNhapHangViewModel();

                if (ChiTietPhieuNhapHangVM == null)
                    ChiTietPhieuNhapHangVM = new ChiTietPhieuNhapHangViewModel();

                if (SidebarPhieuNhapHangVM == null)
                    SidebarPhieuNhapHangVM = new SidebarPhieuNhapHangViewModel();

                if (SidebarChiTietPhieuNhapHangVM == null)
                    SidebarChiTietPhieuNhapHangVM = new SidebarChiTietPhieuNhapHangViewModel();                
                
                IsPhieuNhapVisible = true;
            }

            if (tonKho)
            {
                if (TonKhoVM == null)
                    TonKhoVM = new TonKhoViewModel();                
                
                IsTonKhoVisible = true;
            }
        }
    }
}
