using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectE2248373
{
    public partial class ClassDetailsPage : Form
    {
        public ClassDetailsPage()
        {
            InitializeComponent();
        }

        private void ClassDetailsPage_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            string query1 = "SELECT class_id FROM Class_Table";
            string query2 = "SELECT teacher_id, first_name, last_name FROM Teacher_Table";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da1 = new SqlDataAdapter(query1, conn);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);

                    foreach (DataRow dr in dt1.Rows)
                    {
                        cbClassId.Items.Add(dr["class_id"].ToString());
                    }

                    SqlCommand cmd = new SqlCommand(query2, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int teacherId = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string displayText = $"{teacherId} - {firstName} {lastName}";

                        cbTeacher.Items.Add(displayText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cbClassId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClassId.SelectedIndex == -1) return;

            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            int classId = int.Parse(cbClassId.SelectedItem.ToString().Split(' ')[0]); // Ensure correct parsing of classId if the ComboBox item includes additional text
            string query = @"
        SELECT c.*, t.first_name, t.last_name 
        FROM Class_Table c
        JOIN Teacher_Table t ON c.teacher_id = t.teacher_id
        WHERE c.class_id = @ClassId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassId", classId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtSubject.Text = reader["subject"].ToString();
                            txtGrade.Text = reader["grade"].ToString();
                            cbMedium.SelectedItem = reader["medium"].ToString();
                            txtHallNo.Text = reader["hall_no"].ToString();
                            cbDay.SelectedItem = reader["class_day"].ToString();

                            // Debug: Display the raw time values retrieved
                            string rawStartTime = reader["start_time"].ToString();
                            string rawEndTime = reader["end_time"].ToString();

                            // Parse and set DateTimePicker values
                            if (TimeSpan.TryParse(rawStartTime, out TimeSpan startTime) && TimeSpan.TryParse(rawEndTime, out TimeSpan endTime))
                            {
                                dateTimePickerStart.Value = DateTime.Today.Add(startTime);
                                dateTimePickerFinish.Value = DateTime.Today.Add(endTime);
                            }
                            else
                            {
                                MessageBox.Show($"Error parsing time values: start_time = {rawStartTime}, end_time = {rawEndTime}");
                                return;
                            }

                            // Construct the teacher display value
                            int teacherId = int.Parse(reader["teacher_id"].ToString());
                            string teacherName = $"{teacherId} - {reader["first_name"]} {reader["last_name"]}";

                            // Find and select the item in cbTeacher
                            bool teacherFound = false;
                            foreach (var item in cbTeacher.Items)
                            {
                                if (item.ToString() == teacherName)
                                {
                                    cbTeacher.SelectedItem = item;
                                    teacherFound = true;
                                    break;
                                }
                            }

                            if (!teacherFound)
                            {
                                MessageBox.Show("Teacher not found in ComboBox: " + teacherName);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No class found with the specified class_id.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            string subject = txtSubject.Text;
            int grade;
            if (!int.TryParse(txtGrade.Text, out grade))
            {
                MessageBox.Show("Please enter a valid grade.");
                return;
            }

            string classDay = cbDay.Text;
            int hallNo;
            if (!int.TryParse(txtHallNo.Text, out hallNo))
            {
                MessageBox.Show("Please enter a valid hall number.");
                return;
            }

            TimeSpan startTime = dateTimePickerStart.Value.TimeOfDay;
            TimeSpan finishTime = dateTimePickerFinish.Value.TimeOfDay;
            string medium = cbMedium.Text;

            if (cbTeacher.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a teacher.");
                return;
            }

            int teacherId;
            try
            {
                teacherId = int.Parse(cbTeacher.SelectedItem.ToString().Split(' ')[0]);
            }
            catch
            {
                MessageBox.Show("Invalid teacher selection.");
                return;
            }

            string query = "INSERT INTO Class_Table (subject, grade, medium, class_day, start_time, end_time, hall_no, teacher_id) " +
                           "VALUES (@Subject, @Grade, @Medium, @ClassDay, @StartTime, @EndTime, @HallNo, @TeacherId)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@Grade", grade);
                        cmd.Parameters.AddWithValue("@Medium", medium);
                        cmd.Parameters.AddWithValue("@ClassDay", classDay);
                        cmd.Parameters.AddWithValue("@StartTime", startTime);
                        cmd.Parameters.AddWithValue("@EndTime", finishTime);
                        cmd.Parameters.AddWithValue("@HallNo", hallNo);
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Added Successfully");

                    txtSubject.Clear();
                    txtGrade.Clear();
                    txtHallNo.Clear();
                    cbDay.SelectedIndex = -1;
                    cbMedium.SelectedIndex = -1;
                    cbTeacher.SelectedIndex = -1;
                    dateTimePickerStart.Value = DateTime.Now.Date + new TimeSpan(8, 0, 0); 
                    dateTimePickerFinish.Value = DateTime.Now.Date + new TimeSpan(10, 0, 0); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            if (cbClassId.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a class ID.");
                return;
            }

            int classId;
            try
            {
                classId = int.Parse(cbClassId.SelectedItem.ToString().Split(' ')[0]); // Ensure correct parsing if ComboBox item includes additional text
            }
            catch
            {
                MessageBox.Show("Invalid class selection.");
                return;
            }

            string subject = txtSubject.Text;
            int grade;
            if (!int.TryParse(txtGrade.Text, out grade))
            {
                MessageBox.Show("Please enter a valid grade.");
                return;
            }

            string medium = cbMedium.SelectedItem.ToString();
            string classDay = cbDay.SelectedItem.ToString();
            int hallNo;
            if (!int.TryParse(txtHallNo.Text, out hallNo))
            {
                MessageBox.Show("Please enter a valid hall number.");
                return;
            }
            TimeSpan startTime = dateTimePickerStart.Value.TimeOfDay;
            TimeSpan finishTime = dateTimePickerFinish.Value.TimeOfDay;
            int teacherId;
            try
            {
                teacherId = int.Parse(cbTeacher.SelectedItem.ToString().Split(' ')[0]);
            }
            catch
            {
                MessageBox.Show("Invalid teacher selection.");
                return;
            }

            string query = "UPDATE Class_Table SET subject = @Subject, grade = @Grade, medium = @Medium, class_day = @ClassDay, " +
                           "start_time = @StartTime, end_time = @EndTime, hall_no = @HallNo, teacher_id = @TeacherId " +
                           "WHERE class_id = @ClassId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@Grade", grade);
                        cmd.Parameters.AddWithValue("@Medium", medium);
                        cmd.Parameters.AddWithValue("@ClassDay", classDay);
                        cmd.Parameters.AddWithValue("@StartTime", startTime);
                        cmd.Parameters.AddWithValue("@EndTime", finishTime);
                        cmd.Parameters.AddWithValue("@HallNo", hallNo);
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        cmd.Parameters.AddWithValue("@ClassId", classId);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Updated Successfully");

                    txtSubject.Clear();
                    txtGrade.Clear();
                    txtHallNo.Clear();
                    cbDay.SelectedIndex = -1;
                    cbMedium.SelectedIndex = -1;
                    cbTeacher.SelectedIndex = -1;
                    dateTimePickerStart.Value = DateTime.Now.Date + new TimeSpan(8, 0, 0);
                    dateTimePickerFinish.Value = DateTime.Now.Date + new TimeSpan(10, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            if (cbClassId.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Class ID.");
                return;
            }

            int classId;
            if (!int.TryParse(cbClassId.SelectedItem.ToString().Split(' ')[0], out classId))
            {
                MessageBox.Show("Invalid class selection.");
                return;
            }

            string query = "DELETE FROM Class_Table WHERE class_id = @ClassId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassId", classId);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Deleted Successfully");

                    txtSubject.Clear();
                    txtGrade.Clear();
                    txtHallNo.Clear();
                    cbDay.SelectedIndex = -1;
                    cbMedium.SelectedIndex = -1;
                    cbTeacher.SelectedIndex = -1;
                    dateTimePickerStart.Value = DateTime.Now.Date + new TimeSpan(8, 0, 0);
                    dateTimePickerFinish.Value = DateTime.Now.Date + new TimeSpan(10, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnShowClasses_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            string query = "SELECT class_id, subject, grade, hall_no, class_day, start_time, end_time, medium, teacher_id FROM Class_Table";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvAllClasses.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            HomePage homePage = new HomePage();
            homePage.Show();
        }

        private void ClassDetailsPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
                Login_Form login_Form = new Login_Form();
                login_Form.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void dateTimePickerFinish_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

