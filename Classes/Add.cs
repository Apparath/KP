using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Classes
{
    class Add
    {
        public static void NewImage(SqlConnection connection, FileInfo file, SqlCommand command)
        {
            using (SqlCommand command1 = connection.CreateCommand())
            {
                command1.CommandText = "INSERT INTO Images (Name, Binary) WHERE Name = @name AND Binary = @binary";

                command1.Parameters.AddWithValue("@name", file.Name);
                command1.Parameters.AddWithValue("@binary", Get.ImageFromFile(file.FullName));

                command1.ExecuteNonQuery();

                using (SqlCommand command2 = connection.CreateCommand())
                {
                    command2.CommandText = "SELECT MAX(Id) FROM Images";

                    using (SqlDataReader reader = command2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read()) command.Parameters.AddWithValue("@Id", reader[0]);
                        }
                    }

                    command2.ExecuteNonQuery();
                }
            }
        }
    }
}
