using System.Windows;
using System.Windows.Input;

namespace GoonRunner.MVVM.View
{
    public partial class LogInView 
    {
        public LogInView()
        {
            InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                var storyboard = Resources["SpinnerStoryboard"] as System.Windows.Media.Animation.Storyboard;
            };
        }
        private void DragMoving(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ClosedOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizedOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private bool AuthenticateSuccessful()
        {
            return true;
        }

    }
}