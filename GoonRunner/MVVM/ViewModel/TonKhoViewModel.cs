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
        private string _filterText;
        public string FilterText { get => _filterText; set { _filterText = value; OnPropertyChanged(); FilteredTonKhoList.Refresh(); } }
        private System.ComponentModel.ICollectionView _filteredtonkholist;
        public System.ComponentModel.ICollectionView FilteredTonKhoList { get => _filteredtonkholist; set { _filteredtonkholist = value; OnPropertyChanged(); } }
        public ICommand RefreshCommand { get; set; }
        public TonKhoViewModel()
        {
            TonKhoList = new ObservableCollection<TonKho>(); 
            LoadTonKhoList();
            FilteredTonKhoList = System.Windows.Data.CollectionViewSource.GetDefaultView(TonKhoList);
            FilteredTonKhoList.Filter = FilterTonKho;
            
            RefreshCommand = new RelayCommand<Button>((p) => true, (p) =>
            {
                LoadTonKhoList();
                FilterText = string.Empty;
            });
        }

        private bool FilterTonKho(object obj)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            TonKho tonKho = obj as TonKho;

            if (int.TryParse(FilterText, out var maTK ))
            {
                if (tonKho.STT == maTK)
                    return true;
            }
            
            if ( tonKho.sanpham.TenSP?.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        private void LoadTonKhoList()
        {
            TonKhoList?.Clear();
            int i = 1;
            var DanhSachSanPham = DataProvider.Ins.goonRunnerDB.SANPHAMs;
            foreach (var item in DanhSachSanPham)
            {
                var inputList = DataProvider.Ins.goonRunnerDB.CHITIETPHIEUNHAPHANGs.Where(p => p.MaSP == item.MaSP); 
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
            FilteredTonKhoList?.Refresh();
        }
    }
}
