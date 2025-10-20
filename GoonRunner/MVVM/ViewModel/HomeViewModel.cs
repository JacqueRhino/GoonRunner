using GoonRunner.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;

namespace GoonRunner.MVVM.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set { _seriesCollection = value; OnPropertyChanged(); }
        }

        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set { _labels = value; OnPropertyChanged(); }
        }

        public HomeViewModel()
        {
            LoadTopSellingProducts();
        }

        private void LoadTopSellingProducts()
        {
            var month = DateTime.Now;
            var startDate = new DateTime(month.Year, month.Month, 1);
            var endDate = startDate.AddMonths(1);

            // Sử dụng DataProvider singleton để lấy dữ liệu
            var db = DataProvider.Ins.goonRunnerDB;

            var top5 = db.SANPHAMs
                .Select(sp => new
                {
                    sp.TenSP,
                    TotalSold = sp.CHITIETHOADONs
                        .Where(ct => ct.HOADON.NgayMuaHang >= startDate && ct.HOADON.NgayMuaHang < endDate)
                        .Sum(ct => (int?)ct.SoLuongDat) ?? 0
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToList();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Sản phẩm bán chạy",
                    Values = new ChartValues<int>(top5.Select(p => p.TotalSold))
                }
            };

            Labels = top5.Select(p => p.TenSP).ToArray();
        }
    }
}
