﻿using Microsoft.Win32;
using MusicPlayer.WPF.Models;
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

            this.DataContext = new MainViewModel(this);
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
            TrackList.ItemsSource = _vm.Files;
        }


        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = sender as ListViewItem;
            _vm.Play(x.Content as FileInformation);
        }

        private void TrackListLostFocus(object sender, MouseButtonEventArgs e)
        {
            TrackList.UnselectAll();
        }

        private void ListViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var x = sender as ListViewItem;
                _vm.Play(x.Content as FileInformation);
            }
            else if(e.Key == Key.Delete)
            {
                var x = sender as ListViewItem;
                _vm.DeleteSelectedTrack(x.Content as FileInformation);
            }


        }

        public void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
