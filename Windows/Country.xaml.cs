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
    /// Логика взаимодействия для Country.xaml
    /// </summary>
    public partial class Country : Window
    {
        public Country(bool old, object obj, DataGrid dataGrid)
        {
            InitializeComponent();

            if (old == true)
            {
                this.Title = "Изменение страны";
                save.Content = "Изменить";

                save.Click += (s, e) =>
                {
                    UpdateCountry(obj);
                    Classes.Get.TableOutput(dataGrid, null, 9);
                };
            }
            else
            {
                this.Title = "Добавление страны";
                save.Content = "Добавить";

                save.Click += (s, e) =>
                {
                    InsertCountry();
                    Classes.Get.TableOutput(dataGrid, null, 9);
                };
            }
        }

        private void UpdateCountry(object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Countries WHERE Name = @name";
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
                                MessageBox.Show("Такая страна уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "UPDATE Countries SET Name = @name WHERE Name = @old";
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

        private void InsertCountry()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Countries WHERE Name = @name";
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
                                MessageBox.Show("Такая страна уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "INSERT INTO Countries VALUES (@name)";
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
