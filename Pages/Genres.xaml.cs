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
    /// Логика взаимодействия для Genres.xaml
    /// </summary>
    public partial class Genres : Page
    {
        public Genres()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод показа таблицы ролей после загрузки страницы жанров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(genresGrid, null, 2);
        }

        /// <summary>
        /// Метод поиска сопадений в таблице жанров по слову или части слова в поисковой строке по нажатию кнопки "Искать"
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
                        command.CommandText = "SELECT * FROM Genres WHERE Name LIKE('%' + @name + '%')";
                        command.Parameters.AddWithValue("@name", searchBox.Text);

                        Classes.Get.FillGrid(command, genresGrid);
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

        /// <summary>
        /// Метод обновления страницы по нажатию кнопки "Обновить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(genresGrid, null, 2);
            searchBox.Text = "Поиск";
        }

        /// <summary>
        /// Метод редактирования выделенной записи по нажатию кнопки меню ЛКМ "Редактировать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (genresGrid.SelectedItem != null)
            {
                Windows.Genre window = new Windows.Genre(true, ((DataRowView)genresGrid.SelectedItem)[1], genresGrid);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Изменить можно только существующую роль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
        }

        /// <summary>
        /// Метод удаления выделенной записи по нажатию кнопки меню ЛКМ "Удалить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (genresGrid.SelectedItem != null)
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
                                command.CommandText = "DELETE FROM Genres WHERE Name = @name";

                                command.Parameters.AddWithValue("@name", ((DataRowView)genresGrid.SelectedItem)[1]);
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
                            Classes.Get.TableOutput(genresGrid, null, 2);
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

        /// <summary>
        /// Метод добваления новой записи по нажатию кнопки меню ЛКМ "Добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Windows.Genre window = new Windows.Genre(false, null, genresGrid);
            window.ShowDialog();
        }

        private void showExecutors_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExecutorsInGenre(genresGrid));
        }
    }
}
