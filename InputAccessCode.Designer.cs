namespace IdCard.Hanel
{
    partial class InputAccessCode
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
            pnlAccent = new Panel();
            lblTitle = new Label();
            label1 = new Label();
            txtAccessCode = new TextBox();
            btnOK = new Button();
            SuspendLayout();
            //
            // pnlAccent
            //
            pnlAccent.BackColor = Color.FromArgb(21, 101, 192);
            pnlAccent.Location = new Point(20, 20);
            pnlAccent.Name = "pnlAccent";
            pnlAccent.Size = new Size(4, 44);
            pnlAccent.TabIndex = 0;
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 59, 102);
            lblTitle.Location = new Point(32, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Nhập mã truy cập";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(107, 114, 128);
            label1.Location = new Point(34, 46);
            label1.Name = "label1";
            label1.TabIndex = 2;
            label1.Text = "Access code: MRZ hoặc 6 số cuối của số giấy tờ";
            //
            // txtAccessCode
            //
            txtAccessCode.BackColor = Color.FromArgb(247, 249, 252);
            txtAccessCode.BorderStyle = BorderStyle.FixedSingle;
            txtAccessCode.Font = new Font("Consolas", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtAccessCode.Location = new Point(20, 80);
            txtAccessCode.Multiline = true;
            txtAccessCode.Name = "txtAccessCode";
            txtAccessCode.Size = new Size(480, 96);
            txtAccessCode.TabIndex = 3;
            //
            // btnOK
            //
            btnOK.BackColor = Color.FromArgb(21, 101, 192);
            btnOK.Cursor = Cursors.Hand;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(10, 60, 130);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(13, 71, 161);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(380, 192);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(120, 36);
            btnOK.TabIndex = 4;
            btnOK.Text = "Xác nhận";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            //
            // InputAccessCode
            //
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(520, 246);
            Controls.Add(pnlAccent);
            Controls.Add(lblTitle);
            Controls.Add(label1);
            Controls.Add(txtAccessCode);
            Controls.Add(btnOK);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputAccessCode";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Access code";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlAccent;
        private Label lblTitle;
        private Label label1;
        private TextBox txtAccessCode;
        private Button btnOK;
    }
}
