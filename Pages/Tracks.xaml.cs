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
    /// Логика взаимодействия для Tracks.xaml
    /// </summary>
    public partial class Tracks : Page
    {
        public Tracks()
        {
            InitializeComponent();
            Classes.Get.TableOutput(tracksGrid, null, 3);
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
                        command.CommandText = "EXEC FindLikedTracksPiece @login, @track";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);
                        command.Parameters.AddWithValue("@track", searchBox.Text);

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
                    connection.Close();
                    searchBox.Text = "Поиск";
                }
            }
        }

        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(tracksGrid, null, 3);
            searchBox.Text = "Поиск";
        }

        private void deleteLiked_Click(object sender, RoutedEventArgs e)
        {
            if (tracksGrid.SelectedItem != null)
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
                                command.CommandText = "DELETE FROM Likes WHERE Likes.Login = (SELECT Users.Login FROM Users" +
                                    " WHERE Users.Login = @login) " +
                                    "AND Likes.Track = (SELECT Id FROM Tracks WHERE Tracks.Name = @track)";

                                command.Parameters.AddWithValue("@track", ((DataRowView)tracksGrid.SelectedItem)[0]);
                                command.Parameters.AddWithValue("@login", Classes.Login.Value);

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }
                        finally
                        {
                            connection.Close();
                            Classes.Get.TableOutput(tracksGrid, null, 3);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Удалить можно только существующую запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
        }
    }
}
