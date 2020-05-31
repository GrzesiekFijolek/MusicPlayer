using FontAwesome.WPF;
using Microsoft.Win32;
using MusicPlayer.Library;
using MusicPlayer.WPF.Models;
using MusicPlayer.WPF.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TagLib;

namespace MusicPlayer.WPF.ViewModels
{
    /// <summary>
    /// view model for main window
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region private fields

        /// <summary>
        /// both are required for togle stop/play button
        /// the numbers refer to <see cref="FontAwesomeIcon"/> values
        /// </summary>
        private readonly int _pauseCircleIcon = 62091;
        private readonly int _playCircleIcon = 61764;

        /// <summary>
        /// needed for window state management
        /// </summary>
        private Window _window;

        private FileInformation _actualPlayedFile;

        /// <summary>
        /// it causes to play music
        /// </summary>
        private MediaPlayer _player = new MediaPlayer();

        /// <summary>
        /// required for changing music slider values
        /// </summary>
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
        public int WindowWith { get; set; } = 800;


        /// <summary>
        /// startup window heigh
        /// </summary>
        public int WindowHeigh { get; set; } = 500;

        /// <summary>
        /// an application name
        /// </summary>
        public string AppName { get; set; } = "MP3 Music Player";


        private string _musicTrackDuration;
        /// <summary>
        /// the track duration in mm:ss
        /// </summary>
        public string MusicTrackDuration
        {
            get { return _musicTrackDuration; }
            set
            {
                _musicTrackDuration = value;
                OnPropertyChanged(nameof(MusicTrackDuration));
            }
        }


        private string _actualMusicTrackPosition;
        /// <summary>
        /// the actual track position in mm:ss
        /// </summary>
        public string ActualMusicTrackPosition
        {
            get { return _actualMusicTrackPosition; }
            set
            {
                _actualMusicTrackPosition = value;
                OnPropertyChanged(nameof(ActualMusicTrackPosition));
            }
        }

        private bool _isPreviousButtonEnabled;

        public bool IsPreviousButtomEnabled
        {
            get { return _isPreviousButtonEnabled; }
            set
            {
                _isPreviousButtonEnabled = value;
                OnPropertyChanged(nameof(IsPreviousButtomEnabled));
            }
        }

        private bool _isNextButtonEnabled;

        public bool IsNexButtonEnabled
        {
            get { return _isNextButtonEnabled; }
            set
            {
                _isNextButtonEnabled = value;
                OnPropertyChanged(nameof(IsNexButtonEnabled));
            }
        }




        private bool _isMusicTrackLoaded;
        /// <summary>
        /// indicates if some buttons are enabled
        /// </summary>
        public bool IsMusicTrackLoaded
        {
            get { return _isMusicTrackLoaded; }
            set
            {
                _isMusicTrackLoaded = value;
                OnPropertyChanged(nameof(IsMusicTrackLoaded));
            }
        }


        private int _musicSliderMaximum;
        /// <summary>
        /// number of ticks in music slider which is equals to track duration 
        /// </summary>
        public int MusicSliderMaximum
        {
            get { return _musicSliderMaximum; }
            set
            {
                _musicSliderMaximum = value;
                OnPropertyChanged(nameof(MusicSliderMaximum));
            }
        }



        private int _playIcon;
        /// <summary>
        /// the icon of button for stop and play functions
        /// </summary>
        public int PlayIcon
        {
            get { return _playIcon; }
            set
            {
                _playIcon = value;
                OnPropertyChanged(nameof(PlayIcon));
            }
        }



        private int _musicSliderValue;
        /// <summary>
        /// the actual value in music slider which indicates music track length in seconds
        /// </summary>
        public int MusicSliderValue
        {
            get { return _musicSliderValue; }
            set
            {
                _musicSliderValue = value;
                OnPropertyChanged(nameof(MusicSliderValue));
            }
        }


        private int _actualVolume;
        /// <summary>
        /// the actual value in volume slider which indicates music track volume
        /// </summary>
        public int ActualVolume
        {
            get { return _actualVolume; }
            set
            {
                _actualVolume = value;
                OnPropertyChanged(nameof(ActualVolume));
            }
        }

        private string _musicTrackInfo;

        public string MusicTrackInfo
        {
            get { return _musicTrackInfo; }
            set
            {
                _musicTrackInfo = value;
                OnPropertyChanged(nameof(MusicTrackInfo));
            }
        }

