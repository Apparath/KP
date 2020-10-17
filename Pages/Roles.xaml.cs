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
    /// Логика взаимодействия для Roles.xaml
    /// </summary>
    public partial class Roles : Page
    {
        public Roles()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Метод показа таблицы ролей после загрузки страницы ролей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(rolesGrid, null, 5);
        }

        /// <summary>
        /// Метод поиска сопадений в таблице ролей по слову или части слова в поисковой строке по нажатию кнопки "Искать"
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
                        command.CommandText = "SELECT * FROM Roles WHERE Name LIKE('%' + @name + '%')";
                        command.Parameters.AddWithValue("@name", searchBox.Text);

                        Classes.Get.FillGrid(command, rolesGrid);
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

        /// <summary>
        /// Метод обновления страницы по нажатию кнопки "Обновить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(rolesGrid, null, 5);
            searchBox.Text = "Поиск";
        }

        /// <summary>
        /// Метод редактирования выделенной записи по нажатию кнопки меню ЛКМ "Редактировать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (rolesGrid.SelectedItem != null)
            {
                Windows.Role window = new Windows.Role(true, ((DataRowView)rolesGrid.SelectedItem)[1], rolesGrid);
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
            if (rolesGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Уведомление",MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM Roles WHERE Name = @name";

                                command.Parameters.AddWithValue("@name", ((DataRowView)rolesGrid.SelectedItem)[1]);
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
                            Classes.Get.TableOutput(rolesGrid, null, 5);
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
            Windows.Role window = new Windows.Role(false, null, rolesGrid);
            window.ShowDialog();
        }
    }
}
