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
    /// Логика взаимодействия для Password.xaml
    /// </summary>
    public partial class Password : Window
    {
        public Password(string user)
        {
            InitializeComponent();

            save.Click += (s, e) =>
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT Password FROM Users WHERE Login = @login";

                            command.Parameters.AddWithValue("@login", user);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();

                                    if (old.Text != reader[0].ToString())
                                    {
                                        MessageBox.Show("Старый пароль неверный", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                        return;
                                    }
                                    else
                                    {
                                        reader.Close();
                                        
                                        if (_new.Password.Length > 5 && _new.Password.Length <= 20)
                                        {
                                            if (repeat.Password != _new.Password)
                                            {
                                                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                                old.Text = "";
                                                _new.Password = "";
                                                repeat.Password = "";
                                            }
                                            else
                                            {
                                                using (SqlCommand command1 = connection.CreateCommand())
                                                {
                                                    command1.CommandText = "UPDATE Users SET Password = @pw WHERE Login = @login";

                                                    command1.Parameters.AddWithValue("@login", user);
                                                    command1.Parameters.AddWithValue("@pw", repeat.Password);

                                                    command1.ExecuteNonQuery();
                                                }

                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Пароль должен быть в диапазоне от 6 до 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                            old.Text = "";
                                            _new.Password = "";
                                        }
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
            };
        }
    }
}
