using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PatcherClient
{
    public partial class LoginForm : Form
    {
        private HttpClient httpClient = new HttpClient();
        private string loginServerUrl = "http://localhost:3000/api/login";
        private string userToken = "";
        private int userId = 0;

        public LoginForm()
        {
            InitializeComponent();
            this.Text = "Ragnarok - Login";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = null;

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Ragnarok Online";
            lblTitle.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitle.Location = new System.Drawing.Point(10, 20);
            lblTitle.Size = new System.Drawing.Size(360, 40);
            this.Controls.Add(lblTitle);

            // Username Label
            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new System.Drawing.Point(20, 70);
            lblUsername.AutoSize = true;
            this.Controls.Add(lblUsername);

            // Username TextBox
            TextBox txtUsername = new TextBox();
            txtUsername.Name = "txtUsername";
            txtUsername.Location = new System.Drawing.Point(20, 95);
            txtUsername.Size = new System.Drawing.Size(350, 30);
            txtUsername.Font = new System.Drawing.Font("Arial", 10);
            this.Controls.Add(txtUsername);

            // Password Label
            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new System.Drawing.Point(20, 130);
            lblPassword.AutoSize = true;
            this.Controls.Add(lblPassword);

            // Password TextBox
            TextBox txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Location = new System.Drawing.Point(20, 155);
            txtPassword.Size = new System.Drawing.Size(350, 30);
            txtPassword.Font = new System.Drawing.Font("Arial", 10);
            txtPassword.UseSystemPasswordChar = true;
            this.Controls.Add(txtPassword);

            // Login Button
            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(20, 200);
            btnLogin.Size = new System.Drawing.Size(165, 35);
            btnLogin.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Click += async (s, e) => await BtnLogin_Click(s, e);
            this.Controls.Add(btnLogin);

            // Register Button
            Button btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Location = new System.Drawing.Point(205, 200);
            btnRegister.Size = new System.Drawing.Size(165, 35);
            btnRegister.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            btnRegister.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnRegister.ForeColor = System.Drawing.Color.White;
            btnRegister.Click += (s, e) => MessageBox.Show("Register feature coming soon!");
            this.Controls.Add(btnRegister);

            // Status Label
            Label lblStatus = new Label();
            lblStatus.Name = "lblStatus";
            lblStatus.Location = new System.Drawing.Point(20, 245);
            lblStatus.Size = new System.Drawing.Size(350, 20);
            lblStatus.Text = "";
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.AutoSize = true;
            this.Controls.Add(lblStatus);
        }

        private async Task BtnLogin_Click(object sender, EventArgs e)
        {
            TextBox txtUsername = this.Controls["txtUsername"] as TextBox;
            TextBox txtPassword = this.Controls["txtPassword"] as TextBox;
            Label lblStatus = this.Controls["lblStatus"] as Label;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStatus.Text = "❌ Please enter username and password";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            lblStatus.Text = "⏳ Logging in...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;

            try
            {
                var loginData = new { username = username, password = password };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(loginServerUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic result = JsonConvert.DeserializeObject(responseContent);
                    userToken = result.token;
                    userId = result.user.id;

                    lblStatus.Text = "✅ Login successful! Opening patcher...";
                    lblStatus.ForeColor = System.Drawing.Color.Green;

                    // Open Patcher Form
                    await Task.Delay(1000);
                    PatcherForm patcherForm = new PatcherForm(userToken, userId);
                    this.Hide();
                    patcherForm.Show();
                    patcherForm.FormClosed += (s, e2) => this.Close();
                }
                else
                {
                    dynamic result = JsonConvert.DeserializeObject(responseContent);
                    lblStatus.Text = $"❌ {result.message}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"❌ Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
