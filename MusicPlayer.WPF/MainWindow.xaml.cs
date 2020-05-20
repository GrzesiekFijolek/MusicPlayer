using Microsoft.Win32;
using MusicPlayer.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
            _vm = (MainViewModel)DataContext;

            TrackList.ItemsSource = _vm.Files;
        }

        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            _vm.MusicSliderValueChanged((int)slider.Value);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            _vm.VolumeSliderValueChanged((int)slider.Value);
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            _vm.File_Dropped(sender, e);
            
        }

        private void PlayButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
