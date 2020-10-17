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
    /// Логика взаимодействия для Role.xaml
    /// </summary>
    public partial class Role : Window
    {
        public Role(bool old, object obj, DataGrid dataGrid)
        {
            InitializeComponent();
            
            if (old == true)
            {
                this.Title = "Изменение роли";
                save.Content = "Изменить";

                save.Click += (s, e) =>
                {
                    UpdateRole(obj);
                    Classes.Get.TableOutput(dataGrid, null, 5);
                };
            }
            else
            {
                this.Title = "Добавление роли";
                save.Content = "Добавить";

                save.Click += (s, e) =>
                {
                    InsertRole();
                    Classes.Get.TableOutput(dataGrid, null, 5);
                };
            }
        }

        /// <summary>
        /// Метод обновления существующей роли
        /// </summary>
        /// <param name="role"></param>
        private void UpdateRole(object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Roles WHERE Name = @name";
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
                                MessageBox.Show("Такая роль уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "UPDATE Roles SET Name = @name WHERE Name = @old";
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
        /// Метод добавления новой роли
        /// </summary>
        private void InsertRole()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Roles WHERE Name = @name";
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
                                MessageBox.Show("Такая роль уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                            else
                            {
                                reader.Close();

                                using (SqlCommand command1 = connection.CreateCommand())
                                {
                                    command1.CommandText = "INSERT INTO Roles(Id, Name) VALUES (NEWID(), @name)";
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
