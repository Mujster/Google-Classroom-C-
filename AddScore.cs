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
    public partial class AddScore : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int totalscore,studentid,assignmentid;
        public AddScore(int t,int s,int a)
        {
            this.totalscore = t;
            this.studentid = s; 
            this.assignmentid = a;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddScore_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = int.Parse(textBox1.Text);
                if (temp > totalscore)
                {
                    MessageBox.Show("Error Score Can Not Be Greater Than Total Score");
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("SELECT COUNT(GradeId) FROM grade", cn);
                    int count = Convert.ToInt32(cm.ExecuteScalar());
                    count += 1;
                    cm = new SqlCommand("INSERT INTO grade(GradeId,StudentId,AssignmentId,GradeScore) VALUES (@count,@studentid,@assignmentid,@temp)", cn);
                    cm.Parameters.AddWithValue("@count", count);
                    cm.Parameters.AddWithValue("@studentid", studentid);
                    cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                    cm.Parameters.AddWithValue("@temp", temp);
                    int rows=cm.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Successfully Graded");
                        this.Hide();
                    }
                    cn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
    }
}
