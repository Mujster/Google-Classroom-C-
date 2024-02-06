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
    public partial class UploadMaterialForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int teacherid, classid;
        public UploadMaterialForm(int t,int c)
        {
            this.teacherid = t;
            this.classid = c;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string title = richTextBox1.Text;
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(MaterialId) FROM material", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                count += 1;
                cm = new SqlCommand("INSERT INTO material(MaterialId,Title,ClassId,TeacherId) VALUES (@count,@title,@classid,@teacherid)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@title", title);
                cm.Parameters.AddWithValue("@classid", classid);
                cm.Parameters.AddWithValue("@teacherid",teacherid);
                int row = cm.ExecuteNonQuery();
                if (row > 0) {
                    MessageBox.Show("Material Uploaded Successfully");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
