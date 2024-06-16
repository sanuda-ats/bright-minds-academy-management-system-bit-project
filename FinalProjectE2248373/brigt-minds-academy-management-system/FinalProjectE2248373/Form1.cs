using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FinalProjectE2248373
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=SANUDA-DELL7501\MSSQLSERVER01;Initial Catalog=BrightMindsDB;Integrated Security=True");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                conn.Open();
                String querry = "SELECT * FROM Login_Details WHERE username = '"+txtUsername.Text+"' AND password = '"+txtPassword.Text+"'";
                SqlDataAdapter adapter = new SqlDataAdapter(querry, conn);

                DataTable dTable = new DataTable();
                adapter.Fill(dTable);

                if(dTable.Rows.Count > 0)
                {
                    username = txtUsername.Text;
                    password = txtPassword.Text;

                    MessageBox.Show("Login Success!");
                    this.Hide();
                    HomePage obj = new HomePage();
                    obj.Show();
                }
                else
                {
                    MessageBox.Show("Invalied Login credentials, please check Username and Password and try again");
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else
            { }
        }

        private void Login_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else if (dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
}