using System.Windows;

namespace GoonRunner.MVVM.View
{
    public partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
            Loaded += HomeView_Loaded;
            SizeChanged += HomeView_SizeChanged;
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayoutForWidth(ActualWidth);
        }

        private void HomeView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayoutForWidth(e.NewSize.Width);
        }

        private void UpdateLayoutForWidth(double width)
        {
            if (TopStatsPanel == null) return;

            switch (width)
            {
                case < 935:
                    TopStatsPanel.Columns = 2;
                    TopStatsPanel.Rows = 2;
                    break;
                default:
                    TopStatsPanel.Columns = 4;
                    TopStatsPanel.Rows = 1;
                    break;
            }
        }
    }
}