namespace TestApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbClientId = new System.Windows.Forms.TextBox();
            this.lblClientId = new System.Windows.Forms.Label();
            this.lblRefreshToken = new System.Windows.Forms.Label();
            this.tbRefreshToken = new System.Windows.Forms.TextBox();
            this.btnRefreshToken = new System.Windows.Forms.Button();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.lblScope = new System.Windows.Forms.Label();
            this.tbScope = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 83);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(278, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.OnLogin);
            // 
            // tbClientId
            // 
            this.tbClientId.Location = new System.Drawing.Point(66, 17);
            this.tbClientId.Name = "tbClientId";
            this.tbClientId.Size = new System.Drawing.Size(224, 20);
            this.tbClientId.TabIndex = 1;
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(12, 20);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(48, 13);
            this.lblClientId.TabIndex = 2;
            this.lblClientId.Text = "Client Id:";
            // 
            // lblRefreshToken
            // 
            this.lblRefreshToken.AutoSize = true;
            this.lblRefreshToken.Location = new System.Drawing.Point(12, 126);
            this.lblRefreshToken.Name = "lblRefreshToken";
            this.lblRefreshToken.Size = new System.Drawing.Size(77, 13);
            this.lblRefreshToken.TabIndex = 3;
            this.lblRefreshToken.Text = "Refresh token:";
            // 
            // tbRefreshToken
            // 
            this.tbRefreshToken.Location = new System.Drawing.Point(95, 123);
            this.tbRefreshToken.Name = "tbRefreshToken";
            this.tbRefreshToken.Size = new System.Drawing.Size(195, 20);
            this.tbRefreshToken.TabIndex = 4;
            // 
            // btnRefreshToken
            // 
            this.btnRefreshToken.Location = new System.Drawing.Point(12, 161);
            this.btnRefreshToken.Name = "btnRefreshToken";
            this.btnRefreshToken.Size = new System.Drawing.Size(278, 23);
            this.btnRefreshToken.TabIndex = 5;
            this.btnRefreshToken.Text = "Refresh";
            this.btnRefreshToken.UseVisualStyleBackColor = true;
            this.btnRefreshToken.Click += new System.EventHandler(this.OnRefresh);
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.Location = new System.Drawing.Point(-1, 197);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(310, 20);
            this.tbStatus.TabIndex = 6;
            // 
            // lblScope
            // 
            this.lblScope.AutoSize = true;
            this.lblScope.Location = new System.Drawing.Point(19, 54);
            this.lblScope.Name = "lblScope";
            this.lblScope.Size = new System.Drawing.Size(41, 13);
            this.lblScope.TabIndex = 8;
            this.lblScope.Text = "Scope:";
            // 
            // tbScope
            // 
            this.tbScope.Location = new System.Drawing.Point(66, 51);
            this.tbScope.Name = "tbScope";
            this.tbScope.Size = new System.Drawing.Size(224, 20);
            this.tbScope.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 216);
            this.Controls.Add(this.lblScope);
            this.Controls.Add(this.tbScope);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.btnRefreshToken);
            this.Controls.Add(this.tbRefreshToken);
            this.Controls.Add(this.lblRefreshToken);
            this.Controls.Add(this.lblClientId);
            this.Controls.Add(this.tbClientId);
            this.Controls.Add(this.btnLogin);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(323, 255);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Test Application";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox tbClientId;
        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label lblRefreshToken;
        private System.Windows.Forms.TextBox tbRefreshToken;
        private System.Windows.Forms.Button btnRefreshToken;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Label lblScope;
        private System.Windows.Forms.TextBox tbScope;
    }
}

