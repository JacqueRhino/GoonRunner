using GoonRunner.MVVM.Model;
using GoonRunner.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class SidebarAccNhanVienViewModel : BaseViewModel
    {
        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>
        {
            "Nhân viên bán hàng",
            "Nhân viên kế toán",
            "Nhân viên kỹ thuật",
            "Nhân viên Marketing",
            "Nhân viên tạp vụ",
            "Nhân viên kiểm kho",
            "Nhân viên bảo vệ",
            "Chăm sóc khách hàng",
            "Nhân viên giao hàng",
            "Nhân viên quản trị mạng"
        };

        private ObservableCollection<ACCNHANVIEN> _danhsachaccnhanvien;
        public ObservableCollection<ACCNHANVIEN> DanhSachAccNhanVien { get => _danhsachaccnhanvien; set { _danhsachaccnhanvien = value; OnPropertyChanged(); } }
        private string _selectedrole;
        public string SelectedRole { get => _selectedrole; set { if (_selectedrole != value) { _selectedrole = value; ChucVu = value; OnPropertyChanged(); } } }
        private int _manv;
        public int MaNV { get => _manv; set { _manv = value; OnPropertyChanged(); } }
        private string _displayname;
        public string DisplayName { get => _displayname; set { _displayname = value; OnPropertyChanged(); } }
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string _confirmpassword;
        public string ConfirmPassword { get => _confirmpassword; set { _confirmpassword = value; OnPropertyChanged(); } }
        private string _chucvu;
        public string ChucVu
        {
            get => _chucvu;
            set
            {
                if (_chucvu != value)
                {
                    _chucvu = value;
                    switch (_chucvu)
                    {
                        case "Nhân viên bán hàng":
                            break;
                        case "Nhân viên kế toán":
                            break;
                        case "Nhân viên kỹ thuật":
                            break;
                        case "Nhân viên Marketing":
                            break;
                        case "Nhân viên tạp vụ":
                            break;
                        case "Nhân viên kiểm kho":
                            break;
                        case "Nhân viên bảo vệ":
                            break;
                        case "Chăm sóc khách hàng":
                            break;
                        case "Nhân viên giao hàng":
                            break;
                        case "Nhân viên quản trị mạng":
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }
        public ICommand AddAccNhanVienCommand { get; set; }
        public ICommand UpdateAccNhanVienCommand { get; set; }
        public ICommand DeleteAccNhanVienCommand { get; set; }
        public ICommand ClearFieldCommand { get; set; }
        public SidebarAccNhanVienViewModel()
        {
            SelectedRole = "Nhân viên bán hàng";

            Messenger.AccNhanVienSelected += LoadAccNhanVienInfo;

            AddAccNhanVien();
            DeleteAccNhanVien();

            ClearFieldCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                ClearFields();
            });
        }
        private void AddAccNhanVien()
        {
            DanhSachAccNhanVien = new ObservableCollection<ACCNHANVIEN>(DataProvider.Ins.goonRunnerDB.ACCNHANVIENs);
            AddAccNhanVienCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaNV == 0)
                {
                    MessageBox.Show("Hãy nhập Mã NV");
                    return;
                }

                if (string.IsNullOrEmpty(DisplayName))
                {
                    MessageBox.Show("Hãy nhập tên hiển thị");
                    return;
                }

                if (string.IsNullOrEmpty(Username))
                {
                    MessageBox.Show("Hãy nhập tên người dùng");
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Hãy nhập mật khẩu");
                    return;
                }

                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    MessageBox.Show("Hãy xác minh mật khẩu");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Mật khẩu không trùng khớp");
                    Password = "";
                    ConfirmPassword = "";
                    return;
                }

                int existingAccount = DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.Where(nv => nv.MaNV == MaNV).Count();
                if (existingAccount > 0)
                {
                    MessageBox.Show("Tài khoản đã tồn tại trong hệ thống, Vui lòng kiểm tra lại", "Trùng tài khoản", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MaNV = 0;
                    return;
                }

                var accnhanvien = new ACCNHANVIEN() { MaNV = MaNV, DisplayName = DisplayName, UserName = Username, Pass = Password, Quyen = ChucVu };
                DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.Add(accnhanvien);
                DataProvider.Ins.goonRunnerDB.SaveChanges();
                DanhSachAccNhanVien.Add(accnhanvien);
                MessageBox.Show("Thêm thành công!");
                Messenger.NotifyAccNhanVienChanged(accnhanvien);
                ClearFields();
            });

            ClearFieldCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                ClearFields();
            });
            UpdateAccNhanVienCommand = new RelayCommand<Button>((p) => { return MaNV != 0; }, (p) =>
            {
                UpdateAccNhanVienInfo();
            });
        }

        private void UpdateAccNhanVienInfo()
        {
            try
            {
                var existingAccount = DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.Where(tk => tk.MaNV == MaNV).FirstOrDefault();
                if (MaNV == 0 || existingAccount == null)
                {
                    MessageBox.Show("Tài khoản này không tồn tại");
                    return;
                }

                if (string.IsNullOrEmpty(DisplayName))
                {
                    MessageBox.Show("Hãy nhập tên hiển thị");
                    return;
                }

                if (string.IsNullOrEmpty(Username))
                {
                    MessageBox.Show("Hãy nhập tên người dùng");
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Hãy nhập mật khẩu");
                    return;
                }

                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    MessageBox.Show("Hãy xác minh mật khẩu");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Mật khẩu không trùng khớp");
                    Password = "";
                    ConfirmPassword = "";
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn cập nhật thông tin tài khoản {existingAccount.DisplayName}?",
                    "Xác nhận cập nhật",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Update employee information
                existingAccount.DisplayName = DisplayName;
                existingAccount.UserName = Username;
                existingAccount.Pass = Password;
                existingAccount.Quyen = ChucVu;

                DataProvider.Ins.goonRunnerDB.SaveChanges();

                var localAccount = DanhSachAccNhanVien.FirstOrDefault(nv => nv.MaNV == MaNV);
                if (localAccount != null)
                {
                    localAccount.MaNV = MaNV;
                    localAccount.DisplayName = DisplayName;
                    localAccount.UserName = Username;
                    localAccount.Pass = Password;
                    localAccount.Quyen = ChucVu;
                }

                MessageBox.Show("Cập nhật thành công!");

                Messenger.NotifyAccNhanVienChanged(existingAccount);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteAccNhanVien()
        {
            DeleteAccNhanVienCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (MaNV == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa tài khoản {DisplayName}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var accnhanvien = DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.FirstOrDefault(nv => nv.MaNV == MaNV);

                        if (accnhanvien != null)
                        {
                            DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.Remove(accnhanvien);
                            DataProvider.Ins.goonRunnerDB.SaveChanges();

                            var itemToRemove = DanhSachAccNhanVien.FirstOrDefault(nv => nv.MaNV == MaNV);
                            if (itemToRemove != null)
                            {
                                DanhSachAccNhanVien.Remove(itemToRemove);
                            }

                            MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                            Messenger.NotifyAccNhanVienChanged(accnhanvien);

                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên cần xóa!");
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        string errorMessage = "Lỗi xác thực dữ liệu:\n";
                        foreach (var validationError in ex.EntityValidationErrors)
                        {
                            foreach (var error in validationError.ValidationErrors)
                            {
                                errorMessage += $"- {error.ErrorMessage}\n";
                            }
                        }
                        MessageBox.Show(errorMessage, "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
        public void LoadAccNhanVienInfo(int maNV)
        {
            try
            {
                var accnhanvien = DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.FirstOrDefault(nv => nv.MaNV == maNV);

                if (accnhanvien != null)
                {
                    MaNV = accnhanvien.MaNV;
                    DisplayName = accnhanvien.DisplayName;
                    Username = accnhanvien.UserName;
                    ChucVu = accnhanvien.Quyen;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhân viên: {ex.Message}");
            }
        }
        private void ClearFields()
        {
            MaNV = 0;
            DisplayName = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            SelectedRole = "Nhân viên bán hàng";
        }
    }
}

