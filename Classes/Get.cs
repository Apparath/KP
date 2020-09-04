using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                command.CommandText = "SELECT Country_name FROM Countries";

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
        public static byte[] ImageFromFile(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] photo = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return photo;
        }
    }
}
