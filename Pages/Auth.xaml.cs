using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод перехода на форму регистрации по нажатию кнопки "Зарегистрироваться"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/Reg.xaml", UriKind.RelativeOrAbsolute));
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

        private byte errorCount = 0; //Количество ошибок ввода
        private bool errorMessage = false; //Показ/скрытие капчи

        /// <summary>
        /// Метод перехода на страницу пользователя
        /// </summary>
        /// <param name="reader"></param>
        private void EnterPage(SqlDataReader reader)
        {
            while (reader.Read()) Classes.Login.Value = login.Text;

            NavigationService.Navigate(new Uri("Pages/User.xaml", UriKind.RelativeOrAbsolute));

            login.Text = "";
            errorMessage = false;
            errorCount = 0;

            Classes.Captcha.Check(captcha, verify, refresh, false);
        }

        /// <summary>
        /// Метод авторизации по нажатию на кнопку "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC LoginUser @login, @password";

                        command.Parameters.AddWithValue("@login", login.Text);
                        command.Parameters.AddWithValue("@password", password.Password);

                        if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(password.Password))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows && errorCount < 3) EnterPage(reader);
                                else if (reader.HasRows && errorCount == 3)
                                {
                                    if (verify.Text == captcha.Text) EnterPage(reader);
                                    else
                                    {
                                        MessageBox.Show("Неверное значение Captcha!", "Ошибка ввода Captcha", MessageBoxButton.OK, MessageBoxImage.Error);

                                        Classes.Captcha.Check(captcha, verify, refresh, false);
                                    }
                                }
                                else if (!reader.HasRows && errorCount < 3)
                                {
                                    MessageBox.Show("Пользователя не существует, или\nневерно указаны логин или пароль!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);

                                    errorCount++;
                                }
                                else if (!reader.HasRows && errorCount == 3)
                                {
                                    if (errorMessage == false)
                                    {
                                        MessageBox.Show("Превышен лимит ввода неверных данных!\nВ следующий раз для подтверждения введите Captcha!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);

                                        Classes.Captcha.Check(captcha, verify, refresh, true);

                                        errorMessage = true;
                                    }
                                    else if (verify.Text != captcha.Text)
                                        MessageBox.Show("Неверное значение Captcha!", "Ошибка ввода Captcha", MessageBoxButton.OK, MessageBoxImage.Error);
                                    else if (verify.Text == captcha.Text)
                                        MessageBox.Show("Пользователя не существует, или\nневерно указаны логин или пароль!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);

                                    Classes.Captcha.Check(captcha, verify, refresh, false);
                                }
                            }
                        }
                        else MessageBox.Show("Введите логин и пароль!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
