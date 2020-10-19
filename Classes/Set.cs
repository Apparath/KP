using Microsoft.Win32;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KP.Classes
{
    class Set
    {
        /// <summary>
        /// Метод передачи файла изображения в форму изображения
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static BitmapImage ImageFromFile(out FileInfo file)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Изображения (*.jpg, *.png, *.gif)|*.jpg; *.png; *.gif";

            file = null;
            BitmapImage bitmap = new BitmapImage();

            if (dialog.ShowDialog() == true)
            {
                file = new FileInfo(dialog.FileName);
            }

            if (file != null)
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(file.FullName);
                bitmap.EndInit();
            }
            else
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/KP;component/Resources/addImage.png", UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
            }
            return bitmap;
        }

        public static FileInfo TrackFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Музыка (*.mp3, *.wav, *.flac)|*.mp3; *.wav; *.flac";

            FileInfo file = null;
            BitmapImage bitmap = new BitmapImage();

            if (dialog.ShowDialog() == true)
            {
                file = new FileInfo(dialog.FileName);
            }

            return file;
        }
    }
}
