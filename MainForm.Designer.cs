namespace IdCard.Hanel
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolTip1 = new ToolTip(components);
            timer1 = new System.Windows.Forms.Timer(components);
            pnlTop = new Panel();
            tlpToolbar = new TableLayoutPanel();
            flpToolbar = new FlowLayoutPanel();
            btnSettings = new Button();
            btnFaceMatching = new Button();
            btnCompare = new Button();
            btnFinger = new Button();
            btnClearForm = new Button();
            btnExit = new Button();
            pnlTopLine = new Panel();
            pnlSettings = new Panel();
            lblSecWebhook = new Label();
            lblWebhookUrl = new Label();
            txtWebhookUrl = new TextBox();
            lblWebhookUser = new Label();
            txtWebhookUser = new TextBox();
            lblWebhookPass = new Label();
            txtWebhookPass = new TextBox();
            pnlSettingsLine = new Panel();
            lblSettingsTitle = new Label();
            lblSecRead = new Label();
            label2 = new Label();
            cbReaderTypes = new ComboBox();
            lblProcess = new Label();
            cbProcessStep = new ComboBox();
            chkAutoRead = new CheckBox();
            chkDelteForm = new CheckBox();
            btnRejectCard = new Button();
            lblSecCamera = new Label();
            label7 = new Label();
            cbFaceCameras = new ComboBox();
            btnRefreshCamera = new Button();
            lblFaceTimeout = new Label();
            numFaceTimeout = new NumericUpDown();
            label9 = new Label();
            numFaceDurationTime = new NumericUpDown();
            label8 = new Label();
            cbAntiSpoofing = new ComboBox();
            chkDetectFace = new CheckBox();
            chkAutocapture = new CheckBox();
            chkShowTimeout = new CheckBox();
            chkAutoCompare = new CheckBox();
            lblMatchThreshold = new Label();
            numMatchThreshold = new NumericUpDown();
            lblSecTech = new Label();
            txtFastRead = new CheckBox();
            chkPaused = new CheckBox();
            chkPauseIntCam = new CheckBox();
            label5 = new Label();
            cbOcrMode = new ComboBox();
            btnTest = new Button();
            btnRereadcard1 = new Button();
            btnRereadcard2 = new Button();
            btnReadQR = new Button();
            btnCompareTest = new Button();
            lblSecData = new Label();
            txtMRZ = new TextBox();
            txtDSCert = new TextBox();
            lblSecScan = new Label();
            picIdcardFront = new PictureBox();
            pictureIdcardBack = new PictureBox();
            pnlBody = new Panel();
            cardMain = new CardPanel();
            tlpMain = new TableLayoutPanel();
            tlpStatus = new TableLayoutPanel();
            pnlDevice = new Panel();
            label4 = new Label();
            lbDeviceState = new Label();
            pnlSerial = new Panel();
            label6 = new Label();
            lbReaderStatus = new Label();
            pnlCamera = new Panel();
            lblCamera = new Label();
            lbCameraName = new Label();
            flpStatusMsg = new FlowLayoutPanel();
            lbStatusMessage = new Label();
            spinner = new Spinner();
            flpActions = new FlowLayoutPanel();
            btnFingerMain = new Button();
            btnSend = new Button();
            pnlTime = new Panel();
            lbTime = new Label();
            lbTimeLb = new Label();
            pnlSeparator = new Panel();
            tlpInfo = new TableLayoutPanel();
            tlpFields = new TableLayoutPanel();
            label3 = new Label();
            picChipImage = new PictureBox();
            label1 = new Label();
            picCamera = new PictureBox();
            lblCapDocNumber = new Label();
            txtDocumentNumber = new FieldLabel();
            lblCapPrevNumber = new Label();
            txtPreviousNumber = new FieldLabel();
            lblCapName = new Label();
            txtName = new FieldLabel();
            lblCapSex = new Label();
            txtSex = new FieldLabel();
            lblCapDob = new Label();
            txtDateOfBirth = new FieldLabel();
            lblCapNationality = new Label();
            txtNationality = new FieldLabel();
            lblCapNation = new Label();
            txtNation = new FieldLabel();
            lblCapReligion = new Label();
            txtReligion = new FieldLabel();
            lblCapCharacter = new Label();
            txtCharacter = new FieldLabel();
            lblCapHometown = new Label();
            txtHometown = new FieldLabel();
            lblCapAddress = new Label();
            txtAddress = new FieldLabel();
            lblCapIssueDate = new Label();
            txtIssueDate = new FieldLabel();
            lblCapExpiredDate = new Label();
            txtExpiredDate = new FieldLabel();
            lblCapFather = new Label();
            txtFatherName = new FieldLabel();
            lblCapMother = new Label();
            txtMotherName = new FieldLabel();
            lblCapPartner = new Label();
            chkConsent = new ConsentCheck();
            lblSecCollected = new SuffixLabel();
            txtPartnerName = new FieldLabel();
            pnlTop.SuspendLayout();
            tlpToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFaceTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFaceDurationTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMatchThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picIdcardFront).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureIdcardBack).BeginInit();
            pnlBody.SuspendLayout();
            cardMain.SuspendLayout();
            tlpMain.SuspendLayout();
            tlpStatus.SuspendLayout();
            pnlDevice.SuspendLayout();
            pnlCamera.SuspendLayout();
            pnlSerial.SuspendLayout();
            flpStatusMsg.SuspendLayout();
            flpActions.SuspendLayout();
            pnlTime.SuspendLayout();
            btnCaptureFace = new Button();
            tlpInfo.SuspendLayout();
            tlpFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picChipImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCamera).BeginInit();
            SuspendLayout();
            //
            // pnlTop
            //
            pnlTop.BackColor = Color.White;
            pnlTop.Controls.Add(tlpToolbar);
            pnlTop.Controls.Add(pnlTopLine);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1440, 72);
            pnlTop.TabIndex = 2;
            //
            // tlpToolbar
            //
            tlpToolbar.ColumnCount = 3;
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpToolbar.ColumnStyles.Add(new ColumnStyle());
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpToolbar.Controls.Add(flpToolbar, 1, 0);
            tlpToolbar.Dock = DockStyle.Fill;
            tlpToolbar.Location = new Point(0, 0);
            tlpToolbar.Name = "tlpToolbar";
            tlpToolbar.RowCount = 1;
            tlpToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpToolbar.Size = new Size(1440, 71);
            tlpToolbar.TabIndex = 0;
            //
            // flpToolbar
            //
            flpToolbar.Anchor = AnchorStyles.None;
            flpToolbar.AutoSize = true;
            flpToolbar.Controls.Add(btnFaceMatching);
            flpToolbar.Controls.Add(btnCompare);
            flpToolbar.Controls.Add(btnFinger);
            flpToolbar.Controls.Add(btnClearForm);
            flpToolbar.Controls.Add(btnExit);
            flpToolbar.Location = new Point(534, 9);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Size = new Size(372, 52);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            //
            // btnSettings
            //
            btnSettings.BackColor = Color.White;
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnSettings.ForeColor = Color.FromArgb(11, 59, 102);
            btnSettings.Margin = new Padding(0, 6, 8, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(44, 44);
            btnSettings.TabIndex = 0;
            btnSettings.Text = "";
            toolTip1.SetToolTip(btnSettings, "Cài đặt");
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Visible = false;
            btnSettings.Click += btnSettings_Click;
            //
            // btnFaceMatching
            //
            btnFaceMatching.BackColor = Color.White;
            btnFaceMatching.Cursor = Cursors.Hand;
            btnFaceMatching.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnFaceMatching.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnFaceMatching.FlatStyle = FlatStyle.Flat;
            btnFaceMatching.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnFaceMatching.ForeColor = Color.FromArgb(11, 59, 102);
            btnFaceMatching.Location = new Point(66, 4);
            btnFaceMatching.Margin = new Padding(4);
            btnFaceMatching.Name = "btnFaceMatching";
            btnFaceMatching.Size = new Size(54, 44);
            btnFaceMatching.TabIndex = 1;
            btnFaceMatching.Text = "";
            toolTip1.SetToolTip(btnFaceMatching, "Chụp ảnh khuôn mặt");
            btnFaceMatching.UseVisualStyleBackColor = false;
            btnFaceMatching.Click += btnFaceMatching_Click;
            //
            // btnCompare
            //
            btnCompare.BackColor = Color.White;
            btnCompare.Cursor = Cursors.Hand;
            btnCompare.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnCompare.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnCompare.FlatStyle = FlatStyle.Flat;
            btnCompare.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnCompare.ForeColor = Color.FromArgb(11, 59, 102);
            btnCompare.Location = new Point(128, 4);
            btnCompare.Margin = new Padding(4);
            btnCompare.Name = "btnCompare";
            btnCompare.Size = new Size(54, 44);
            btnCompare.TabIndex = 2;
            btnCompare.Text = "";
            toolTip1.SetToolTip(btnCompare, "So khớp khuôn mặt");
            btnCompare.UseVisualStyleBackColor = false;
            btnCompare.Click += btnCompare_Click;
            //
            // btnFinger
            //
            btnFinger.BackColor = Color.White;
            btnFinger.Cursor = Cursors.Hand;
            btnFinger.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnFinger.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnFinger.FlatStyle = FlatStyle.Flat;
            btnFinger.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnFinger.ForeColor = Color.FromArgb(11, 59, 102);
            btnFinger.Location = new Point(190, 4);
            btnFinger.Margin = new Padding(4);
            btnFinger.Name = "btnFinger";
            btnFinger.Size = new Size(54, 44);
            btnFinger.TabIndex = 3;
            btnFinger.Text = "";
            toolTip1.SetToolTip(btnFinger, "Lấy vân tay");
            btnFinger.UseVisualStyleBackColor = false;
            btnFinger.Visible = false;
            btnFinger.Click += btnFinger_Click;
            //
            // btnClearForm
            //
            btnClearForm.BackColor = Color.White;
            btnClearForm.Cursor = Cursors.Hand;
            btnClearForm.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnClearForm.FlatAppearance.MouseOverBackColor = Color.FromArgb(253, 236, 236);
            btnClearForm.FlatStyle = FlatStyle.Flat;
            btnClearForm.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnClearForm.ForeColor = Color.FromArgb(211, 47, 47);
            btnClearForm.Location = new Point(252, 4);
            btnClearForm.Margin = new Padding(4);
            btnClearForm.Name = "btnClearForm";
            btnClearForm.Size = new Size(54, 44);
            btnClearForm.TabIndex = 4;
            btnClearForm.Text = "";
            toolTip1.SetToolTip(btnClearForm, "Xóa dữ liệu");
            btnClearForm.UseVisualStyleBackColor = false;
            btnClearForm.Click += btnClearForm_Click;
            //
            // btnExit
            //
            btnExit.BackColor = Color.White;
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe MDL2 Assets", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnExit.ForeColor = Color.FromArgb(11, 59, 102);
            btnExit.Location = new Point(314, 4);
            btnExit.Margin = new Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(54, 44);
            btnExit.TabIndex = 5;
            btnExit.Text = "";
            toolTip1.SetToolTip(btnExit, "Thoát");
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnClose_Click;
            //
            // pnlTopLine
            //
            pnlTopLine.BackColor = Color.FromArgb(225, 231, 239);
            pnlTopLine.Dock = DockStyle.Bottom;
            pnlTopLine.Location = new Point(0, 71);
            pnlTopLine.Name = "pnlTopLine";
            pnlTopLine.Size = new Size(1440, 1);
            pnlTopLine.TabIndex = 1;
            //
            // pnlSettings
            //
            pnlSettings.AutoScroll = true;
            pnlSettings.BackColor = Color.White;
            pnlSettings.Controls.Add(pnlSettingsLine);
            pnlSettings.Controls.Add(lblSettingsTitle);
            pnlSettings.Controls.Add(lblSecRead);
            pnlSettings.Controls.Add(chkAutoRead);
            pnlSettings.Controls.Add(chkDelteForm);
            pnlSettings.Controls.Add(btnRejectCard);
            pnlSettings.Controls.Add(lblSecCamera);
            pnlSettings.Controls.Add(lblFaceTimeout);
            pnlSettings.Controls.Add(numFaceTimeout);
            pnlSettings.Controls.Add(label9);
            pnlSettings.Controls.Add(numFaceDurationTime);
            pnlSettings.Controls.Add(label8);
            pnlSettings.Controls.Add(cbAntiSpoofing);
            pnlSettings.Controls.Add(chkDetectFace);
            pnlSettings.Controls.Add(chkAutocapture);
            pnlSettings.Controls.Add(chkShowTimeout);
            pnlSettings.Controls.Add(chkAutoCompare);
            pnlSettings.Controls.Add(lblMatchThreshold);
            pnlSettings.Controls.Add(numMatchThreshold);
            pnlSettings.Controls.Add(lblSecWebhook);
            pnlSettings.Controls.Add(lblWebhookUrl);
            pnlSettings.Controls.Add(txtWebhookUrl);
            pnlSettings.Controls.Add(lblWebhookUser);
            pnlSettings.Controls.Add(txtWebhookUser);
            pnlSettings.Controls.Add(lblWebhookPass);
            pnlSettings.Controls.Add(txtWebhookPass);
            pnlSettings.Dock = DockStyle.Right;
            pnlSettings.Location = new Point(1100, 72);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(340, 788);
            pnlSettings.TabIndex = 1;
            pnlSettings.Visible = false;
            //
            // pnlSettingsLine
            //
            pnlSettingsLine.BackColor = Color.FromArgb(225, 231, 239);
            pnlSettingsLine.Dock = DockStyle.Left;
            pnlSettingsLine.Location = new Point(0, 0);
            pnlSettingsLine.Name = "pnlSettingsLine";
            pnlSettingsLine.Size = new Size(1, 788);
            pnlSettingsLine.TabIndex = 0;
            //
            // lblSettingsTitle
            //
            lblSettingsTitle.AutoSize = true;
            lblSettingsTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblSettingsTitle.ForeColor = Color.FromArgb(11, 59, 102);
            lblSettingsTitle.Location = new Point(18, 14);
            lblSettingsTitle.Name = "lblSettingsTitle";
            lblSettingsTitle.TabIndex = 1;
            lblSettingsTitle.Text = "Cài đặt";
            //
            // lblSecRead
            //
            lblSecRead.AutoSize = true;
            lblSecRead.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecRead.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecRead.Location = new Point(20, 56);
            lblSecRead.Name = "lblSecRead";
            lblSecRead.TabIndex = 2;
            lblSecRead.Text = "ĐỌC THẺ";
            //
            // label2
            //
            label2.Location = new Point(20, 80);
            label2.Name = "label2";
            label2.Size = new Size(90, 24);
            label2.TabIndex = 3;
            label2.Text = "Loại thẻ";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cbReaderTypes
            //
            cbReaderTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            cbReaderTypes.FormattingEnabled = true;
            cbReaderTypes.Items.AddRange(new object[] { "IDCARD", "PASSPORT", "PARTYMEMBER" });
            cbReaderTypes.Location = new Point(115, 80);
            cbReaderTypes.Name = "cbReaderTypes";
            cbReaderTypes.Size = new Size(195, 23);
            cbReaderTypes.TabIndex = 4;
            cbReaderTypes.SelectedIndexChanged += cbReaderTypes_SelectedIndexChanged;
            //
            // lblProcess
            //
            lblProcess.Location = new Point(20, 80);
            lblProcess.Name = "lblProcess";
            lblProcess.Size = new Size(90, 24);
            lblProcess.TabIndex = 5;
            lblProcess.Text = "Quy trình";
            lblProcess.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cbProcessStep
            //
            cbProcessStep.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProcessStep.FormattingEnabled = true;
            cbProcessStep.Items.AddRange(new object[] { "Chỉ đọc thẻ", "Đọc thẻ => Xác minh khuôn mặt", "Đọc thẻ => Chụp ảnh 2 mặt" });
            cbProcessStep.Location = new Point(115, 80);
            cbProcessStep.Name = "cbProcessStep";
            cbProcessStep.Size = new Size(195, 23);
            cbProcessStep.TabIndex = 6;
            cbProcessStep.SelectedIndexChanged += cbProcessStep_SelectedIndexChanged;
            //
            // chkAutoRead
            //
            chkAutoRead.AutoSize = true;
            chkAutoRead.Checked = true;
            chkAutoRead.CheckState = CheckState.Checked;
            chkAutoRead.Location = new Point(22, 80);
            chkAutoRead.Name = "chkAutoRead";
            chkAutoRead.TabIndex = 7;
            chkAutoRead.Text = "Tự động đọc khi có thẻ";
            chkAutoRead.UseVisualStyleBackColor = true;
            chkAutoRead.CheckedChanged += chkAutoRead_CheckedChanged;
            //
            // chkDelteForm
            //
            chkDelteForm.AutoSize = true;
            chkDelteForm.Checked = true;
            chkDelteForm.CheckState = CheckState.Checked;
            chkDelteForm.Location = new Point(22, 106);
            chkDelteForm.Name = "chkDelteForm";
            chkDelteForm.TabIndex = 8;
            chkDelteForm.Text = "Xóa dữ liệu khi rút thẻ";
            chkDelteForm.UseVisualStyleBackColor = true;
            //
            // btnRejectCard
            //
            btnRejectCard.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnRejectCard.FlatStyle = FlatStyle.Flat;
            btnRejectCard.Location = new Point(435, 78);
            btnRejectCard.Name = "btnRejectCard";
            btnRejectCard.Size = new Size(95, 28);
            btnRejectCard.TabIndex = 9;
            btnRejectCard.Text = "Reject card";
            btnRejectCard.UseVisualStyleBackColor = true;
            btnRejectCard.Visible = false;
            btnRejectCard.Click += btnRejectCard_Click;
            //
            // lblSecCamera
            //
            lblSecCamera.AutoSize = true;
            lblSecCamera.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecCamera.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecCamera.Location = new Point(20, 148);
            lblSecCamera.Name = "lblSecCamera";
            lblSecCamera.TabIndex = 10;
            lblSecCamera.Text = "CAMERA KHUÔN MẶT";
            //
            // label7
            //
            label7.Location = new Point(20, 172);
            label7.Name = "label7";
            label7.Size = new Size(110, 24);
            label7.TabIndex = 11;
            label7.Text = "Camera";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cbFaceCameras
            //
            cbFaceCameras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFaceCameras.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            cbFaceCameras.FormattingEnabled = true;
            cbFaceCameras.Location = new Point(90, 14);
            cbFaceCameras.Name = "cbFaceCameras";
            cbFaceCameras.Size = new Size(270, 28);
            cbFaceCameras.Visible = false;
            cbFaceCameras.TabIndex = 12;
            cbFaceCameras.SelectedIndexChanged += cbFaceCameras_SelectedIndexChanged;
            //
            // btnRefreshCamera
            //
            btnRefreshCamera.BackColor = Color.White;
            btnRefreshCamera.Cursor = Cursors.Hand;
            btnRefreshCamera.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnRefreshCamera.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnRefreshCamera.FlatStyle = FlatStyle.Flat;
            btnRefreshCamera.ForeColor = Color.FromArgb(21, 101, 192);
            btnRefreshCamera.Location = new Point(450, 171);
            btnRefreshCamera.Name = "btnRefreshCamera";
            btnRefreshCamera.Size = new Size(80, 25);
            btnRefreshCamera.TabIndex = 13;
            btnRefreshCamera.Text = "Làm mới";
            btnRefreshCamera.UseVisualStyleBackColor = false;
            btnRefreshCamera.Click += btnRefreshCamera_Click;
            //
            // lblFaceTimeout
            //
            lblFaceTimeout.Location = new Point(20, 206);
            lblFaceTimeout.Name = "lblFaceTimeout";
            lblFaceTimeout.Size = new Size(180, 24);
            lblFaceTimeout.TabIndex = 14;
            lblFaceTimeout.Text = "Thời gian chờ chụp (ms)";
            lblFaceTimeout.TextAlign = ContentAlignment.MiddleLeft;
            //
            // numFaceTimeout
            //
            numFaceTimeout.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numFaceTimeout.Location = new Point(430, 206);
            numFaceTimeout.Maximum = new decimal(new int[] { 300000, 0, 0, 0 });
            numFaceTimeout.Name = "numFaceTimeout";
            numFaceTimeout.Size = new Size(100, 23);
            numFaceTimeout.TabIndex = 15;
            numFaceTimeout.TextAlign = HorizontalAlignment.Right;
            numFaceTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            //
            // label9
            //
            label9.Location = new Point(20, 240);
            label9.Name = "label9";
            label9.Size = new Size(180, 24);
            label9.TabIndex = 16;
            label9.Text = "Duy trì khuôn mặt (ms)";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            //
            // numFaceDurationTime
            //
            numFaceDurationTime.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numFaceDurationTime.Location = new Point(430, 240);
            numFaceDurationTime.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numFaceDurationTime.Name = "numFaceDurationTime";
            numFaceDurationTime.Size = new Size(100, 23);
            numFaceDurationTime.TabIndex = 17;
            numFaceDurationTime.TextAlign = HorizontalAlignment.Right;
            numFaceDurationTime.Value = new decimal(new int[] { 200, 0, 0, 0 });
            numFaceDurationTime.ValueChanged += numFaceDurationTime_ValueChanged;
            //
            // label8
            //
            label8.Location = new Point(20, 274);
            label8.Name = "label8";
            label8.Size = new Size(180, 24);
            label8.TabIndex = 18;
            label8.Text = "Chống giả mạo";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cbAntiSpoofing
            //
            cbAntiSpoofing.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAntiSpoofing.FormattingEnabled = true;
            cbAntiSpoofing.Items.AddRange(new object[] { "Tắt", "Thấp", "Trung bình", "Cao" });
            cbAntiSpoofing.Location = new Point(430, 274);
            cbAntiSpoofing.Name = "cbAntiSpoofing";
            cbAntiSpoofing.Size = new Size(100, 23);
            cbAntiSpoofing.TabIndex = 19;
            cbAntiSpoofing.SelectedIndexChanged += cbAntiSpoofing_SelectedIndexChanged;
            //
            // chkDetectFace
            //
            chkDetectFace.AutoSize = true;
            chkDetectFace.Checked = true;
            chkDetectFace.CheckState = CheckState.Checked;
            chkDetectFace.Location = new Point(22, 308);
            chkDetectFace.Name = "chkDetectFace";
            chkDetectFace.TabIndex = 20;
            chkDetectFace.Text = "Nhận diện khuôn mặt";
            chkDetectFace.UseVisualStyleBackColor = true;
            chkDetectFace.CheckedChanged += chkAutocapture_CheckedChanged;
            //
            // chkAutocapture
            //
            chkAutocapture.AutoSize = true;
            chkAutocapture.Checked = true;
            chkAutocapture.CheckState = CheckState.Checked;
            chkAutocapture.Location = new Point(22, 334);
            chkAutocapture.Name = "chkAutocapture";
            chkAutocapture.TabIndex = 21;
            chkAutocapture.Text = "Chụp ảnh tự động";
            chkAutocapture.UseVisualStyleBackColor = true;
            chkAutocapture.CheckedChanged += chkAutocapture_CheckedChanged;
            //
            // chkShowTimeout
            //
            chkShowTimeout.AutoSize = true;
            chkShowTimeout.Checked = true;
            chkShowTimeout.CheckState = CheckState.Checked;
            chkShowTimeout.Location = new Point(22, 360);
            chkShowTimeout.Name = "chkShowTimeout";
            chkShowTimeout.TabIndex = 22;
            chkShowTimeout.Text = "Hiển thị thời gian còn lại";
            chkShowTimeout.UseVisualStyleBackColor = true;
            chkShowTimeout.CheckedChanged += chkAutocapture_CheckedChanged;
            //
            // chkAutoCompare
            //
            chkAutoCompare.AutoSize = true;
            chkAutoCompare.Checked = true;
            chkAutoCompare.CheckState = CheckState.Checked;
            chkAutoCompare.Location = new Point(22, 386);
            chkAutoCompare.Name = "chkAutoCompare";
            chkAutoCompare.TabIndex = 23;
            chkAutoCompare.Text = "Tự động so khớp khuôn mặt sau khi chụp";
            chkAutoCompare.UseVisualStyleBackColor = true;
            //
            // lblMatchThreshold
            //
            lblMatchThreshold.Location = new Point(20, 414);
            lblMatchThreshold.Name = "lblMatchThreshold";
            lblMatchThreshold.Size = new Size(300, 24);
            lblMatchThreshold.Text = "Tỷ lệ so khớp khuôn mặt tối thiểu (%)";
            lblMatchThreshold.TextAlign = ContentAlignment.MiddleLeft;
            //
            // numMatchThreshold
            //
            numMatchThreshold.Location = new Point(430, 414);
            numMatchThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMatchThreshold.Name = "numMatchThreshold";
            numMatchThreshold.Size = new Size(100, 23);
            numMatchThreshold.TabIndex = 24;
            numMatchThreshold.TextAlign = HorizontalAlignment.Right;
            numMatchThreshold.Value = new decimal(new int[] { 70, 0, 0, 0 });
            //
            // lblSecWebhook
            //
            lblSecWebhook.AutoSize = true;
            lblSecWebhook.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecWebhook.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecWebhook.Location = new Point(20, 460);
            lblSecWebhook.Name = "lblSecWebhook";
            lblSecWebhook.Text = "NHẬN DỮ LIỆU";
            //
            // lblWebhookUrl
            //
            lblWebhookUrl.Location = new Point(20, 484);
            lblWebhookUrl.Name = "lblWebhookUrl";
            lblWebhookUrl.Size = new Size(110, 24);
            lblWebhookUrl.Text = "Webhook URL";
            lblWebhookUrl.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtWebhookUrl
            //
            txtWebhookUrl.Location = new Point(140, 484);
            txtWebhookUrl.Name = "txtWebhookUrl";
            txtWebhookUrl.PlaceholderText = "https://...";
            txtWebhookUrl.Size = new Size(390, 23);
            //
            // lblWebhookUser
            //
            lblWebhookUser.Location = new Point(20, 518);
            lblWebhookUser.Name = "lblWebhookUser";
            lblWebhookUser.Size = new Size(110, 24);
            lblWebhookUser.Text = "Tài khoản";
            lblWebhookUser.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtWebhookUser
            //
            txtWebhookUser.Location = new Point(140, 518);
            txtWebhookUser.Name = "txtWebhookUser";
            txtWebhookUser.Size = new Size(390, 23);
            //
            // lblWebhookPass
            //
            lblWebhookPass.Location = new Point(20, 552);
            lblWebhookPass.Name = "lblWebhookPass";
            lblWebhookPass.Size = new Size(110, 24);
            lblWebhookPass.Text = "Mật khẩu";
            lblWebhookPass.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtWebhookPass
            //
            txtWebhookPass.Location = new Point(140, 552);
            txtWebhookPass.Name = "txtWebhookPass";
            txtWebhookPass.Size = new Size(390, 23);
            //
            // lblSecTech
            //
            lblSecTech.AutoSize = true;
            lblSecTech.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecTech.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecTech.Location = new Point(20, 470);
            lblSecTech.Name = "lblSecTech";
            lblSecTech.TabIndex = 23;
            lblSecTech.Text = "KỸ THUẬT";
            //
            // txtFastRead
            //
            txtFastRead.AutoSize = true;
            txtFastRead.Location = new Point(22, 494);
            txtFastRead.Name = "txtFastRead";
            txtFastRead.TabIndex = 24;
            txtFastRead.Text = "Đọc nhanh";
            txtFastRead.UseVisualStyleBackColor = true;
            txtFastRead.CheckedChanged += txtFastRead_CheckedChanged;
            //
            // chkPaused
            //
            chkPaused.AutoSize = true;
            chkPaused.Location = new Point(140, 494);
            chkPaused.Name = "chkPaused";
            chkPaused.TabIndex = 25;
            chkPaused.Text = "Pause";
            chkPaused.UseVisualStyleBackColor = true;
            chkPaused.CheckedChanged += chkPaused_CheckedChanged;
            //
            // chkPauseIntCam
            //
            chkPauseIntCam.AutoSize = true;
            chkPauseIntCam.Location = new Point(22, 520);
            chkPauseIntCam.Name = "chkPauseIntCam";
            chkPauseIntCam.TabIndex = 26;
            chkPauseIntCam.Text = "Pause Int-Cam";
            chkPauseIntCam.UseVisualStyleBackColor = true;
            chkPauseIntCam.CheckedChanged += chkPauseIntCam_CheckedChanged;
            //
            // label5
            //
            label5.Location = new Point(20, 548);
            label5.Name = "label5";
            label5.Size = new Size(90, 24);
            label5.TabIndex = 27;
            label5.Text = "OCR mode";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            //
            // cbOcrMode
            //
            cbOcrMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOcrMode.FormattingEnabled = true;
            cbOcrMode.Items.AddRange(new object[] { "FAST", "MID", "SLOW", "AUTO" });
            cbOcrMode.Location = new Point(115, 548);
            cbOcrMode.Name = "cbOcrMode";
            cbOcrMode.Size = new Size(100, 23);
            cbOcrMode.TabIndex = 28;
            cbOcrMode.SelectedIndexChanged += cbOcrMode_SelectedIndexChanged;
            //
            // btnTest
            //
            btnTest.BackColor = Color.White;
            btnTest.Cursor = Cursors.Hand;
            btnTest.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnTest.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.Location = new Point(20, 584);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(90, 28);
            btnTest.TabIndex = 29;
            btnTest.Text = "Waken";
            btnTest.UseVisualStyleBackColor = false;
            btnTest.Click += btnTest_Click;
            //
            // btnRereadcard1
            //
            btnRereadcard1.BackColor = Color.White;
            btnRereadcard1.Cursor = Cursors.Hand;
            btnRereadcard1.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnRereadcard1.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnRereadcard1.FlatStyle = FlatStyle.Flat;
            btnRereadcard1.Location = new Point(117, 584);
            btnRereadcard1.Name = "btnRereadcard1";
            btnRereadcard1.Size = new Size(93, 28);
            btnRereadcard1.TabIndex = 30;
            btnRereadcard1.Text = "Re-Read 1";
            btnRereadcard1.UseVisualStyleBackColor = false;
            btnRereadcard1.Click += btnRereadcard1_Click;
            //
            // btnRereadcard2
            //
            btnRereadcard2.BackColor = Color.White;
            btnRereadcard2.Cursor = Cursors.Hand;
            btnRereadcard2.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnRereadcard2.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnRereadcard2.FlatStyle = FlatStyle.Flat;
            btnRereadcard2.Location = new Point(217, 584);
            btnRereadcard2.Name = "btnRereadcard2";
            btnRereadcard2.Size = new Size(93, 28);
            btnRereadcard2.TabIndex = 31;
            btnRereadcard2.Text = "Re-Read 2";
            btnRereadcard2.UseVisualStyleBackColor = false;
            btnRereadcard2.Click += btnRereadcard2_Click;
            //
            // btnReadQR
            //
            btnReadQR.BackColor = Color.White;
            btnReadQR.Cursor = Cursors.Hand;
            btnReadQR.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnReadQR.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnReadQR.FlatStyle = FlatStyle.Flat;
            btnReadQR.Location = new Point(20, 618);
            btnReadQR.Name = "btnReadQR";
            btnReadQR.Size = new Size(90, 28);
            btnReadQR.TabIndex = 32;
            btnReadQR.Text = "AA";
            btnReadQR.UseVisualStyleBackColor = false;
            btnReadQR.Click += btnReadQR_Click;
            //
            // btnCompareTest
            //
            btnCompareTest.BackColor = Color.White;
            btnCompareTest.FlatAppearance.BorderColor = Color.FromArgb(197, 211, 227);
            btnCompareTest.FlatStyle = FlatStyle.Flat;
            btnCompareTest.Location = new Point(117, 618);
            btnCompareTest.Name = "btnCompareTest";
            btnCompareTest.Size = new Size(93, 28);
            btnCompareTest.TabIndex = 33;
            btnCompareTest.Text = "Cmp";
            btnCompareTest.UseVisualStyleBackColor = false;
            btnCompareTest.Visible = false;
            btnCompareTest.Click += btnCompareTest_Click;
            //
            // lblSecData
            //
            lblSecData.AutoSize = true;
            lblSecData.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecData.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecData.Location = new Point(20, 664);
            lblSecData.Name = "lblSecData";
            lblSecData.TabIndex = 34;
            lblSecData.Text = "DỮ LIỆU KỸ THUẬT";
            //
            // txtMRZ
            //
            txtMRZ.BackColor = Color.FromArgb(243, 246, 250);
            txtMRZ.BorderStyle = BorderStyle.FixedSingle;
            txtMRZ.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtMRZ.Location = new Point(20, 688);
            txtMRZ.Multiline = true;
            txtMRZ.Name = "txtMRZ";
            txtMRZ.ReadOnly = true;
            txtMRZ.Size = new Size(290, 60);
            txtMRZ.TabIndex = 35;
            //
            // txtDSCert
            //
            txtDSCert.BackColor = Color.FromArgb(243, 246, 250);
            txtDSCert.BorderStyle = BorderStyle.FixedSingle;
            txtDSCert.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtDSCert.Location = new Point(20, 756);
            txtDSCert.Multiline = true;
            txtDSCert.Name = "txtDSCert";
            txtDSCert.ReadOnly = true;
            txtDSCert.ScrollBars = ScrollBars.Vertical;
            txtDSCert.Size = new Size(290, 90);
            txtDSCert.TabIndex = 36;
            //
            // lblSecScan
            //
            lblSecScan.AutoSize = true;
            lblSecScan.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecScan.ForeColor = Color.FromArgb(107, 114, 128);
            lblSecScan.Location = new Point(20, 864);
            lblSecScan.Name = "lblSecScan";
            lblSecScan.TabIndex = 37;
            lblSecScan.Text = "ẢNH QUÉT 2 MẶT THẺ";
            //
            // picIdcardFront
            //
            picIdcardFront.BackColor = Color.FromArgb(243, 246, 250);
            picIdcardFront.Location = new Point(20, 888);
            picIdcardFront.Name = "picIdcardFront";
            picIdcardFront.Size = new Size(290, 183);
            picIdcardFront.SizeMode = PictureBoxSizeMode.Zoom;
            picIdcardFront.TabIndex = 38;
            picIdcardFront.TabStop = false;
            picIdcardFront.Visible = false;
            //
            // pictureIdcardBack
            //
            pictureIdcardBack.BackColor = Color.FromArgb(243, 246, 250);
            pictureIdcardBack.Location = new Point(20, 1079);
            pictureIdcardBack.Name = "pictureIdcardBack";
            pictureIdcardBack.Size = new Size(290, 183);
            pictureIdcardBack.SizeMode = PictureBoxSizeMode.Zoom;
            pictureIdcardBack.TabIndex = 39;
            pictureIdcardBack.TabStop = false;
            pictureIdcardBack.Visible = false;
            //
            // pnlBody
            //
            pnlBody.BackColor = Color.FromArgb(238, 242, 247);
            pnlBody.Controls.Add(cardMain);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 72);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(20);
            pnlBody.Size = new Size(1440, 788);
            pnlBody.TabIndex = 0;
            //
            // cardMain
            //
            cardMain.Controls.Add(tlpMain);
            cardMain.Dock = DockStyle.Fill;
            cardMain.Location = new Point(20, 20);
            cardMain.Name = "cardMain";
            cardMain.Padding = new Padding(28, 16, 28, 20);
            cardMain.Size = new Size(1400, 748);
            cardMain.TabIndex = 0;
            //
            // tlpMain
            //
            tlpMain.BackColor = Color.White;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(tlpStatus, 0, 0);
            tlpMain.Controls.Add(pnlSeparator, 0, 1);
            tlpMain.Controls.Add(tlpInfo, 0, 2);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(28, 16);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 3;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.Size = new Size(1344, 712);
            tlpMain.TabIndex = 0;
            //
            // tlpStatus
            //
            tlpStatus.ColumnCount = 5;
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpStatus.Controls.Add(flpStatusMsg, 0, 0);
            tlpStatus.Controls.Add(pnlDevice, 1, 0);
            tlpStatus.Controls.Add(pnlSerial, 2, 0);
            tlpStatus.Controls.Add(pnlCamera, 3, 0);
            tlpStatus.Controls.Add(flpActions, 4, 0);
            tlpStatus.Dock = DockStyle.Fill;
            tlpStatus.Location = new Point(0, 0);
            tlpStatus.Margin = new Padding(0);
            tlpStatus.Name = "tlpStatus";
            tlpStatus.RowCount = 1;
            tlpStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpStatus.Size = new Size(1344, 56);
            tlpStatus.TabIndex = 0;
            //
            // pnlDevice
            //
            pnlDevice.Controls.Add(label4);
            pnlDevice.Controls.Add(lbDeviceState);
            pnlDevice.Dock = DockStyle.Fill;
            pnlDevice.Margin = new Padding(0);
            pnlDevice.Name = "pnlDevice";
            pnlDevice.TabIndex = 0;
            //
            // label4
            //
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(90, 56);
            label4.TabIndex = 0;
            label4.Text = "Thiết bị:";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lbDeviceState
            //
            lbDeviceState.AutoEllipsis = true;
            lbDeviceState.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbDeviceState.ForeColor = Color.FromArgb(211, 47, 47);
            lbDeviceState.Location = new Point(90, 0);
            lbDeviceState.Name = "lbDeviceState";
            lbDeviceState.Size = new Size(155, 56);
            lbDeviceState.TabIndex = 1;
            lbDeviceState.Text = "CHƯA KẾT NỐI";
            lbDeviceState.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlSerial
            //
            pnlSerial.Controls.Add(label6);
            pnlSerial.Controls.Add(lbReaderStatus);
            pnlSerial.Dock = DockStyle.Fill;
            pnlSerial.Margin = new Padding(0);
            pnlSerial.Name = "pnlSerial";
            pnlSerial.TabIndex = 1;
            //
            // label6
            //
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(70, 56);
            label6.TabIndex = 0;
            label6.Text = "Serial:";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lbReaderStatus
            //
            lbReaderStatus.AutoEllipsis = true;
            lbReaderStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbReaderStatus.ForeColor = Color.FromArgb(211, 47, 47);
            lbReaderStatus.Location = new Point(70, 0);
            lbReaderStatus.Name = "lbReaderStatus";
            lbReaderStatus.Size = new Size(185, 56);
            lbReaderStatus.TabIndex = 1;
            lbReaderStatus.Text = "KHÔNG CÓ";
            lbReaderStatus.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlCamera
            //
            pnlCamera.Controls.Add(lblCamera);
            pnlCamera.Controls.Add(lbCameraName);
            pnlCamera.Controls.Add(cbFaceCameras);
            pnlCamera.Dock = DockStyle.Fill;
            pnlCamera.Margin = new Padding(0);
            pnlCamera.Name = "pnlCamera";
            pnlCamera.TabIndex = 2;
            //
            // lblCamera
            //
            lblCamera.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCamera.Location = new Point(0, 0);
            lblCamera.Name = "lblCamera";
            lblCamera.Size = new Size(90, 56);
            lblCamera.TabIndex = 0;
            lblCamera.Text = "Camera:";
            lblCamera.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lbCameraName
            //
            lbCameraName.AutoEllipsis = true;
            lbCameraName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbCameraName.ForeColor = Color.FromArgb(211, 47, 47);
            lbCameraName.Location = new Point(90, 0);
            lbCameraName.Name = "lbCameraName";
            lbCameraName.Size = new Size(285, 56);
            lbCameraName.TabIndex = 1;
            lbCameraName.Text = "KHÔNG CÓ";
            lbCameraName.TextAlign = ContentAlignment.MiddleLeft;
            //
            // flpStatusMsg
            //
            flpStatusMsg.Controls.Add(lbStatusMessage);
            flpStatusMsg.Controls.Add(spinner);
            flpStatusMsg.Dock = DockStyle.Fill;
            flpStatusMsg.Margin = new Padding(0);
            flpStatusMsg.Name = "flpStatusMsg";
            flpStatusMsg.TabIndex = 8;
            flpStatusMsg.WrapContents = false;
            //
            // lbStatusMessage
            //
            lbStatusMessage.AutoSize = true;
            lbStatusMessage.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lbStatusMessage.ForeColor = Color.FromArgb(11, 59, 102);
            lbStatusMessage.Margin = new Padding(0, 10, 16, 0);
            lbStatusMessage.Name = "lbStatusMessage";
            lbStatusMessage.TabIndex = 0;
            lbStatusMessage.Text = "ĐANG TÌM THIẾT BỊ...";
            //
            // spinner
            //
            spinner.Margin = new Padding(0, 10, 0, 0);
            spinner.Name = "spinner";
            spinner.Size = new Size(36, 36);
            spinner.TabIndex = 1;
            //
            // flpActions
            //
            flpActions.Controls.Add(btnSend);
            flpActions.Controls.Add(btnFingerMain);
            flpActions.Controls.Add(btnSettings);
            flpActions.AutoSize = true;
            flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActions.Dock = DockStyle.Fill;
            flpActions.FlowDirection = FlowDirection.RightToLeft;
            flpActions.Margin = new Padding(0);
            flpActions.Name = "flpActions";
            flpActions.TabIndex = 9;
            flpActions.WrapContents = false;
            //
            // btnSend
            //
            btnSend.BackColor = Color.FromArgb(46, 125, 50);
            btnSend.Cursor = Cursors.Hand;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatAppearance.MouseOverBackColor = Color.FromArgb(27, 94, 32);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnSend.ForeColor = Color.White;
            btnSend.Margin = new Padding(0, 6, 0, 0);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(250, 44);
            btnSend.TabIndex = 2;
            btnSend.Text = "GỬI THÔNG TIN";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            //
            // btnFingerMain
            //
            btnFingerMain.BackColor = Color.FromArgb(21, 101, 192);
            btnFingerMain.Cursor = Cursors.Hand;
            btnFingerMain.FlatAppearance.BorderSize = 0;
            btnFingerMain.FlatAppearance.MouseOverBackColor = Color.FromArgb(13, 71, 161);
            btnFingerMain.FlatStyle = FlatStyle.Flat;
            btnFingerMain.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnFingerMain.ForeColor = Color.White;
            btnFingerMain.Margin = new Padding(0, 6, 8, 0);
            btnFingerMain.Name = "btnFingerMain";
            btnFingerMain.Size = new Size(110, 44);
            btnFingerMain.TabIndex = 1;
            btnFingerMain.Text = "VÂN TAY";
            btnFingerMain.UseVisualStyleBackColor = false;
            btnFingerMain.Visible = false;
            btnFingerMain.Click += btnFinger_Click;
            //
            // pnlTime
            //
            pnlTime.BackColor = Color.FromArgb(243, 246, 250);
            pnlTime.Controls.Add(lbTime);
            pnlTime.Controls.Add(lbTimeLb);
            pnlTime.Dock = DockStyle.Fill;
            pnlTime.Margin = new Padding(0, 12, 0, 4);
            pnlTime.Name = "pnlTime";
            pnlTime.Padding = new Padding(16, 0, 16, 0);
            pnlTime.TabIndex = 9;
            //
            // lbTime
            //
            lbTime.Dock = DockStyle.Fill;
            lbTime.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lbTime.ForeColor = Color.FromArgb(21, 101, 192);
            lbTime.Name = "lbTime";
            lbTime.TabIndex = 1;
            lbTime.Text = "00.000 s";
            lbTime.TextAlign = ContentAlignment.MiddleRight;
            //
            // lbTimeLb
            //
            lbTimeLb.Dock = DockStyle.Left;
            lbTimeLb.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lbTimeLb.ForeColor = Color.FromArgb(107, 114, 128);
            lbTimeLb.Name = "lbTimeLb";
            lbTimeLb.Size = new Size(120, 54);
            lbTimeLb.TabIndex = 0;
            lbTimeLb.Text = "THỜI GIAN ĐỌC";
            lbTimeLb.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlSeparator
            //
            pnlSeparator.BackColor = Color.FromArgb(225, 231, 239);
            pnlSeparator.Dock = DockStyle.Fill;
            pnlSeparator.Margin = new Padding(0, 10, 0, 10);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.TabIndex = 1;
            //
            // tlpInfo
            //
            tlpInfo.ColumnCount = 7;
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 36F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInfo.Controls.Add(label3, 0, 0);
            tlpInfo.Controls.Add(picChipImage, 0, 1);
            tlpInfo.Controls.Add(label1, 0, 6);
            tlpInfo.Controls.Add(picCamera, 0, 7);
            tlpInfo.Controls.Add(tlpFields, 2, 0);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Margin = new Padding(0);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.RowCount = 12;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpInfo.SetRowSpan(picChipImage, 5);
            tlpInfo.SetRowSpan(picCamera, 5);
            tlpInfo.SetColumnSpan(tlpFields, 5);
            tlpInfo.SetRowSpan(tlpFields, 12);
            tlpInfo.TabIndex = 2;
            //
            // tlpFields
            //
            tlpFields.ColumnCount = 5;
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 36F));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpFields.Controls.Add(lblSecCollected, 0, 3);
            tlpFields.Controls.Add(chkConsent, 0, 2);
            tlpFields.Controls.Add(lblCapDocNumber, 0, 4);
            tlpFields.Controls.Add(txtDocumentNumber, 1, 4);
            tlpFields.Controls.Add(lblCapPrevNumber, 3, 4);
            tlpFields.Controls.Add(txtPreviousNumber, 4, 4);
            tlpFields.Controls.Add(lblCapName, 0, 5);
            tlpFields.Controls.Add(txtName, 1, 5);
            tlpFields.Controls.Add(lblCapSex, 3, 5);
            tlpFields.Controls.Add(txtSex, 4, 5);
            tlpFields.Controls.Add(lblCapDob, 0, 6);
            tlpFields.Controls.Add(txtDateOfBirth, 1, 6);
            tlpFields.Controls.Add(lblCapNationality, 3, 6);
            tlpFields.Controls.Add(txtNationality, 4, 6);
            tlpFields.Controls.Add(lblCapNation, 0, 7);
            tlpFields.Controls.Add(txtNation, 1, 7);
            tlpFields.Controls.Add(lblCapReligion, 3, 7);
            tlpFields.Controls.Add(txtReligion, 4, 7);
            tlpFields.Controls.Add(lblCapCharacter, 0, 8);
            tlpFields.Controls.Add(txtCharacter, 1, 8);
            tlpFields.Controls.Add(lblCapHometown, 0, 10);
            tlpFields.Controls.Add(txtHometown, 1, 10);
            tlpFields.Controls.Add(lblCapAddress, 0, 11);
            tlpFields.Controls.Add(txtAddress, 1, 11);
            tlpFields.Controls.Add(lblCapIssueDate, 0, 12);
            tlpFields.Controls.Add(txtIssueDate, 1, 12);
            tlpFields.Controls.Add(lblCapExpiredDate, 3, 12);
            tlpFields.Controls.Add(txtExpiredDate, 4, 12);
            tlpFields.Controls.Add(lblCapFather, 0, 13);
            tlpFields.Controls.Add(txtFatherName, 1, 13);
            tlpFields.Controls.Add(lblCapMother, 3, 13);
            tlpFields.Controls.Add(txtMotherName, 4, 13);
            tlpFields.Controls.Add(lblCapPartner, 0, 14);
            tlpFields.Controls.Add(txtPartnerName, 1, 14);
            tlpFields.Dock = DockStyle.Fill;
            tlpFields.Margin = new Padding(0);
            tlpFields.Name = "tlpFields";
            tlpFields.RowCount = 15;
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpFields.SetColumnSpan(lblSecCollected, 5);
            tlpFields.SetColumnSpan(chkConsent, 5);
            tlpFields.SetColumnSpan(txtCharacter, 4);
            tlpFields.SetColumnSpan(txtHometown, 4);
            tlpFields.SetColumnSpan(txtAddress, 4);
            tlpFields.SetColumnSpan(txtPartnerName, 4);
            tlpFields.TabIndex = 0;
            //
            // label3
            //
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(21, 101, 192);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.TabIndex = 0;
            label3.Text = "Ảnh trong CCCD";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            //
            // picChipImage
            //
            picChipImage.BackColor = Color.FromArgb(243, 246, 250);
            picChipImage.Dock = DockStyle.Fill;
            picChipImage.Margin = new Padding(0, 4, 0, 8);
            picChipImage.Name = "picChipImage";
            picChipImage.SizeMode = PictureBoxSizeMode.Zoom;
            picChipImage.TabIndex = 1;
            picChipImage.TabStop = false;
            //
            // btnCaptureFace
            //
            btnCaptureFace.Anchor = AnchorStyles.None;
            btnCaptureFace.BackColor = Color.FromArgb(243, 246, 250);
            btnCaptureFace.Cursor = Cursors.Hand;
            btnCaptureFace.FlatAppearance.BorderSize = 0;
            btnCaptureFace.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 251);
            btnCaptureFace.FlatStyle = FlatStyle.Flat;
            btnCaptureFace.Font = new Font("Segoe MDL2 Assets", 26F, FontStyle.Regular, GraphicsUnit.Point);
            btnCaptureFace.ForeColor = Color.FromArgb(21, 101, 192);
            btnCaptureFace.Margin = new Padding(0);
            btnCaptureFace.Name = "btnCaptureFace";
            btnCaptureFace.Size = new Size(72, 72);
            btnCaptureFace.TabIndex = 1;
            btnCaptureFace.Text = "";
            toolTip1.SetToolTip(btnCaptureFace, "Chụp ảnh chân dung");
            btnCaptureFace.UseVisualStyleBackColor = false;
            btnCaptureFace.Click += btnFaceMatching_Click;
            //
            // label1
            //
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(21, 101, 192);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.TabIndex = 2;
            label1.Text = "Ảnh chân dung";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            //
            // picCamera
            //
            picCamera.BackColor = Color.FromArgb(243, 246, 250);
            picCamera.Controls.Add(btnCaptureFace);
            picCamera.Cursor = Cursors.Hand;
            picCamera.Dock = DockStyle.Fill;
            picCamera.Margin = new Padding(0, 4, 0, 4);
            picCamera.Name = "picCamera";
            picCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picCamera.TabIndex = 3;
            picCamera.TabStop = false;
            toolTip1.SetToolTip(picCamera, "Bấm để chụp ảnh chân dung");
            picCamera.Click += btnFaceMatching_Click;
            picCamera.Resize += picCamera_Resize;
            //
            // lblCapDocNumber
            //
            lblCapDocNumber.AutoEllipsis = true;
            lblCapDocNumber.Dock = DockStyle.Fill;
            lblCapDocNumber.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapDocNumber.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapDocNumber.Name = "lblCapDocNumber";
            lblCapDocNumber.TabIndex = 10;
            lblCapDocNumber.Text = "Số CCCD";
            lblCapDocNumber.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtDocumentNumber
            //
            txtDocumentNumber.CaptionLabel = lblCapDocNumber;
            txtDocumentNumber.Dock = DockStyle.Fill;
            txtDocumentNumber.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtDocumentNumber.Margin = new Padding(0, 7, 0, 7);
            txtDocumentNumber.Name = "txtDocumentNumber";
            txtDocumentNumber.TabIndex = 11;
            //
            // lblCapPrevNumber
            //
            lblCapPrevNumber.AutoEllipsis = true;
            lblCapPrevNumber.Dock = DockStyle.Fill;
            lblCapPrevNumber.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapPrevNumber.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapPrevNumber.Name = "lblCapPrevNumber";
            lblCapPrevNumber.TabIndex = 12;
            lblCapPrevNumber.Text = "Số CMND";
            lblCapPrevNumber.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtPreviousNumber
            //
            txtPreviousNumber.CaptionLabel = lblCapPrevNumber;
            txtPreviousNumber.Dock = DockStyle.Fill;
            txtPreviousNumber.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtPreviousNumber.Margin = new Padding(0, 7, 0, 7);
            txtPreviousNumber.Name = "txtPreviousNumber";
            txtPreviousNumber.TabIndex = 13;
            //
            // lblCapName
            //
            lblCapName.AutoEllipsis = true;
            lblCapName.Dock = DockStyle.Fill;
            lblCapName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapName.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapName.Name = "lblCapName";
            lblCapName.TabIndex = 14;
            lblCapName.Text = "Họ và tên";
            lblCapName.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtName
            //
            txtName.CaptionLabel = lblCapName;
            txtName.Dock = DockStyle.Fill;
            txtName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtName.Margin = new Padding(0, 7, 0, 7);
            txtName.Name = "txtName";
            txtName.TabIndex = 15;
            //
            // lblCapSex
            //
            lblCapSex.AutoEllipsis = true;
            lblCapSex.Dock = DockStyle.Fill;
            lblCapSex.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapSex.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapSex.Name = "lblCapSex";
            lblCapSex.TabIndex = 16;
            lblCapSex.Text = "Giới tính";
            lblCapSex.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtSex
            //
            txtSex.CaptionLabel = lblCapSex;
            txtSex.Dock = DockStyle.Fill;
            txtSex.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtSex.Margin = new Padding(0, 7, 0, 7);
            txtSex.Name = "txtSex";
            txtSex.TabIndex = 17;
            //
            // lblCapDob
            //
            lblCapDob.AutoEllipsis = true;
            lblCapDob.Dock = DockStyle.Fill;
            lblCapDob.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapDob.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapDob.Name = "lblCapDob";
            lblCapDob.TabIndex = 18;
            lblCapDob.Text = "Ngày sinh";
            lblCapDob.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtDateOfBirth
            //
            txtDateOfBirth.CaptionLabel = lblCapDob;
            txtDateOfBirth.Dock = DockStyle.Fill;
            txtDateOfBirth.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtDateOfBirth.Margin = new Padding(0, 7, 0, 7);
            txtDateOfBirth.Name = "txtDateOfBirth";
            txtDateOfBirth.TabIndex = 19;
            //
            // lblCapNationality
            //
            lblCapNationality.AutoEllipsis = true;
            lblCapNationality.Dock = DockStyle.Fill;
            lblCapNationality.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapNationality.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapNationality.Name = "lblCapNationality";
            lblCapNationality.TabIndex = 20;
            lblCapNationality.Text = "Quốc tịch";
            lblCapNationality.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtNationality
            //
            txtNationality.CaptionLabel = lblCapNationality;
            txtNationality.Dock = DockStyle.Fill;
            txtNationality.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtNationality.Margin = new Padding(0, 7, 0, 7);
            txtNationality.Name = "txtNationality";
            txtNationality.TabIndex = 21;
            //
            // lblCapNation
            //
            lblCapNation.AutoEllipsis = true;
            lblCapNation.Dock = DockStyle.Fill;
            lblCapNation.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapNation.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapNation.Name = "lblCapNation";
            lblCapNation.TabIndex = 22;
            lblCapNation.Text = "Dân tộc";
            lblCapNation.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtNation
            //
            txtNation.CaptionLabel = lblCapNation;
            txtNation.Dock = DockStyle.Fill;
            txtNation.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtNation.Margin = new Padding(0, 7, 0, 7);
            txtNation.Name = "txtNation";
            txtNation.TabIndex = 23;
            //
            // lblCapReligion
            //
            lblCapReligion.AutoEllipsis = true;
            lblCapReligion.Dock = DockStyle.Fill;
            lblCapReligion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapReligion.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapReligion.Name = "lblCapReligion";
            lblCapReligion.TabIndex = 24;
            lblCapReligion.Text = "Tôn giáo";
            lblCapReligion.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtReligion
            //
            txtReligion.CaptionLabel = lblCapReligion;
            txtReligion.Dock = DockStyle.Fill;
            txtReligion.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtReligion.Margin = new Padding(0, 7, 0, 7);
            txtReligion.Name = "txtReligion";
            txtReligion.TabIndex = 25;
            //
            // lblCapCharacter
            //
            lblCapCharacter.AutoEllipsis = true;
            lblCapCharacter.Dock = DockStyle.Fill;
            lblCapCharacter.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapCharacter.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapCharacter.Name = "lblCapCharacter";
            lblCapCharacter.TabIndex = 26;
            lblCapCharacter.Text = "Nhận dạng";
            lblCapCharacter.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtCharacter
            //
            txtCharacter.CaptionLabel = lblCapCharacter;
            txtCharacter.Dock = DockStyle.Fill;
            txtCharacter.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtCharacter.Margin = new Padding(0, 7, 0, 7);
            txtCharacter.Name = "txtCharacter";
            txtCharacter.TabIndex = 27;
            //
            // lblCapHometown
            //
            lblCapHometown.AutoEllipsis = true;
            lblCapHometown.Dock = DockStyle.Fill;
            lblCapHometown.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapHometown.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapHometown.Name = "lblCapHometown";
            lblCapHometown.TabIndex = 30;
            lblCapHometown.Text = "Quê quán";
            lblCapHometown.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtHometown
            //
            txtHometown.CaptionLabel = lblCapHometown;
            txtHometown.Dock = DockStyle.Fill;
            txtHometown.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtHometown.Margin = new Padding(0, 7, 0, 7);
            txtHometown.Name = "txtHometown";
            txtHometown.TabIndex = 31;
            //
            // lblCapAddress
            //
            lblCapAddress.AutoEllipsis = true;
            lblCapAddress.Dock = DockStyle.Fill;
            lblCapAddress.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapAddress.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapAddress.Name = "lblCapAddress";
            lblCapAddress.TabIndex = 32;
            lblCapAddress.Text = "Địa chỉ";
            lblCapAddress.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtAddress
            //
            txtAddress.CaptionLabel = lblCapAddress;
            txtAddress.Dock = DockStyle.Fill;
            txtAddress.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtAddress.Margin = new Padding(0, 7, 0, 7);
            txtAddress.Name = "txtAddress";
            txtAddress.TabIndex = 33;
            //
            // lblCapIssueDate
            //
            lblCapIssueDate.AutoEllipsis = true;
            lblCapIssueDate.Dock = DockStyle.Fill;
            lblCapIssueDate.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapIssueDate.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapIssueDate.Name = "lblCapIssueDate";
            lblCapIssueDate.TabIndex = 34;
            lblCapIssueDate.Text = "Ngày cấp";
            lblCapIssueDate.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtIssueDate
            //
            txtIssueDate.CaptionLabel = lblCapIssueDate;
            txtIssueDate.Dock = DockStyle.Fill;
            txtIssueDate.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtIssueDate.Margin = new Padding(0, 7, 0, 7);
            txtIssueDate.Name = "txtIssueDate";
            txtIssueDate.TabIndex = 35;
            //
            // lblCapExpiredDate
            //
            lblCapExpiredDate.AutoEllipsis = true;
            lblCapExpiredDate.Dock = DockStyle.Fill;
            lblCapExpiredDate.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapExpiredDate.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapExpiredDate.Name = "lblCapExpiredDate";
            lblCapExpiredDate.TabIndex = 36;
            lblCapExpiredDate.Text = "Ngày hết hạn";
            lblCapExpiredDate.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtExpiredDate
            //
            txtExpiredDate.CaptionLabel = lblCapExpiredDate;
            txtExpiredDate.Dock = DockStyle.Fill;
            txtExpiredDate.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtExpiredDate.Margin = new Padding(0, 7, 0, 7);
            txtExpiredDate.Name = "txtExpiredDate";
            txtExpiredDate.TabIndex = 37;
            //
            // lblCapFather
            //
            lblCapFather.AutoEllipsis = true;
            lblCapFather.Dock = DockStyle.Fill;
            lblCapFather.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapFather.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapFather.Name = "lblCapFather";
            lblCapFather.TabIndex = 38;
            lblCapFather.Text = "Họ tên bố";
            lblCapFather.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtFatherName
            //
            txtFatherName.CaptionLabel = lblCapFather;
            txtFatherName.Dock = DockStyle.Fill;
            txtFatherName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtFatherName.Margin = new Padding(0, 7, 0, 7);
            txtFatherName.Name = "txtFatherName";
            txtFatherName.TabIndex = 39;
            //
            // lblCapMother
            //
            lblCapMother.AutoEllipsis = true;
            lblCapMother.Dock = DockStyle.Fill;
            lblCapMother.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapMother.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapMother.Name = "lblCapMother";
            lblCapMother.TabIndex = 40;
            lblCapMother.Text = "Họ tên mẹ";
            lblCapMother.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtMotherName
            //
            txtMotherName.CaptionLabel = lblCapMother;
            txtMotherName.Dock = DockStyle.Fill;
            txtMotherName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtMotherName.Margin = new Padding(0, 7, 0, 7);
            txtMotherName.Name = "txtMotherName";
            txtMotherName.TabIndex = 41;
            //
            // lblCapPartner
            //
            lblCapPartner.AutoEllipsis = true;
            lblCapPartner.Dock = DockStyle.Fill;
            lblCapPartner.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCapPartner.ForeColor = Color.FromArgb(21, 101, 192);
            lblCapPartner.Name = "lblCapPartner";
            lblCapPartner.TabIndex = 42;
            lblCapPartner.Text = "Vợ/Chồng";
            lblCapPartner.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtPartnerName
            //
            txtPartnerName.CaptionLabel = lblCapPartner;
            txtPartnerName.Dock = DockStyle.Fill;
            txtPartnerName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            txtPartnerName.Margin = new Padding(0, 7, 0, 7);
            txtPartnerName.Name = "txtPartnerName";
            txtPartnerName.TabIndex = 43;
            //
            // chkConsent
            //
            chkConsent.Dock = DockStyle.Fill;
            chkConsent.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            chkConsent.Margin = new Padding(0);
            chkConsent.Name = "chkConsent";
            chkConsent.TabIndex = 50;
            chkConsent.Text = "Tôi đồng ý với [chính sách, điều khoản] xử lý bảo vệ dữ liệu cá nhân của [EasyCA!]";
            //
            // lblSecCollected
            //
            lblSecCollected.Dock = DockStyle.Fill;
            lblSecCollected.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblSecCollected.ForeColor = Color.FromArgb(11, 59, 102);
            lblSecCollected.Heading = true;
            lblSecCollected.Margin = new Padding(0);
            lblSecCollected.Name = "lblSecCollected";
            lblSecCollected.TabIndex = 49;
            lblSecCollected.Text = "THÔNG TIN THU THẬP";
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(238, 242, 247);
            ClientSize = new Size(1440, 860);
            Controls.Add(pnlBody);
            Controls.Add(pnlSettings);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            MinimumSize = new Size(1024, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xác thực Căn cước công dân - HANEL eKYC";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            pnlTop.ResumeLayout(false);
            tlpToolbar.ResumeLayout(false);
            tlpToolbar.PerformLayout();
            flpToolbar.ResumeLayout(false);
            pnlSettings.ResumeLayout(false);
            pnlSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFaceTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMatchThreshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFaceDurationTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)picIdcardFront).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureIdcardBack).EndInit();
            pnlBody.ResumeLayout(false);
            cardMain.ResumeLayout(false);
            tlpMain.ResumeLayout(false);
            tlpStatus.ResumeLayout(false);
            pnlDevice.ResumeLayout(false);
            pnlCamera.ResumeLayout(false);
            pnlSerial.ResumeLayout(false);
            flpStatusMsg.ResumeLayout(false);
            flpActions.ResumeLayout(false);
            flpStatusMsg.PerformLayout();
            pnlTime.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            tlpFields.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picChipImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCamera).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private Panel pnlTop;
        private TableLayoutPanel tlpToolbar;
        private FlowLayoutPanel flpToolbar;
        private Button btnSettings;
        private Button btnFaceMatching;
        private Button btnCompare;
        private Button btnFinger;
        private Button btnClearForm;
        private Button btnExit;
        private Panel pnlTopLine;
        private Panel pnlSettings;
        private Panel pnlSettingsLine;
        private Label lblSettingsTitle;
        private Label lblSecRead;
        private Label label2;
        private ComboBox cbReaderTypes;
        private Label lblSecWebhook;
        private Label lblWebhookUrl;
        private TextBox txtWebhookUrl;
        private Label lblWebhookUser;
        private TextBox txtWebhookUser;
        private Label lblWebhookPass;
        private TextBox txtWebhookPass;
        private Button btnCaptureFace;
        private Label lblProcess;
        private ComboBox cbProcessStep;
        private CheckBox chkAutoRead;
        private CheckBox chkDelteForm;
        private Button btnRejectCard;
        private Label lblSecCamera;
        private Label label7;
        private ComboBox cbFaceCameras;
        private Button btnRefreshCamera;
        private Label lblFaceTimeout;
        private NumericUpDown numFaceTimeout;
        private Label label9;
        private NumericUpDown numFaceDurationTime;
        private Label label8;
        private ComboBox cbAntiSpoofing;
        private CheckBox chkDetectFace;
        private CheckBox chkAutocapture;
        private CheckBox chkShowTimeout;
        private CheckBox chkAutoCompare;
        private Label lblMatchThreshold;
        private NumericUpDown numMatchThreshold;
        private Label lblSecTech;
        private CheckBox txtFastRead;
        private CheckBox chkPaused;
        private CheckBox chkPauseIntCam;
        private Label label5;
        private ComboBox cbOcrMode;
        private Button btnTest;
        private Button btnRereadcard1;
        private Button btnRereadcard2;
        private Button btnReadQR;
        private Button btnCompareTest;
        private Label lblSecData;
        private TextBox txtMRZ;
        private TextBox txtDSCert;
        private Label lblSecScan;
        private PictureBox picIdcardFront;
        private PictureBox pictureIdcardBack;
        private Panel pnlBody;
        private CardPanel cardMain;
        private TableLayoutPanel tlpMain;
        private TableLayoutPanel tlpStatus;
        private Panel pnlDevice;
        private Label label4;
        private Label lbDeviceState;
        private Panel pnlSerial;
        private Label label6;
        private Label lbReaderStatus;
        private Panel pnlCamera;
        private Label lblCamera;
        private Label lbCameraName;
        private FlowLayoutPanel flpStatusMsg;
        private Label lbStatusMessage;
        private Spinner spinner;
        private FlowLayoutPanel flpActions;
        private Button btnFingerMain;
        private Button btnSend;
        private Panel pnlTime;
        private Label lbTime;
        private Label lbTimeLb;
        private Panel pnlSeparator;
        private TableLayoutPanel tlpInfo;
        private TableLayoutPanel tlpFields;
        private Label label3;
        private PictureBox picChipImage;
        private Label label1;
        private PictureBox picCamera;
        private Label lblCapDocNumber;
        private FieldLabel txtDocumentNumber;
        private Label lblCapPrevNumber;
        private FieldLabel txtPreviousNumber;
        private Label lblCapName;
        private FieldLabel txtName;
        private Label lblCapSex;
        private FieldLabel txtSex;
        private Label lblCapDob;
        private FieldLabel txtDateOfBirth;
        private Label lblCapNationality;
        private FieldLabel txtNationality;
        private Label lblCapNation;
        private FieldLabel txtNation;
        private Label lblCapReligion;
        private FieldLabel txtReligion;
        private Label lblCapCharacter;
        private FieldLabel txtCharacter;
        private Label lblCapHometown;
        private FieldLabel txtHometown;
        private Label lblCapAddress;
        private FieldLabel txtAddress;
        private Label lblCapIssueDate;
        private FieldLabel txtIssueDate;
        private Label lblCapExpiredDate;
        private FieldLabel txtExpiredDate;
        private Label lblCapFather;
        private FieldLabel txtFatherName;
        private Label lblCapMother;
        private FieldLabel txtMotherName;
        private Label lblCapPartner;
        private ConsentCheck chkConsent;
        private SuffixLabel lblSecCollected;
        private FieldLabel txtPartnerName;
    }
}
