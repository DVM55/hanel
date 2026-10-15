namespace IdCard.Hanel
{
    partial class CaptureFace
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
            lblHint = new Label();
            picFromCamera = new PictureBox();
            btnCapture = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)picFromCamera).BeginInit();
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
            lblTitle.Text = "Chụp ảnh khuôn mặt";
            //
            // lblHint
            //
            lblHint.AutoSize = true;
            lblHint.ForeColor = Color.FromArgb(107, 114, 128);
            lblHint.Location = new Point(34, 46);
            lblHint.Name = "lblHint";
            lblHint.TabIndex = 2;
            lblHint.Text = "Nhìn thẳng vào camera và giữ khuôn mặt trong khung hình";
            //
            // picFromCamera
            //
            picFromCamera.BackColor = Color.FromArgb(17, 24, 39);
            picFromCamera.Location = new Point(20, 80);
            picFromCamera.Name = "picFromCamera";
            picFromCamera.Size = new Size(760, 428);
            picFromCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picFromCamera.TabIndex = 3;
            picFromCamera.TabStop = false;
            //
            // btnCapture
            //
            btnCapture.BackColor = Color.FromArgb(21, 101, 192);
            btnCapture.Cursor = Cursors.Hand;
            btnCapture.FlatAppearance.BorderSize = 0;
            btnCapture.FlatAppearance.MouseDownBackColor = Color.FromArgb(10, 60, 130);
            btnCapture.FlatAppearance.MouseOverBackColor = Color.FromArgb(13, 71, 161);
            btnCapture.FlatStyle = FlatStyle.Flat;
            btnCapture.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnCapture.ForeColor = Color.White;
            btnCapture.Location = new Point(530, 524);
            btnCapture.Name = "btnCapture";
            btnCapture.Size = new Size(120, 38);
            btnCapture.TabIndex = 4;
            btnCapture.Text = "Chụp";
            btnCapture.UseVisualStyleBackColor = false;
            btnCapture.Click += btnCapture_Click;
            //
            // btnCancel
            //
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 244, 251);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.FromArgb(31, 41, 55);
            btnCancel.Location = new Point(660, 524);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 38);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Bỏ qua";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            //
            // CaptureFace
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(800, 580);
            ControlBox = false;
            Controls.Add(pnlAccent);
            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(picFromCamera);
            Controls.Add(btnCapture);
            Controls.Add(btnCancel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CaptureFace";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chụp ảnh khuôn mặt";
            FormClosing += CaptureFace_FormClosing;
            FormClosed += CaptureFace_FormClosed;
            Load += CaptureFace_Load;
            Shown += CaptureFace_Shown;
            ((System.ComponentModel.ISupportInitialize)picFromCamera).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlAccent;
        private Label lblTitle;
        private Label lblHint;
        private PictureBox picFromCamera;
        private Button btnCapture;
        private Button btnCancel;
    }
}
