using System.Windows;
using System.Windows.Controls;

namespace GoonRunner.CustomControl
{
    public partial class Textbox : UserControl
    {
        public Textbox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SetHeaderProperty = DependencyProperty.Register(
            nameof(SetHeader), typeof(string), typeof(Textbox), new PropertyMetadata(null, OnSetHeaderPropertyChanged));

        public string SetHeader
        {
            get => (string)GetValue(SetHeaderProperty);
            set => SetValue(SetHeaderProperty, value);
        }

        private static void OnSetHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Textbox textbox) textbox.OnSetHeaderPropertyChanged(e);
        }

        private void OnSetHeaderPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            GroupBox.Header = e.NewValue as string;
        }


        // placeholder
        public static readonly DependencyProperty SetPlaceholderProperty = DependencyProperty.Register(
            nameof(SetPlaceholder), typeof(string), typeof(Textbox), new PropertyMetadata(null, OnSetPlaceholderPropertyChanged));

        public string SetPlaceholder
        {
            get => (string)GetValue(SetPlaceholderProperty);
            set => SetValue(SetPlaceholderProperty, value);
        }

        private static void OnSetPlaceholderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Textbox textbox) textbox.OnSetPlaceholderPropertyChanged(e);
        }

        private void OnSetPlaceholderPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Placeholder.Text = e.NewValue as string;
        }

        // binding
        //public static readonly DependencyProperty SetBindingProperty = DependencyProperty.Register(
        //    nameof(SetBinding), typeof(string), typeof(Textbox), new PropertyMetadata(null, OnSetBindingPropertyChanged));

        //public new string SetBinding
        //{
        //    get => (string)GetValue(SetBindingProperty);
        //    set => SetValue(SetBindingProperty, value);
        //}

        //private static void OnSetBindingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is Textbox textbox) textbox.OnSetBindingPropertyChanged(e);
        //}

        //private void OnSetBindingPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    Inputbox.Text = e.NewValue as string ?? string.Empty;
        //}
        public static readonly DependencyProperty SetBindingProperty =
            DependencyProperty.Register("SetBinding", typeof(object), typeof(Textbox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SetBinding
        {
            get { return GetValue(SetBindingProperty); }
            set { SetValue(SetBindingProperty, value); }
        }
    }
}