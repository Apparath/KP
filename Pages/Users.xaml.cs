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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(usersGrid, null, 6);
        }

        /// <summary>
        /// Метод поиска сопадений в таблице пользователей по слову или части слова в поисковой строке по нажатию кнопки "Искать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindUsersPiece @search";
                        command.Parameters.AddWithValue("@search", searchBox.Text);

                        Classes.Get.FillGrid(command, usersGrid);
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
            Classes.Get.TableOutput(usersGrid, null, 6);
            searchBox.Text = "Поиск";
        }

        private void userInfos_Click(object sender, RoutedEventArgs e)
        {
            Windows.User user = new Windows.User(((DataRowView)usersGrid.SelectedItem)[1]);
            user.ShowDialog();
            Classes.Get.TableOutput(usersGrid, null, 6);
            searchBox.Text = "Поиск";
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new Uri("Pages/Reg.xaml", UriKind.RelativeOrAbsolute));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (usersGrid.SelectedItem != null)
            {
                NavigationService.RemoveBackEntry();
                NavigationService.Navigate(new EditUser(((DataRowView)usersGrid.SelectedItem)[1]));
            }
            else
            {
                MessageBox.Show("Изменить можно только существующего пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (usersGrid.SelectedItem != null)
            {
                if (((DataRowView)usersGrid.SelectedItem)[1].ToString() != Classes.Login.Value)
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
                                    command.CommandText = "DELETE FROM Users WHERE Login = @name";

                                    command.Parameters.AddWithValue("@name", ((DataRowView)usersGrid.SelectedItem)[1]);
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
                                Classes.Get.TableOutput(usersGrid, null, 6);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Нельзя удалить текущего пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
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
