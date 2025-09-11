using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoonRunner.Utils
{
    /// <summary>
    /// Converts a <see cref="bool"/> value to <see cref="Visibility"/> in inverted logic:
    /// true → Collapsed, false → Visible.
    /// Useful when you want to hide elements when a condition is true.
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to Visibility (inverted).
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>Visibility.Visible if false, Visibility.Collapsed if true.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && !boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts back from Visibility to boolean (inverted).
        /// </summary>
        /// <param name="value">The visibility value.</param>
        /// <param name="targetType">The target type (bool).</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>False if Visibility.Visible, True if Visibility.Collapsed.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Converts a <see cref="bool"/> value to <see cref="Visibility"/> directly:
    /// true → Visible, false → Collapsed.
    /// Useful for showing elements when a condition is true.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to Visibility.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>Visibility.Visible if true, Visibility.Collapsed if false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts back from Visibility to boolean.
        /// </summary>
        /// <param name="value">The visibility value.</param>
        /// <param name="targetType">The target type (bool).</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>True if Visibility.Visible, False if Visibility.Collapsed.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}
