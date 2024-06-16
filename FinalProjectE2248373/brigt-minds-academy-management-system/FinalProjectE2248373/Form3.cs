using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectE2248373
{
    public partial class StudentDetails : Form
    {
        public StudentDetails()
        {
            InitializeComponent();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void StudentDetails_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Student_Details_Table";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    cbRegNo.Items.Add(dr["student_id"].ToString());
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Student_Details_Table";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach(DataRow dr in dt.Rows)
                {
                    txtFName.Text = dr["first_name"].ToString();
                    txtLName.Text = dr["last_name"].ToString();
                    dateTimePickerDOB.Text = dr["date_of_birth"].ToString();
                    txtAddress.Text = dr["address"].ToString();
                    txtEmail.Text = dr["email"].ToString();
                    txtMphone.Text = dr["mobile_no"].ToString();
                    txtHphone.Text = dr["home_no"].ToString();
                    txtPName.Text = dr["parent_name"].ToString();
                    txtNIC.Text = dr["nic"].ToString();
                    txtPCNumber.Text = dr["parent_contact"].ToString();
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");
            conn.Open();
            String firstName = txtFName.Text;
            String lastName = txtLName.Text;
            String dateOfBirth = dateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            String gender;
            if (rbMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            int mobilePhone = int.Parse(txtMphone.Text);
            int homePhone = int.Parse(txtHphone.Text);
            string parentName = txtPName.Text;
            string nic = txtNIC.Text;
            int contactNo = int.Parse(txtPCNumber.Text);

            string Query = "Insert INTO Student_Details_Table (first_name, last_name, date_of_birth, gender, address, email, mobile_no, home_no, parent_name, nic, parent_contact) VALUES ('" + firstName + "', '" + lastName + "', '" + dateOfBirth + "', '" + gender + "', '" + address + "', '" + email + "'," + mobilePhone + ", " + homePhone + ", '" + parentName + "', '" + nic + "', " + contactNo + ")";
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Record Added Successfully");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");
            conn.Open();

            int studentId = int.Parse(cbRegNo.Text);
            String firstName = txtFName.Text;
            String lastName = txtLName.Text;
            String dateOfBirth = dateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            String gender;
            if (rbMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            int mobilePhone = int.Parse(txtMphone.Text);
            int homePhone = int.Parse(txtHphone.Text);
            string parentName = txtPName.Text;
            string nic = txtNIC.Text;
            int contactNo = int.Parse(txtPCNumber.Text);

            String Query = "UPDATE Student_Details_Table SET first_name = '" + firstName + "', last_name = '" + lastName + "', date_of_birth = '" + dateOfBirth + "', gender = '" + gender + "', address = '" + address + "', email = '" + email + "', mobile_no = " + mobilePhone + ", home_no = " + homePhone + ", parent_name = '" + parentName + "', nic = '" + nic + "', parent_contact = " + contactNo + " WHERE student_id = " + studentId;
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Record Updated Successfully");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");
            conn.Open();

            int studentId = int.Parse(cbRegNo.Text);
            String firstName = txtFName.Text;
            String lastName = txtLName.Text;
            String dateOfBirth = dateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            String gender;
            if (rbMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            int mobilePhone = int.Parse(txtMphone.Text);
            int homePhone = int.Parse(txtHphone.Text);
            string parentName = txtPName.Text;
            string nic = txtNIC.Text;
            int contactNo = int.Parse(txtPCNumber.Text);

            string Query = "DELETE FROM Student_Details_Table WHERE student_id=" + studentId;
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Record Deleted Successfully");

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            HomePage homePage = new HomePage();
            homePage.Show();
        }
    }
}
