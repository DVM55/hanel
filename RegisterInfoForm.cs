using log4net;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.InteropServices;

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
        private readonly ConsentCheck _chkTemplate = new();
        private readonly FlowLayoutPanel _templates = new();
        private readonly List<SignatureTemplateItem> _templateItems = new();
        private readonly string _customerName;
        private FlowLayoutPanel _drawButtons = null!;
        private readonly LinkLabel _lnkView = new();
        private readonly Button _btnStart = new();

        /// <summary>Chữ ký tay dạng PNG, có giá trị khi người dùng bấm Bắt đầu.</summary>
        public byte[]? SignaturePng { get; private set; }

        public RegisterInfoForm(RegisterCustomerInfo info, string webhookUrl, string token, string sessionId)
        {
            _webhookUrl = webhookUrl;
            _token = token;
            _sessionId = sessionId;
            _customerName = info.Name ?? "";

            Text = "Thông tin đăng ký";
            Font = new Font("Segoe UI", 12F);
            BackColor = Theme.Background;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            // Vị trí và chiều cao đặt lại trong OnLoad (sau khi scale theo DPI) để chiếm full chiều cao màn hình
            StartPosition = FormStartPosition.Manual;
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            // Màn lớn để dễ đọc thông tin và có nhiều chỗ ký tay; không vượt quá vùng làm việc của màn hình
            var work = Screen.FromPoint(Cursor.Position).WorkingArea;
            ClientSize = new Size(Math.Min(640, work.Width - 40), work.Height - 40);

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

        /// <summary>Chiếm full chiều cao vùng làm việc (trừ thanh taskbar), căn giữa theo chiều ngang.</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var work = (Owner != null ? Screen.FromControl(Owner) : Screen.FromPoint(Cursor.Position)).WorkingArea;
            int width = Math.Min(Width, work.Width);
            Bounds = new Rectangle(work.Left + (work.Width - width) / 2, work.Top, width, work.Height);
        }

        /// <summary>
        /// Windows 10 có viền trong suốt (~7px) quanh cửa sổ nên phần nhìn thấy bị hụt ở đáy;
        /// khi cửa sổ đã hiện thì lấy khung thật từ DWM rồi nới cửa sổ để phần nhìn thấy chạm sát taskbar.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var work = Screen.FromControl(this).WorkingArea;
            if (DwmGetWindowAttribute(Handle, DwmwaExtendedFrameBounds, out var frame, Marshal.SizeOf<NativeRect>()) == 0)
            {
                int top = frame.Top - Top, bottom = Bottom - frame.Bottom;
                Bounds = new Rectangle(Left, work.Top - top, Width, work.Height + top + bottom);
            }
        }

        private const int DwmwaExtendedFrameBounds = 9;

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRect
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int attribute, out NativeRect value, int size);

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
            var buttons = _drawButtons = new FlowLayoutPanel
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

            var hint = new Label
            {
                Text = "*Vui lòng ký tay vào ô trống bên dưới hoặc tích chọn \"Chọn mẫu chữ ký\" có sẵn.",
                Font = new Font(Font.FontFamily, 10.5F),
                ForeColor = Accent,
                Dock = DockStyle.Top,
                AutoSize = true,
                MaximumSize = new Size(ClientSize.Width - 64, 0),
                Padding = new Padding(16, 0, 16, 6),
                BackColor = Theme.Surface
            };

            _chkTemplate.Text = "Chọn mẫu chữ ký";
            _chkTemplate.Dock = DockStyle.Top;
            _chkTemplate.Height = 44;
            _chkTemplate.BoxSize = 26;
            _chkTemplate.Padding = new Padding(16, 0, 0, 0);
            _chkTemplate.CheckColor = Accent;
            _chkTemplate.Font = new Font(Font.FontFamily, 13F);
            _chkTemplate.ForeColor = Theme.TextPrimary;
            _chkTemplate.BackColor = Theme.Surface;
            _chkTemplate.CheckedChanged += (_, _) => UpdateSignatureMode();

            _templates.Dock = DockStyle.Fill;
            _templates.FlowDirection = FlowDirection.TopDown;
            _templates.WrapContents = false;
            _templates.AutoScroll = true;
            _templates.Padding = new Padding(16, 4, 16, 8);
            _templates.BackColor = Theme.Surface;
            _templates.Visible = false;
            _templates.Resize += (_, _) => LayoutTemplateItems();
            BuildTemplateItems();

            _signature.Dock = DockStyle.Fill;
            _signature.Margin = new Padding(0);

            // Dock: thêm phần Fill trước, các phần Top thêm sau sẽ nằm trên cùng
            card.Controls.Add(_signature);
            card.Controls.Add(_templates);
            card.Controls.Add(_chkTemplate);
            card.Controls.Add(hint);
            card.Controls.Add(header);
            card.Dock = DockStyle.Fill;
            return card;
        }

        private void BuildTemplateItems()
        {
            var templates = SignatureTemplates.Build(_customerName);
            for (int i = 0; i < templates.Count; i++)
            {
                var item = new SignatureTemplateItem(templates[i], $"Mẫu {i + 1}", Accent)
                {
                    Margin = new Padding(0, 0, 0, 10),
                    Selected = i == 0
                };
                item.Click += (sender, _) =>
                {
                    foreach (var other in _templateItems)
                        other.Selected = other == sender;
                };
                _templateItems.Add(item);
                _templates.Controls.Add(item);
            }
        }

        private void LayoutTemplateItems()
        {
            int w = _templates.ClientSize.Width - _templates.Padding.Horizontal;
            foreach (var item in _templateItems)
                item.Width = Math.Max(100, w);
        }

        /// <summary>Tích "Chọn mẫu chữ ký" thì hiện danh sách mẫu, bỏ tích thì quay lại ô ký tay.</summary>
        private void UpdateSignatureMode()
        {
            bool useTemplate = _chkTemplate.Checked;
            _templates.Visible = useTemplate;
            _signature.Visible = !useTemplate;
            _drawButtons.Visible = !useTemplate;
            if (useTemplate)
                LayoutTemplateItems();
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
            byte[] png;
            if (_chkTemplate.Checked)
            {
                var selected = _templateItems.FirstOrDefault(t => t.Selected);
                if (selected == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn một mẫu chữ ký.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                png = selected.Template.ToPng();
            }
            else
            {
                if (_signature.IsEmpty)
                {
                    MessageBox.Show(this, "Vui lòng ký vào phiếu đăng ký và hợp đồng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                png = _signature.ToPng();
            }
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

    /// <summary>Một mẫu chữ ký sinh từ họ tên khách hàng bằng font viết tay có sẵn trên Windows.</summary>
    internal sealed record SignatureTemplate(string Text, string FontName, FontStyle Style, float SizeScale)
    {
        /// <summary>Màu mực chữ ký mẫu (xanh đậm như bút ký).</summary>
        public static readonly Color Ink = Color.FromArgb(28, 45, 110);

        private Font CreateFont(float size) => new(SignatureTemplates.ResolveFont(FontName), size * SizeScale, Style, GraphicsUnit.Pixel);

        /// <summary>Xuất ảnh chữ ký nền trong suốt.</summary>
        public byte[] ToPng(int width = 600, int height = 200)
        {
            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Nền trong suốt không dùng ClearType được
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Draw(g, new RectangleF(10, 10, width - 20, height - 20), StringAlignment.Center);
            }
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        /// <summary>Vẽ chữ ký giữa khung theo chiều dọc, tự thu nhỏ để vừa khung.</summary>
        public void Draw(Graphics g, RectangleF area, StringAlignment align)
        {
            using var format = new StringFormat(StringFormat.GenericTypographic)
            {
                Alignment = align,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };
            float size = area.Height * 0.55f;
            var font = CreateFont(size);
            try
            {
                var measured = g.MeasureString(Text, font, PointF.Empty, format);
                float scale = Math.Min(area.Width / Math.Max(1, measured.Width), area.Height / Math.Max(1, measured.Height));
                if (scale < 1f)
                {
                    font.Dispose();
                    font = CreateFont(size * scale * 0.95f);
                }
                using var brush = new SolidBrush(Ink);
                g.DrawString(Text, font, brush, area, format);
            }
            finally
            {
                font.Dispose();
            }
        }
    }

    internal static class SignatureTemplates
    {
        private const string FallbackFont = "Segoe Script";
        private static readonly HashSet<string> Installed = new(
            new InstalledFontCollection().Families.Select(f => f.Name), StringComparer.OrdinalIgnoreCase);

        /// <summary>Font không có trên máy thì dùng Segoe Script (có sẵn từ Windows 7).</summary>
        public static string ResolveFont(string name) => Installed.Contains(name) ? name : FallbackFont;

        /// <summary>
        /// 5 mẫu theo họ tên: họ tên, họ tên kiểu nghiêng, tên, họ tên đậm, viết tắt (N.T.Huệ).
        /// Họ tên trên CCCD viết hoa nên đổi sang dạng viết hoa chữ đầu.
        /// </summary>
        public static IReadOnlyList<SignatureTemplate> Build(string name)
        {
            var vi = CultureInfo.GetCultureInfo("vi-VN");
            string full = vi.TextInfo.ToTitleCase(string.Join(' ',
                name.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToLower(vi));
            if (full.Length == 0)
                full = "Chữ ký";
            var words = full.Split(' ');
            string given = words[^1];
            string initials = words.Length > 1
                ? string.Concat(words[..^1].Select(w => char.ToUpper(w[0], vi) + ".")) + given
                : given;

            return new[]
            {
                new SignatureTemplate(full, "Segoe Script", FontStyle.Regular, 0.8f),
                new SignatureTemplate(full, "Gabriola", FontStyle.Italic, 1.25f),
                new SignatureTemplate(given, "Segoe Print", FontStyle.Bold, 0.9f),
                new SignatureTemplate(full, "Segoe Script", FontStyle.Bold, 0.8f),
                new SignatureTemplate(initials, "Ink Free", FontStyle.Italic, 1f)
            };
        }
    }

    /// <summary>Một dòng chọn mẫu chữ ký: nút tròn, chữ ký mẫu và nhãn "Mẫu n"; dòng được chọn có viền và nền màu nhấn.</summary>
    internal sealed class SignatureTemplateItem : Control
    {
        private readonly string _caption;
        private readonly Color _accent;
        private bool _selected;
        private bool _hover;

        public SignatureTemplate Template { get; }

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value)
                    return;
                _selected = value;
                Invalidate();
            }
        }

        public SignatureTemplateItem(SignatureTemplate template, string caption, Color accent)
        {
            Template = template;
            _caption = caption;
            _accent = accent;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Theme.Surface;
            Cursor = Cursors.Hand;
            Height = 64;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            float k = DeviceDpi / 96f;

            var rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var path = RoundedRect(rect, 8 * k))
            {
                using (var fill = new SolidBrush(_selected ? Color.FromArgb(255, 244, 236) : Theme.Surface))
                    g.FillPath(fill, path);
                using var border = new Pen(_selected ? _accent : _hover ? Color.FromArgb(203, 213, 225) : Theme.Border, (_selected ? 1.5f : 1f) * k);
                g.DrawPath(border, path);
            }

            // Nút tròn
            float r = 9 * k;
            var circle = new RectangleF(16 * k, Height / 2f - r, r * 2, r * 2);
            using (var pen = new Pen(_selected ? _accent : Color.FromArgb(203, 213, 225), 1.5f * k))
                g.DrawEllipse(pen, circle);
            if (_selected)
            {
                var dot = RectangleF.Inflate(circle, -4.5f * k, -4.5f * k);
                using var b = new SolidBrush(_accent);
                g.FillEllipse(b, dot);
            }

            // Nhãn "Mẫu n" bên phải
            using var captionFont = new Font(Font.FontFamily, 10F);
            var captionSize = TextRenderer.MeasureText(_caption, captionFont);
            int captionLeft = Width - (int)(14 * k) - captionSize.Width;
            TextRenderer.DrawText(g, _caption, captionFont, new Point(captionLeft, (Height - captionSize.Height) / 2), Theme.TextMuted);

            float left = circle.Right + 14 * k;
            var area = new RectangleF(left, 4 * k, Math.Max(10, captionLeft - left - 10 * k), Height - 8 * k);
            Template.Draw(g, area, StringAlignment.Near);
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
