using GoonRunner.MVVM.Model;
using GoonRunner.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class AccNhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<ACCNHANVIEN> _accnhanvienlist;
        public ObservableCollection<ACCNHANVIEN> AccNhanVienList { get { return _accnhanvienlist; } set { _accnhanvienlist = value; OnPropertyChanged(); } }
        private bool _isUpdatingSelection;
        private ACCNHANVIEN _selectedItem;
        public ACCNHANVIEN SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged();

                if (_isUpdatingSelection) return;

                try
                {
                    _isUpdatingSelection = true;
                    Messenger.NotifyAccNhanVienSelected(_selectedItem?.MaNV ?? 0);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }
        }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand LoadToSidebar { get; set; }
        public ICommand RefreshCommand { get; set; }
        public AccNhanVienViewModel()
        {
            LoadAccNhanVienList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadAccNhanVienList(); });
            Messenger.AccNhanVienChanged += nv =>
            {
                LoadAccNhanVienList();
            };

            Messenger.AccNhanVienSelected += manv =>
            {
                SelectedItem = AccNhanVienList.FirstOrDefault(nv => nv.MaNV == manv);
            };

        }
        public void LoadAccNhanVienList()
        {
            AccNhanVienList = new ObservableCollection<ACCNHANVIEN>();
            var danhSachAccNhanVien = DataProvider.Ins.goonRunnerDB.ACCNHANVIENs.Where(n => n.MaNV > 0);

            foreach (var item in danhSachAccNhanVien)
            {
                var accnhanvien = new ACCNHANVIEN
                {
                    DisplayName = item.DisplayName,
                    UserName = item.UserName,
                    Quyen = item.Quyen,
                    MaNV = item.MaNV
                };
                AccNhanVienList.Add(accnhanvien);
            }
        }
    }
}
