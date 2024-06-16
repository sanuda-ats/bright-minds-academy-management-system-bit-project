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
    public partial class TeacherDetails : Form
    {
        public TeacherDetails()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TeacherDetails_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            string query = "SELECT teacher_id FROM Teacher_Table";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        cbTeacherId.Items.Add(dr["teacher_id"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cbTeacherId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTeacherId.SelectedIndex == -1) return;

            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";
            int teacherId = int.Parse(cbTeacherId.SelectedItem.ToString());
            string query = "SELECT * FROM Teacher_Table WHERE teacher_id = @TeacherId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtFName.Text = reader["first_name"].ToString();
                            txtLName.Text = reader["last_name"].ToString();
                            dateTimePickerDOB.Value = DateTime.Parse(reader["date_of_birth"].ToString());
                            if (reader["gender"].ToString() == "Male")
                            {
                                rbMale.Checked = true;
                            }
                            else
                            {
                                rbFemale.Checked = true;
                            }
                            txtAddress.Text = reader["address"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            txtMphone.Text = reader["mobile_no"].ToString();
                            txtHphone.Text = reader["home_no"].ToString();
                            txtSubject.Text = reader["subject"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            string firstName = txtFName.Text;
            string lastName = txtLName.Text;
            string dateOfBirth = dateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            string gender = rbMale.Checked ? "Male" : "Female";
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string subject = txtSubject.Text;

            if (!long.TryParse(txtMphone.Text, out long mobilePhone) || !long.TryParse(txtHphone.Text, out long homePhone))
            {
                MessageBox.Show("Please enter valid phone numbers.");
                return;
            }

            string query = "INSERT INTO Teacher_Table (first_name, last_name, date_of_birth, gender, address, email, mobile_no, home_no, subject) " +
                           "VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Address, @Email, @MobilePhone, @HomePhone, @Subject)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@MobilePhone", mobilePhone);
                        cmd.Parameters.AddWithValue("@HomePhone", homePhone);
                        cmd.Parameters.AddWithValue("@Subject", subject);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Added Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            if (cbTeacherId.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a teacher ID.");
                return;
            }

            int teacherId = int.Parse(cbTeacherId.SelectedItem.ToString());
            string firstName = txtFName.Text;
            string lastName = txtLName.Text;
            string dateOfBirth = dateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            string gender = rbMale.Checked ? "Male" : "Female";
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string subject = txtSubject.Text;

            if (!long.TryParse(txtMphone.Text, out long mobilePhone) || !long.TryParse(txtHphone.Text, out long homePhone))
            {
                MessageBox.Show("Please enter valid phone numbers.");
                return;
            }

            string query = "UPDATE Teacher_Table SET first_name = @FirstName, last_name = @LastName, date_of_birth = @DateOfBirth, " +
                           "gender = @Gender, address = @Address, email = @Email, mobile_no = @MobilePhone, home_no = @HomePhone, subject = @Subject " +
                           "WHERE teacher_id = @TeacherId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@MobilePhone", mobilePhone);
                        cmd.Parameters.AddWithValue("@HomePhone", homePhone);
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Updated Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True";

            if (cbTeacherId.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a teacher ID.");
                return;
            }

            int teacherId = int.Parse(cbTeacherId.SelectedItem.ToString());
            string query = "DELETE FROM Teacher_Table WHERE teacher_id = @TeacherId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Record Deleted Successfully");
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
            string query = @"
                SELECT c.class_id, c.grade, c.subject, c.class_day, c.start_time, c.end_time
                FROM Teacher_Table t
                JOIN Teacher_Classes_Table tc ON t.teacher_id = tc.teacher_id
                JOIN Class_Table c ON tc.class_id = c.class_id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvClassesAssigned.DataSource = dataTable;
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
    }
}
