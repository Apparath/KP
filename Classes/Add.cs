using System.Data.SqlClient;
using System.IO;

namespace KP.Classes
{
    class Add
    {
        /// <summary>
        /// Метод доавления нового изображения в бд
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="file"></param>
        /// <param name="command"></param>
        public static void NewImage(SqlConnection connection, FileInfo file, SqlCommand command)
        {
            using (SqlCommand command1 = connection.CreateCommand())
            {
                command1.CommandText = "INSERT INTO Images VALUES (@name, @binary)";

                command1.Parameters.AddWithValue("@name", file.Name);
                command1.Parameters.AddWithValue("@binary", Get.BytesFromFile(file.FullName));

                command1.ExecuteNonQuery();
            }

            using (SqlCommand command2 = connection.CreateCommand())
            {
                command2.CommandText = "SELECT MAX(Id) FROM Images";

                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read()) command.Parameters.AddWithValue("@image", reader[0]);
                    }
                }
            }
        }
    }
}
