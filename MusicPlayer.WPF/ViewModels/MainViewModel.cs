using FontAwesome.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicPlayer.WPF.ViewModels
{
    /// <summary>
    /// view model for main window
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region private fields

        private readonly int _pauseCircleIcon = 62091;
        private readonly int _playCircleIcon = 61764;


        private MediaPlayer _player = new MediaPlayer();

        private DispatcherTimer _timer;

        /// <summary>
        /// it is always equals to <see cref="MusicSliderValue"/> but is required for <see cref="SliderValueChanged(int)"/>
        /// in order to check if the user changed the slider position
        /// </summary>
        private int _musicSliderValueHelper;

        #endregion

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

        private bool _isPlayButtonEnabled;

        public bool IsPlayButtonEnabled
        {
            get { return _isPlayButtonEnabled; }
            set { 
                _isPlayButtonEnabled = value;
                OnPropertyChanged("IsPlayButtonEnabled");
            }
        }


        /// <summary>
        /// number of ticks in music slider which is equals to track duration 
        /// </summary>
        private int _musicSliderMaximum;

        public int MusicSliderMaximum
        {   
            get { return _musicSliderMaximum; }
            set { 
                    _musicSliderMaximum = value;
                    OnPropertyChanged("MusicSliderMaximum");
                }
        }

        private int _playIcon;

        public int PlayIcon
        {
            get { return _playIcon; }
            set 
            { 
                _playIcon = value;
                OnPropertyChanged("PlayIcon");
            }
        }



        /// <summary>
        /// the actual value in music slider which indicates music track length in seconds
        /// </summary>
        private int _musicSliderValue;

        public int MusicSliderValue
        {
            get { return _musicSliderValue; }
            set
            {
                _musicSliderValue = value;
                OnPropertyChanged("MusicSliderValue");
            }
        }

        /// <summary>
        /// the actual value in volume slider which indicates music track volume
        /// </summary>
        private int _actualVolume;

        public int ActualVolume
        {
            get { return _actualVolume; }
            set
            { 
                _actualVolume = value;
                OnPropertyChanged("ActualVolume");
            }
        }

        private string _musicTrackInfo;

        public string MusicTrackInfo
        {
            get { return _musicTrackInfo; }
            set 
            { 
                _musicTrackInfo = value;
                OnPropertyChanged("MusicTrackInfo");
            }
        }



        #endregion

        #region Commands

        public ICommand OpenFileCommand { get; set; }

        public ICommand PlayButtonCommand { get; set; }

        #endregion

        #region events

        public MainViewModel()
        {
            SetTimer();
            MusicSliderValue = 0;
            MusicSliderMaximum = 100;
            ActualVolume = 50;
            PlayIcon = _playCircleIcon;
            IsPlayButtonEnabled = false;
            PlayButtonCommand = new RelayCommand(PlayOrPause);
            OpenFileCommand = new RelayCommand(OpenFile);

            _player.MediaOpened += _player_MediaOpened;
        }

        private void _player_MediaOpened(object sender, EventArgs e)
        {
            MusicTrackInfo = Path.GetFileName(_player.Source.ToString());
            MusicSliderValue = 0;
            MusicSliderMaximum = (int)Double.Parse(_player.NaturalDuration.TimeSpan.TotalSeconds.ToString());
            IsPlayButtonEnabled = true;
        }


        /// <summary>
        /// called once every second for changing the thumb position of music slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            _musicSliderValueHelper = MusicSliderValue;
            MusicSliderValue++;
        }

        #endregion


        #region Methods

        /// <summary>
        /// It sets timer for manipulating music slider value
        /// </summary>
        private void SetTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

        }

        /// <summary>
        /// the method for <see cref="PlayButtonCommand"/> which starts to play a track music
        /// </summary>
        private void PlayOrPause()
        {
            if(_timer.IsEnabled)
            {
                _timer.Stop();
                PlayIcon = _playCircleIcon;
                _player.Pause();
            }
            else
            {
                _timer.Start();
                PlayIcon = _pauseCircleIcon;
                _player.Play();
            }
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _player.Open(new Uri(openFileDialog.FileName));
            }
            
        }

        /// <summary>
        /// called when the thumb position of volume slider was changed by the user
        /// </summary>
        /// <param name="newValue">the value of volume slider</param>
        public void VolumeSliderValueChanged(int newValue)
        {
            ///MediaPlayer.Volume has scale between 0 and 1
            _player.Volume = (double) newValue/100;
        }

        /// <summary>
        /// called when the thumb positon on music slider was changed by the user
        /// </summary>
        /// <param name="newPosition">a new position of thumb</param>
        public void MusicSliderValueChanged(int newPosition)
        {
            if(MusicSliderValue != _musicSliderValueHelper + 1)
            _player.Position = TimeSpan.FromSeconds(newPosition);
        }

        /// <summary>
        /// called when the file was dragged and dropped on window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void File_Dropped(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            _player.Open(new Uri(files[0]));
        }

        #endregion
    }
}
