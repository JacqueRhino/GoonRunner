using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GoonRunner.Utils
{
    internal class Extensions
    {
        public static readonly DependencyProperty Header =
            DependencyProperty.RegisterAttached("Header", typeof(string), typeof(Extensions),
                new PropertyMetadata(default(string)));

        public static void SetHeader(UIElement element, string value)
        {
            element.SetValue(Header, value);
        }


        public static string GetHeader(UIElement element)
        {
            return (string)element.GetValue(Header);
        }

        public static readonly DependencyProperty Icon =
            DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(Extensions),
                new PropertyMetadata(default(string)));

        public static void SetIcon(UIElement icon, string value)
        {
            icon.SetValue(Icon, value);
        }

        public static string GetIcon(UIElement icon)
        {
            return (string)icon.GetValue(Icon);
        }


        // Tạo ra một DateTemplate tại vì sử dụng nó trong style ko được
        // yes, t biết nó lỏ nhưng chịu    
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.RegisterAttached(
                "Binding",
                typeof(string),
                typeof(Extensions),
                new PropertyMetadata(null, OnBindingChanged));

        public static void SetBinding(DependencyObject element, string value)
            => element.SetValue(BindingProperty, value);

        public static string GetBinding(DependencyObject element)
            => (string)element.GetValue(BindingProperty);

        private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewColumn column && e.NewValue is string bindingPath)
            {
                var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(bindingPath));
                textBlockFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                textBlockFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

                // Optional: Apply style
                textBlockFactory.SetResourceReference(FrameworkElement.StyleProperty, "CenteredGridViewCellStyle");

                var template = new DataTemplate { VisualTree = textBlockFactory };
                column.CellTemplate = template;
            }
        }
    }
}