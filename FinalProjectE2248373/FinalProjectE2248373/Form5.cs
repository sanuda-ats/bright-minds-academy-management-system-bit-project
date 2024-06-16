using System;
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
    public partial class Attendance : Form
    {
        public Attendance()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            string query1 = "SELECT class_id, subject, grade, medium FROM Class_Table";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query1, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int classId = reader.GetInt32(0);
                        string subject = reader.GetString(1);
                        int grade = reader.GetInt32(2);
                        string medium = reader.GetString(3);

                        string displayText = $"{classId} - {subject} - Grade {grade} - {medium} Medium";
                        cbClass.Items.Add(displayText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading classes: " + ex.Message);
                }
            }

            string query2 = "SELECT student_id FROM Student_Details_Table";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand studentCmd = new SqlCommand(query2, conn);
                    SqlDataReader studentReader = studentCmd.ExecuteReader();

                    while (studentReader.Read())
                    {
                        int studentId = studentReader.GetInt32(0);
                        cbStudentId.Items.Add(studentId);
                    }
                    studentReader.Close(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading students: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbClass.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a class.");
                return;
            }

            if (cbStudentId.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a student ID.");
                return;
            }

            DateTime attendanceDate = dateTimePickerDate.Value.Date;
            string selectedClass = cbClass.SelectedItem.ToString();
            int studentId = (int)cbStudentId.SelectedItem;

            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            string query = "INSERT INTO Attendance_Table (date, class, student_id) VALUES (@Date, @Class, @StudentId)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", attendanceDate);
                        cmd.Parameters.AddWithValue("@Class", selectedClass);
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Attendance record added successfully.");

                    int currentTotal;
                    if (int.TryParse(labelTotal.Text, out currentTotal))
                    {
                        currentTotal += 1;
                        labelTotal.Text = currentTotal.ToString();
                    }
                    else
                    {
                        labelTotal.Text = "1"; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void btnNewDay_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to start a new day?", "Confirm New Day", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                labelTotal.Text = "0";

                dateTimePickerDate.Value = DateTime.Today;
                cbClass.SelectedIndex = -1;
                cbStudentId.SelectedIndex = -1;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            HomePage homePage = new HomePage();
            homePage.Show();
        }

        private void Attendance_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
