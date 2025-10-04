using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using GoonRunner.MVVM.ViewModel;

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
            if (!(MainGrid.ColumnDefinitions[5].Width.Value < 242)) return;
            Split2.Visibility = Visibility.Collapsed;
            SidebarButton.IsChecked = false;
        }

        private void SetSidebarWidth(object sender, DragCompletedEventArgs e)
        {
            if (Split2.Visibility != Visibility.Collapsed) return;
            MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
            MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
        }
        private void CollapseSidebar()
        {
            Split2.Visibility = Visibility.Collapsed;
            MainGrid.ColumnDefinitions[5].Width = new GridLength(0);
            MainGrid.ColumnDefinitions[4].Width = new GridLength(0);
            SidebarButton.IsChecked = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            const double desktopBreakpoint = 1024;
            const double tabletBreakpoint = 768;
            var width = e.NewSize.Width;
            if (width < desktopBreakpoint)
            {
                Split1.Visibility = Visibility.Visible;
                MainGrid.ColumnDefinitions[1].MaxWidth = 95;
            }
            else if (MainGrid.ColumnDefinitions[1].Width.Value >= 95)
            {
                MainGrid.ColumnDefinitions[1].MaxWidth = 250;
            }
        }

        private void Split2_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            {
                if (!(DataContext is MainViewModel vm)) return;
                if (!vm.IsSplit2Enabled)
                    e.Handled = true; // blocks dragging
            }
        }

        private void Split2_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!(DataContext is MainViewModel vm)) return;

            if (!vm.IsSplit2Enabled)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                e.Handled = true;
            }
            else
            {
            }
        }

        private void Split2_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            
        }
    }
}