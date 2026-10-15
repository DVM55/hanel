namespace IdCard.Hanel
{
    partial class FingerForm
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
            pnlSelect = new Panel();
            fingerPicker = new FingerPicker();
            lblSelected = new Label();
            pnlCapture = new Panel();
            lblFingerName = new Label();
            pictureBoxFinger = new PictureBox();
            lbMessage = new Label();
            pnlFooterLine = new Panel();
            btnBack = new Button();
            btnCancel = new Button();
            btnStart = new Button();
            btnRetry = new Button();
            btnDone = new Button();
            pnlSelect.SuspendLayout();
            pnlCapture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFinger).BeginInit();
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
            lblTitle.Text = "Lấy mẫu vân tay";
            //
            // lblHint
            //
            lblHint.AutoSize = true;
            lblHint.ForeColor = Color.FromArgb(107, 114, 128);
            lblHint.Location = new Point(34, 46);
            lblHint.Name = "lblHint";
            lblHint.TabIndex = 2;
            lblHint.Text = "Bước 1/2: Chọn ngón tay cần thu thập";
            //
            // pnlSelect
            //
            pnlSelect.Controls.Add(fingerPicker);
            pnlSelect.Controls.Add(lblSelected);
            pnlSelect.Location = new Point(20, 80);
            pnlSelect.Name = "pnlSelect";
            pnlSelect.Size = new Size(600, 420);
            pnlSelect.TabIndex = 3;
            //
            // fingerPicker
            //
            fingerPicker.Dock = DockStyle.Fill;
            fingerPicker.Name = "fingerPicker";
            fingerPicker.TabIndex = 0;
            fingerPicker.SelectedFingerChanged += fingerPicker_SelectedFingerChanged;
            //
            // lblSelected
            //
            lblSelected.Dock = DockStyle.Bottom;
            lblSelected.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblSelected.ForeColor = Color.FromArgb(21, 101, 192);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(600, 40);
            lblSelected.TabIndex = 1;
            lblSelected.Text = "Bấm vào ngón tay để chọn";
            lblSelected.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlCapture
            //
            pnlCapture.Controls.Add(pictureBoxFinger);
            pnlCapture.Controls.Add(lblFingerName);
            pnlCapture.Controls.Add(lbMessage);
            pnlCapture.Location = new Point(20, 80);
            pnlCapture.Name = "pnlCapture";
            pnlCapture.Size = new Size(600, 420);
            pnlCapture.TabIndex = 4;
            pnlCapture.Visible = false;
            //
            // lblFingerName
            //
            lblFingerName.Dock = DockStyle.Top;
            lblFingerName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblFingerName.ForeColor = Color.FromArgb(11, 59, 102);
            lblFingerName.Name = "lblFingerName";
            lblFingerName.Size = new Size(600, 34);
            lblFingerName.TabIndex = 0;
            lblFingerName.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pictureBoxFinger
            //
            pictureBoxFinger.BackColor = Color.FromArgb(247, 249, 252);
            pictureBoxFinger.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxFinger.Location = new Point(185, 40);
            pictureBoxFinger.Name = "pictureBoxFinger";
            pictureBoxFinger.Size = new Size(230, 310);
            pictureBoxFinger.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFinger.TabIndex = 1;
            pictureBoxFinger.TabStop = false;
            //
            // lbMessage
            //
            lbMessage.Dock = DockStyle.Bottom;
            lbMessage.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lbMessage.ForeColor = Color.FromArgb(21, 101, 192);
            lbMessage.Name = "lbMessage";
            lbMessage.Size = new Size(600, 56);
            lbMessage.TabIndex = 2;
            lbMessage.Text = "Vui lòng chờ...";
            lbMessage.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pnlFooterLine
            //
            pnlFooterLine.BackColor = Color.FromArgb(225, 231, 239);
            pnlFooterLine.Location = new Point(20, 512);
            pnlFooterLine.Name = "pnlFooterLine";
            pnlFooterLine.Size = new Size(600, 1);
            pnlFooterLine.TabIndex = 5;
            //
            // btnBack
            //
            btnBack.BackColor = Color.White;
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 244, 251);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnBack.ForeColor = Color.FromArgb(31, 41, 55);
            btnBack.Location = new Point(20, 528);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(150, 40);
            btnBack.TabIndex = 6;
            btnBack.Text = "Chọn ngón khác";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Visible = false;
            btnBack.Click += btnBack_Click;
            //
            // btnCancel
            //
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 244, 251);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.FromArgb(31, 41, 55);
            btnCancel.Location = new Point(340, 528);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            //
            // btnStart
            //
            btnStart.BackColor = Color.FromArgb(21, 101, 192);
            btnStart.Cursor = Cursors.Hand;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatAppearance.MouseOverBackColor = Color.FromArgb(13, 71, 161);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(470, 528);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(150, 40);
            btnStart.TabIndex = 8;
            btnStart.Text = "Bắt đầu thu thập";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            //
            // btnRetry
            //
            btnRetry.BackColor = Color.White;
            btnRetry.Cursor = Cursors.Hand;
            btnRetry.FlatAppearance.BorderColor = Color.FromArgb(21, 101, 192);
            btnRetry.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnRetry.FlatStyle = FlatStyle.Flat;
            btnRetry.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnRetry.ForeColor = Color.FromArgb(21, 101, 192);
            btnRetry.Location = new Point(340, 528);
            btnRetry.Name = "btnRetry";
            btnRetry.Size = new Size(120, 40);
            btnRetry.TabIndex = 9;
            btnRetry.Text = "Thu lại";
            btnRetry.UseVisualStyleBackColor = false;
            btnRetry.Visible = false;
            btnRetry.Click += btnRetry_Click;
            //
            // btnDone
            //
            btnDone.BackColor = Color.FromArgb(46, 125, 50);
            btnDone.Cursor = Cursors.Hand;
            btnDone.DialogResult = DialogResult.OK;
            btnDone.FlatAppearance.BorderSize = 0;
            btnDone.FlatAppearance.MouseOverBackColor = Color.FromArgb(27, 94, 32);
            btnDone.FlatStyle = FlatStyle.Flat;
            btnDone.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnDone.ForeColor = Color.White;
            btnDone.Location = new Point(470, 528);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(150, 40);
            btnDone.TabIndex = 10;
            btnDone.Text = "Hoàn tất";
            btnDone.UseVisualStyleBackColor = false;
            btnDone.Visible = false;
            //
            // FingerForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(640, 584);
            Controls.Add(pnlAccent);
            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(pnlSelect);
            Controls.Add(pnlCapture);
            Controls.Add(pnlFooterLine);
            Controls.Add(btnBack);
            Controls.Add(btnCancel);
            Controls.Add(btnStart);
            Controls.Add(btnRetry);
            Controls.Add(btnDone);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FingerForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vân tay";
            FormClosing += FingerForm_FormClosing;
            Load += FingerForm_Load;
            pnlSelect.ResumeLayout(false);
            pnlCapture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxFinger).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlAccent;
        private Label lblTitle;
        private Label lblHint;
        private Panel pnlSelect;
        private FingerPicker fingerPicker;
        private Label lblSelected;
        private Panel pnlCapture;
        private Label lblFingerName;
        private PictureBox pictureBoxFinger;
        private Label lbMessage;
        private Panel pnlFooterLine;
        private Button btnBack;
        private Button btnCancel;
        private Button btnStart;
        private Button btnRetry;
        private Button btnDone;
    }
}
