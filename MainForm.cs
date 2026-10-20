using AForge.Video.DirectShow;
using log4net;
using Microsoft.VisualBasic.Logging;
using Org.BouncyCastle.Crypto.Paddings;
using RAR.IdCard.Sdk.Reader;
using RAR.IdCard.Sdk.Reader.HN212;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace IdCard.Hanel
{
    public partial class MainForm : Form
    {
        // VnPcscReader _reader = new VnPcscReader();
        VnHn212Reader _reader = new VnHn212Reader();
        Timer _timer = new Timer();
        Stopwatch _stw = new Stopwatch();
        readonly AppSettings _settings = AppSettings.Load();
        int? _lastMatchScore;
        // Dữ liệu thẻ của lần đọc thành công gần nhất, null khi chưa đọc hoặc đã xoá form
        CardFullData? _cardData;
        string _readerSerial = "";
        // Thẻ đang nằm trong đầu đọc (theo sự kiện cắm/rút thẻ)
        bool _cardPresent;
        byte[]? _ocrFrontBts;
        byte[]? _ocrBackBts;
        // Vân tay đã thu: mã ngón (1-10) -> ảnh
        Dictionary<int, byte[]> _fingers = new();
        byte[]? _ChipFaceBts = null;
        byte[]? _CamFaceBts = null;
        // Kết quả so khớp hiển thị đè lên ảnh chụp
        readonly Label lblMatchResult = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 32,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Visible = false
        };
        /// <summary>Trang chính sách bảo mật mở khi bấm vào "chính sách, điều khoản" ở ô đồng ý.</summary>
        private const string PrivacyPolicyUrl = "https://softdreams.vn/chinh-sach-bao-mat";

        public MainForm()
        {
            InitializeComponent();
            // Icon trên thanh tiêu đề/taskbar lấy từ icon của exe (app.ico)
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (Exception) { }
            chkConsent.LinkClicked += (_, _) =>
            {
                using var policy = new PolicyViewerForm("Chính sách bảo vệ dữ liệu cá nhân", PrivacyPolicyUrl);
                policy.ShowDialog(this);
            };
            picCamera.Controls.Add(lblMatchResult);
            lblMatchResult.Click += btnFaceMatching_Click;
            Resize += (s, e) => ApplyResponsiveLayout();
            txtWebhookPass.Enter += txtWebhookPass_Enter;
            txtWebhookPass.KeyDown += txtWebhookPass_KeyDown;
            txtWebhookPass.KeyPress += txtWebhookPass_KeyPress;
            txtWebhookPass.ContextMenuStrip = new ContextMenuStrip();
            // Áp sau khi WinForms đã tự co giãn theo DPI để tránh nhân tỉ lệ hai lần
            Shown += (s, e) => { _layoutReady = true; ApplyResponsiveLayout(); };
            // Đăng ký sau khi có handle để BeginInvoke được; link mở app lần đầu sẽ được phát ngay lúc này
            Shown += (s, e) => DeepLink.Received += OnDeepLink;
        }

        /// <summary>
        /// Xử lý link hanelekyc://... (xem DeepLink). Không cho link đổi cấu hình webhook:
        /// trang web bất kỳ đều gọi được link nên nếu cho đổi sẽ bị lấy cắp dữ liệu CCCD.
        /// </summary>
        private void OnDeepLink(DeepLinkRequest req)
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                try { BeginInvoke(() => OnDeepLink(req)); } catch (Exception) { }
                return;
            }

            WriteLog($"DeepLink: {req.RawUrl}");
            BringToFrontWindow();
            switch (req.Action)
            {
                case "open":
                    break;
                case "read":
                    // Xoá dữ liệu lần đọc trước để sẵn sàng đọc thẻ mới
                    ResetFormInfo();
                    _transactionId = ParseTransactionId(req.Get("transactionId"));
                    WriteLog($"DeepLink: transactionId={_transactionId ?? "(null)"}");
                    break;
                default:
                    WriteLog($"DeepLink: action không hỗ trợ '{req.Action}'");
                    break;
            }
        }

        private string? _transactionId;

        private string? ParseTransactionId(string? value)
        {
            value = value?.Trim();
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value;
            WriteLog($"DeepLink: transactionId không hợp lệ, bỏ qua: {value}");
            return null;
        }

        private void BringToFrontWindow()
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Show();
            // Bật/tắt TopMost để Windows chịu đưa cửa sổ lên trên cùng
            TopMost = true;
            TopMost = false;
            Activate();
        }

        private bool _layoutReady;
        private bool? _compactHeight;
        private bool? _compactWidth;

        /// <summary>
        /// Thu gọn bố cục cho màn hình laptop (cửa sổ thấp/hẹp), giữ nguyên bố cục gốc trên màn hình lớn.
        /// </summary>
        private void ApplyResponsiveLayout()
        {
            if (!_layoutReady || WindowState == FormWindowState.Minimized)
                return;
            float k = DeviceDpi / 96f;
            int S(int v) => (int)Math.Round(v * k);
            bool shortH = ClientSize.Height < S(820);
            bool narrow = ClientSize.Width < S(1300);
            if (_compactHeight == shortH && _compactWidth == narrow)
                return;
            _compactHeight = shortH;
            _compactWidth = narrow;

            SuspendLayout();
            pnlBody.Padding = new Padding(S(shortH ? 10 : 20));
            cardMain.Padding = shortH ? new Padding(S(20), S(8), S(20), S(10)) : new Padding(S(28), S(16), S(28), S(20));

            tlpMain.RowStyles[0].Height = S(shortH ? 48 : 56);
            tlpMain.RowStyles[1].Height = S(shortH ? 13 : 21);
            pnlSeparator.Margin = new Padding(0, S(shortH ? 6 : 10), 0, S(shortH ? 6 : 10));

            int btnTop = S(shortH ? 2 : 6);
            btnSend.Margin = new Padding(0, btnTop, 0, 0);
            btnFingerMain.Margin = new Padding(0, btnTop, S(8), 0);
            btnSettings.Margin = new Padding(0, btnTop, S(8), 0);
            lbStatusMessage.Margin = new Padding(0, S(shortH ? 6 : 10), S(16), 0);
            spinner.Margin = new Padding(0, S(shortH ? 6 : 10), 0, 0);

            tlpInfo.RowStyles[0].Height = S(shortH ? 30 : 40);
            tlpInfo.RowStyles[6].Height = S(shortH ? 30 : 40);
            tlpInfo.ColumnStyles[0].Width = S(narrow ? 190 : 240);
            tlpFields.RowStyles[0].Height = S(shortH ? 30 : 40);
            tlpFields.RowStyles[2].Height = S(shortH ? 30 : 36);
            tlpFields.RowStyles[3].Height = S(shortH ? 30 : 40);
            tlpFields.ColumnStyles[0].Width = S(narrow ? 130 : 160);
            tlpFields.ColumnStyles[3].Width = S(narrow ? 130 : 160);
            ResumeLayout(true);
            LayoutStatusPair(pnlDevice, label4, lbDeviceState);
            LayoutStatusPair(pnlSerial, label6, lbReaderStatus);
            LayoutStatusPair(pnlCamera, lblCamera, lbCameraName, cbFaceCameras);
        }

        /// <summary>Nhãn vừa khít chữ, giá trị đặt ngay sau với khoảng cách cố định và giãn theo độ rộng cột.</summary>
        private void LayoutStatusPair(Panel pnl, Label caption, params Control[] values)
        {
            int gap = (int)Math.Round(12 * DeviceDpi / 96f);
            int h = pnl.ClientSize.Height;
            caption.SetBounds(0, 0, caption.PreferredWidth, h);
            int left = caption.Right + gap;
            int width = Math.Max(0, pnl.ClientSize.Width - left - gap);
            foreach (var v in values)
            {
                v.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                if (v is ComboBox)
                    v.SetBounds(left, (h - v.Height) / 2, width, v.Height);
                else
                    v.SetBounds(left, 0, width, h);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbOcrMode.SelectedIndex = 0; //ThanhNV test vietin
            cbReaderTypes.SelectedIndex = 0;
            cbAntiSpoofing.SelectedIndex = 0;
            cbProcessStep.SelectedIndex = 0; //ThanhNV test vietin
            _timer.Tick += _timer_Tick;
            _timer.Interval = 5;
            ResetFormInfo();
            txtWebhookUrl.Text = _settings.WebhookUrl;
            txtWebhookUser.Text = _settings.WebhookUser;
            chkAutoCompare.Checked = _settings.AutoCompareFace;
            numMatchThreshold.Value = Math.Clamp(_settings.FaceMatchThreshold, (int)numMatchThreshold.Minimum, (int)numMatchThreshold.Maximum);
            chkAutocapture.Checked = _settings.AutoCaptureFace;
            numFaceDurationTime.Value = Math.Clamp(_settings.AutoCaptureFaceMinimumMs, (int)numFaceDurationTime.Minimum, (int)numFaceDurationTime.Maximum);
            numFaceDurationTime.Enabled = chkAutocapture.Checked;
            chkDetectFace.Checked = _settings.CaptureFaceDetectFace;
            chkShowTimeout.Checked = _settings.CaptureFaceShowTimeout;
            numFaceTimeout.Value = Math.Clamp(_settings.CaptureFaceTimeoutMs, (int)numFaceTimeout.Minimum, (int)numFaceTimeout.Maximum);
            ShowPasswordMask();
            SetStatusMessage("ĐANG TÌM THIẾT BỊ...", Theme.Navy, true);
            InitReader();
            ActiveControl = null;
            // Load camera list
            // btnRefreshCamera_Click(new object(), new EventArgs());
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            lbTime.Text = $"{_stw.Elapsed.ToString(@"ss\.fff")} s";
            LoadExternalCameras();
        }

        private void ResetFormInfo()
        {
            _timer.Stop();
            _stw.Stop();
            lbTime.Text = "00.000 s";


            //Personal infor
            txtDocumentNumber.Text = "Số CCCD: ";
            txtDateOfBirth.Text = "Ngày sinh: ";
            txtIssueDate.Text = "Ngày cấp: ";
            txtPreviousNumber.Text = "Số CMND: ";
            txtName.Text = "Họ và tên: ";
            txtSex.Text = "Giới tính: ";
            txtNationality.Text = "Quốc tịch: ";
            txtNation.Text = "Dân tộc: ";
            txtReligion.Text = "Tôn giáo: ";
            txtHometown.Text = "Quê quán: ";
            txtAddress.Text = "Địa chỉ: ";
            txtCharacter.Text = "Nhận dạng: ";
            txtExpiredDate.Text = "Ngày hết hạn: ";
            txtFatherName.Text = "Họ tên bố: ";
            txtMotherName.Text = "Họ tên mẹ: ";
            txtPartnerName.Text = "Vợ/Chồng: ";
            txtMRZ.Text = "MRZ: ";
            txtDSCert.Text = "";

            //Chip image
            picChipImage.Image = null;
            _fingers.Clear();
            picIdcardFront.Image = null;
            pictureIdcardBack.Image = null;
            _ChipFaceBts = null;
            _cardData = null;
            _ocrFrontBts = null;
            _ocrBackBts = null;

            lblMatchResult.Visible = false;
        }

        private void SetStatusMessage(string text, Color color, bool busy)
        {
            lbStatusMessage.Text = text;
            lbStatusMessage.ForeColor = color;
            spinner.Active = busy;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using var dlg = new Form
            {
                Text = "Cài đặt cấu hình",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                BackColor = Color.White,
                Font = this.Font,
                ClientSize = new Size(560, 660)
            };

            var btnCloseDlg = new Button
            {
                Text = "Lưu",
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(21, 101, 192),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(100, 34),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCloseDlg.FlatAppearance.BorderSize = 0;
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 54, BackColor = Color.FromArgb(243, 246, 250) };
            btnCloseDlg.Location = new Point(pnlFooter.Width - btnCloseDlg.Width - 16, 10);
            pnlFooter.Controls.Add(btnCloseDlg);
            dlg.AcceptButton = btnCloseDlg;
            dlg.CancelButton = btnCloseDlg;

            // Mượn panel cài đặt sẵn có để giữ nguyên control, giá trị và sự kiện
            Controls.Remove(pnlSettings);
            pnlSettingsLine.Visible = false;
            pnlSettings.Dock = DockStyle.Fill;
            pnlSettings.Visible = true;
            dlg.Controls.Add(pnlSettings);
            dlg.Controls.Add(pnlFooter);
            try
            {
                dlg.FormClosing += (s, ev) =>
                {
                    string url = txtWebhookUrl.Text.Trim();
                    if (url.Length > 0 && !(Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)))
                    {
                        MessageBox.Show(dlg, "Webhook URL không hợp lệ (phải bắt đầu bằng http:// hoặc https://)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtWebhookUrl.Focus();
                        ev.Cancel = true;
                    }
                };
                dlg.ShowDialog(this);
                SaveWebhookSettings();
            }
            finally
            {
                dlg.Controls.Remove(pnlSettings);
                pnlSettings.Visible = false;
                Controls.Add(pnlSettings);
            }
        }

        private void SaveWebhookSettings()
        {
            _settings.WebhookUrl = txtWebhookUrl.Text.Trim();
            _settings.WebhookUser = txtWebhookUser.Text.Trim();
            _settings.AutoCompareFace = chkAutoCompare.Checked;
            _settings.FaceMatchThreshold = (int)numMatchThreshold.Value;
            _settings.AutoCaptureFace = chkAutocapture.Checked;
            _settings.AutoCaptureFaceMinimumMs = (int)numFaceDurationTime.Value;
            _settings.CaptureFaceDetectFace = chkDetectFace.Checked;
            _settings.CaptureFaceShowTimeout = chkShowTimeout.Checked;
            _settings.CaptureFaceTimeoutMs = (int)numFaceTimeout.Value;
            // Ô đang hiện mặt nạ "********" nghĩa là người dùng không đổi mật khẩu
            if (!_passMasked)
                _settings.SetWebhookPassword(txtWebhookPass.Text);
            try
            {
                _settings.Save();
                ShowPasswordMask();
            }
            catch (Exception ex)
            {
                WriteLog($"SaveSettings: {ex}");
                MessageBox.Show("Không lưu được cấu hình: " + ErrorText.Translate(ex.Message), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private const string PasswordMask = "********";
        private bool _passMasked;

        /// <summary>Mật khẩu đã lưu chỉ hiển thị dạng 8 dấu *, không lộ nội dung và độ dài.</summary>
        private void ShowPasswordMask()
        {
            _passMasked = !string.IsNullOrEmpty(_settings.GetWebhookPassword());
            txtWebhookPass.Text = _passMasked ? PasswordMask : "";
        }

        private void txtWebhookPass_Enter(object? sender, EventArgs e)
        {
            if (_passMasked)
                BeginInvoke(new Action(txtWebhookPass.SelectAll));
        }

        private void txtWebhookPass_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!_passMasked)
                return;
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                // Xóa mật khẩu đã lưu
                ClearPasswordMask();
                e.SuppressKeyPress = true;
            }
            else if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                ClearPasswordMask();
            }
        }

        private void txtWebhookPass_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Gõ ký tự mới thì thay toàn bộ mặt nạ bằng mật khẩu mới
            if (_passMasked && !char.IsControl(e.KeyChar))
                ClearPasswordMask();
        }

        private void ClearPasswordMask()
        {
            _passMasked = false;
            txtWebhookPass.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void OnEvent(object sender, StatusEventArgs e)
        {
            switch (e.EventName)
            {
                case EVENT_NAMES.READER:
                    ProcessReaderEvent(e as StatusEventReaderArgs);
                    break;
                case EVENT_NAMES.CARD:
                    ProcessCardEvent(e as StatusEventCardArgs);
                    break;
                case EVENT_NAMES.READ:
                    ProcessReadEvent(e as StatusEventReadArgs);
                    break;
                case EVENT_NAMES.SCAN:
                    ProcessScanEvent(e as ScanEventArgs);
                    break;
                case EVENT_NAMES.READER_CAMERA:
                    ProcessCameraEvent(e as StatusEventCameraArgs);
                    break;
                case EVENT_NAMES.FINGER_PRINT:
                    break;
            }
        }
        private void InitReader()
        {
            /*Register all reader status event*/
            _reader.OnStatusChanged += Reader_OnStatusChanged;

            // var config = new VnPcscsConfig();
            var config = new VnHn212Config();

            /* set this to false: input access code or scan & read as required
             * true: Auto scan card & read data*/
            // config.AutoReadWhenPresent = false;

            /*Set this flag to process VerifySOD*/
            config.DoVerifySOD = !txtFastRead.Checked;

            /*Set this flag to process AA/CA Authen*/
            config.CheckAACAAuthen = !txtFastRead.Checked;

            /* Capture face, Minimum time have real face*/
            /* Lấy từ cấu hình (.env / settings.json), mặc định 200ms */
            config.AutoCaptureFaceMinimumMs = (int)numFaceDurationTime.Value;
            /* Spoofing mode */
            config.SpoofingMode = (HN212SpoofMode)(cbAntiSpoofing.SelectedIndex);

            /* Retry count */
            config.ReadCardRetryCount = 2;

            /* Seq */
            if (cbProcessStep.SelectedIndex == 1)
                config.ProcessSequence = HN212ProSeq.CardThenFace;
            if (cbProcessStep.SelectedIndex == 2)
                config.ProcessSequence = HN212ProSeq.CardAndCapture2Side;
            else
                config.ProcessSequence = HN212ProSeq.CardOnly;

            config.OcrMode = (HN212OcrMode)(cbOcrMode.SelectedIndex + 1);

            config.HidLibUsed = HN212HidLibrary.HidApi; //ThanhNV test

            config.CaptureFaceUseFirstCamAvaiable = true;

            config.internalDecodeQrcodeType = InternalDecodeQrcodeType.DECODE_ONCE;

            // Tạm tắt vân tay để tránh lỗi thư viện vân tay khi khởi động reader
            // config.EnableFingerPrint = true;

            /*Start reader monitoring ...*/
            _reader.StartMonitor(config);
        }
        // Logger "App" khai báo trong reader.log4net.config, ghi ra Logs\App.log cạnh file exe
        static readonly ILog _log = LogManager.GetLogger("App");

        private static void WriteLog(string log)
        {
            System.Diagnostics.Debug.WriteLine(log);
            _log.Info(log);
        }
        private void ProcessReaderEvent(StatusEventReaderArgs? ev)
        {
            if (ev == null)
                return;
            WriteLog($"----> [{DateTime.Now.ToString("HH:mm:ss")}] Device {ev.ReaderSerialNumber} change stated from: {ev.LastState} to: {ev.NewState}.");
            bool connected = ev.NewState == READER_STATUS.ADDED;
            _readerSerial = connected ? ev.ReaderSerialNumber ?? "" : "";
            lbDeviceState.Text = connected ? "ĐÃ KẾT NỐI" : "CHƯA KẾT NỐI";
            lbDeviceState.ForeColor = connected ? Theme.Success : Theme.Danger;
            lbReaderStatus.Text = connected ? $"{ev.ReaderSerialNumber}" : "KHÔNG CÓ";
            lbReaderStatus.ForeColor = connected ? Theme.Info : Theme.Danger;
            if (connected)
                SetStatusMessage("CHỜ CẮM THẺ...", Theme.Navy, true);
            else
            {
                _cardPresent = false;
                cbFaceCameras.DataSource = null;
                UpdateCameraDisplay();
                SetStatusMessage("CHƯA KẾT NỐI THIẾT BỊ", Theme.Danger, false);
            }
        }

        private void ProcessCameraEvent(StatusEventCameraArgs? ev)
        {
            if (ev == null)
                return;
            WriteLog($"----> [{DateTime.Now.ToString("HH:mm:ss")}] {ev.Message}.");
            btnRefreshCamera_Click(new object(), new EventArgs());
        }

        private void ProcessCardEvent(StatusEventCardArgs? ev)
        {
            if (ev == null)
                return;
            WriteLog($"----> [{DateTime.Now.ToString("HH:mm:ss")}] Card in Device {ev.ReaderSerialNumber} change stated from: {ev.LastState} to: {ev.NewState}.");
            _cardPresent = ev.NewState != CARD_STATUS.EMPTY;

            //Have new card, start timer to show time processing
            if (ev.NewState == CARD_STATUS.PRESENT)
            {
                //Clear form
                ResetFormInfo();
                SetStatusMessage("ĐANG ĐỌC THẺ...", Theme.Info, true);
                //Start counter if auto read
                if (_reader.ReaderConfig.AutoReadWhenPresent)
                {
                    _stw.Restart();
                    _timer.Start();
                }
                else //Show form input Access code
                {
                    //Test for ePassport
                    if (cbReaderTypes.SelectedIndex == 1)
                    {
                        _reader.Hn212Device.PowerOnOffLight(true, true, 200);
                        _reader.StartReadCard("150559");

                        _stw.Restart();
                        _timer.Start();
                    }
                    /*
                    InputAccessCode frm = new InputAccessCode();
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;
                    _stw.Restart();
                    _timer.Start();
                    _reader.StartReadCard(frm.AccessCode);
                    */
                }
            }
            if (ev.NewState == CARD_STATUS.EMPTY)
            {
                if (chkDelteForm.Checked)
                {
                    ResetFormInfo();
                    ResetPortrait();
                }
                SetStatusMessage("CHỜ CẮM THẺ...", Theme.Navy, true);
            }
        }
        private void OnScancardEvent(StatusEventReadArgs ev)
        {
        }
        private void OnReadCardFinish(StatusEventReadArgs ev)
        {
            _stw.Stop();
            _timer.Stop();
            //Check error
            if (ev.Status != READ_CARD_STATUS.SUCCESS)
            {
                SetStatusMessage("ĐỌC THẺ THẤT BẠI", Theme.Danger, false);
                MessageBox.Show("Đọc thẻ thất bại: " + ErrorText.Translate(ev.Message), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _cardData = _reader.CardData;
            SetStatusMessage("ĐỌC THẺ THÀNH CÔNG", Theme.Success, false);

            //Show data
            if (_reader.CardData.Dg2File != null && !string.IsNullOrEmpty(_reader.CardData.Dg2File.FaceImage))
            {
                //Chip image
                var faceStr = _reader.CardData.Dg2File.FaceImage;
                var chipFaceBase64 = faceStr[(faceStr.IndexOf(',') + 1)..];
                _ChipFaceBts = Convert.FromBase64String(chipFaceBase64);
                using (var ms = new MemoryStream(_ChipFaceBts))
                {
                    picChipImage.Image = Image.FromStream(ms);
                }
                // Đã chụp khuôn mặt trước khi đọc thẻ thì so khớp luôn
                if (chkAutoCompare.Checked && _CamFaceBts != null)
                    CompareFaces();
            }
            if (cbReaderTypes.SelectedIndex == 1) //Test display info
            {
                txtDocumentNumber.Text = "Số HC: " + "E01150559";
                txtDateOfBirth.Text = "Ngày sinh: " + "31/10/1978";
                txtIssueDate.Text = "Ngày cấp: " + "29/11/2023";
                txtName.Text = "Họ và tên: " + "NGUYEN VAN THANH";
            }

            if (_reader.CardData.Dg13File != null)
            {
                //Personal info
                txtDocumentNumber.Text = "Số CCCD: " + _reader.CardData.Dg13File.DocumentNumber;
                txtDateOfBirth.Text = "Ngày sinh: " + _reader.CardData.Dg13File.DateOfBirth;
                txtIssueDate.Text = "Ngày cấp: " + _reader.CardData.Dg13File.IssueDate;
                txtPreviousNumber.Text = "Số CMND: " + _reader.CardData.Dg13File.PreviousNumber;
                txtName.Text = "Họ và tên: " + _reader.CardData.Dg13File.Name;
                txtSex.Text = "Giới tính: " + _reader.CardData.Dg13File.Sex;
                txtNationality.Text = "Quốc tịch: " + _reader.CardData.Dg13File.Nationality;
                txtNation.Text = "Dân tộc: " + _reader.CardData.Dg13File.Nation;
                txtReligion.Text = "Tôn giáo: " + _reader.CardData.Dg13File.Religion;
                txtHometown.Text = "Quê quán: " + _reader.CardData.Dg13File.Hometown;
                txtAddress.Text = "Địa chỉ: " + _reader.CardData.Dg13File.Address;
                txtCharacter.Text = "Nhận dạng: " + _reader.CardData.Dg13File.Character;
                txtExpiredDate.Text = "Ngày hết hạn: " + _reader.CardData.Dg13File.ExpiredDate;
                txtFatherName.Text = "Họ tên bố: " + _reader.CardData.Dg13File.FatherName;
                txtMotherName.Text = "Họ tên mẹ: " + _reader.CardData.Dg13File.MotherName;
                txtPartnerName.Text = "Vợ/Chồng: " + _reader.CardData.Dg13File.PartnerName;
                //MRZ
                if (_reader.CardData.Dg1File != null)
                {
                    txtMRZ.Text = "MRZ: " + _reader.CardData.Dg1File.Mrz;
                }
                txtDSCert.Text = "DSCert: " + _reader.CardData.DSCert;
                //VerifySOD
                WriteLog($"Verify SOD: {_reader.CardData.VerifySOD}");

                //Active/Chip Authentication
                WriteLog($"Chip Authen: {_reader.CardData.AaCaAuthen}");

                //Raw DATA
                /*
                _reader.CardData.EfCom
                _reader.CardData.Dg1 ...
                _reader.CardData.Dg15
                _reader.CardData.Sod
                */
            }
            //Thẻ đảng
            if (_reader.CardData.Dg13PartyMemFile != null)
            {
                //Personal info
                txtName.Text = "Họ và tên: " + _reader.CardData.Dg13PartyMemFile.Name;
                txtPartnerName.Text = ""; // "Ngày cấp: " + _reader.CardData.Dg13PartyMemFile.IssueDate;

                txtDocumentNumber.Text = "Số thẻ: " + _reader.CardData.Dg13PartyMemFile.DocumentNumber;
                txtPreviousNumber.Text = ""; // "Dân tộc: " + _reader.CardData.Dg13PartyMemFile.Nation;

                txtSex.Text = "Giới tính: " + _reader.CardData.Dg13PartyMemFile.Sex;
                txtNationality.Text = ""; // "Tôn giáo: " + _reader.CardData.Dg13PartyMemFile.Religion;

                txtExpiredDate.Text = "Ngày vào Đảng: " + _reader.CardData.Dg13PartyMemFile.PartyJoinDate;
                txtReligion.Text = "Ngày chính thức: " + _reader.CardData.Dg13PartyMemFile.PartyJoinDate2;

                txtFatherName.Text = "Ngày sinh: " + _reader.CardData.Dg13PartyMemFile.DateOfBirth;
                txtMotherName.Text = ""; // "Ngày cấp: " + _reader.CardData.Dg13PartyMemFile.IssueDate;

                txtDateOfBirth.Text = "Ngày cấp: " + _reader.CardData.Dg13PartyMemFile.IssueDate;
                txtNation.Text = "Đơn vị: " + _reader.CardData.Dg13PartyMemFile.PartyCommittee;

                txtCharacter.Text = "Dân tộc: " + _reader.CardData.Dg13PartyMemFile.Nation;
                txtHometown.Text = "Tôn giáo: " + _reader.CardData.Dg13PartyMemFile.Religion;
                txtAddress.Text = "Quê quán: " + _reader.CardData.Dg13PartyMemFile.Hometown;

                txtIssueDate.Text = "";
                
                //MRZ
                if (_reader.CardData.Dg1File != null)
                {
                    txtMRZ.Text = "MRZ: " + _reader.CardData.Dg1File.Mrz;
                }
                txtDSCert.Text = "DSCert: " + _reader.CardData.DSCert;
                //VerifySOD
                WriteLog($"Verify SOD: {_reader.CardData.VerifySOD}");

                //Active/Chip Authentication
                WriteLog($"Chip Authen: {_reader.CardData.AaCaAuthen}");
            }
        }
        private void ProcessReadEvent(StatusEventReadArgs? ev)
        {
            try
            {
                if (ev == null)
                    return;
                WriteLog($"----> [{DateTime.Now.ToString("HH:mm:ss")}] Device {ev.ReaderSerialNumber} do step: {ev.Step}; status: {ev.Status}; message={ev.Message}.");
                switch (ev.Step)
                {
                    case READ_CARD_STEPS.SCANCARD:
                        OnScancardEvent(ev);
                        break;
                    case READ_CARD_STEPS.START:
                    case READ_CARD_STEPS.CONNECT_CARD:
                    case READ_CARD_STEPS.PACE:
                    case READ_CARD_STEPS.READ_DGS:
                    case READ_CARD_STEPS.VERIFY_SOD:
                    case READ_CARD_STEPS.AACA_AUTHEN:
                        break;
                    case READ_CARD_STEPS.FINISH:
                        OnReadCardFinish(ev);
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        private void ProcessScanEvent(ScanEventArgs? args)
        {
            //Display Scan image
            try
            {
                if (args == null)
                    return;
                if (args.FrontData != null)
                {
                    _ocrFrontBts = args.FrontData;
                    using (var ms = new MemoryStream(args.FrontData))
                    {
                        picIdcardFront.Image = Image.FromStream(ms);
                        picIdcardFront.Visible = true;
                    }
                }
                if (args.BackData != null)
                {
                    _ocrBackBts = args.BackData;
                    using (var ms = new MemoryStream(args.BackData))
                    {
                        pictureIdcardBack.Image = Image.FromStream(ms);
                        pictureIdcardBack.Visible = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void Reader_OnStatusChanged(object sender, StatusEventArgs e)
        {
            //Call from other thread
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    OnEvent(sender, e);
                }));
            }
            else
            {
                OnEvent(sender, e);
            }
        }

        private void picCamera_Resize(object? sender, EventArgs e)
        {
            btnCaptureFace.Location = new Point((picCamera.ClientSize.Width - btnCaptureFace.Width) / 2,
                (picCamera.ClientSize.Height - btnCaptureFace.Height) / 2);
        }

        private void btnFaceMatching_Click(object? sender, EventArgs e)
        {
            picCamera.Image = null;
            lblMatchResult.Visible = false;
            btnCaptureFace.Visible = true;
            _CamFaceBts = null;
            if (cbFaceCameras.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Camera chụp ảnh khuôn mặt!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Change config
            _reader.ReaderConfig.CaptureFaceDetectFace = chkDetectFace.Checked;
            _reader.ReaderConfig.CaptureFaceDisplayTimeout = chkShowTimeout.Checked;
            _reader.ReaderConfig.AutoCaptureFaceEnabled = chkAutocapture.Checked;
            _reader.ReaderConfig.AutoCaptureFaceMinimumMs = (int)numFaceDurationTime.Value;
            _reader.ReaderConfig.CaptureFaceCameraPath = ((CameraDevice)cbFaceCameras.SelectedItem).Id;
            _reader.ReaderConfig.CaptureFaceTimeout = (int)numFaceTimeout.Value;

            CaptureFace frm = new CaptureFace(_reader);
            frm.ShowDialog();
            //Show Face
            try
            {
                if (frm.FaceImage != null)
                {
                    _CamFaceBts = frm.FaceImage.ToArray();
                    using (var ms = new MemoryStream(frm.FaceImage))
                    {
                        picCamera.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception) { }
            // Có ảnh thì ẩn icon, bấm vào ảnh để chụp lại
            btnCaptureFace.Visible = picCamera.Image == null;

            if (frm.Status != FACE_CAPTURE_STATAUS.SUCCESS)
            {
                string reason = "không xác định";
                switch (frm.FaceType)
                {
                    case FACE_DETECT_TYPES.FAIL_SPOOR_FACE:
                        reason = "không phải người thật";
                        break;
                    case FACE_DETECT_TYPES.FAIL_NO_FACE:
                        reason = "không có khuôn mặt";
                        break;
                    case FACE_DETECT_TYPES.FAIL_MULTI_FACE:
                        reason = "nhiều khuôn mặt";
                        break;
                    default:
                        break;
                }

                WriteLog($"FaceCapture: {frm.Message} ({reason})");
                MessageBox.Show(ErrorText.Translate(frm.Message), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Chưa đọc thẻ thì chờ, sẽ so khớp khi đọc thẻ xong
            if (chkAutoCompare.Checked && _ChipFaceBts != null)
                CompareFaces();
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            ResetFormInfo();
        }

        private void btnReadQR_Click(object sender, EventArgs e)
        {
            if (_reader == null || !_reader.IsDeviceConnected())
                return;
           var rest = _reader.StartAA(Encoding.UTF8.GetBytes("12345678"));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeepLink.Received -= OnDeepLink;
            _reader.StopMonitor();
        }

        private void txtFastRead_CheckedChanged(object sender, EventArgs e)
        {
            _reader.ReaderConfig.DoVerifySOD = !txtFastRead.Checked;
            _reader.ReaderConfig.CheckAACAAuthen = !txtFastRead.Checked;
        }

        private void chkAutocapture_CheckedChanged(object sender, EventArgs e)
        {
            numFaceDurationTime.Enabled = chkAutocapture.Checked;
        }
        private void LoadExternalCameras()
        {
            if (_reader.Hn212Device == null || _reader.Hn212Device.OcrCam.IsEmpty())
                return;
            if (cbFaceCameras.Items.Count > 0)
                return;
            btnRefreshCamera_Click(new object(), new EventArgs());
        }
        private void btnRefreshCamera_Click(object sender, EventArgs e)
        {
            if (_reader.Hn212Device == null)
                return;
            cbFaceCameras.DataSource = null;
            var cams = _reader.GetCameraList(false);
            cbFaceCameras.DataSource = cams;
            cbFaceCameras.DisplayMember = "Name";
            cbFaceCameras.ValueMember = "Id";
            if (cbFaceCameras.Items.Count > 0)
            {
                cbFaceCameras.SelectedIndex = 0;
                //set facecam id
                _reader.FaceCaptureCamId = (cams[0].Id);
            }
            UpdateCameraDisplay();
        }

        /// <summary>1 camera thì hiện tên, nhiều camera thì hiện danh sách để chọn.</summary>
        private void UpdateCameraDisplay()
        {
            int count = cbFaceCameras.Items.Count;
            bool multi = count > 1;
            cbFaceCameras.Visible = multi;
            lbCameraName.Visible = !multi;
            if (count == 1)
            {
                lbCameraName.Text = cbFaceCameras.GetItemText(cbFaceCameras.Items[0]);
                lbCameraName.ForeColor = Theme.Info;
            }
            else
            {
                lbCameraName.Text = "KHÔNG CÓ";
                lbCameraName.ForeColor = Theme.Danger;
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (_ChipFaceBts == null || _CamFaceBts == null)
            {
                MessageBox.Show("Vui lòng chụp ảnh và đọc thẻ chip trước!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CompareFaces();
        }

        /// <summary>So khớp ảnh chụp với ảnh chip; dưới ngưỡng thì báo chụp lại. Trả về true nếu khớp.</summary>
        private bool CompareFaces()
        {
            if (_ChipFaceBts == null || _CamFaceBts == null)
                return false;
            try
            {
                int score = _reader.CompareFace(_ChipFaceBts, _CamFaceBts);
                _lastMatchScore = score;
                int threshold = (int)numMatchThreshold.Value;
                bool matched = score >= threshold;
                if (!matched)
                {
                    SetStatusMessage("ẢNH CHÂN DUNG KHÔNG KHỚP", Theme.Danger, false);
                    var retake = MessageBox.Show($"Ảnh chân dung không khớp với ảnh trong CCCD (độ khớp {score}%).\n" +
                        "Bấm OK để mở camera chụp lại.", "Chụp lại", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    // Mở camera sau khi luồng hiện tại kết thúc, tránh gọi lồng trong btnFaceMatching_Click
                    if (retake == DialogResult.OK)
                        BeginInvoke(() => btnFaceMatching_Click(this, EventArgs.Empty));
                }
                return matched;
            }
            catch (Exception ex)
            {
                WriteLog($"CompareFace: {ex}");
                MessageBox.Show(ErrorText.Translate(ex.Message), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await SendCardInfoAsync();
        }

        bool _sending = false;

        /// <summary>Gửi dữ liệu thẻ qua webhook khi bấm nút Gửi, không bắt buộc chụp ảnh, không hỏi xác nhận.</summary>
        private async Task SendCardInfoAsync()
        {
            if (_sending)
                return;
            var dg13 = _cardData?.Dg13File;
            if (dg13 == null || _ChipFaceBts == null)
            {
                MessageBox.Show("Vui lòng đọc thẻ CCCD trước khi gửi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool hasV1 = !string.IsNullOrWhiteSpace(_settings.WebhookUrl) && !string.IsNullOrEmpty(_settings.WebhookUser);
            bool hasV2 = !string.IsNullOrWhiteSpace(_settings.WebhookUrlV2);
            if (!hasV1 && !hasV2)
            {
                MessageBox.Show("Chưa cấu hình Webhook URL hoặc tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // SĐT và đồng ý chính sách bắt buộc, email có nhập thì phải đúng định dạng: lỗi thì dừng, sửa xong bấm Gửi để gửi lại
            string? phoneErr = ValidatePhone();
            string? emailErr = ValidateEmail();
            string? consentErr = chkConsent.Checked ? null : ValidationConstants.ConsentRequired;
            var inputErrors = new[] { phoneErr, emailErr, consentErr }.Where(x => x != null).ToList();
            if (inputErrors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", inputErrors), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Control first = phoneErr != null ? inpPhone : emailErr != null ? inpEmail : chkConsent;
                first.Focus();
                return;
            }

            // Chụp ảnh không bắt buộc; có ảnh thì so khớp lại, không đạt ngưỡng thì dừng để chụp lại
            if (_CamFaceBts != null)
            {
                if (!CompareFaces())
                    return;
            }
            else
                _lastMatchScore = null;

            _sending = true;
            btnSend.Enabled = false;
            btnSend.Text = "ĐANG GỬI...";
            SetStatusMessage("ĐANG GỬI THÔNG TIN...", Theme.Info, true);
            var errors = new List<string>();
            var cardData = new WebhookCardData(_cardData!, _ChipFaceBts!,_CamFaceBts, _ocrFrontBts, _ocrBackBts, _readerSerial, Phone, Email);
            string? sessionId = null;
            string? v1Token = null;
            try
            {
                if (hasV1)
                {
                    try
                    {
                        var (requestId, sid, token) = await WebhookClient.SendNfcInfoAsync(
                            _settings.WebhookUrl, _settings.WebhookUser, _settings.GetWebhookPassword(), _settings.Username, cardData);
                        sessionId = sid;
                        v1Token = token;
                        WriteLog($"SendNfcInfo: requestId={requestId}, sessionId={sessionId}");
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"SendNfcInfo: {ex}");
                        errors.Add((hasV2 ? "Webhook: " : "") + WebhookClient.DescribeSendError(ex));
                    }
                }
                if (hasV2)
                {
                    try
                    {
                        await WebhookClient.SendIdentificationV2Async(_settings.WebhookUrlV2, _transactionId, cardData);
                        WriteLog($"SendIdentificationV2: transactionId={_transactionId ?? "(không có)"}");
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"SendIdentificationV2: {ex}");
                        errors.Add((hasV1 ? "Webhook v2: " : "") + WebhookClient.DescribeSendError(ex));
                    }
                }
            }
            finally
            {
                _sending = false;
                btnSend.Enabled = true;
                btnSend.Text = "ĐĂNG KÝ CHỨNG THƯ SỐ";
            }

            if (errors.Count > 0)
            {
                SetStatusMessage("GỬI THẤT BẠI", Theme.Danger, false);
                MessageBox.Show("Gửi thông tin thất bại:\n" + string.Join("\n", errors), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetStatusMessage("GỬI THÀNH CÔNG", Theme.Success, false);
            if (sessionId != null && v1Token != null)
            {
                // Webhook v1 thành công: hiển thị thông tin đăng ký, xem phiếu đăng ký và ký tay
                var dg = _cardData!.Dg13File!;
                var customer = new RegisterCustomerInfo(dg.Name, dg.DocumentNumber, dg.DateOfBirth, dg.Sex, dg.IssueDate, dg.Hometown, dg.Address);
                using var registerForm = new RegisterInfoForm(customer, _settings.WebhookUrl, v1Token, sessionId);
                if (registerForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Đăng ký chứng thư số thành công: báo người dùng, bấm OK hoặc sau 2 giây thì tự đóng tool
                    WriteLog("Đăng ký chứng thư số thành công, tự động đóng ứng dụng.");
                    AutoCloseMessageBox.Show(this, "Đăng ký chứng thư số thành công.", "Thông báo", TimeSpan.FromSeconds(2));
                    Close();
                    return;
                }
            }
            if (hasV1 && hasV2)
            {
                // Gửi thành công cả 2 luồng (webhook + webhook v2) thì tự động đóng tool
                WriteLog("Gửi thành công cả 2 luồng, tự động đóng ứng dụng.");
                await Task.Delay(1500);
                Close();
            }
            else if (sessionId == null || v1Token == null)
            {
                MessageBox.Show("Gửi thành công.", "Kết quả gửi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chkAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            _reader.ReaderConfig.AutoReadWhenPresent = chkAutoRead.Checked;
        }

        //Change reader type
        private void cbReaderTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            picIdcardFront.Visible = cbReaderTypes.SelectedIndex > 0;
            pictureIdcardBack.Visible = cbReaderTypes.SelectedIndex > 0;
            btnRejectCard.Visible = cbReaderTypes.SelectedIndex > 0;
            _reader.Hn212Device.SetSupportDeviceType(cbReaderTypes.SelectedIndex == 0 ? DeviceTypes.DeviceTypesHN212 : DeviceTypes.DeviceTypesScan);
            */
            if (cbReaderTypes.SelectedIndex == 0)
            {
                _reader.ReaderConfig.AutoReadWhenPresent = true;
                _reader.ReaderConfig.AllowCardType = SupportCardType.SP_TYPE_ALL;
            }
            if (cbReaderTypes.SelectedIndex == 1)
            {
                _reader.ReaderConfig.AutoReadWhenPresent = false;
                _reader.ReaderConfig.AllowCardType = SupportCardType.SP_TYPE_PASSPORT;
            }
            if (cbReaderTypes.SelectedIndex == 2)
            {
                _reader.ReaderConfig.AutoReadWhenPresent = true;
                _reader.ReaderConfig.AllowCardType = SupportCardType.SP_TYPE_PARTY_MEMBER;
            }
        }

        private void btnRejectCard_Click(object sender, EventArgs e)
        {
            // _reader.RejectCard(true, true);
        }

        private void cbAntiSpoofing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAntiSpoofing.SelectedIndex >= 0 && cbAntiSpoofing.SelectedIndex <= (int)HN212SpoofMode.HIGH)
                _reader.FaceAntiSpoofMode = ((HN212SpoofMode)cbAntiSpoofing.SelectedIndex);
        }

        private void cbFaceCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFaceCameras.SelectedValue is string)
                _reader.FaceCaptureCamId = cbFaceCameras.SelectedValue.ToString();
        }

        private void cbProcessStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbProcessStep.SelectedIndex)
            {
                case 0:
                    _reader.ReaderConfig.ProcessSequence = HN212ProSeq.CardOnly;
                    break;
                case 1:
                    _reader.ReaderConfig.ProcessSequence = HN212ProSeq.CardThenFace;
                    break;
                case 2:
                    _reader.ReaderConfig.ProcessSequence = HN212ProSeq.CardAndCapture2Side;
                    break;
                default:
                    _reader.ReaderConfig.ProcessSequence = HN212ProSeq.CardOnly;
                    break;
            }
        }

        private void chkPaused_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaused.Checked)
                _reader.Pause();
            else
                _reader.Resume();
        }

        private void numFaceDurationTime_ValueChanged(object sender, EventArgs e)
        {
        }

        private void cbOcrMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _reader.OcrMode = (HN212OcrMode)(cbOcrMode.SelectedIndex + 1);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            _reader.Transaction(true);
        }

        private void btnCompareTest_Click(object sender, EventArgs e)
        {
            EKycForm frm = new EKycForm();
            frm.ShowDialog();
        }

        private void chkPauseIntCam_CheckedChanged(object sender, EventArgs e)
        {
            _reader.ReqInternalCamToPause(chkPauseIntCam.Checked, chkPauseIntCam.Checked ? 5000 : 0);
        }
        private void btnFinger_Click(object sender, EventArgs e)
        {
            // Tạm tắt chức năng vân tay (xem config.EnableFingerPrint trong StartMonitor)
            //if (!_reader.IsDeviceConnected() || !_reader.Hn212Device.IsReady())
            //{
            //    MessageBox.Show("Chưa kết nối thiết bị đọc thẻ hoặc thiết bị chưa sẵn sàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //using FingerForm dlg = new FingerForm(_reader, _fingers);
            //dlg.ShowDialog(this);
            //// Ngón thu thành công được lưu ngay, nên giữ lại kể cả khi đóng form bằng Hủy
            //_fingers = dlg.Fingers;
        }

        private static readonly Regex PhoneRegex = new(ValidationConstants.PhonePattern);
        private static readonly Regex EmailRegex = new(ValidationConstants.EmailPattern);

        /// <summary>Số điện thoại đã nhập (đã bỏ khoảng trắng), rỗng nếu chưa nhập.</summary>
        private string Phone => inpPhone.Text.Trim();
        /// <summary>Email đã nhập (đã bỏ khoảng trắng), rỗng nếu chưa nhập.</summary>
        private string Email => inpEmail.Text.Trim();

        private void inpPhone_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Chỉ cho nhập số, dấu + chỉ ở đầu
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;
            if (e.KeyChar == '+' && inpPhone.SelectionStart == 0 && !inpPhone.Text.Contains('+'))
                return;
            e.Handled = true;
        }

        /// <summary>Kiểm tra khi bấm Gửi, lỗi báo bằng hộp thoại. Trả về thông báo lỗi, null nếu hợp lệ.</summary>
        private string? ValidatePhone() =>
            Phone.Length == 0 ? ValidationConstants.PhoneRequired
            : PhoneRegex.IsMatch(Phone) ? null : ValidationConstants.PhoneInvalid;

        /// <summary>Email không bắt buộc: để trống thì hợp lệ (gửi email mặc định), có nhập thì phải đúng định dạng.</summary>
        private string? ValidateEmail() =>
            Email.Length == 0 || EmailRegex.IsMatch(Email) ? null : ValidationConstants.EmailInvalid;

        private void ResetPortrait()
        {
            picCamera.Image = null;
            btnCaptureFace.Visible = true;
            _CamFaceBts = null;
            lblMatchResult.Visible = false;
        }

        private void btnRereadcard1_Click(object sender, EventArgs e)
        {
            ResetFormInfo();
            _reader.StartReadCard("", true);
            _stw.Restart();
            _timer.Start();
        }

        private void btnRereadcard2_Click(object sender, EventArgs e)
        {
            ResetFormInfo();
            _reader.StartReadCard("", false);
            _stw.Restart();
            _timer.Start();
        }
    }
}