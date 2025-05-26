using GoonRunner.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class OwnerHomeViewModel : BaseViewModel
    {
        private ObservableCollection<DoanhThuTheoNgay> _doanhthutheongaylist;
        public ObservableCollection<DoanhThuTheoNgay> DoanhThuTheoNgayList { get { return _doanhthutheongaylist; } set { _doanhthutheongaylist = value; OnPropertyChanged(); } }
        private ObservableCollection<DoanhThuTheoNgay> _loinhuantheothang;
        public ObservableCollection<DoanhThuTheoNgay> LoiNhuanTheoThangList { get { return _loinhuantheothang; } set { _loinhuantheothang = value; OnPropertyChanged(); } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; HienThiDoanhThu = 0; LoadDoanhThu(); OnPropertyChanged(); } } }
        private int _ngaychon;
        public int NgayChon { get => _ngaychon; set { _ngaychon = value; _ngaychon = SelectedDate.Day; OnPropertyChanged(); } }
        private int _hienthidoanhthu;
        public int HienThiDoanhThu { get => _hienthidoanhthu; set { _hienthidoanhthu = value; OnPropertyChanged(); } }
        private int _hienthiloinhuan;
        public int HienThiLoiNhuan { get => _hienthiloinhuan; set { _hienthiloinhuan = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public OwnerHomeViewModel()
        {
            HienThiDoanhThu = 0;
            HienThiLoiNhuan = 0;
            SelectedDate = DateTime.Today;
            LoadLoiNhuan();
            LoadDoanhThu();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { RefreshView();});
        }

        private void RefreshView()
        {
            LoadDoanhThu();
            LoadLoiNhuan();
        }


        private void LoadDoanhThu()
        {
            DoanhThuTheoNgayList = new ObservableCollection<DoanhThuTheoNgay>();
            int year = SelectedDate.Year;
            int month = SelectedDate.Month;
            int day = SelectedDate.Day;

            var doanhThu = DataProvider.Ins.goonRunnerDB.DoanhThuTheoNgays.Where(n => n.Ngay == day && n.Thang == month && n.Nam == year && n.TongDoanhThu > 0).ToList();
            foreach (var _ in doanhThu)
            {
                DoanhThuTheoNgay doanhthutheongay = new DoanhThuTheoNgay();
                HienThiDoanhThu = doanhThu.Sum(n => n.TongDoanhThu) ?? 0;
                DoanhThuTheoNgayList.Add(doanhthutheongay);
            }
        }
        
        private void LoadLoiNhuan()
        {
            LoiNhuanTheoThangList = new ObservableCollection<DoanhThuTheoNgay>();
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var loinhuan = DataProvider.Ins.goonRunnerDB.DoanhThuTheoNgays.Where(n => n.Thang == month && n.Nam == year && n.LoiNhuan > 0).ToList();
            foreach (var _ in loinhuan)
            {
                DoanhThuTheoNgay loinhuantheothang = new DoanhThuTheoNgay();
                HienThiLoiNhuan = loinhuan.Sum(n => n.LoiNhuan) ?? 0;
                LoiNhuanTheoThangList.Add(loinhuantheothang);

            } 
        }
    }
}
