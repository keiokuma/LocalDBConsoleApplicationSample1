using LocalDB接続のサンプル;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalDB接続のUPDATE
{
    public partial class Form1 : Form
    {
        string constr = null;

        public Form1()
        {
            InitializeComponent();
            constr = LocalDBAdapter.GetLocalDBConnectionString(@"Database1.mdf");
        }

        /// <summary>
        /// SELECT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            //string constr = LocalDBAdapter.GetLocalDBConnectionString(@"Database1.mdf");
            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        textBox1.Text += string.Format("{0}:{1}:{2}", reader["Id"], reader["Name"], reader["Price"]);
                        textBox1.Text += Environment.NewLine;
                    }
                }
                finally
                {
                    connection.Close();
                    //Console.ReadKey();
                }
            }

        }

        /// <summary>
        /// UPDATE
        /// http://stackoverflow.com/questions/9698568/if-exists-update-else-insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 入力値の検証
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("IDを入力してください。");
                return;
            }

            int output;
            if (!Int32.TryParse(txtID.Text, out output))
            {
                if (output == 0)
                {
                    MessageBox.Show("IDを半角数字で入力してください。");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nameを入力してください。");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Priceを入力してください。");
                return;
            }

            if (!Int32.TryParse(txtPrice.Text, out output))
            {
                if (output == 0)
                {
                    MessageBox.Show("Priceを半角数字で入力してください。");
                    return;
                }
            }


            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;

                //command.CommandText = "INSERT INTO Products (Id, Name, Price) VALUES(@Id, @Name, @Price)";
                //command.CommandText = "INSERT INTO Products (Id, name, price) VALUES(5, 'hiroshima', 98)";

                command.CommandText =
                "IF NOT EXISTS(SELECT 1 from Products where Id=@Id)" +
                " Insert INTO Products(Id, Name, Price) VALUES(@Id,@Name,@Price)" +
                " else" +
                " UPDATE Products SET Id=@Id,Name=@Name,Price=@Price WHERE Id=@Id";

                command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtID.Text));
                command.Parameters.AddWithValue("@Name", txtName.Text);
                command.Parameters.AddWithValue("@Price", Convert.ToInt32(txtPrice.Text));

                try
                {
                    connection.Open();
                    int num = command.ExecuteNonQuery();
                    if (num != -1)
                    {
                        MessageBox.Show("UPDATEしました。");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 入力値の検証
            if (string.IsNullOrWhiteSpace(txtDelID.Text))
            {
                MessageBox.Show("IDを入力してください。");
                return;
            }

            int output;
            if (!Int32.TryParse(txtDelID.Text, out output))
            {
                if (output == 0)
                {
                    MessageBox.Show("IDを半角数字で入力してください。");
                    return;
                }
            }

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;


                command.CommandText =
                "IF EXISTS(SELECT 1 from Products where Id=@Id)" +
                " DELETE Products WHERE Id=@Id";

                command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtDelID.Text));


                try
                {
                    connection.Open();
                     int num = command.ExecuteNonQuery();
                    if (num != -1)
                    {
                        MessageBox.Show("DELETEしました。");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
