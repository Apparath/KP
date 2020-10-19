using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace KP.Windows
{
    /// <summary>
    /// Логика взаимодействия для Genre.xaml
    /// </summary>
    public partial class Genre : Window
    {
        public Genre(bool old, object obj, DataGrid dataGrid)
        {
            InitializeComponent();

            if (old == true)
            {
                this.Title = "Изменение жанра";
                save.Content = "Изменить";

                save.Click += (s, e) =>
                {
                    UpdateGenre(obj);
                    Classes.Get.TableOutput(dataGrid, null, 2);
                };
            }
            else
            {
                this.Title = "Добавление жанра";
                save.Content = "Добавить";

                save.Click += (s, e) =>
                {
                    InsertGenre();
                    Classes.Get.TableOutput(dataGrid, null, 2);
                };
            }
        }
        /// <summary>
        /// Метод обновления существующего жанра
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateGenre(object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Genres WHERE Name = @name";
                        if (name.Text != null && (!name.Text.StartsWith(" ") || !name.Text.Contains("  "))) command.Parameters.AddWithValue("@name", name.Text.Trim());
                        else
                        {
                            MessageBox.Show("Неверный формат ввода", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("Такой жанр уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "UPDATE Genres SET Name = @name WHERE Name = @old";
                                    command1.Parameters.AddWithValue("@name", name.Text.Trim());
                                    command1.Parameters.AddWithValue("@old", obj);
                                    command1.ExecuteNonQuery();
                                }

                                this.Close();
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
        /// Метод добавления нового жанра
        /// </summary>
        private void InsertGenre()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Genres WHERE Name = @name";
                        if (name.Text != null && (!name.Text.StartsWith(" ") || !name.Text.Contains("  "))) command.Parameters.AddWithValue("@name", name.Text.Trim());
                        else
                        {
                            MessageBox.Show("Неверный формат ввода", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("Такой жанр уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "INSERT INTO Genres VALUES @name";
                                    command1.Parameters.AddWithValue("@name", name.Text.Trim());
                                    command1.ExecuteNonQuery();
                                }

                                this.Close();
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
    }
}
