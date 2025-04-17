using GoonRunner.MVVM.ViewModel;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GoonRunner.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        }
        private void ClosedOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized);                
        }

        private void MinimizedOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DragMoving(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (WindowState == WindowState.Maximized)
            {
                var point = e.GetPosition(this);
                Top = 0;
                Left = (Width > point.X ?  point.X/2 : Width/2 + point.X );
                WindowState = WindowState.Normal;
            }

            DragMove();
        }
        
        private Point GetMousePosition(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            return new Point(point.X, point.Y);
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
                MainGrid.ColumnDefinitions[5].Width = new GridLength(250);
                MainGrid.ColumnDefinitions[4].Width = new GridLength(10);
            }
        }

        private void CheckSidebarWidth(object sender, DragDeltaEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[5].Width.Value < 150)
            {
               Split2.Visibility = Visibility.Collapsed;
            }
        }

        private void SetSidebarWidth(object sender, DragCompletedEventArgs e)
        {
            if (Split2.Visibility == Visibility.Collapsed)
            {
               MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
               MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
            }
        }

        private void DisableSidebar(object sender, RoutedEventArgs e)
        {
            Split2.Visibility = Visibility.Collapsed;
            MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
            MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
        }
    }
}