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
    public partial class EditAnnouncementForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int announcementid;
        string announcementcontent,announcementtitle;
        public EditAnnouncementForm(int a,string t,string c)
        {
            this.announcementid = a;
            this.announcementtitle = t;
            this.announcementcontent = c;
            cn = new SqlConnection(dbcon.MyConnection());
            textBox1.Text = t;
            textBox2.Text = c;  
            InitializeComponent();
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
                cm = new SqlCommand("UPDATE announcement SET AnnouncementTitle = @announcementtitle AND AnnouncementContent = @announcementcontent WHERE AnnouncementId=@announcementid", cn);
                cm.Parameters.AddWithValue("@announcementtitle", textBox1.Text);
                cm.Parameters.AddWithValue("@announcementcontent", textBox2.Text);
                cm.Parameters.AddWithValue("@announcementid", announcementid);
                int row= cm.ExecuteNonQuery();
                cn.Close();
                if (row > 0)
                {
                    MessageBox.Show("Announcement updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unable to update Announcement.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private void EditAnnouncementForm_Load(object sender, EventArgs e)
        {

        }
    }
}
