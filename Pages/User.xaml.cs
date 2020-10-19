using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Configuration;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindRole @login";

                        command.Parameters.AddWithValue("@login", Classes.Login.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[0].ToString() == "Администратор")
                                {
                                    roles.Visibility = Visibility.Visible;
                                    users.Visibility = Visibility.Visible;
                                }
                            }
                        }
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
                }
            }
        }

        /// <summary>
        /// Метод перехода на главную страницу по нажатию кнопки "Главная"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Main.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу жанров по нажатию кнопки "Жанры"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void genres_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Genres.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу треков по нажатию кнопки "Треки"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tracks_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Tracks.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод переходв на страницу исполнителей по нажатию кнопки "Исполнители"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executors_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Executors.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу ролей по нажатию кнопки "Роли"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roles_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Roles.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу пользователей по нажаитю кнопки "Пользователи"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void users_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Pages/Users.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу профиля пользователя по нажатию кнопки "Профиль"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profile_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Profile(Classes.Login.Value));
        }
        
        /// <summary>
        /// Метод возврата на страницу авторизации по нажатию кнопки "Выйти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Auth.xaml", UriKind.RelativeOrAbsolute));

            Classes.Login.Value = "";
        }

    }
}
