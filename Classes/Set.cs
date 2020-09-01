using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KP.Classes
{
    class Set
    {
        public static BitmapImage ImageFromFile(out FileInfo file)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Изображения (*.jpg, *.png, *.gif)|*.jpg; *.png; *.gif";

            file = null;
            BitmapImage bitmap = new BitmapImage();

            if (dialog.ShowDialog() == true)
            {
                file = new FileInfo(dialog.FileName);

                if (file != null)
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(file.FullName);
                    bitmap.EndInit();
                }
            }
            return bitmap;
        }

        public static BitmapImage ImageFromDB(ComboBox comboBox)
        {
            BitmapImage bitmap = new BitmapImage();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Binary FROM Images WHERE Name = @name";

                        command.Parameters.AddWithValue("@name", comboBox.SelectedItem);

                        if (comboBox.SelectedItem.ToString().Contains("Не выбрано"))
                        {
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri("/Audiophiles;component/Resources/addImage.png", UriKind.RelativeOrAbsolute);
                            bitmap.EndInit();
                        }
                        else
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        byte[] photo = (byte[])reader[0];

                                        bitmap.BeginInit();
                                        bitmap.StreamSource = new MemoryStream(photo);
                                        bitmap.EndInit();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }

            return bitmap;
        }
    }
}
