using System.Windows;
using System.Windows.Controls;

namespace GoonRunner.MVVM.View
{
    public partial class SidebarKhachHangView : UserControl
    {
        public SidebarKhachHangView()
        {
            InitializeComponent();
        }

        private void Test(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }
    }
}