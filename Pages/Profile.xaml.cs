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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile(object user)
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "EXEC GetInfos @login";

                            command.Parameters.AddWithValue("@login", user);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                BitmapImage bitmap = new BitmapImage();
                                while (reader.Read())
                                {
                                    login.Text = reader[0].ToString();
                                    name.Text = reader[2].ToString();
                                    surname.Text = reader[3].ToString();
                                    gender.Text = reader[4].ToString();
                                    country.Text = reader[5].ToString();

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
                                }

                                image.Source = bitmap;
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

            changeInfos.Click += (s, e) =>
            {
                NavigationService.Navigate(new EditUser(user));
            };

            changePw.Click += (s, e) =>
            {
                Windows.Password password = new Windows.Password(user);
                password.ShowDialog();
            };
        }
    }
}
