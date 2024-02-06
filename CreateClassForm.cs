using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectDB2
{
    public partial class CreateClassForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        int teacherId;
        string name, code;
        public CreateClassForm(int t)
        {
            this.teacherId = t;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void CreateClassForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                name = textBox1.Text;
                code = textBox2.Text;

                cn.Open();
                cm = new SqlCommand("SELECT COUNT(*) FROM Class WHERE ClassCode=@classCode", cn);
                cm.Parameters.AddWithValue("@classCode", code);
                int check = Convert.ToInt32(cm.ExecuteScalar());
                if (check > 0)
                {
                    MessageBox.Show("Class With Same ClassCode Already Exists");
                }
                else
                {
                    cm.Parameters.Clear();
                    cm = new SqlCommand("SELECT COUNT(ClassId) FROM class", cn);
                    int count = Convert.ToInt32(cm.ExecuteScalar());
                    count += 1;

                    cm = new SqlCommand("INSERT INTO class(ClassId,ClassName,TeacherId,ClassCode) VALUES (@count,@name,@teacherId,@code)", cn);
                    cm.Parameters.AddWithValue("@count", count);
                    cm.Parameters.AddWithValue("@name", name);
                    cm.Parameters.AddWithValue("@teacherId", teacherId);
                    cm.Parameters.AddWithValue("@code", code);

                    int row = cm.ExecuteNonQuery();
                    if (row > 0)
                    {
                        MessageBox.Show("Class Created Successfully");
                        this.Hide(); 
                    }
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed To Create A New Class. Error: " + ex.Message);
            }
        }
    }
}
