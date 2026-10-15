namespace IdCard.Hanel
{
    partial class QRForm
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
            label1 = new Label();
            label2 = new Label();
            txtQRCodeVal = new TextBox();
            SuspendLayout();
            //
            // label1
            //
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(11, 59, 102);
            label1.Location = new Point(20, 28);
            label1.Name = "label1";
            label1.Size = new Size(640, 44);
            label1.TabIndex = 0;
            label1.Text = "Vui lòng đưa thẻ vào đầu đọc";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            //
            // label2
            //
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(198, 40, 40);
            label2.Location = new Point(20, 76);
            label2.Name = "label2";
            label2.Size = new Size(640, 28);
            label2.TabIndex = 1;
            label2.Text = "(quay mặt trước có QR Code vào phía trong)";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            //
            // txtQRCodeVal
            //
            txtQRCodeVal.BackColor = Color.FromArgb(247, 249, 252);
            txtQRCodeVal.BorderStyle = BorderStyle.FixedSingle;
            txtQRCodeVal.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtQRCodeVal.Location = new Point(24, 128);
            txtQRCodeVal.Multiline = true;
            txtQRCodeVal.Name = "txtQRCodeVal";
            txtQRCodeVal.ReadOnly = true;
            txtQRCodeVal.Size = new Size(632, 150);
            txtQRCodeVal.TabIndex = 2;
            //
            // QRForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(680, 302);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(txtQRCodeVal);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QRForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đọc QR Code";
            FormClosing += QRForm_FormClosing;
            Load += QRForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtQRCodeVal;
    }
}
