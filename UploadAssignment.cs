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
    public partial class UploadAssignment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        int assignmentid,teacherid,classid,assignmentscore;
        DateTime dt;
        string assignmentTitle,assignmentContent;
        public UploadAssignment(int t,int c)
        {
            this.teacherid = t;
            this.classid = c;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                assignmentscore = int.Parse(textBox2.Text);
                assignmentTitle = textBox1.Text;
                assignmentContent = richTextBox1.Text;
                dt = dateTimePicker1.Value;
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(AssignmentId) FROM assignment", cn);
                assignmentid = Convert.ToInt32(cm.ExecuteScalar());
                assignmentid += 1;
                //
                cm = new SqlCommand("SELECT COUNT(DeadlineId) FROM deadline", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                count += 1;
                
                cm = new SqlCommand("INSERT INTO deadline(DeadlineId,AssignmentId,DeadlineDate,TotalScore) VALUES (@count,@assignmentid,@dt,@assignmentscore)", cn);
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("dt", dt);
                cm.ExecuteNonQuery();
                cm.Parameters.Clear();
                cm = new SqlCommand("INSERT INTO assignment (AssignmentId, AssignmentTitle, AssignmentContent, DeadlineId, ClassId, TeacherId) VALUES (@assignmentid, @assignmentTitle, @assignmentContent, @count, @classid, @teacherid)", cn);
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                cm.Parameters.AddWithValue("@assignmentTitle", assignmentTitle);
                cm.Parameters.AddWithValue("@assignmentContent", assignmentContent);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@classid", classid);
                cm.Parameters.AddWithValue("@teacherid", teacherid);
                cm.Parameters.AddWithValue("@assignmentscore", assignmentscore);


                int row = cm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("Assignment Created Successfully");
                }
                else
                {
                    MessageBox.Show("Failed to create assignment");
                }

                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error"+ ex.Message);
            }
        }
    }
}
