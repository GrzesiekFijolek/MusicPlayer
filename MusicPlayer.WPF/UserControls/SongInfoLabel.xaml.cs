using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for BorderWithInnerShadow.xaml
    /// </summary>
    public partial class SongInfoLabel : UserControl
    {

        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(SongInfoLabel),
                new PropertyMetadata(new PropertyChangedCallback(SetLabelText)));
        public SongInfoLabel()
        {
            InitializeComponent();
            SetLabelHeight();
        }

        public string LabelContent
        {
            get => (string)GetValue(LabelContentProperty);
            set => SetValue(LabelContentProperty, value);
        }

        private static void SetLabelText(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as SongInfoLabel).InnerLabel.Content = e.NewValue.ToString();
        }

        private void SetLabelHeight()
        {
            double songInfoFontSize = (double)Application.Current.Resources["SongInfoFontSize"];
            Thickness songInfoMargin = (Thickness)Application.Current.Resources["SongInfoMargin"];

            this.Height = songInfoFontSize + songInfoMargin.Top + songInfoMargin.Bottom + 16;
        }
    }
}
