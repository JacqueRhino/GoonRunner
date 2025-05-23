using System.Windows;
using System.Windows.Input;

namespace GoonRunner.MVVM.View
{
    /// <summary>
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : Window
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                var storyboard = Resources["SpinnerStoryboard"] as System.Windows.Media.Animation.Storyboard;
            };
        }

        private void MinimizedOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ClosedOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DragMoving(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
