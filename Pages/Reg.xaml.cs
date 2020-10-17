using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

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

            Classes.Captcha.Refresh(captcha, verify);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    Classes.Get.Countries(connection, country);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Метод возврата на страницу авторизации по нажатию кнопки "Назад"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Метод обновления капчи по нажатию кнопки "Обновить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Classes.Captcha.Refresh(captcha, verify);
        }

        private FileInfo file; //Для передачи пути изображения

        /// <summary>
        /// Метод регистрации пользователя понажатию кнопки "Зарегистрироваться"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text != "" && password.Password != "" && repeat.Password != "" && gender.SelectedIndex!= 0 && (surname.Text != "" || name.Text != ""))
            {
                if (login.Text.Length > 2 && login.Text.Length <= 20)
                {
                    if (password.Password.Length > 7 && password.Password.Length <= 20)
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
                                            command.CommandText = "EXEC AddNewUser @login, @password, @name, @surname, @gender, @country, @image";

                                            command.Parameters.AddWithValue("@login", login.Text);
                                            command.Parameters.AddWithValue("@password", password.Password);
                                            command.Parameters.AddWithValue("@surname", surname.Text);
                                            command.Parameters.AddWithValue("@name", name.Text);

                                            switch (gender.SelectedIndex)
                                            {
                                                case 1:
                                                    command.Parameters.AddWithValue("@gender", "Мужчина");
                                                    break;
                                                case 2:
                                                    command.Parameters.AddWithValue("@gender", "Женщина");
                                                    break;
                                                default:
                                                    break;
                                            }

                                            if (country.SelectedIndex != 0) command.Parameters.AddWithValue("@country", country.SelectedItem.ToString());
                                            else command.Parameters.AddWithValue("@country", DBNull.Value);

                                            if (!image.Source.ToString().Contains("addImage.png"))
                                            {
                                                using (SqlCommand command1 = connection.CreateCommand())
                                                {
                                                    command1.CommandText = "EXEC FindImage @binary";

                                                    command1.Parameters.AddWithValue("@binary", Classes.Get.ImageFromFile(file.FullName));

                                                    using (SqlDataReader reader = command1.ExecuteReader())
                                                    {
                                                        if (reader.HasRows)
                                                        {
                                                            while (reader.Read()) command.Parameters.AddWithValue("@image", reader[0]);
                                                        }
                                                        else
                                                        {
                                                            reader.Close();

                                                            Classes.Add.NewImage(connection, file, command);
                                                        }
                                                    }
                                                }
                                            }
                                            else command.Parameters.AddWithValue("@image", DBNull.Value);

                                            command.ExecuteNonQuery();
                                        }

                                        MessageBox.Show("Пользователь успешно зарегистрирован!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                                        Classes.Login.Value = login.Text;

                                        NavigationService.Navigate(new Uri("Pages/User.xaml", UriKind.RelativeOrAbsolute));
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
                        MessageBox.Show("Пароль должен быть в диапазоне от 3 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                        login.Text = "";
                        repeat.Password = "";
                        Classes.Captcha.Refresh(captcha, verify);
                    }
                }
                else
                {
                    MessageBox.Show("Логин должен быть в диапазоне от 3 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                    login.Text = "";
                    repeat.Password = "";
                    Classes.Captcha.Refresh(captcha, verify);
                }
            }
            else
            {
                MessageBox.Show("Заполните все необходимые данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                repeat.Password = "";
                Classes.Captcha.Refresh(captcha, verify);
            }
        }

        /// <summary>
        /// Метод обновления страницы по нажатию кнопки "Обновить страницу"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        /// <summary>
        /// Метод вставки изображения из файла в форму регистрации после отжатия ЛКМ на форме изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.Source = Classes.Set.ImageFromFile(out file);
        }
    }
}
