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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(tracksGrid, null, 0);
            Classes.Get.TableOutput(executorsGrid, null, 1);
        }

        private void updatePage_Click(object sender, RoutedEventArgs e)
        {
            Classes.Get.TableOutput(tracksGrid, null, 0);
            Classes.Get.TableOutput(executorsGrid, null, 1);
            searchBox.Text = "Поиск";
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindTracksPiece @login, @track";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);
                        command.Parameters.AddWithValue("@track", searchBox.Text);

                        Classes.Get.FillGrid(command, tracksGrid);
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "EXEC FindExsPiece @login, @executor";
                        command.Parameters.AddWithValue("@login", Classes.Login.Value);
                        command.Parameters.AddWithValue("@executor", searchBox.Text);

                        Classes.Get.FillGrid(command, executorsGrid);
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
        }
    }
}
