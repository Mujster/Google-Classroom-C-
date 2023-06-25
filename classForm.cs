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
    public partial class classForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        string className;
        int classId,studentId,id;

        public classForm(int s,int c,string n)
        {
            this.classId = c;
            this.studentId = s;
            this.className = n;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            label1.Text = className;

            LoadComments();
            LoadAnnouncement();
            LoadMaterial();
            LoadAssignment();
        }
        private ContextMenuStrip contextMenuStrip5 = new ContextMenuStrip();
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);// this.pictureBox1, new Point(0, this.pictureBox1.Height));
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ToolStripMenuItem logOutMenuItem = new ToolStripMenuItem("Log Out");
            ToolStripMenuItem unenrollClassMenuItem = new ToolStripMenuItem("Un-Enroll Class");
            contextMenuStrip1.Items.Add(logOutMenuItem);
            contextMenuStrip1.Items.Add(unenrollClassMenuItem);

            // Set event handlers for menu items
            logOutMenuItem.Click += LogOutMenuItem_Click;
            unenrollClassMenuItem.Click += UnenrollClassMenuItem_Click;
        }
        private void LogOutMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.Show();
        }
        private void UnenrollClassMenuItem_Click(object sender, EventArgs e)
        {
            try {
                cn.Open();
                cm = new SqlCommand("DELETE FROM student_class WHERE StudentId = @studentId AND ClassId =@classId", cn);
                cm.Parameters.AddWithValue("@studentId", studentId);
                cm.Parameters.AddWithValue("@classId", classId);
                int row = cm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("You have been unenrolled from the class.");
                
                }
                else
                {
                    MessageBox.Show("Error 404");
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show("Unknown Error Occured"+ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                this.Hide();
                student sf = new student(studentId,"","");
                sf.Show();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(CommentId) FROM comment", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                count += 1;
                string content = textBox1.Text,date="2023-05-19",commenter="Student";
                cm = new SqlCommand("INSERT INTO comment(CommentId,CommentContent, DateCreated, ClassId,CommenterType, TeacherId, StudentId) VALUES (@count,@content,@date,@classId,@commenter,@teacherid,@studentId)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@content", content);
                cm.Parameters.AddWithValue("@date", date);
                cm.Parameters.AddWithValue("@classId", classId);
                cm.Parameters.AddWithValue("@commenter",commenter);
                cm.Parameters.AddWithValue("@teacherid", 10000);
                cm.Parameters.AddWithValue("@studentId",studentId);
                int row=cm.ExecuteNonQuery();
                cn.Close();
                if (row > 0)
                {
                    MessageBox.Show("Comment Added Successfully");
                    LoadComments();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Errofehwrfofwr" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            student sf = new student(studentId, "", "");
            sf.Show();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            
        }
        private void classFrom_Load(object sender, EventArgs e)
        {

        }
        private void LoadComments()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT c.CommentId, c.CommentContent, c.CommenterType, CASE WHEN c.CommenterType = 'student' THEN s.StudentName ELSE t.TeacherName END AS Commenter FROM comment c LEFT JOIN student s ON c.CommenterType = 'student' AND c.StudentId = s.StudentId LEFT JOIN teacher t ON c.CommenterType = 'teacher' AND c.TeacherId = t.TeacherId WHERE c.ClassId = @classId ", cn);
                cm.Parameters.AddWithValue("@classId", classId);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView3.Clear();
                listView3.View = View.Details;
                listView3.Columns.Add("Comment Content", 200);
                listView3.Columns.Add("Commenter", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["CommentContent"].ToString());
                    item.SubItems.Add(row["Commenter"].ToString());
                    item.Tag = row["CommentId"];
                    listView3.Items.Add(item);
                    InitializeMenu();
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadAnnouncement()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT AnnouncementId, AnnouncementTitle, AnnouncementContent, AnnouncementDate FROM Announcement WHERE classId=@classId", cn);
                cm.Parameters.AddWithValue("@classId", classId);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Title", 200);
                listView1.Columns.Add("Content", 500);
                listView1.Columns.Add("Date", 100);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["AnnouncementTitle"].ToString());
                    item.SubItems.Add(row["AnnouncementContent"].ToString());
                    item.SubItems.Add(Convert.ToDateTime(row["AnnouncementDate"]).ToString("yyyy-MM-dd"));
                    listView1.Items.Add(item);
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        bool IsCommentByLoggedInStudent(int commentId,int studentId)
        {
            cn.Open();
            cm = new SqlCommand("SELECT CommentId FROM comment WHERE CommentId = @commentId AND StudentId = @studentId", cn);
            cm.Parameters.AddWithValue("@commentId", commentId);
            cm.Parameters.AddWithValue("@studentId", studentId);

            object result = cm.ExecuteScalar();
            bool isCommentByLoggedInStudent = (result != null && result != DBNull.Value);

            cn.Close();

            return isCommentByLoggedInStudent;
        }
        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView3.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView3.SelectedItems[0];
                int commentId = Convert.ToInt32(selectedItem.Tag);
                int loggedInStudentId = studentId; // Replace this with the actual code to retrieve the logged-in student's ID

                if (IsCommentByLoggedInStudent(commentId, loggedInStudentId))
                {
                    contextMenuStrip5.Show(Cursor.Position);
                }
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView3.SelectedItems[0];
                int commentId = Convert.ToInt32(selectedItem.Tag);
                string commentContent = selectedItem.SubItems[0].Text;
                EditCommentForm editForm = new EditCommentForm(commentId, commentContent);
                editForm.ShowDialog();
                LoadComments();
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView3.SelectedItems[0];
                int commentId = Convert.ToInt32(selectedItem.Tag);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this comment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new SqlCommand("DELETE FROM comment WHERE CommentId = @commentId", cn);
                        cm.Parameters.AddWithValue("@commentId", commentId);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadComments();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            SubmitAssignmentForm vf = new SubmitAssignmentForm(id, studentId);
            vf.Show();
        }

        private void InitializeMenu()
        {
            ToolStripMenuItem editToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem deleteToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem.Text = "Edit";
            deleteToolStripMenuItem.Text = "Delete";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            contextMenuStrip5.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });
            listView3.ContextMenuStrip = contextMenuStrip5;
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView4!=null && listView4.SelectedItems.Count > 0)
            {
                string assignmenttitle = listView4.SelectedItems[0].Text;
                int assignmentid;
                if (listView4.SelectedItems[0].SubItems.Count>1) {
                    if (int.TryParse(listView4.SelectedItems[0].SubItems[1].Text, out assignmentid))
                    {
                        ViewAssignmentForm cf = new ViewAssignmentForm(id, assignmenttitle);
                        this.Hide();
                        cf.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid class ID");
                    } 
                }
                else
                {
                    MessageBox.Show("Invalid Selection");
                }
            }
        }

        private void LoadMaterial()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT m.Title, t.TeacherName FROM material m JOIN teacher t ON m.TeacherId = t.TeacherId JOIN class c ON m.ClassId = c.ClassId WHERE c.ClassId = @classId", cn);
                cm.Parameters.AddWithValue("@classId", classId);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView2.Clear();
                listView2.View = View.Details;
                listView2.Columns.Add("Material Title", 200);
                listView2.Columns.Add("Teacher Name", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["Title"].ToString());
                    item.SubItems.Add(row["TeacherName"].ToString());
                    listView2.Items.Add(item);
                }

                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show("Unknown Error Occured" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
        private void LoadAssignment() 
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT AssignmentId,AssignmentTitle FROM assignment WHERE ClassId=@classId", cn);
                cm.Parameters.AddWithValue("@classId", classId);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView4.Clear();
                listView4.View = View.Details;
                listView4.Columns.Add("AssignmentTitle", 200);
                if (dt.Rows.Count > 0)
                {
                    id = Convert.ToInt32(dt.Rows[0]["AssignmentId"]);
                    foreach (DataRow row in dt.Rows)
                    {
                        ListViewItem item = new ListViewItem(row["AssignmentTitle"].ToString());
                        item.SubItems.Add(row["AssignmentId"].ToString());
                        listView4.Items.Add(item);
                    }
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
    }
}
