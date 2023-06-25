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
    public partial class student : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        int studentId,classId;
        string email, password;
        string classcode = "";

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ClassId FROM class WHERE ClassCode = @classcode", cn);
                cm.Parameters.AddWithValue("@classcode", textBox1.Text);
                dr=cm.ExecuteReader();
                //dr.Read();
                if (dr.HasRows)
                {
                    dr.Read();
                    classId = Convert.ToInt32(dr["ClassId"]);
                    dr.Close();
                    SqlCommand cm_ = new SqlCommand("SELECT * FROM student_class WHERE StudentId = @studentId AND ClassId = @classId", cn);
                    cm_.Parameters.AddWithValue("@studentId",studentId);
                    cm_.Parameters.AddWithValue("@classId",classId);
                    SqlDataReader dr_= cm_.ExecuteReader();
                    if (dr_.HasRows)
                    {
                        MessageBox.Show("Already Enrolled In Class");
                    }
                    else
                    {
                        dr_.Close();
                        SqlCommand _cm = new SqlCommand("INSERT INTO student_class (StudentId, ClassId) VALUES (@studentId, @classId)", cn);
                        _cm.Parameters.AddWithValue("@studentId", studentId);
                        _cm.Parameters.AddWithValue("@classId", classId);
                        _cm.ExecuteNonQuery();
                        MessageBox.Show("Class Enrolled Successfully");
                    }
                    dr_.Close();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Class Does Not Exists");
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (listView1.SelectedItems.Count > 0)
                {
                    string className = listView1.SelectedItems[0].Text;
                    int classId;
                    if (int.TryParse(listView1.SelectedItems[0].SubItems[1].Text, out classId))
                    {
                        classForm cf = new classForm(studentId, classId, className);
                        this.Hide();
                        cf.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid class ID");
                    }
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadEnrolledClasses();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm nf = new LoginForm();
            nf.Show();
        }

        private void LoadEnrolledClasses()
        {
            try
            {
                cn.Open();
                SqlCommand cm_c = new SqlCommand("SELECT class.ClassName, class.ClassId FROM class INNER JOIN student_class ON class.ClassId = student_class.ClassId WHERE student_class.StudentId = @studentId",cn);
                cm_c.Parameters.AddWithValue("@studentId",studentId);
                dr = cm_c.ExecuteReader();
                listView1.Items.Clear();
                while (dr.Read())
                {
                    ListViewItem lv=new ListViewItem(dr["ClassName"].ToString());
                    lv.SubItems.Add(dr["ClassId"].ToString());
                    listView1.Items.Add(lv);
                }
                dr.Close();
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error"+ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        public student(int studentid,string e,string p)
        {
            this.studentId = studentid;
            this.email = e;
            this.password = p;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            LoadEnrolledClasses();
        }

    }
}
