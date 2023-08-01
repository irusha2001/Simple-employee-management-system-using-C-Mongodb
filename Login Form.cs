using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework_4
{
    public partial class Form2 : Form
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";
        public Form2()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Text;

            // Check if the entered username and password match the admin credentials
            if (enteredUsername == AdminUsername && enteredPassword == AdminPassword)
            {
                // If the credentials are correct, proceed to the next form
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide(); // Hide the current login form
            }
            else
            {
                // If the credentials are incorrect, show an error message
                MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
