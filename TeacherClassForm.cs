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
    public partial class TeacherClassForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBconnection dbcon = new DBconnection();
        SqlDataReader dr;
        string className,AnnouncementContent;
        int classid, teacherid,studentid;
        private ContextMenuStrip contextMenuStripComments = new ContextMenuStrip();
        private ContextMenuStrip contextMenuStripAnnouncement= new ContextMenuStrip();
        private ContextMenuStrip contextMenuStripStudents = new ContextMenuStrip();
        public TeacherClassForm(int t,int c,string name)
        {
            this.teacherid = t;
            this.classid = c;
            this.className = name;
            cn = new SqlConnection(dbcon.MyConnection());
            InitializeComponent();
            label1.Text = name;
            LoadAnnouncements();
            LoadAssignments();
            LoadComments();
            LoadMaterial();
            LoadStudents();
            InitializeMenuComments();
            InitializeMenuStudents();
            InitializeMenuAnnouncment();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ToolStripMenuItem dashboardToolStripMenuItem = new ToolStripMenuItem("DashBoard Class");
            ToolStripMenuItem logOutMenuItem = new ToolStripMenuItem("Log Out");
            ToolStripMenuItem deleteclassMenuItem = new ToolStripMenuItem("Delete Class");
            contextMenuStrip1.Items.Add(logOutMenuItem);
            contextMenuStrip1.Items.Add(dashboardToolStripMenuItem);
            contextMenuStrip1.Items.Add(deleteclassMenuItem);

            logOutMenuItem.Click += LogOutMenuItem_Click;
            dashboardToolStripMenuItem.Click += dashboardToolStripMenuItem_Click;
            deleteclassMenuItem.Click += deleteclassMenuItem_click;
        }
        private void deleteclassMenuItem_click(object sender, EventArgs e)
        {
            cn.Open();
            cm=new SqlCommand("DELETE FROM announcement WHERE ClassId=@classid",cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();
            cm = new SqlCommand("DELETE FROM comment WHERE ClassId=@classid", cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();
        
            cm = new SqlCommand("DELETE FROM assignment WHERE ClassId=@classid", cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("DELETE FROM material WHERE ClassId=@classid", cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();
            cm = new SqlCommand("DELETE FROM student_class WHERE ClassId=@classid", cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();
            cm = new SqlCommand("DELETE FROM class WHERE ClassId=@classid", cn);
            cm.Parameters.AddWithValue("@classid", classid);
            cm.ExecuteNonQuery();
            cn.Close();
            this.Hide();
            teacher tc = new teacher(teacherid, "", "");
            tc.Show();
        }
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            teacher tc = new teacher(teacherid, "", "");
            tc.Show();
        }
        private void LogOutMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            UploadAssignment nf = new UploadAssignment(teacherid,classid);
            nf.Show();
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView3.Items.Count > 0)
                {
                    string assignmenttitle = listView3.SelectedItems[0].Text;
                    int assignmentid;
                    if (int.TryParse(listView3.SelectedItems[0].SubItems[1].Text, out assignmentid))
                    {
                        int selectedid = Convert.ToInt32(listView3.SelectedItems[0].SubItems[2].Text);
                        GradeAssignmentForm gf = new GradeAssignmentForm(assignmentid, selectedid, teacherid);
                        gf.Show();
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void LoadAssignments()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT a.AssignmentId, st.StudentName,st.StudentId, a.AssignmentTitle FROM assignment_student AS s INNER JOIN assignment AS a ON s.AssignmentId = a.AssignmentId INNER JOIN student AS st ON s.StudentId = st.StudentId WHERE a.ClassId = @classid AND a.TeacherId = @teacherid ", cn);
                cm.Parameters.AddWithValue("@teacherid", teacherid);
                cm.Parameters.AddWithValue("@classid", classid);
                listView3.Items.Clear();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    int assignmentId = Convert.ToInt32(dr["AssignmentId"]);
                    studentid = Convert.ToInt32(dr["StudentId"]);
                    string assignmentTitle = dr["AssignmentTitle"].ToString();
                    string student = dr["StudentName"].ToString();
                    ListViewItem item = new ListViewItem(student);
                    item.SubItems.Add(assignmentId.ToString());
                    item.SubItems.Add(studentid.ToString());
                    item.SubItems.Add(assignmentTitle);
                    listView3.Items.Add(item);
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error"+ex.Message);    
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

        private void LoadAnnouncements()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT AnnouncementId, AnnouncementTitle, AnnouncementContent, AnnouncementDate FROM Announcement WHERE classId=@classid", cn);
                cm.Parameters.AddWithValue("@classid", classid);
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
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView3.SelectedItems[0];
                int AnnouncementId = Convert.ToInt32(selectedItem.Tag);
                string AnnouncementContent = selectedItem.SubItems[0].Text;
                string AnnouncementTitle = selectedItem.SubItems[0].Text;   
                EditAnnouncementForm editForm = new EditAnnouncementForm(AnnouncementId,AnnouncementTitle,AnnouncementContent);
                editForm.ShowDialog();
                LoadAnnouncements();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(AnnouncementId) FROM announcement", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                count += 1;
                AnnouncementContent = textBox2.Text;
                string time = "2023-05-10";
                int studentid = 0;
                cm = new SqlCommand("INSERT INTO announcement(AnnouncementId, AnnouncementContent, AnnouncementTitle, ClassId, TeacherId, AnnouncementDate) VALUES (@count,@AnnouncementContent,@AnnouncementContent,@classid,@teacherid,@time)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@classid", classid);
                cm.Parameters.AddWithValue("@AnnouncementContent", AnnouncementContent);
                cm.Parameters.AddWithValue("@studentid", studentid);
                cm.Parameters.AddWithValue("@teacherid", teacherid);
                cm.Parameters.AddWithValue("@time", time);

                int row = cm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("Announcement inserted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to insert announcement.");
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                if(cn.State== ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView3.SelectedItems[0];
                int AnnouncementId = Convert.ToInt32(selectedItem.Tag);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this comment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new SqlCommand("DELETE FROM announcement WHERE AnnouncementId=@AnnouncementId", cn);
                        cm.Parameters.AddWithValue("@AnnouncementId", AnnouncementId);
                        cm.ExecuteNonQuery();
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
            }
        }
        private void InitializeMenuAnnouncment()
        {
            ToolStripMenuItem editToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem deleteToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem.Text = "Edit";
            deleteToolStripMenuItem.Text = "Delete";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            contextMenuStripAnnouncement.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });
            listView1.ContextMenuStrip = contextMenuStripAnnouncement;
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT COUNT(CommentId) FROM comment", cn);
                int count = Convert.ToInt32(cm.ExecuteScalar());
                count += 1;
                string content = textBox1.Text, date = "2023-05-19", commenter = "Teacher";
                cm = new SqlCommand("INSERT INTO comment(CommentId,CommentContent, DateCreated, ClassId,CommenterType, TeacherId, StudentId) VALUES (@count,@content,@date,@classid,@commenter,@teacherid,@studentid)", cn);
                cm.Parameters.AddWithValue("@count", count);
                cm.Parameters.AddWithValue("@content", content);
                cm.Parameters.AddWithValue("@date", date);
                cm.Parameters.AddWithValue("@classid", classid);
                cm.Parameters.AddWithValue("@commenter", commenter);
                cm.Parameters.AddWithValue("@teacherid", teacherid);
                cm.Parameters.AddWithValue("@studentid", 10000);
                int row = cm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("Comment Added Successfully");
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        private void editToolStripMenuItemC_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView2.SelectedItems[0];
                int commentId = Convert.ToInt32(selectedItem.Tag);
                string commentContent = selectedItem.SubItems[0].Text;
                EditCommentForm editForm = new EditCommentForm(commentId, commentContent);
                editForm.ShowDialog();
                LoadComments();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            UploadMaterialForm mf = new UploadMaterialForm(teacherid, classid);
            mf.Show();
        }

        private void TeacherClassForm_Load(object sender, EventArgs e)
        {

        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItemC_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView2.SelectedItems[0];
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
                    finally
                    {
                        if(cn.State==ConnectionState.Open)
                            cn.Close();
                    }
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AddStudentForm ad = new AddStudentForm(classid);
            ad.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            LoadAnnouncements();
            LoadAssignments();
            LoadComments();
            LoadMaterial();
            LoadStudents();
        }
        private void deleteToolStripMenuItemS_Click(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView5.SelectedItems[0];
                studentid = Convert.ToInt32(selectedItem.Tag);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new SqlCommand("DELETE FROM student_class WHERE StudentId= @studentid AND ClassId=@classid", cn);
                        cm.Parameters.AddWithValue("@studentid", studentid);
                        cm.Parameters.AddWithValue("@classid", classid);
                        int row=cm.ExecuteNonQuery();
                        if (row > 0)
                        {
                            MessageBox.Show("Student Deleted");
                        }
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if(cn.State==ConnectionState.Open)
                            cn.Close();
                    }
                }
            }
        }

        private void InitializeMenuStudents()
        {
            ToolStripMenuItem deleteToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItemS_Click;
            contextMenuStripStudents.Items.AddRange(new ToolStripItem[] {deleteToolStripMenuItem });
            listView5.ContextMenuStrip = contextMenuStripStudents;
        }
        private void InitializeMenuComments()
        {
            ToolStripMenuItem editToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem deleteToolStripMenuItem = new ToolStripMenuItem();

            editToolStripMenuItem.Text = "Edit";
            deleteToolStripMenuItem.Text = "Delete";
            editToolStripMenuItem.Click += editToolStripMenuItemC_Click;
            deleteToolStripMenuItem.Click += deleteToolStripMenuItemC_Click;

            contextMenuStripComments.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });

            listView2.ContextMenuStrip = contextMenuStripComments;
        }
        private void LoadComments()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT c.CommentId, c.CommentContent, c.CommenterType, CASE WHEN c.CommenterType = 'student' THEN s.StudentName ELSE t.TeacherName END AS Commenter FROM comment c LEFT JOIN student s ON c.CommenterType = 'student' AND c.StudentId = s.StudentId LEFT JOIN teacher t ON c.CommenterType = 'teacher' AND c.TeacherId = t.TeacherId WHERE c.ClassId = @classid ", cn);
                cm.Parameters.AddWithValue("@classid", classid);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView2.Clear();
                listView2.View = View.Details;
                listView2.Columns.Add("Comment Content", 200);
                listView2.Columns.Add("Commenter", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["CommentContent"].ToString());
                    item.SubItems.Add(row["Commenter"].ToString());
                    item.Tag = row["CommentId"];
                    listView2.Items.Add(item);
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
        private void LoadMaterial()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT m.Title, t.TeacherName FROM material m JOIN teacher t ON m.TeacherId = t.TeacherId JOIN class c ON m.ClassId = c.ClassId WHERE c.ClassId = @classid", cn);
                cm.Parameters.AddWithValue("@classid", classid);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                listView4.Clear();
                listView4.View = View.Details;
                listView4.Columns.Add("Material Title", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["Title"].ToString());
                    listView4.Items.Add(item);
                }
                cn.Close();
            }
            catch (Exception ex)
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
        private void LoadStudents()
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT s.StudentId, s.StudentName FROM student s JOIN student_class ON s.StudentId = student_class.StudentId WHERE student_class.ClassId = @classid", cn);
                cm.Parameters.AddWithValue("@classid", classid);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                listView5.Clear();
                listView5.View = View.Details;
                listView5.Columns.Add("Student ID", 100);
                listView5.Columns.Add("Student Name", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["StudentId"].ToString());
                    item.SubItems.Add(row["StudentName"].ToString());
                    listView5.Items.Add(item);
                }
                cn.Close();
            }
            catch (Exception ex)
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
