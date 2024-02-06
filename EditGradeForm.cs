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
    public partial class EditGradeForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int studentid, assignmentid;
        public EditGradeForm(int s,int a)
        {
            this.studentid = s;
            this.assignmentid = a;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void EditGradeForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                int score = int.Parse(textBox1.Text);
                cm = new SqlCommand("UPDATE grade SET GradeScore=@score WHERE StudentId=@studentid AND AssignmentId=@assignmentid",cn);
                cm.Parameters.AddWithValue("@score", score);
                cm.Parameters.AddWithValue("@studentid",studentid );
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                int row = cm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("New Grade Added");
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
    }
}
