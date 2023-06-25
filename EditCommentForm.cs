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
    public partial class EditCommentForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        int commentId;
        string commentText;
        public EditCommentForm(int id,string t)
        {
            this.commentId = id;
            this.commentText = t;
            cn = new SqlConnection(dbcon.MyConnection());
            textBox1.Text = t;
            InitializeComponent();
        }
        public bool CommentEdited { get; private set; }
        private void EditCommentForm_Load(object sender, EventArgs e)
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
                    cm = new SqlCommand("UPDATE comment SET CommentContent=@comment WHERE CommentId=@commentId", cn);
                    cm.Parameters.AddWithValue("@comment", textBox1.Text);
                    cm.Parameters.AddWithValue("@commentId", commentId);
                    int row= cm.ExecuteNonQuery();
                    cn.Close();
                    if (row> 0)
                    {
                        CommentEdited = true;
                        MessageBox.Show("Comment updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unable to update comment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
