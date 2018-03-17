using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace lab1
{
    public partial class Form1 : Form
    {
        private SqlConnection connection = new SqlConnection("Data Source =.\\TIHAMER; Initial Catalog = Fussballverein; Integrated Security = True; MultipleActiveResultSets=true");
        SqlDataAdapter adapt;
        SqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
            //display_db();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
          
        }

        private void display_db()
        {
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from club", connection);
            adapt.Fill(dt);
            dataGridView2.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //conect
            try
            {
                connection.Open();
                MessageBox.Show("Connection Open ! ");
                display_db();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                Console.WriteLine(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //disconnect
            connection.Close();
            MessageBox.Show("Connection Closed ! ");
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow index = this.dataGridView2.Rows[e.RowIndex];
                string key = index.Cells["clubID"].Value.ToString();
                SqlCommand cmd = new SqlCommand(" select * from Fan where Fan.clubID = " + key, connection);
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    BindingSource bind = new BindingSource();
                    bind.DataSource = table;
                    dataGridView1.DataSource = bind;
                    adapter.Update(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void DisplayData()
        {
            connection.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from Fan", connection);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void ClearData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            //ID = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //delete elem
            if (textBox1.Text != "")
            {
                connection.Open();
                string selectString = "SELECT fanID FROM fan WHERE fanID = '" + textBox1.Text + "'";

                SqlCommand myCommand = new SqlCommand(selectString, connection);
                String strResult = String.Empty;
                Object result = myCommand.ExecuteScalar();

                if (result != null)
                {
                    strResult = myCommand.ExecuteScalar().ToString();
                }
                if (strResult.Length != 0)
                {
                    cmd = new SqlCommand("delete Fan where fanID=@fanID", connection);
                    cmd.Parameters.AddWithValue("@fanID", textBox1.Text);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Record Deleted Successfully!");
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("Item don't found ! ");
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //insert elem
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                connection.Open();
                string selectString = "SELECT fanID FROM fan WHERE fanID = '" + textBox1.Text + "'";

                SqlCommand myCommand = new SqlCommand(selectString, connection);
                String strResult = String.Empty;
                Object result = myCommand.ExecuteScalar();
         
                if (result != null)
                {
                    strResult = myCommand.ExecuteScalar().ToString();
                }

                if (strResult.Length == 0)
                {
                    string selectString2 = "SELECT clubID FROM fan WHERE clubID = '" + textBox2.Text + "'";

                    SqlCommand myCommand2 = new SqlCommand(selectString2, connection);
                    String strResult2 = String.Empty;
                    Object result2 = myCommand2.ExecuteScalar();

                    if (result2 != null)
                    {
                        strResult2 = myCommand2.ExecuteScalar().ToString();
                    }

                    if (strResult2.Length != 0)
                    {
                        cmd = new SqlCommand("insert into fan(fanID,clubID,firstName,lastName,country,age) values(@fanID,@clubID,@firstName,@lastName,@country,@age)", connection);
                        cmd.Parameters.AddWithValue("@fanID", textBox1.Text);
                        cmd.Parameters.AddWithValue("@clubID", textBox2.Text);
                        cmd.Parameters.AddWithValue("@firstName", textBox3.Text);
                        cmd.Parameters.AddWithValue("@lastName", textBox4.Text);
                        cmd.Parameters.AddWithValue("@country", textBox5.Text);
                        cmd.Parameters.AddWithValue("@age", textBox6.Text);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Record Inserted Successfully");
                        DisplayData();
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Wrong foreign key :(");
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong primary key :(");
                    connection.Close();
                }
                
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //update elem
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                connection.Open();
                string selectString = "SELECT clubID FROM fan WHERE clubID = '" + textBox2.Text + "'";

                SqlCommand myCommand = new SqlCommand(selectString, connection);
                String strResult = String.Empty;
                Object result = myCommand.ExecuteScalar();

                if (result != null)
                {
                    strResult = myCommand.ExecuteScalar().ToString();
                }

                if (strResult.Length != 0)
                {
                    cmd = new SqlCommand("update Fan set clubID=@clubID,firstName=@firstName,lastName=@lastName,country=@country,age=@age where fanID=@fanID", connection);
                    cmd.Parameters.AddWithValue("@fanID", textBox1.Text);
                    cmd.Parameters.AddWithValue("@clubID", textBox2.Text);
                    cmd.Parameters.AddWithValue("@firstName", textBox3.Text);
                    cmd.Parameters.AddWithValue("@lastName", textBox4.Text);
                    cmd.Parameters.AddWithValue("@country", textBox5.Text);
                    cmd.Parameters.AddWithValue("@age", textBox6.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Updated Successfully");
                    connection.Close();
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("Wrong foreign key ! ");
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }
    }
}
