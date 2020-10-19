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

            edit.Click += (s, e) =>
            {

            };

            edit1.Click += (s, e) =>
            {
                Windows.Executor executor = new Windows.Executor(true, executorsGrid);
                executor.ShowDialog();
            };

            delete1.Click += (s, e) =>
            {

            };
        }

        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(tracksGrid, null, 0);
            Classes.Get.TableOutput(executorsGrid, null, 1);
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
                        command.CommandText = "EXEC FindExsPiece @login, @executor";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);
                        command.Parameters.AddWithValue("@executor", searchBox.Text);

                        Classes.Get.FillGrid(command, executorsGrid);
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

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC DeleteTrack @track, @album, @executor, @binary";

                        command.Parameters.AddWithValue("@track", ((DataRowView)tracksGrid.SelectedItem)[0]);
                        command.Parameters.AddWithValue("@track", ((DataRowView)tracksGrid.SelectedItem)[1]);
                        command.Parameters.AddWithValue("@login", ((DataRowView)tracksGrid.SelectedItem)[2]);
                        command.Parameters.AddWithValue("@track", ((DataRowView)tracksGrid.SelectedItem)[3]);

                        Classes.Get.FillGrid(command, tracksGrid);
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/Track.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
