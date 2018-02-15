namespace BarcoDenverPlanningSysteem
{
    partial class Login
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
            this.btLogIn = new System.Windows.Forms.Button();
            this.tbInlogCode = new System.Windows.Forms.TextBox();
            this.lblTextCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btLogIn
            // 
            this.btLogIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btLogIn.Location = new System.Drawing.Point(45, 80);
            this.btLogIn.Name = "btLogIn";
            this.btLogIn.Size = new System.Drawing.Size(170, 23);
            this.btLogIn.TabIndex = 0;
            this.btLogIn.Text = "Inloggen";
            this.btLogIn.UseVisualStyleBackColor = true;
            this.btLogIn.Click += new System.EventHandler(this.btLogIn_Click);
            // 
            // tbInlogCode
            // 
            this.tbInlogCode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbInlogCode.Location = new System.Drawing.Point(45, 35);
            this.tbInlogCode.Name = "tbInlogCode";
            this.tbInlogCode.Size = new System.Drawing.Size(170, 20);
            this.tbInlogCode.TabIndex = 1;
            this.tbInlogCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbInlogCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbInlogCode_KeyPress);
            // 
            // lblTextCode
            // 
            this.lblTextCode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTextCode.AutoSize = true;
            this.lblTextCode.Location = new System.Drawing.Point(99, 19);
            this.lblTextCode.Name = "lblTextCode";
            this.lblTextCode.Size = new System.Drawing.Size(60, 13);
            this.lblTextCode.TabIndex = 2;
            this.lblTextCode.Text = "Inlog code:";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 137);
            this.Controls.Add(this.lblTextCode);
            this.Controls.Add(this.tbInlogCode);
            this.Controls.Add(this.btLogIn);
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLogIn;
        private System.Windows.Forms.TextBox tbInlogCode;
        private System.Windows.Forms.Label lblTextCode;
    }
}

