using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MusicPlayer.WPF.ViewModels
{
    /// <summary>
    /// a base view model which extends <see cref="INotifyPropertyChanged"/>
    /// </summary>
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// it fires <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
