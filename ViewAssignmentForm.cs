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
    public partial class ViewAssignmentForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int id;
        string title;
        public ViewAssignmentForm(int a,string n)
        {
            this.id = a;    
            this.title = n;
            cn = new SqlConnection(dbcon.MyConnection());
            label1.Text=n;
            InitializeComponent();
            load();
        }
        private void load()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT AssignmentContent FROM assignment WHERE AssignmentId=@id", cn);
                cm.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("AssignmentContent", 200);
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["AssignmentContent"].ToString());
                    listView1.Items.Add(item);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ViewAssignmentForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
