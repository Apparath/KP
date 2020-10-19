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
using System.Windows.Shapes;

namespace KP.Windows
{
    /// <summary>
    /// Логика взаимодействия для Executor.xaml
    /// </summary>
    public partial class Executor : Window
    {
        private FileInfo file; //Для передачи пути изображения

        public Executor(bool old, DataGrid dataGrid)
        {
            InitializeComponent();

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
            if (old == false)
            {
                track.Click += (s, e) =>
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "";

                                command.Parameters.AddWithValue("@name", name.Text);
                                if (date.Text != null) command.Parameters.AddWithValue("@date", date.Text);
                                else command.Parameters.AddWithValue("@date", DBNull.Value);

                                if (country.SelectedIndex != -1) command.Parameters.AddWithValue("@country", country.SelectedItem);
                                else command.Parameters.AddWithValue("@country", DBNull.Value);

                                if (!image.Source.ToString().Contains("addImage.png"))
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
                                else command.Parameters.AddWithValue("@image", DBNull.Value);

                                command.ExecuteNonQuery();
                            }

                            MessageBox.Show("Исполнитель успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
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
                    Classes.Get.TableOutput(dataGrid, null, 1);
                };
            }
            else
            {
                track.Click += (s, e) =>
                {

                };
            }
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
