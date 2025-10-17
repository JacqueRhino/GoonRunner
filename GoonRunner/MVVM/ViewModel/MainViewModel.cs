using GoonRunner.MVVM.Model;
using GoonRunner.MVVM.View;
using GoonRunner.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable InconsistentNaming

namespace GoonRunner.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region ICommand
        public ICommand HomeViewCommand { get; }
        public ICommand EmployeeViewCommand { get; }
        public ICommand EmployeeAccountViewCommand { get; }
        public ICommand CustomerViewCommand { get; }
        public ICommand ProductViewCommand { get; }
        public ICommand GoodsReceiptViewCommand { get; }
        public ICommand InvoiceViewCommand { get; }
        public ICommand InventoryViewCommand { get; }
        public ICommand SignOutCommand { get; }

        #endregion
        #region ViewModel

        private KhachHangViewModel KhachHangVM { get; set; }
        public NhanVienViewModel NhanVienVM { get; set; }
        public AccNhanVienViewModel AccNhanVienVM { get; set; }
        private SanPhamViewModel SanPhamVM { get; set; }
        public PhieuNhapHangViewModel PhieuNhapHangVM { get; private set; }
        public ChiTietPhieuNhapHangViewModel ChiTietPhieuNhapHangVM { get; set; }
        private HoaDonViewModel HoaDonVM { get; set; }
        public ChiTietHoaDonViewModel ChiTietHoaDonVM { get; private set; }
        private TonKhoViewModel TonKhoVM { get; set; }
        private HomeViewModel HomeVM { get; set; }
        private OwnerHomeViewModel OwnerHomeVM { get; set; }
        #endregion
        #region SidebarViewModel
        public SidebarNhanVienViewModel SidebarNhanVienVM { get; private set; }
        public SidebarAccNhanVienViewModel SidebarAccNhanVienVM { get; private set; }
        private SidebarKhachHangViewModel SidebarKhachHangVM { get; set; }
        private SidebarPhieuNhapHangViewModel SidebarPhieuNhapHangVM { get; set; }
        public SidebarChiTietPhieuNhapHangViewModel SidebarChiTietPhieuNhapHangVM { get; set; }
        private SidebarHoaDonViewModel SidebarHoaDonVM { get; set; }
        private SidebarChiTietHoaDonViewModel SidebarChiTietHoaDonVM { get; set; }
        #endregion

        private UserSession _currentSession;

        private UserSession CurrentSession
        {
            get => _currentSession;
            set
            {
                _currentSession = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayName));
                OnPropertyChanged(nameof(Privilege));
                OnPropertyChanged(nameof(Role));
            }
        }

        public string DisplayName => CurrentSession?.DisplayName;
        public string Privilege => CurrentSession?.RoleName;
        public EmployeeRoles.Roles Role => CurrentSession?.Role ?? EmployeeRoles.Roles.NhanVienBanHang;

        #region Visibility
        private bool _isHomeVisible;
        public bool IsHomeVisible
        {
            get => _isHomeVisible;
            set { _isHomeVisible = value; OnPropertyChanged(); }
        }

        private bool _isNhanVienVisible;
        public bool IsNhanVienVisible
        {
            get => _isNhanVienVisible;
            set { _isNhanVienVisible = value; OnPropertyChanged(); }
        }

        private bool _isAccNhanVienVisible;
        public bool IsAccNhanVienVisible
        {
            get => _isAccNhanVienVisible;
            set { _isAccNhanVienVisible = value; OnPropertyChanged(); }
        }

        private bool _isSanPhamVisible;
        public bool IsSanPhamVisible
        {
            get => _isSanPhamVisible;
            set { _isSanPhamVisible = value; OnPropertyChanged(); }
        }

        private bool _isHoaDonVisible;
        public bool IsHoaDonVisible
        {
            get => _isHoaDonVisible;
            set { _isHoaDonVisible = value; OnPropertyChanged(); }
        }

        private bool _isPhieuNhapVisible;
        public bool IsPhieuNhapVisible
        {
            get => _isPhieuNhapVisible;
            set { _isPhieuNhapVisible = value; OnPropertyChanged(); }
        }

        private bool _isTonKhoVisible;
        public bool IsTonKhoVisible
        {
            get => _isTonKhoVisible;
            set { _isTonKhoVisible = value; OnPropertyChanged(); }
        }

        private bool _isKhachHangVisible;
        public bool IsKhachHangVisible
        {
            get => _isKhachHangVisible;
            set { _isKhachHangVisible = value; OnPropertyChanged(); }
        }

        private bool _isKhuyenMaiVisible;
        public bool IsKhuyenMaiVisible
        {
            get => _isKhuyenMaiVisible;
            set { _isKhuyenMaiVisible = value; OnPropertyChanged(); }
        }

        private bool _isBaoHanhVisible;
        public bool IsBaoHanhVisible
        {
            get => _isBaoHanhVisible;
            set { _isBaoHanhVisible = value; OnPropertyChanged(); }
        }
        #endregion

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView == value) return;
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


        public bool IsSidebarButtonEnabled
        {
            get => _isSidebarButtonEnabled;
            set
            {
                _isSidebarButtonEnabled = value;
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
            private set
            {
                _sidebarWidth = value;
                OnPropertyChanged();
            }
        }

        public bool IsSplit2Enabled
        {
            get => _issplit2enabled;
            set
            {
                _issplit2enabled = value;
                OnPropertyChanged();
            }
        }

        private string _title = "GoonRunner - Trang Chủ";
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }

        private object _currentView;
        private object _currentSidebarView;
        private bool _isSidebarButtonEnabled;
        private int _sidebarLeftGapWidth;
        private int _sidebarWidth;
        private bool _issplit2enabled;

        public MainViewModel()
        {
            HomeViewCommand = new RelayCommand<RadioButton>(o =>
            {
                if (Role == EmployeeRoles.Roles.ChuCuaHang)
                {
                    CurrentView = OwnerHomeVM;
                }
                else
                {
                    CurrentView = HomeVM;
                }
                Title = "GoonRunner - Trang Chủ";

                DisableSidebar();
            });

            EmployeeViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = NhanVienVM;
                CurrentSidebarView = SidebarNhanVienVM;
                Title = "GoonRunner - Nhân Viên";
                IsSplit2Enabled = true;
                EnableSidebar();
            });

            EmployeeAccountViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = AccNhanVienVM;
                CurrentSidebarView = SidebarAccNhanVienVM;
                Title = "GoonRunner - Tài Khoản Nhân Viên";
                IsSplit2Enabled = true;
                EnableSidebar();
            });

            CustomerViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = KhachHangVM;
                CurrentSidebarView = SidebarKhachHangVM;
                Title = "GoonRunner - Khách Hàng";
                IsSplit2Enabled = true;
                EnableSidebar();
            });


            ProductViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = SanPhamVM;
                Title = "GoonRunner - Sản Phẩm";
                DisableSidebar();
            });

            GoodsReceiptViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = PhieuNhapHangVM;
                CurrentSidebarView = SidebarPhieuNhapHangVM;
                IsSplit2Enabled = true;
                Title = "GoonRunner - Phiếu Nhập Hàng";
                EnableSidebar();
            });

            InvoiceViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = HoaDonVM;
                CurrentSidebarView = SidebarHoaDonVM;
                IsSplit2Enabled = true;
                Title = "GoonRunner - Hóa Đơn";
                EnableSidebar();
            });

            InventoryViewCommand = new RelayCommand<RadioButton>(o =>
            {
                CurrentView = TonKhoVM;
                Title = "GoonRunner - Tồn Kho";
                DisableSidebar();
            });

            SignOutCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Thông báo",
                    Content = "Bạn có muốn đăng xuất?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    PrimaryButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Danger,
                    CloseButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Secondary
                };

                var result = messageBox.ShowDialogAsync().GetAwaiter().GetResult();
                if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
                    SignOut(p);

            });
        }

        public void SetCurrentSession(int userId, string roleInName, EmployeeRoles.Roles role, string displayName)
        {
            CurrentSession = new UserSession
            {
                UserId = userId,
                DisplayName = displayName,
                Role = role,
                RoleName = roleInName
            };

            SetAuthorization(role);
            SetAuthorization(CurrentSession.Role);
        }
        public void SetAuthorization(EmployeeRoles.Roles role)
        {
            InitializeHomeViewModel(role);

            if (!EmployeeRoles.Permissions.Map.TryGetValue(role, out var permissions))
                return;

            foreach (var perm in permissions)
            {
                switch (perm)
                {
                    case EmployeeRoles.Permission.Home:
                        IsHomeVisible = true;
                        break;

                    case EmployeeRoles.Permission.NhanVien:
                        if (NhanVienVM == null)
                            NhanVienVM = new NhanVienViewModel();
                        if (SidebarNhanVienVM == null)
                            SidebarNhanVienVM = new SidebarNhanVienViewModel();
                        IsNhanVienVisible = true;
                        break;

                    case EmployeeRoles.Permission.AccNhanVien:
                        if (AccNhanVienVM == null)
                            AccNhanVienVM = new AccNhanVienViewModel();
                        if (SidebarAccNhanVienVM == null)
                            SidebarAccNhanVienVM = new SidebarAccNhanVienViewModel();
                        IsAccNhanVienVisible = true;
                        break;

                    case EmployeeRoles.Permission.KhachHang:
                        if (KhachHangVM == null)
                            KhachHangVM = new KhachHangViewModel();
                        if (SidebarKhachHangVM == null)
                            SidebarKhachHangVM = new SidebarKhachHangViewModel();
                        IsKhachHangVisible = true;
                        break;

                    case EmployeeRoles.Permission.SanPham:
                        if (SanPhamVM == null)
                            SanPhamVM = new SanPhamViewModel();
                        IsSanPhamVisible = true;
                        break;

                    case EmployeeRoles.Permission.HoaDon:
                        if (HoaDonVM == null)
                            HoaDonVM = new HoaDonViewModel(maHD =>
                            {
                                // Dependency Injection cho navigateBack
                                ChiTietHoaDonVM = new ChiTietHoaDonViewModel(maHD, () =>
                                {
                                    CurrentView = HoaDonVM;
                                    CurrentSidebarView = SidebarHoaDonVM;
                                });

                                SidebarChiTietHoaDonVM = new SidebarChiTietHoaDonViewModel(maHD);

                                CurrentView = ChiTietHoaDonVM;
                                CurrentSidebarView = SidebarChiTietHoaDonVM;
                            });
                        if (SidebarHoaDonVM == null)
                            SidebarHoaDonVM = new SidebarHoaDonViewModel(CurrentSession, HoaDonVM.Refresh);
                        IsHoaDonVisible = true;
                        break;

                    case EmployeeRoles.Permission.PhieuNhap:
                        if (PhieuNhapHangVM == null)
                            PhieuNhapHangVM = new PhieuNhapHangViewModel(mapnh =>
                            {
                                ChiTietPhieuNhapHangVM = new ChiTietPhieuNhapHangViewModel(mapnh, () =>
                                {
                                    CurrentView = PhieuNhapHangVM;
                                    CurrentSidebarView = SidebarPhieuNhapHangVM;
                                });
                                SidebarChiTietPhieuNhapHangVM = new SidebarChiTietPhieuNhapHangViewModel(mapnh);
                                CurrentView = ChiTietPhieuNhapHangVM;
                                CurrentSidebarView = SidebarChiTietPhieuNhapHangVM;
                            });
                        if (SidebarPhieuNhapHangVM == null)
                            SidebarPhieuNhapHangVM = new SidebarPhieuNhapHangViewModel(CurrentSession);
                        IsPhieuNhapVisible = true;
                        break;

                    case EmployeeRoles.Permission.TonKho:
                        if (TonKhoVM == null)
                            TonKhoVM = new TonKhoViewModel();
                        IsTonKhoVisible = true;
                        break;

                    case EmployeeRoles.Permission.KhuyenMai:
                        IsKhuyenMaiVisible = true;
                        break;

                    case EmployeeRoles.Permission.BaoHanh:
                        IsBaoHanhVisible = true;
                        break;
                }
            }
        }

        private void SignOut(Window p)
        {
            ResetState();
            var loginWindow = new LogInView();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            loginVM?.ResetLogin();

            loginWindow.Show();
            p.Close();
        }

        private void ResetState()
        {
            CurrentSidebarView = null;
            IsSidebarButtonEnabled = false;
            SidebarWidth = 0;
            IsSplit2Enabled = false;
            ClearViewModels();

        }

        private void ClearViewModels()
        {
            KhachHangVM = null;
            NhanVienVM = null;
            AccNhanVienVM = null;
            SanPhamVM = null;
            PhieuNhapHangVM = null;
            ChiTietPhieuNhapHangVM = null;
            HoaDonVM = null;
            ChiTietHoaDonVM = null;
            TonKhoVM = null;
            HomeVM = null;
            OwnerHomeVM = null;
            SidebarNhanVienVM = null;
            SidebarAccNhanVienVM = null;
            SidebarKhachHangVM = null;
            SidebarPhieuNhapHangVM = null;
            SidebarChiTietPhieuNhapHangVM = null;
            SidebarHoaDonVM = null;
            SidebarChiTietHoaDonVM = null;
        }
        private void EnableSidebar()
        {
            IsSidebarButtonEnabled = true;
        }
        private void DisableSidebar()
        {
            SidebarWidth = 0;
            IsSidebarButtonEnabled = false;
        }


        private void InitializeHomeViewModel(EmployeeRoles.Roles role)
        {
            if (role == EmployeeRoles.Roles.ChuCuaHang)
            {
                if (OwnerHomeVM == null)
                    OwnerHomeVM = new OwnerHomeViewModel();

                CurrentView = OwnerHomeVM;
                IsHomeVisible = true;
            }
            else
            {
                if (HomeVM == null)
                    HomeVM = new HomeViewModel();

                CurrentView = HomeVM;
                IsHomeVisible = true;
            }
        }
    }
}
