using log4net;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace IdCard.Hanel
{
    /// <summary>Thông tin khách hàng hiển thị trên màn đăng ký.</summary>
    internal sealed record RegisterCustomerInfo(
        string? Name,
        string? Number,
        string? DateOfBirth,
        string? Sex,
        string? RegisterDate,
        string? Hometown,
        string? Address);

    /// <summary>
    /// Màn hiển thị sau khi gửi webhook v1 thành công: thông tin khách hàng đăng ký,
    /// xem phiếu đăng ký (GET get-certificate-register theo sessionId) và ký tay.
    /// </summary>
    internal sealed class RegisterInfoForm : Form
    {
        private static readonly Color Accent = Color.FromArgb(255, 107, 0);
        private static readonly ILog _log = LogManager.GetLogger("App");

        private readonly string _webhookUrl;
        private readonly string _token;
        private readonly string _sessionId;
        private readonly SignaturePad _signature = new();
        private readonly LinkLabel _lnkView = new();
        private readonly Button _btnStart = new();

        /// <summary>Chữ ký tay dạng PNG, có giá trị khi người dùng bấm Bắt đầu.</summary>
        public byte[]? SignaturePng { get; private set; }

        public RegisterInfoForm(RegisterCustomerInfo info, string webhookUrl, string token, string sessionId)
        {
            _webhookUrl = webhookUrl;
            _token = token;
            _sessionId = sessionId;

            Text = "Thông tin đăng ký";
            Font = new Font("Segoe UI", 12F);
            BackColor = Theme.Background;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            // Màn lớn để dễ đọc thông tin và có nhiều chỗ ký tay; không vượt quá vùng làm việc của màn hình
            var work = Screen.FromPoint(Cursor.Position).WorkingArea;
            ClientSize = new Size(Math.Min(640, work.Width - 40), Math.Min(900, work.Height - 60));

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 1,
                RowCount = 4
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(layout);

            layout.Controls.Add(BuildInfoCard(info), 0, 0);
            layout.Controls.Add(BuildRegisterCard(), 0, 1);
            layout.Controls.Add(BuildSignatureCard(), 0, 2);

            _btnStart.Text = "Đăng ký";
            _btnStart.Dock = DockStyle.Fill;
            _btnStart.Height = 56;
            _btnStart.Margin = new Padding(0, 14, 0, 0);
            _btnStart.FlatStyle = FlatStyle.Flat;
            _btnStart.FlatAppearance.BorderSize = 0;
            _btnStart.BackColor = Accent;
            _btnStart.ForeColor = Color.White;
            _btnStart.Font = new Font(Font, FontStyle.Bold);
            _btnStart.Cursor = Cursors.Hand;
            _btnStart.Click += BtnStart_Click;
            layout.Controls.Add(_btnStart, 0, 3);
        }

        private Control BuildInfoCard(RegisterCustomerInfo info)
        {
            var card = NewCard();
            var rows = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(16, 14, 16, 14),
                BackColor = Theme.Surface
            };
            rows.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 36));
            rows.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var title = new Label
            {
                Text = "Thông tin khách hàng đăng ký",
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                Margin = new Padding(0, 0, 0, 10)
            };
            rows.Controls.Add(title, 0, 0);
            rows.SetColumnSpan(title, 2);

            // Glyph Segoe MDL2 Assets: người, thẻ, lịch, giới tính, lịch, ghim bản đồ
            AddInfoRow(rows, "", info.Name);
            AddInfoRow(rows, "", info.Number);
            AddInfoRow(rows, "", info.DateOfBirth);
            AddInfoRow(rows, "", info.Sex);
            AddInfoRow(rows, "", info.RegisterDate);
            AddInfoRow(rows, "", info.Hometown);
            AddInfoRow(rows, "", info.Address);

            card.Controls.Add(rows);
            // CardPanel không tự co theo nội dung docked, tính chiều cao theo bảng thông tin
            card.Height = rows.GetPreferredSize(new Size(ClientSize.Width - 32, 0)).Height + 2;
            return card;
        }

        private void AddInfoRow(TableLayoutPanel rows, string glyph, string? value)
        {
            rows.Controls.Add(new Label
            {
                Text = glyph,
                Font = new Font("Segoe MDL2 Assets", 13F),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Margin = new Padding(0, 6, 0, 6)
            });
            rows.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(value) ? "---" : value,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                MaximumSize = new Size(ClientSize.Width - 100, 0),
                Margin = new Padding(0, 5, 0, 5)
            });
        }

        private Control BuildRegisterCard()
        {
            var card = NewCard();
            card.Height = 66;
            card.Margin = new Padding(0, 14, 0, 0);

            var label = new Label
            {
                Text = "Phiếu đăng ký",
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(16, 20)
            };
            _lnkView.Text = "Xem";
            _lnkView.AutoSize = true;
            _lnkView.LinkColor = Accent;
            _lnkView.ActiveLinkColor = Accent;
            _lnkView.LinkBehavior = LinkBehavior.HoverUnderline;
            _lnkView.Font = new Font(Font, FontStyle.Bold);
            _lnkView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lnkView.Location = new Point(card.Width - 66, 20);
            _lnkView.LinkClicked += async (_, _) => await ViewRegisterAsync();

            card.Controls.Add(label);
            card.Controls.Add(_lnkView);
            return card;
        }

        private Control BuildSignatureCard()
        {
            var card = NewCard();
            card.Margin = new Padding(0, 14, 0, 0);

            var header = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Theme.Surface };
            var label = new Label
            {
                Text = "Ký PĐK và Hợp đồng",
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(16, 15)
            };
            var btnUndo = IconButton("", Accent, "Xoá nét vừa ký");
            btnUndo.Click += (_, _) => _signature.Undo();
            var btnClear = IconButton("", Theme.Danger, "Xoá chữ ký");
            btnClear.Click += (_, _) => _signature.Clear();
            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 4, 8, 0)
            };
            buttons.Controls.Add(btnUndo);
            buttons.Controls.Add(btnClear);
            header.Controls.Add(buttons);
            header.Controls.Add(label);

            _signature.Dock = DockStyle.Fill;
            _signature.Margin = new Padding(0);

            card.Controls.Add(_signature);
            card.Controls.Add(header);
            card.Dock = DockStyle.Fill;
            return card;
        }

        private static Button IconButton(string glyph, Color color, string tooltip)
        {
            var btn = new Button
            {
                Text = glyph,
                Font = new Font("Segoe MDL2 Assets", 15F),
                ForeColor = color,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(44, 44),
                Cursor = Cursors.Hand,
                Margin = new Padding(2, 0, 2, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            new ToolTip().SetToolTip(btn, tooltip);
            return btn;
        }

        private static CardPanel NewCard() => new()
        {
            Dock = DockStyle.Top,
            Margin = new Padding(0),
            Padding = new Padding(1)
        };

        /// <summary>Gọi API lấy phiếu đăng ký rồi xem trực tiếp trong ứng dụng, không lưu file xuống máy.</summary>
        private async Task ViewRegisterAsync()
        {
            _lnkView.Enabled = false;
            _lnkView.Text = "Đang tải...";
            UseWaitCursor = true;
            try
            {
                byte[] pdf = await WebhookClient.GetCertificateRegisterAsync(_webhookUrl, _token, _sessionId);
                _log.Info($"ViewRegister: sessionId={_sessionId}, {pdf.Length} bytes");

                using var viewer = new PdfViewerForm("Phiếu đăng ký", pdf);
                viewer.ShowDialog(this);
            }
            catch (Exception ex)
            {
                _log.Info($"ViewRegister: {ex}");
                MessageBox.Show(this, "Không lấy được phiếu đăng ký:\n" + WebhookClient.DescribeSendError(ex),
                    "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                _lnkView.Text = "Xem";
                _lnkView.Enabled = true;
            }
        }

        /// <summary>Lấy ảnh chữ ký tay, gửi lên send-sign-image kèm sessionId; thành công thì đóng màn hình.</summary>
        private async void BtnStart_Click(object? sender, EventArgs e)
        {
            if (_signature.IsEmpty)
            {
                MessageBox.Show(this, "Vui lòng ký vào phiếu đăng ký và hợp đồng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] png = _signature.ToPng();
            string text = _btnStart.Text;
            _btnStart.Enabled = false;
            _btnStart.Text = "Đang gửi...";
            UseWaitCursor = true;
            try
            {
                await WebhookClient.SendSignImageAsync(_webhookUrl, _token, _sessionId, png);
                _log.Info($"SendSignImage: sessionId={_sessionId}, {png.Length} bytes");
                SignaturePng = png;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _log.Info($"SendSignImage: {ex}");
                MessageBox.Show(this, "Gửi chữ ký thất bại:\n" + WebhookClient.DescribeSendError(ex),
                    "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                _btnStart.Text = text;
                _btnStart.Enabled = true;
            }
        }
    }

    /// <summary>Ô ký tay bằng chuột / bút cảm ứng, lưu từng nét để có thể xoá nét vừa ký.</summary>
    internal sealed class SignaturePad : Control
    {
        private readonly List<List<Point>> _strokes = new();
        private List<Point>? _current;

        public bool IsEmpty => _strokes.Count == 0;

        public SignaturePad()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Theme.Surface;
            Cursor = Cursors.Cross;
        }

        public void Undo()
        {
            if (_strokes.Count == 0)
                return;
            _strokes.RemoveAt(_strokes.Count - 1);
            Invalidate();
        }

        public void Clear()
        {
            _strokes.Clear();
            Invalidate();
        }

        /// <summary>Xuất chữ ký nền trong suốt, cùng kích thước ô ký.</summary>
        public byte[] ToPng()
        {
            using var bmp = new Bitmap(Math.Max(1, Width), Math.Max(1, Height), PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
                DrawStrokes(g);
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
                return;
            _current = new List<Point> { e.Location };
            _strokes.Add(_current);
            Capture = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_current == null)
                return;
            _current.Add(e.Location);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _current = null;
            Capture = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawStrokes(e.Graphics);
        }

        private void DrawStrokes(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.Black, 2.5f * DeviceDpi / 96f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
            foreach (var stroke in _strokes)
            {
                if (stroke.Count == 1)
                    g.FillEllipse(Brushes.Black, stroke[0].X - 1.5f, stroke[0].Y - 1.5f, 3, 3);
                else
                    g.DrawLines(pen, stroke.ToArray());
            }
        }
    }
}