        private string _trackAlbum;
        /// <summary>
        /// the album from <see cref="TagLib.Tag"/>
        /// </summary>
        public string TrackAlbum
        {
            get { return _trackAlbum; }
            set
            {
                _trackAlbum = value;
                OnPropertyChanged(nameof(TrackAlbum));
            }
        }

        private string _trackTitle;
        /// <summary>
        /// the music track title from <see cref="TagLib.Tag"/>
        /// </summary>
        public string TrackTitle
        {
            get { return _trackTitle; }
            set
            {
                _trackTitle = value;
                OnPropertyChanged(nameof(TrackTitle));
            }
        }

        private string _trackArtist;
        /// <summary>
        /// the track artist from <see cref="TagLib.Tag"/>
        /// </summary>
        public string TrackArtist
        {
            get { return _trackArtist; }
            set
            {
                _trackArtist = value;
                OnPropertyChanged(nameof(TrackArtist));
            }
        }

        private ImageSource _trackImage;

        /// <summary>
        /// the track cover from <see cref="TagLib.Tag.Pictures"/>
        /// </summary>
        public ImageSource TrackImage
        {
            get { return _trackImage; }
            set
            {
                _trackImage = value;
                OnPropertyChanged(nameof(TrackImage));
            }
        }

        private ObservableCollection<FileInformation> _files;
        /// <summary>
        /// the collection contains infomration needed for displaying track list 
        /// </summary>
        public ObservableCollection<FileInformation> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                //while (value == null || value.Count < 10)
                //    _files.Add(null);

