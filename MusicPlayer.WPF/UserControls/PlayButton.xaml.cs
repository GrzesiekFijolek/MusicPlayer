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
    /// Interaction logic for PlayButton.xaml
    /// </summary>
    public partial class PlayButton : UserControl
    {
        public static readonly DependencyProperty dependencyProperty =
            DependencyProperty.Register("IconId", typeof(int), typeof(PlayButton),
                new PropertyMetadata(new PropertyChangedCallback(SetIcon)));

        public PlayButton()
        {
            InitializeComponent();
        }

        public string IconId
        {
            get => (string)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private static void SetIcon(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as PlayButton).FontAwesome.Icon = (FontAwesome.WPF.FontAwesomeIcon)e.NewValue;
        }
    }
}
