using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KP.Classes
{
    class Get
    {
        /// <summary>
        /// Метод получения названий стран из бд в выпадающий список
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="country"></param>
        public static void Countries(SqlConnection connection, ComboBox country)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Name FROM Countries";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            country.Items.Add(reader[0]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод получения изображения из бд
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] BytesFromFile(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] bytes = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return bytes;
        }

        /// <summary>
        /// Метод заполнения списка ролей
        /// </summary>
        /// <param name="command"></param>
        public static void FillGrid(SqlCommand command, DataGrid rolesGrid)
        {
            command.ExecuteNonQuery();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable rolesTable = new DataTable();
                adapter.Fill(rolesTable);

                rolesGrid.ItemsSource = rolesTable.DefaultView;
            }
        }

        /// <summary>
        /// Метод показа таблицы ролей
        /// </summary>
        public static void TableOutput(DataGrid dataGrid, object old, byte i)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        switch (i)
                        {
                            case 0:
                                command.CommandText = "EXEC FindTracks @login";
                                command.Parameters.AddWithValue("@login", Login.Value);
                                break;
                            case 1:
                                command.CommandText = "select Executors.Name as Executor, YEar(Executors.Date) as Date," +
                                    "Countries.Name as Country" +
                                                       " from Executors inner join Records on Executors.Id = Executor" +
                                                       " left join Countries on Countries.Id = Executors.Country";
                                break;
                            case 2:
                                command.CommandText = "SELECT * FROM Genres";
                                break;
                            case 3:
                                command.CommandText = "EXEC FindLikedTracks @login";
                                command.Parameters.AddWithValue("@login", Login.Value);
                                break;
                            case 4:
                                command.CommandText = "EXEC FindLikedExecutors @login";
                                command.Parameters.AddWithValue("@login", Login.Value);
                                break;
                            case 5:
                                command.CommandText = "SELECT * FROM Roles";
                                break;
                            case 6:
                                command.CommandText = "EXEC FindUsers";
                                break;
                            case 7:
                                command.CommandText = "EXEC FindExecutorsInGenre @name";
                                command.Parameters.AddWithValue("@name", old);
                                break;
                            case 8:
                                command.CommandText = "SELECT Login, Password, Users.Name as Name, Surname, Gender, Countries.Name as Country FROM Users INNER JOIN" +
                                    " Roles ON Roles.Id = Role Left JOIN Countries ON Countries.Id = Users.Country WHERE Roles.Name = @role";
                                command.Parameters.AddWithValue("@role", old);
                                break;
                            case 9:
                                command.CommandText = "Select Name as Country FROM countries";
                                break;
                            case 10:
                                command.CommandText = "SELECT Albums.Name AS Album, YEAR(Albums.Date) As Date FROM Albums " +
                                                        "inner join Records on Albums.Id = Records.Album";
                                break;
                            default:
                                break;
                        }

                        FillGrid(command, dataGrid);
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
