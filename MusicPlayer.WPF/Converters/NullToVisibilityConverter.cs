using MusicPlayer.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;

namespace MusicPlayer.WPF.Converters
{
    class NullToVisibilityConverter : BaseValueConverter<NullToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var files = (ObservableCollection<FileInformation>)value;

            if (files == null) return Visibility.Visible;
            else if (files.Count == 0) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
