using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Roles.Name FROM Roles INNER JOIN Users ON Role = Id WHERE Login = @login";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[0].ToString() == "Обычный пользователь")
                                {
                                    add.Visibility = Visibility.Hidden;
                                    edit.Visibility = Visibility.Hidden;
                                    delete.Visibility = Visibility.Hidden;
                                    edit1.Visibility = Visibility.Hidden;
                                    delete1.Visibility = Visibility.Hidden;
                                    edit2.Visibility = Visibility.Hidden;
                                    delete2.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                    }
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
            Classes.Get.TableOutput(tracksGrid, null, 0);
            Classes.Get.TableOutput(executorsGrid, null, 1);
            Classes.Get.TableOutput(albumsGrid, null, 10);

            edit.Click += (s, e) =>
            {
                
            };

            edit1.Click += (s, e) =>
            {
                Windows.Executor executor = new Windows.Executor(true, executorsGrid);
                executor.ShowDialog();
            };

            like.Click += (s, e) =>
            {
                if (tracksGrid.SelectedItem != null)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "INSERT INTO Likes VALUES ((SELECT Login FROM Users WHERE Login = @login), (SELECT Id FROM Tracks WHERE Tracks.Name = @track))";

                                command.Parameters.AddWithValue("@login", Classes.Login.Value);
                                command.Parameters.AddWithValue("@track", ((DataRowView)tracksGrid.SelectedItem)[0]);
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }
                        finally
                        {
                            searchBox.Text = "Поиск";
                            connection.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Добавить в избранное можно только выделенную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            };

            delete1.Click += (s, e) =>
            {
                if (executorsGrid.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                        {
                            try
                            {
                                connection.Open();

                                using (SqlCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = "DELETE FROM Records WHERE Records.Executor = (SELECT Id FROM Executors WHERE Executors.Name = @executor)";

                                    command.Parameters.AddWithValue("@executor", ((DataRowView)executorsGrid.SelectedItem)[0]);

                                    Classes.Get.FillGrid(command, tracksGrid);
                                    Classes.Get.FillGrid(command, executorsGrid);
                                    Classes.Get.FillGrid(command, albumsGrid);
                                }
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            finally
                            {
                                Classes.Get.TableOutput(tracksGrid, null, 0);
                                Classes.Get.TableOutput(executorsGrid, null, 1);
                                Classes.Get.TableOutput(albumsGrid, null, 10);
                                searchBox.Text = "Поиск";
                                connection.Close();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Удалить можно только существующую запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            };

            delete2.Click += (s, e) =>
            {
                if (albumsGrid.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                        {
                            try
                            {
                                connection.Open();

                                using (SqlCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = "DELETE FROM Records WHERE Records.Album = (SELECT Id FROM Albums WHERE Albums.Name = @album)";

                                    command.Parameters.AddWithValue("@album", ((DataRowView)executorsGrid.SelectedItem)[0]);

                                    Classes.Get.FillGrid(command, tracksGrid);
                                    Classes.Get.FillGrid(command, executorsGrid);
                                    Classes.Get.FillGrid(command, albumsGrid);
                                }
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            finally
                            {
                                Classes.Get.TableOutput(tracksGrid, null, 0);
                                Classes.Get.TableOutput(executorsGrid, null, 1);
                                Classes.Get.TableOutput(albumsGrid, null, 10);
                                searchBox.Text = "Поиск";
                                connection.Close();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Удалить можно только существующую запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            };
        }

        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(tracksGrid, null, 0);
            Classes.Get.TableOutput(executorsGrid, null, 1);
            Classes.Get.TableOutput(albumsGrid, null, 10);
            searchBox.Text = "Поиск";
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindTracksPiece @login, @track";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);
                        command.Parameters.AddWithValue("@track", searchBox.Text);

                        Classes.Get.FillGrid(command, tracksGrid);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindExsPiece @executor";
                        command.Parameters.AddWithValue("@executor", searchBox.Text);

                        Classes.Get.FillGrid(command, executorsGrid);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindAlbumPiece @album";
                        command.Parameters.AddWithValue("@album", searchBox.Text);

                        Classes.Get.FillGrid(command, albumsGrid);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
                finally
                {
                    searchBox.Text = "Поиск";
                    connection.Close();
                }
            }
        }
    }
}
