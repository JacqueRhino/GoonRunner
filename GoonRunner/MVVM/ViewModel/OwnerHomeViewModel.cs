using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class OwnerHomeViewModel : BaseViewModel
    {
        private ObservableCollection<DoanhThuTheoNgay> _doanhthutheongaylist;
        public ObservableCollection<DoanhThuTheoNgay> DoanhThuTheoNgayList { get { return _doanhthutheongaylist; } set { _doanhthutheongaylist = value; OnPropertyChanged(); } }
        private DateTime _selecteddate;
        public DateTime SelectedDate { get => _selecteddate; set { if (_selecteddate != value) { _selecteddate = value; HienThiDoanhThu = 0; LoadDoanhThu(); OnPropertyChanged(); } } }
        private int _ngaychon;
        public int NgayChon { get => _ngaychon; set { _ngaychon = SelectedDate.Day; OnPropertyChanged(); } }
        private int _hienthidoanhthu;
        public int HienThiDoanhThu { get => _hienthidoanhthu; set { _hienthidoanhthu = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public OwnerHomeViewModel()
        {
            SelectedDate = DateTime.Today;
            LoadDoanhThu();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadDoanhThu(); });
        }
        private void LoadDoanhThu()
        {
            DoanhThuTheoNgayList = new ObservableCollection<DoanhThuTheoNgay>();
            int year = SelectedDate.Year;
            int month = SelectedDate.Month;
            int day = SelectedDate.Day;

            var DoanhThu = DataProvider.Ins.goonRunnerDB.DoanhThuTheoNgays.Where(n => n.Ngay == day && n.Thang == month && n.Nam == year && n.TongDoanhThu > 0).ToList();
            foreach (var item in DoanhThu)
            {
                DoanhThuTheoNgay doanhthutheongay = new DoanhThuTheoNgay();
                if (DoanhThu != null)
                {
                    HienThiDoanhThu = DoanhThu.Sum(n => n.TongDoanhThu) ?? 0;
                }
                else
                {
                    HienThiDoanhThu = 0;
                }
                DoanhThuTheoNgayList.Add(doanhthutheongay);
            }
        }
    }
}
