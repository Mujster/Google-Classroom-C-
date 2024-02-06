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
    public partial class GradeAssignmentForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        int assignmentid,studentid,teacherid,totalscore;
        public GradeAssignmentForm(int a,int s,int t)
        {
            this.assignmentid = a;
            this.studentid = s;
            this.teacherid = t;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            LoadAssignment();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            EditGradeForm ef = new EditGradeForm(studentid,assignmentid);
            ef.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("SELECT GradeScore FROM grade WHERE StudentId=@studentid AND AssignmentId=@assignmentid", cn);
            cm.Parameters.AddWithValue("@studentid", studentid);
            cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                int score = Convert.ToInt32(cm.ExecuteScalar());
                label1.Text = score.ToString();
            cn.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //try {
            //    cn.Open();
            //    cm = new SqlCommand("", cn);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show("Error" + ex.Message);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AddScore ad = new AddScore(totalscore,studentid,assignmentid);
            ad.Show();
        }

        private void ViewAssignmentForm_Load(object sender, EventArgs e)
        {

        }
        private void LoadAssignment()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT GradeScore FROM grade WHERE StudentId=@studentid AND AssignmentId=@assignmentid", cn);
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                cm.Parameters.AddWithValue("@studentid", @studentid);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    string grade = dr["GradeScore"].ToString();
                    label1.Text = grade;
                }
                dr.Close();
                cm.Parameters.Clear();
                cm = new SqlCommand("SELECT a.AssignmentTitle,a.TotalScore,AssignmentWork FROM assignment_student AS s INNER JOIN assignment AS a ON s.AssignmentId = a.AssignmentId WHERE s.AssignmentId = @assignmentid AND s.StudentId = @studentid AND a.TeacherId = @teacherid; ", cn);
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                cm.Parameters.AddWithValue("@studentid", @studentid);
                cm.Parameters.AddWithValue("@teacherid", @teacherid);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string assignmenttitle = dr["AssignmentTitle"].ToString();
                    string assignmentwork = dr["AssignmentWork"].ToString();
                    string assignmentscore = dr["TotalScore"].ToString();
                    label5.Text = assignmenttitle;
                    label2.Text = "  "+assignmentscore;
                    totalscore = int.Parse(assignmentscore);
                    ListViewItem item = new ListViewItem(assignmentwork);
                    listView1.Items.Add(item);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
    }
}
