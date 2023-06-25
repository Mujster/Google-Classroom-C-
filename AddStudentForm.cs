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
    public partial class AddStudentForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int classid;
        public AddStudentForm(int c)
        {
            this.classid = c;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                int studentid = int.Parse(textBox1.Text);
                cm = new SqlCommand("SELECT COUNT(StudentId) FROM student_class WHERE ClassId=@classid AND StudentId=@studentid", cn);
                cm.Parameters.AddWithValue("@classid", classid);
                cm.Parameters.AddWithValue("@studentid", studentid);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                cm = new SqlCommand("SELECT COUNT(studentId) FROM student", cn);
                int temp = Convert.ToInt32(cm.ExecuteScalar());
                if (count == 0&&studentid<=temp)
                {
                    cm = new SqlCommand("INSERT INTO student_class(StudentId,ClassId) VALUES (@studentid,@classid)", cn);
                    cm.Parameters.Clear();
                    cm.Parameters.AddWithValue("@classid", classid);
                    cm.Parameters.AddWithValue("@studentid", studentid);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Student Added Successfully");
                }
                else
                {
                    MessageBox.Show("Student Already Enrolled Or InValid StudentId");
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddStudentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
