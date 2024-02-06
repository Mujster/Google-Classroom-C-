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
    public partial class LoginForm : Form
    {
        SqlConnection cn=new SqlConnection();
        SqlCommand cm=new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        public string email = "", password = "",name="";
        public int userid;
        public LoginForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
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
                cn.Open();
                cm = new SqlCommand("SELECT * FROM student WHERE StudentEmail = @email AND StudentPassword = @password", cn);
                cm.Parameters.AddWithValue("@email", textBox1.Text);
                cm.Parameters.AddWithValue("@password", textBox2.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    userid = Convert.ToInt32(dr["StudentId"]);
                    email = dr["StudentEmail"].ToString();
                    password = dr["StudentPassword"].ToString();
                    student form = new student(userid, email, password);
                    this.Hide();
                    form.Show();
                    cn.Close();    
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password. Access Denied");
                }
                dr.Close();
                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(StudentId) FROM student", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                cm = new SqlCommand("INSERT INTO student (StudentId,StudentName,StudentEmail, StudentPassword) VALUES (@count,@name,@email, @password)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@name", textBox5.Text);
                cm.Parameters.AddWithValue("@email", textBox1.Text);
                cm.Parameters.AddWithValue("@password", textBox2.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Registration Successful!");
                textBox1.Clear();
                textBox2.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT * FROM teacher WHERE TeacherEmail= @email AND TeacherPassword = @password", cn);
                cm.Parameters.AddWithValue("@email", textBox3.Text);
                cm.Parameters.AddWithValue("@password", textBox4.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    userid = Convert.ToInt32(dr["TeacherId"]);
                    email = dr["TeacherEmail"].ToString();
                    password = dr["TeacherPassword"].ToString();
                    teacher form = new teacher(userid,email,password);
                    this.Hide();
                    form.Show();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password. Access Denied");
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(TeacherId) FROM teacher", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                cm = new SqlCommand("INSERT INTO teacher (TeacherId,TeacherName,TeacherEmail, TeacherPassword) VALUES (@count,@name,@email, @password)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@name", textBox6.Text);
                cm.Parameters.AddWithValue("@email", textBox3.Text);
                cm.Parameters.AddWithValue("@password", textBox4.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Registration Successful!");
                textBox3.Clear();
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }
    }
}
