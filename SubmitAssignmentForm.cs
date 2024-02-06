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
    public partial class SubmitAssignmentForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int assignmentid,studentid;
        public SubmitAssignmentForm(int a,int s)
        {
            this.assignmentid = a;
            this.studentid = s;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }
        bool AssignmentTurnedIn()
        {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(*) AS CountExists FROM assignment_student WHERE AssignmentId = @assignmentid AND StudentId = @studentid", cn);
                cm.Parameters.AddWithValue("@assignmentid", assignmentid);
                cm.Parameters.AddWithValue("@studentid", studentid);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                cn.Close();
                return (count > 0) ? true : false;
         }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AssignmentTurnedIn())
                {
                    cn.Open();
                    string work = richTextBox1.Text,date="2023-05-16";
                    cm = new SqlCommand("INSERT INTO assignment_student(AssignmentId,StudentId,SubmittedDate,AssignmentWork) VALUES (@assignmentid,@studentid,@date,@work)",cn);
                    cm.Parameters.AddWithValue("@assignmentid",assignmentid);
                    cm.Parameters.AddWithValue("@studentid", studentid);
                    cm.Parameters.AddWithValue("@date", date);
                    cm.Parameters.AddWithValue("@work",work);
                    int row = cm.ExecuteNonQuery();
                    if (row > 0)
                    {
                        MessageBox.Show("Assignment Handed In Successfully");
                    }
                }
                else
                {
                    MessageBox.Show("Assignment Already Submitted");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void ViewAssignmentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
