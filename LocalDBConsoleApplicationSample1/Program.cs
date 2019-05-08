using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB接続のサンプル
{
    /// <summary>
    /// LocalDB接続のサンプル
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 専用クラスでLocalDBの接続文字列を得る
            string constr = LocalDBAdapter.GetLocalDBConnectionString(@"Database1.mdf");

            //using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;Integrated Security=True;AttachDbFileName=C:\Users\kei.okuma\Documents\visual studio 2015\Projects\LocalDBConsoleApplicationSample1\LocalDBConsoleApplicationSample1\Database1.mdf"))
            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0}:{1}:{2}", reader["Id"], reader["Name"], reader["Price"]);
                    }
                }
                finally
                {
                    connection.Close();
                    Console.ReadKey();
                }
            }
        }
    }
}