                OnPropertyChanged(nameof(Files));
            }
        }


        #endregion

        #region Commands

        public ICommand OpenFileCommand { get; set; }

        public ICommand PlayButtonCommand { get; set; }

        public ICommand MinimalizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public ICommand PlayPreviousCommand { get; set; }

        public ICommand StopCommand { get; set; }

        #endregion

        public MainViewModel(Window window)
        {
            _window = window;

            SetTimer();
            MusicSliderValue = 0;
            MusicSliderMaximum = 100;
            ActualVolume = 50;
            PlayIcon = _playCircleIcon;
            IsMusicTrackLoaded = false;

            PlayButtonCommand = new RelayCommand(PlayOrPause);
            OpenFileCommand = new RelayCommand(OpenFile);
            MinimalizeCommand = new RelayCommand(Minimalize);
            CloseCommand = new RelayCommand(Close);
            PlayNextCommand = new RelayCommand(NextTrack, NextTrackEvaluator);
            PlayPreviousCommand = new RelayCommand(PreviousTrack, PreviousTrackEvaluator);
            StopCommand = new RelayCommand(StopPlayback);

            _player.MediaOpened += _player_MediaOpened;

            OpenLastTrackList();

        }

        private void Minimalize()
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void Close()
        {
            SaveTrackList();
            _window.Close();
        }

        private void StopPlayback()
        {
            _player.Close();
            _actualPlayedFile = null;
            IsMusicTrackLoaded = false;
            MusicTrackDuration = null;
            ActualMusicTrackPosition = null;
            TrackImage = null;
            MusicTrackInfo = null;
            _timer.Stop();
            MusicSliderValue = 0;
            TrackTitle = null;
            TrackAlbum = null;
            TrackArtist = null;
        }

        #region private methods

        private void _player_MediaOpened(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MusicTrackInfo = Path.GetFileName(_player.Source.ToString());
                MusicSliderValue = 0;
                MusicSliderMaximum = (int)Double.Parse(_player.NaturalDuration.TimeSpan.TotalSeconds.ToString());
                IsMusicTrackLoaded = true;
                ActualMusicTrackPosition = "0:00";
                MusicTrackDuration = _player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                SetTrackInfoFromTags();
            });
        }

        private void NextTrack()
        {
            int index = Files.IndexOf(_actualPlayedFile);

            _actualPlayedFile = Files.ElementAt(index + 1);
            _player.Open(new Uri(_actualPlayedFile.FileUri));
            _player.Play();
            _timer.Start();
            PlayIcon = _pauseCircleIcon;
            SetNextAndPreviousButtons();
        }

        private bool NextTrackEvaluator()
        {
            int index = Files.IndexOf(_actualPlayedFile);

            if (Files.Count <= 1) return false;
            else if (Files.Count == index + 1) return false;
            else return true;
        }

        private void PreviousTrack()
        {
            int index = Files.IndexOf(_actualPlayedFile);

            _actualPlayedFile = Files.ElementAt(index - 1);
            _player.Open(new Uri(_actualPlayedFile.FileUri));
            _player.Play();
            _timer.Start();
            PlayIcon = _pauseCircleIcon;

            SetNextAndPreviousButtons();
        }

        private bool PreviousTrackEvaluator()
        {
            int index = Files.IndexOf(_actualPlayedFile);

            if (Files.Count <= 1) return false;
            if (index == 0) return false;
            else return true;
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
            ActualMusicTrackPosition = _player.Position.ToString(@"mm\:ss");
        }

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
            if (_timer.IsEnabled)
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
                string path = openFileDialog.FileName;
                string[] s = { path };

                //Files = FileInformation.CreateFilesList(s);
                OpenLastTrackList();
            }
        }

        /// <summary>
        /// converts picture from <see cref="IPicture"/> to <see cref="System.Windows.Controls.Image"/>
        /// </summary>
        /// <param name="pictures"></param>
        private void SetTrackImage(IPicture[] pictures)
        {
            IPicture picture = pictures[0];
            MemoryStream memoryStream = new MemoryStream(picture.Data.Data);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var image = System.Drawing.Image.FromStream(memoryStream);
            var bitmap = new Bitmap(image);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();

            TrackImage = bitmapSource;
        }

        /// <summary>
        /// sets actual track information from its tags
        /// </summary>
        /// <param name="file"></param>
        private void SetTrackInfoFromTags()
        {
            TrackArtist = _actualPlayedFile.File.Tag.Performers[0] ?? null;
            TrackAlbum = _actualPlayedFile.File.Tag.Album ?? null;
            TrackTitle = _actualPlayedFile.File.Tag.Title ?? null;

            SetTrackImage(_actualPlayedFile.File.Tag.Pictures);
        }

        /// <summary>
        /// saves actual tracck list to the database
        /// </summary>
        private void SaveTrackList()
        {
            var trackList = new List<LastOpenedModel>();

            foreach(var item in Files)
            {
                trackList.Add(new LastOpenedModel() { FilePath = item.FileUri });
            }

            SQLiteDataAccess.SaveLastTrackList(trackList);
        }

        private void OpenLastTrackList()
        {

            var filePaths = SQLiteDataAccess.LoadLastTrackList();

            Files = FileInformation.CreateFilesList(filePaths);
        }

        private void SetNextAndPreviousButtons()
        {
            int index = Files.IndexOf(_actualPlayedFile);

            if(Files.Count == 1 || Files == null)
            {
                IsPreviousButtomEnabled = false;
                IsNexButtonEnabled = false;
                return;
            }

            if (index == 0) IsPreviousButtomEnabled = false;
            else IsPreviousButtomEnabled= true;

            if (index == Files.Count - 1) IsNexButtonEnabled = false;
            else IsNexButtonEnabled = true;
        }
        #endregion


        #region public methods

        public void DeleteSelectedTrack(FileInformation file)
        {
            Files.Remove(file);
            OnPropertyChanged(nameof(Files));
        }


        /// <summary>
        /// called when the thumb position of volume slider was changed by the user
        /// </summary>
        /// <param name="newValue">the value of volume slider</param>
        public void VolumeSliderValueChanged(int newValue)
        {
            ///MediaPlayer.Volume has scale between 0 and 1
            _player.Volume = (double)newValue / 100;
        }

        /// <summary>
        /// called when the thumb positon on music slider was changed by the user
        /// </summary>
        /// <param name="newPosition">a new position of thumb</param>
        public void MusicSliderValueChanged(int newPosition)
        {
            if (MusicSliderValue != _musicSliderValueHelper + 1)
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
            Files = FileInformation.CreateFilesList(files);
            
        }

        /// <summary>
        /// plays track selected from track list by user
        /// </summary>
        /// <param name="file"></param>
        public void Play(FileInformation file)
        {
            _actualPlayedFile = file;
            _player.Open(new Uri(_actualPlayedFile.FileUri));
            _player.Play();
            _timer.Start();
            PlayIcon = _pauseCircleIcon;
            MusicSliderValue = 0;
            SetNextAndPreviousButtons();
        }

        #endregion
    }
}
