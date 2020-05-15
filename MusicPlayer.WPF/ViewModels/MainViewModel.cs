using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MusicPlayer.WPF.ViewModels
{
    /// <summary>
    /// view model for main window
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region properties

        /// <summary>
        /// startup window width
        /// </summary>
        public int WindowWith { get; set; } = 500;
       
        /// <summary>
        /// startup window heigh
        /// </summary>
        public int WindowHeigh { get; set; } = 500;
       
        /// <summary>
        /// an application name
        /// </summary>
        public string AppName { get; set; } = "MP3 Music Player";

        #endregion

    }
}
