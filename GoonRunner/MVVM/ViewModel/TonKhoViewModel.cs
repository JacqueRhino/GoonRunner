using GoonRunner.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoonRunner.MVVM.ViewModel
{
    public class TonKhoViewModel : BaseViewModel
    {
        private ObservableCollection<TonKho> _tonkholist;
        public ObservableCollection<TonKho> TonKhoList { get { return _tonkholist; } set { _tonkholist = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public TonKhoViewModel()
        {
            LoadTonKhoList();
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) => { LoadTonKhoList(); });
        }
        private void LoadTonKhoList()
        {
            TonKhoList = new ObservableCollection<TonKho>();
            int i = 1;
            var DanhSachSanPham = DataProvider.Ins.goonRunnerDB.SANPHAMs;
            foreach (var item in DanhSachSanPham)
            {
                var inputList = DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs.Where(p => p.MaSP == item.MaSP); // Mã SP của CHITIETPHIEUNHAPHANG == Mã SP của SANPHAM
                var outputList = DataProvider.Ins.goonRunnerDB.CHITIETHOADONs.Where(p => p.MaSP == item.MaSP);

                int sumInput = 0;
                int sumOutput = 0;

                if (inputList != null)
                {
                    sumInput = inputList.Sum(p => p.SoLuongNhap) ?? 0;
                }
                if (outputList != null)
                {
                    sumOutput = outputList.Sum(p => p.SoLuongDat) ?? 0;
                }

                TonKho tonkho = new TonKho();
                tonkho.STT = i;
                tonkho.SoLuong = sumInput - sumOutput;
                tonkho.sanpham = item;
                TonKhoList.Add(tonkho);
                i++;
            }
        }
    }
}
