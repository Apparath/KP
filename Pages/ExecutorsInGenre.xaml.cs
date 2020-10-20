using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для ExecutorsInGenre.xaml
    /// </summary>
    public partial class ExecutorsInGenre : Page
    {
        public ExecutorsInGenre(object obj)
        {
            InitializeComponent();


            Classes.Get.TableOutput(exsInGenreGrid, obj, 7);
            updatePage.Click += (s, e) =>
            {
                Classes.Get.TableOutput(exsInGenreGrid, obj, 7);
                searchBox.Text = "Поиск";
            };
            searchBtn.Click += (s, e) =>
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "EXEC FindExsInGenrePiece @genre, @executor";
                            command.Parameters.AddWithValue("@executor", searchBox.Text);
                            command.Parameters.AddWithValue("@genre", obj);

                            Classes.Get.FillGrid(command, exsInGenreGrid);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }
                    finally
                    {
                        searchBox.Text = "Поиск";
                        connection.Close();
                    }
                }
            };
        }
    }
}
