using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KP.Classes
{
    class Get
    {
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
    }
}
