using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Classes.Captcha.Refresh(captcha, verify);
        }

        private FileInfo file; //Для передачи пути изображения

        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text != "" && password.Password != "" && repeat.Password != "" && (surname.Text != "" || name.Text != ""))
            {
                if (repeat.Password == password.Password)
                {
                    if (verify.Text == captcha.Text)
                    {
                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                        {
                            try
                            {
                                connection.Open();

                                using (SqlCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = "EXEC AddNewUser @login, @password, @surname, @name, @gender, @countryid, @imageid";

                                    command.Parameters.AddWithValue("@login", login.Text);
                                    command.Parameters.AddWithValue("@password", password.Password);
                                    command.Parameters.AddWithValue("@surname", surname.Text);
                                    command.Parameters.AddWithValue("@name", name.Text);

                                    switch (gender.SelectedIndex)
                                    {
                                        case 0: 
                                            command.Parameters.AddWithValue("@gender", "М");
                                            break;
                                        case 1: 
                                            command.Parameters.AddWithValue("@gender", "Ж");
                                            break;
                                        default:
                                            break;
                                    }

                                    command.Parameters.AddWithValue("@countryid", country.SelectedIndex);

                                    if (image.Source != null)
                                    {
                                        using (SqlCommand command1 = connection.CreateCommand())
                                        {
                                            command1.CommandText = "SELECT Id, Name FROM Images WHERE Name = @name";

                                            command1.Parameters.AddWithValue("@name", file.Name);

                                            using (SqlDataReader reader = command1.ExecuteReader())
                                            {
                                                if (reader.HasRows)
                                                {
                                                    while (reader.Read()) command.Parameters.AddWithValue("@imageid", reader[0]);
                                                }
                                                else
                                                {
                                                    Classes.Add.NewImage(connection, file, command);

                                                    using (SqlCommand command2 = connection.CreateCommand())
                                                    {
                                                        command2.CommandText = "SELECT MAX(Id) FROM Images";

                                                        using (SqlDataReader reader1 = command2.ExecuteReader())
                                                        {
                                                            if (reader1.HasRows)
                                                            {
                                                                while (reader1.Read()) command.Parameters.AddWithValue("@imageid", reader[0]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else command.Parameters.AddWithValue("@imageid", DBNull.Value);

                                    command.ExecuteNonQuery();
                                }

                                MessageBox.Show("Пользователь успешно зарегистрирован!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                                NavigationService.Navigate(new Uri("Pages/UserPage.xaml", UriKind.RelativeOrAbsolute));
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
                    else
                    {
                        MessageBox.Show("Неверное значения Captcha!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        repeat.Password = "";
                        Classes.Captcha.Refresh(captcha, verify);
                    }
                }
                else
                {
                    MessageBox.Show("Введенные пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    repeat.Password = "";
                    Classes.Captcha.Refresh(captcha, verify);
                }
            }
            else
            {
                MessageBox.Show("Заполните все неоходимые данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                repeat.Password = "";
                Classes.Captcha.Refresh(captcha, verify);
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.Source = Classes.Set.ImageFromFile(out file);
        }
    }
}
