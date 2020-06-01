using MusicPlayer.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TagLib;

namespace MusicPlayer.WPF.Models
{
    /// <summary>
    /// provides information taken from <see cref="TagLib.Tag"/> properties
    /// </summary>
    public class FileInformation
    {
        /// <summary>
        /// the file created from <see cref="FileUri"/>
        /// </summary>
        public TagLib.File File { get; set; }
        /// <summary>
        /// the file location
        /// </summary>
        public string FileUri { get; set; }

        /// <summary>
        /// the artist from track tag
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// the album form track tag
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// the title from track tag
        /// </summary>
        public string Title { get; set; }

        private string _fileName;

        /// <summary>
        /// the file name 
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        /// <summary>
        /// the track cover from tag
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// flag which indicates if the file path is correct and the file has audio
        /// </summary>
        public bool IsCorrect { get; set; }


        private FileInformation(string fileUri)
        {
            IsCorrect = true;

            try
            {
                var x = Path.GetExtension(fileUri);

                if (x != ".mp3")
                {
                    IsCorrect = false;
                    return;
                }
                
                SetProperties(fileUri);
            }
            catch(Exception )
            {
                IsCorrect = false;
                return;
            }
        }

        /// <summary>
        /// Set track tags if file path is valid
        /// </summary>
        private void SetProperties(string uri)
        {
            File = TagLib.File.Create(uri);
            FileUri = uri;
            if (File.Tag.Performers.Length > 0)
                Artist = File.Tag.Performers[0] ?? null;
            else 
                Artist = null;
            Album = File.Tag.Album ?? null;
            Title = File.Tag.Title ?? null;

            FileName = Path.GetFileName(FileUri); 

            if (File.Tag.Pictures == null || File.Tag.Pictures.Length == 0)
            {
                Image = null;
            }
            else
            {
                Image = SetTrackImage();
            }
        }

        /// <summary>
        /// converts picture from <see cref="IPicture"/> to <see cref="ImageSource"/>
        /// </summary>
        private BitmapSource SetTrackImage()
        {
            IPicture picture = File.Tag.Pictures[0];
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

            return bitmapSource;
        }

        /// <summary>
        /// gets object instance only if file uri is valid
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static FileInformation GetInstance(string uri)
        {
            var x = new FileInformation(uri);

            if (x.IsCorrect)
                return x;
            else
                return null;

        }

        /// <summary>
        /// creates collection of tracks informations from dropped or selected files
        /// </summary>
        /// <param name="files">selected files paths</param>
        /// <returns></returns>
        public static ObservableCollection<FileInformation> CreateFilesList(string[] files)
        {
            var x = new ObservableCollection<FileInformation>();
            for(int i=0; i<files.Length; i++)
            {
                FileInformation file = GetInstance(files[i]);

                if (file != null) x.Add(file);
            }

            return x;
        }

        /// <summary>
        /// creates collection of tracks informations from database
        /// </summary>
        /// <param name="lastOpeneds">files paths from database</param>
        /// <returns></returns>
        public static ObservableCollection<FileInformation> CreateFilesList(List<LastOpenedModel> lastOpeneds)
        {
            var x = new ObservableCollection<FileInformation>();

            foreach(var item in lastOpeneds)
            {
                FileInformation file = GetInstance(item.FilePath);

                if (file != null) x.Add(file);
            }

            return x;
        }

        
    }
}
