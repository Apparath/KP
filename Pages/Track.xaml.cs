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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Track.xaml
    /// </summary>
    public partial class Track : Page
    {
        public Track()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    Classes.Get.Countries(connection, country);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        void NewEx(SqlConnection connection)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Executors VALUES (@executor, (SELECT Id FROM Countries WHERE Name = @country), @found, @image)";

                command.Parameters.AddWithValue("@executor", executor.Text);

                if (country.SelectedIndex != -1) command.Parameters.AddWithValue("@country", country.SelectedItem);
                else command.Parameters.AddWithValue("@country", DBNull.Value);

                if (found.Text != "") command.Parameters.AddWithValue("@found", found.Text);
                else command.Parameters.AddWithValue("@found", DBNull.Value);

                if (executorF != null)
                {
                    using (SqlCommand command1 = connection.CreateCommand())
                    {
                        command1.CommandText = "EXEC FindImage @binary";

                        command1.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(executorF.FullName));

                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            if (reader1.HasRows)
                            {
                                while (reader1.Read()) command.Parameters.AddWithValue("@image", reader1[0]);
                            }
                            else
                            {
                                reader1.Close();

                                Classes.Add.NewImage(connection, executorF, command);
                            }
                        }
                    }
                }
                else command.Parameters.AddWithValue("@image", DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        void NewAlb(SqlConnection connection)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Albums(Name, Date, Image) VALUES (@album, @release, @image)";

                command.Parameters.AddWithValue("@album", album.Text);

                if (release.Text != "") command.Parameters.AddWithValue("@release", release.Text);
                else command.Parameters.AddWithValue("@release", DBNull.Value);

                if (albumF != null)
                {
                    using (SqlCommand command1 = connection.CreateCommand())
                    {
                        command1.CommandText = "EXEC FindImage @binary";

                        command1.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(albumF.FullName));

                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            if (reader1.HasRows)
                            {
                                while (reader1.Read()) command.Parameters.AddWithValue("@image", reader1[0]);
                            }
                            else
                            {
                                reader1.Close();

                                Classes.Add.NewImage(connection, albumF, command);
                            }
                        }
                    }
                }
                else command.Parameters.AddWithValue("@image", DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        private FileInfo trackF = null;
        private FileInfo albumF = null;
        private FileInfo executorF = null;

        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void exImg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            exImg.Source = Classes.Set.ImageFromFile(out executorF);
        }

        private void albumImg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            albumImg.Source = Classes.Set.ImageFromFile(out albumF);
        }

        private void addTrackF_Click(object sender, RoutedEventArgs e)
        {
            trackF = Classes.Set.TrackFile();
        }

        private void addTrack_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    NewEx(connection);
                    NewAlb(connection);

                    using (SqlCommand command6 = connection.CreateCommand())
                    {
                        command6.CommandText = "INSERT INTO Tracks(Name, Binary, Image) VALUES (@track, CAST(@binary AS varbinary(MAX)), NULL)";

                        command6.Parameters.AddWithValue("@track", track.Text);

                        if (trackF != null)
                        {
                            command6.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(trackF.FullName));
                        }
                        else command6.Parameters.AddWithValue("@binary", DBNull.Value);

                        command6.ExecuteNonQuery();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Records(Track, Album, Executor) VALUES ((SELECT Id FROM Tracks WHERE Name = @track AND Binary = CAST(@binary AS varbinary(MAX)))," +
                            " (SELECT Albums.Id FROM Albums WHERE Albums.Name = @album AND Image = @albumimg), " +
                            "(SELECT Executers.Id FROM Executors WHERE Executors.Name = @executor AND Image = @eximg)";

                        command.Parameters.AddWithValue("@track", track.Text);
                        command.Parameters.AddWithValue("@album", album.Text);
                        command.Parameters.AddWithValue("@executor", executor.Text);

                        if (trackF != null)
                        {
                            command.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(trackF.FullName));
                        }
                        else command.Parameters.AddWithValue("@binary", DBNull.Value);

                        if (albumF != null)
                        {
                            using (SqlCommand command1 = connection.CreateCommand())
                            {
                                command1.CommandText = "EXEC FindImage @binary";

                                command1.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(albumF.FullName));

                                using (SqlDataReader reader1 = command1.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read()) command.Parameters.AddWithValue("@albumimg", reader1[0]);
                                    }
                                    else
                                    {
                                        reader1.Close();

                                        Classes.Add.NewImage(connection, albumF, command);
                                    }
                                }
                            }
                        }
                        else command.Parameters.AddWithValue("@albumimg", DBNull.Value);

                        if (executorF != null)
                        {
                            using (SqlCommand command1 = connection.CreateCommand())
                            {
                                command1.CommandText = "EXEC FindImage @binary";

                                command1.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(executorF.FullName));

                                using (SqlDataReader reader1 = command1.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read()) command.Parameters.AddWithValue("@eximg", reader1[0]);
                                    }
                                    else
                                    {
                                        reader1.Close();

                                        Classes.Add.NewImage(connection, executorF, command);
                                    }
                                }
                            }
                        }
                        else command.Parameters.AddWithValue("@eximg", DBNull.Value);

                        command.ExecuteNonQuery();
                    }


                    MessageBox.Show("Трек успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.GoBack();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
