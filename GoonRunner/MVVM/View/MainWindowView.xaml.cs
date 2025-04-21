using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GoonRunner.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        }
        private void MinimizeOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void ClosedOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeAndNormalOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized);                
            
            MaximizeButton.Content = (WindowState == WindowState.Maximized ?
                "" : "" );
        }
        private void DragMoving(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (WindowState == WindowState.Maximized)
            {
                var point = e.GetPosition(this);
                Top = 0;
                Left = (Width > point.X ? point.X / 2 : Width / 2 + point.X);
                WindowState = WindowState.Normal;
                MaximizeButton.Content = "";
            }

            DragMove();
        }
        private void SetMenuWidth(object sender, DragDeltaEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[1].Width.Value < 205)
                MainGrid.ColumnDefinitions[1].Width = new GridLength(95);
        }
        
        private void ControlSidebar(object sender, RoutedEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[5].Width.Value > 0)
            {
                Split2.Visibility = Visibility.Collapsed;
                MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
                MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
            }
            else
            {
                Split2.Visibility = Visibility.Visible;
                MainGrid.ColumnDefinitions[5].Width = new GridLength(280);
                MainGrid.ColumnDefinitions[4].Width = new GridLength(10);
            }
        }

        private void CheckSidebarWidth(object sender, DragDeltaEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[5].Width.Value < 242)
            {
               Split2.Visibility = Visibility.Collapsed;
            }

            // Displayname.Text = MainGrid.ColumnDefinitions[5].Width.Value.ToString();

        }

        private void SetSidebarWidth(object sender, DragCompletedEventArgs e)
        {
            if (Split2.Visibility == Visibility.Collapsed)
            {
               MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
               MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
            }
        }

        private void DisableSidebarAndChangeTitle(object sender, RoutedEventArgs e)
        {
            Split2.Visibility = Visibility.Collapsed;
            MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
            MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
            
            this.Title = "GoonRunner - Trang chủ";
        }

        private void ChangeTitleNhanvien(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Nhân viên";
        }

        private void ChangeTitleDonhang(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Đơn hàng";
        }

        private void ChangeTitleSanpham(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Sản phẩm";
        }

        private void ChangeTitleKhachhang(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Khách Hàng";
        }

        private void ChangeTitleKhuyenmai(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Khuyến mại";
        }

        private void ChangeTitleBaohanh(object sender, RoutedEventArgs e)
        {
            this.Title = "GoonRunner - Bảo hành";
        }
    }
}