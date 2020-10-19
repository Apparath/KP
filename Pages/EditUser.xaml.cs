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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        BitmapImage bitmap = new BitmapImage();

        public EditUser(object obj)
        {
            InitializeComponent();

            gender.Items.Add("Мужчина");
            gender.Items.Add("Женщина");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Name FROM Roles";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    role.Items.Add(reader[0].ToString());
                                }
                            }
                        }
                    }

                    Classes.Get.Countries(connection, country);

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC GetInfos @login";
                        command.Parameters.AddWithValue("@login", obj);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    login.Text = reader[0].ToString();
                                    password.Password = reader[1].ToString();
                                    name.Text = reader[2].ToString();
                                    surname.Text = reader[3].ToString();
                                    gender.SelectedItem = reader[4].ToString();
                                    country.SelectedItem = reader[5].ToString();
                                    if (reader[6] != DBNull.Value)
                                    {
                                        bitmap.BeginInit();
                                        bitmap.StreamSource = new MemoryStream((byte[])reader[6]);
                                        bitmap.EndInit();
                                    }
                                    else
                                    {
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri("/KP;component/Resources/addImage.png", UriKind.RelativeOrAbsolute);
                                        bitmap.EndInit();
                                    }
                                    role.SelectedItem = reader[7].ToString();
                                }

                                image.Source = bitmap;
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

            if (obj.ToString() == Classes.Login.Value)
            {
                role.IsEnabled = false;
                repeat.Password = password.Password;
                password.IsEnabled = false;
                repeat.IsEnabled = false;
            }

            Classes.Captcha.Refresh(captcha, verify);

            updateUser.Click += (s, e) =>
            {
                UpdateUser(obj);
                if (obj.ToString() == Classes.Login.Value)
                {
                    role.IsEnabled = false;
                    repeat.Password = password.Password;
                    password.IsEnabled = false;
                    repeat.IsEnabled = false;
                }
            };
        }

        private void UpdateUser(object obj)
        {
            if (login.Text != "" && password.Password != "" && repeat.Password != "" && gender.SelectedItem != null && (surname.Text != "" || name.Text != ""))
            {
                if (login.Text.Length > 2 && login.Text.Length <= 20)
                {
                    if (password.Password.Length > 4 && password.Password.Length <= 20)
                    {
                        if (name.Text.Length > 0 && name.Text.Length <= 20)
                        {
                            if (surname.Text.Length > 0 && surname.Text.Length <= 20)
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
                                                    command.CommandText = "EXEC UpdateUser @login, @password, @name, @surname, @gender, @country, @image, @role, @search";

                                                    command.Parameters.AddWithValue("@search", obj);
                                                    command.Parameters.AddWithValue("@login", login.Text);
                                                    command.Parameters.AddWithValue("@password", password.Password);
                                                    command.Parameters.AddWithValue("@surname", surname.Text);
                                                    command.Parameters.AddWithValue("@name", name.Text);
                                                    command.Parameters.AddWithValue("@gender", gender.SelectedItem);

                                                    if (country.SelectedIndex != -1) command.Parameters.AddWithValue("@country", country.SelectedItem);
                                                    else command.Parameters.AddWithValue("@country", DBNull.Value);

                                                    if (image != null && file != null)
                                                    {
                                                        using (SqlCommand command1 = connection.CreateCommand())
                                                        {
                                                            command1.CommandText = "EXEC FindImage @binary";

                                                            command1.Parameters.AddWithValue("@binary", Classes.Get.BytesFromFile(file.FullName));

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
                                                    else if (image != null && file == null)
                                                    {
                                                        using (SqlCommand command1 = connection.CreateCommand())
                                                        {
                                                            command1.CommandText = "SELECT Image FROM Users WHERE Login = @login";
                                                            command1.Parameters.AddWithValue("@login", obj);
                                                            using (SqlDataReader reader = command1.ExecuteReader())
                                                            {
                                                                if (reader.HasRows)
                                                                {
                                                                    while (reader.Read()) command.Parameters.AddWithValue("@image", reader[0]);
                                                                }
                                                                else
                                                                {
                                                                    reader.Close();

                                                                    command.Parameters.AddWithValue("@image", DBNull.Value);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (image == null && file == null) command.Parameters.AddWithValue("@image", DBNull.Value);

                                                    command.Parameters.AddWithValue("@role", role.SelectedItem);

                                                    command.ExecuteNonQuery();
                                                }

                                                MessageBox.Show("Данные успешно изменены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                                                NavigationService.GoBack();
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

                                        password.Password = "";
                                        repeat.Password = "";
                                        Classes.Captcha.Refresh(captcha, verify);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Введенные пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                                    password.Password = "";
                                    repeat.Password = "";
                                    Classes.Captcha.Refresh(captcha, verify);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Фамилия должна быть в диапазоне от 1 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                                surname.Text = "";
                                password.Password = "";
                                repeat.Password = "";
                                Classes.Captcha.Refresh(captcha, verify);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Имя должно быть в диапазоне от 1 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                            name.Text = "";
                            password.Password = "";
                            repeat.Password = "";
                            Classes.Captcha.Refresh(captcha, verify);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароль должен быть в диапазоне от 5 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                        password.Password = "";
                        repeat.Password = "";
                        Classes.Captcha.Refresh(captcha, verify);
                    }
                }
                else
                {
                    MessageBox.Show("Логин должен быть в диапазоне от 3 до 20 символов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                    login.Text = "";
                    password.Password = "";
                    repeat.Password = "";
                    Classes.Captcha.Refresh(captcha, verify);
                }
            }
            else
            {
                MessageBox.Show("Заполните все необходимые данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                password.Password = "";
                repeat.Password = "";
                Classes.Captcha.Refresh(captcha, verify);
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private FileInfo file; //Для передачи пути изображения

        /// <summary>
        /// Метод вставки изображения из файла в форму регистрации после отжатия ЛКМ на форме изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.Source = Classes.Set.ImageFromFile(out file);
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
    }
}
