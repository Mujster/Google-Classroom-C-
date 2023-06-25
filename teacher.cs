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
    public partial class teacher : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        int teacherId;
        string email, password;
        public teacher(int t,string e,string p)
        {
            this.teacherId = t;
            this.email = e;
            this.password = p;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            LoadClasses();
        }

        private void teacher_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateClassForm cf=new CreateClassForm(teacherId);
            cf.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf=new LoginForm();
            lf.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadClasses();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (listView1.SelectedItems.Count > 0)
                {
                    string className = listView1.SelectedItems[0].Text;
                    int classId;
                    if (int.TryParse(listView1.SelectedItems[0].SubItems[1].Text, out classId))
                    {
                        TeacherClassForm cf = new TeacherClassForm(teacherId, classId, className);
                        this.Hide();
                        cf.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid class ID");
                    }
                }
        }

        private void LoadClasses()
        {
            try
            {
                listView1.Items.Clear();
                cn.Open();
                cm = new SqlCommand("SELECT ClassName,ClassId FROM class WHERE TeacherId=@teacherId", cn);
                cm.Parameters.AddWithValue("@teacherId", teacherId);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem lv = new ListViewItem(dr["ClassName"].ToString());
                    lv.SubItems.Add(dr["ClassId"].ToString());
                    listView1.Items.Add(lv);
                }
                dr.Close();
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
    }
}
