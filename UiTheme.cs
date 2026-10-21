using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IdCard.Hanel
{
    /// <summary>
    /// Bảng màu dùng chung cho toàn bộ giao diện (tông sáng).
    /// </summary>
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(238, 242, 247);
        public static readonly Color Surface = Color.White;
        public static readonly Color SurfaceAlt = Color.FromArgb(243, 246, 250);
        public static readonly Color Border = Color.FromArgb(225, 231, 239);
        public static readonly Color Navy = Color.FromArgb(11, 59, 102);
        public static readonly Color Primary = Color.FromArgb(21, 101, 192);
        public static readonly Color Placeholder = Color.FromArgb(120, 134, 220);
        public static readonly Color TextPrimary = Color.FromArgb(31, 41, 55);
        public static readonly Color TextMuted = Color.FromArgb(107, 114, 128);
        public static readonly Color Success = Color.FromArgb(46, 125, 50);
        public static readonly Color Danger = Color.FromArgb(211, 47, 47);
        public static readonly Color Info = Color.FromArgb(21, 101, 192);

        public static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Panel dạng "card": nền trắng, viền mảnh, có thể có tiêu đề với vạch nhấn.
    /// </summary>
    public class CardPanel : Panel
    {
        private string _title = "";

        [DefaultValue("")]
        [Category("Appearance")]
        public string Title
        {
            get => _title;
            set { _title = value ?? ""; Invalidate(); }
        }

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Theme.Surface;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            float k = DeviceDpi / 96f;

            using (var border = new Pen(Theme.Border))
            {
                g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
                if (_title.Length == 0)
                    return;
                int lineY = (int)(36 * k);
                g.DrawLine(border, 1, lineY, Width - 2, lineY);
            }

            using (var accent = new SolidBrush(Theme.Primary))
                g.FillRectangle(accent, (int)(16 * k), (int)(11 * k), (int)(4 * k), (int)(15 * k));

            using var titleFont = new Font(Font.FontFamily, 9.75F, FontStyle.Bold);
            var rect = new Rectangle((int)(28 * k), 0, Width - (int)(40 * k), (int)(36 * k));
            TextRenderer.DrawText(g, _title, titleFont, rect, Theme.TextPrimary,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>
    /// Ô giá trị của một trường dữ liệu. Text dạng "Nhãn: giá trị":
    /// phần giá trị được vẽ trong ô bo góc, phần nhãn được đẩy sang <see cref="CaptionLabel"/>
    /// (nếu có) để nhãn luôn khớp với dữ liệu, kể cả khi đổi loại thẻ.
    /// </summary>
    public class FieldLabel : Label
    {
        [DefaultValue(null)]
        [Category("Behavior")]
        public Control? CaptionLabel { get; set; }

        [DefaultValue("---")]
        [Category("Appearance")]
        public string Placeholder { get; set; } = "---";

        public FieldLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
            BackColor = Theme.SurfaceAlt;
            ForeColor = Theme.Navy;
        }

        private (string caption, string value) Split()
        {
            var text = Text ?? "";
            int idx = text.IndexOf(':');
            return idx >= 0 ? (text[..idx].Trim(), text[(idx + 1)..].Trim()) : ("", text.Trim());
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            var (caption, _) = Split();
            if (CaptionLabel != null && caption.Length > 0)
                CaptionLabel.Text = caption;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Theme.Surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float k = DeviceDpi / 96f;
            using (var path = Theme.RoundedRect(new RectangleF(0, 0, Width - 1, Height - 1), 6 * k))
            using (var fill = new SolidBrush(BackColor))
                g.FillPath(fill, path);

            var (_, value) = Split();
            bool empty = value.Length == 0;
            if (empty)
                value = Placeholder;

            int pad = (int)(12 * k);
            var flags = TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            var area = new Rectangle(pad, 0, Width - 2 * pad, Height);
            var size = TextRenderer.MeasureText(g, value, Font, new Size(area.Width, int.MaxValue), flags);
            int h = Math.Min(size.Height, Height);
            var rect = new Rectangle(area.X, (Height - h) / 2, area.Width, h);
            TextRenderer.DrawText(g, value, Font, rect, empty ? Theme.Placeholder : ForeColor, flags);
        }
    }

    public enum IndicatorState
    {
        None,
        Ok,
        Fail
    }

    /// <summary>
    /// Đèn trạng thái một bước xử lý: vòng tròn rỗng / tích xanh / dấu X đỏ + chữ.
    /// </summary>
    public class StatusIndicator : Control
    {
        private IndicatorState _state = IndicatorState.None;

        [DefaultValue(IndicatorState.None)]
        [Category("Appearance")]
        public IndicatorState State
        {
            get => _state;
            set { _state = value; Invalidate(); }
        }

        public StatusIndicator()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            ForeColor = Theme.TextPrimary;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Theme.Surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float k = DeviceDpi / 96f;
            float d = 22 * k;
            var circle = new RectangleF(1, (Height - d) / 2f, d, d);

            switch (_state)
            {
                case IndicatorState.Ok:
                    using (var b = new SolidBrush(Theme.Success)) g.FillEllipse(b, circle);
                    using (var p = new Pen(Color.White, 2.2f * k) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                        g.DrawLines(p, new[]
                        {
                            new PointF(circle.X + d * 0.27f, circle.Y + d * 0.52f),
                            new PointF(circle.X + d * 0.44f, circle.Y + d * 0.68f),
                            new PointF(circle.X + d * 0.74f, circle.Y + d * 0.36f)
                        });
                    break;
                case IndicatorState.Fail:
                    using (var b = new SolidBrush(Theme.Danger)) g.FillEllipse(b, circle);
                    using (var p = new Pen(Color.White, 2.2f * k) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                    {
                        float a = d * 0.33f, z = d * 0.67f;
                        g.DrawLine(p, circle.X + a, circle.Y + a, circle.X + z, circle.Y + z);
                        g.DrawLine(p, circle.X + z, circle.Y + a, circle.X + a, circle.Y + z);
                    }
                    break;
                default:
                    using (var p = new Pen(Color.FromArgb(160, 170, 184), 1.8f * k)) g.DrawEllipse(p, circle);
                    break;
            }

            var textRect = new Rectangle((int)(d + 10 * k), 0, Width - (int)(d + 10 * k), Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>
    /// Vòng xoay chờ đơn giản gồm 8 chấm.
    /// </summary>
    public class Spinner : Control
    {
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 90 };
        private int _step;

        public Spinner()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            ForeColor = Theme.Primary;
            _timer.Tick += (s, e) => { _step = (_step + 1) % 8; Invalidate(); };
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool Active
        {
            get => _timer.Enabled;
            set
            {
                _timer.Enabled = value && !DesignMode;
                Visible = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Theme.Surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float size = Math.Min(Width, Height);
            float dot = size * 0.2f;
            float radius = (size - dot) / 2f;
            float cx = Width / 2f, cy = Height / 2f;
            for (int i = 0; i < 8; i++)
            {
                int age = (_step - i + 8) % 8;
                int alpha = 255 - age * 28;
                double angle = i * Math.PI / 4 - Math.PI / 2;
                float x = cx + (float)(radius * Math.Cos(angle)) - dot / 2;
                float y = cy + (float)(radius * Math.Sin(angle)) - dot / 2;
                using var b = new SolidBrush(Color.FromArgb(alpha, ForeColor));
                g.FillEllipse(b, x, y, dot, dot);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Ô nhập liệu dạng thẻ: nền trắng bo góc, icon bên trái, nút xoá bên phải,
    /// viền xanh khi đang nhập, viền đỏ kèm thông báo lỗi trong ô khi <see cref="ErrorText"/> có giá trị.
    /// </summary>
    public class InputBox : Control
    {
        private readonly TextBox _box = new() { BorderStyle = BorderStyle.None, BackColor = Color.White };
        private string _icon = "";
        private string _errorText = "";
        private bool _hover, _hoverClear;

        public InputBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            ForeColor = Theme.Navy;
            Cursor = Cursors.IBeam;
            _box.ForeColor = Theme.Navy;
            _box.TextChanged += (s, e) => { ErrorText = ""; Invalidate(); OnTextChanged(e); };
            _box.GotFocus += (s, e) => Invalidate();
            _box.LostFocus += (s, e) => { Invalidate(); OnLeave(e); };
            _box.KeyPress += (s, e) => OnKeyPress(e);
            _box.MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            _box.MouseLeave += (s, e) => { _hover = ClientRectangle.Contains(PointToClient(MousePosition)); Invalidate(); };
            Controls.Add(_box);
        }

        /// <summary>Ký tự icon trong font Segoe MDL2 Assets.</summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string Icon
        {
            get => _icon;
            set { _icon = value ?? ""; PerformLayout(); Invalidate(); }
        }

        [DefaultValue("")]
        [Category("Appearance")]
        public string Placeholder
        {
            get => _box.PlaceholderText;
            set => _box.PlaceholderText = value ?? "";
        }

        [DefaultValue(32767)]
        [Category("Behavior")]
        public int MaxLength
        {
            get => _box.MaxLength;
            set => _box.MaxLength = value;
        }

        /// <summary>Thông báo lỗi; khác rỗng thì viền đỏ và hiện lỗi ở bên phải ô.</summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string ErrorText
        {
            get => _errorText;
            set
            {
                value ??= "";
                if (_errorText == value)
                    return;
                _errorText = value;
                PerformLayout();
                Invalidate();
            }
        }

        [AllowNull]
        public override string Text
        {
            get => _box.Text;
            set => _box.Text = value ?? "";
        }

        public int SelectionStart => _box.SelectionStart;

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _box.Font = Font;
            _errorFont?.Dispose();
            _errorFont = null;
            PerformLayout();
        }

        private float K => DeviceDpi / 96f;
        private bool HasError => _errorText.Length > 0;
        private Rectangle BoxRect => new(0, 0, Width - 1, Height - 1);
        private int ErrorWidth => HasError ? TextRenderer.MeasureText(_errorText, ErrorFont).Width + (int)(8 * K) : 0;
        private Font? _errorFont;
        private Font ErrorFont => _errorFont ??= new Font(Font.FontFamily, 9F, FontStyle.Regular);
        private int IconWidth => _icon.Length > 0 ? (int)(40 * K) : (int)(14 * K);
        private Rectangle ClearRect
        {
            get
            {
                int w = (int)(34 * K);
                var b = BoxRect;
                return new Rectangle(b.Right - w, b.Y, w, b.Height);
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            var b = BoxRect;
            int x = IconWidth;
            int w = Math.Max(0, ClearRect.X - ErrorWidth - x);
            _box.SetBounds(x, b.Y + (b.Height - _box.Height) / 2, w, _box.Height);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_box.TextLength > 0 && ClearRect.Contains(e.Location))
                _box.Clear();
            _box.Focus();
            _box.SelectionStart = _box.TextLength;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool over = _box.TextLength > 0 && ClearRect.Contains(e.Location);
            Cursor = over ? Cursors.Hand : Cursors.IBeam;
            if (!_hover || over != _hoverClear)
            {
                _hover = true;
                _hoverClear = over;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = _box.ClientRectangle.Contains(_box.PointToClient(MousePosition));
            _hoverClear = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _box.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Theme.Surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float k = K;
            bool focused = _box.Focused;
            var b = BoxRect;

            Color border = HasError ? Theme.Danger
                : focused ? Theme.Primary
                : _hover ? Color.FromArgb(160, 176, 196)
                : Color.FromArgb(203, 213, 225);
            float bw = (focused || HasError ? 2f : 1f) * k;

            // Quầng sáng nhẹ quanh ô khi đang nhập
            if (focused)
            {
                var glow = new RectangleF(b.X + 1, b.Y + 1, b.Width - 2, b.Height - 2);
                using var gp = Theme.RoundedRect(glow, 8 * k);
                using var gpen = new Pen(Color.FromArgb(40, HasError ? Theme.Danger : Theme.Primary), 4 * k);
                g.DrawPath(gpen, gp);
            }

            var inner = new RectangleF(b.X + bw / 2 + 1, b.Y + bw / 2 + 1, b.Width - bw - 2, b.Height - bw - 2);
            using (var path = Theme.RoundedRect(inner, 7 * k))
            {
                using (var fill = new SolidBrush(Color.White))
                    g.FillPath(fill, path);
                using var pen = new Pen(border, bw);
                g.DrawPath(pen, path);
            }

            if (_icon.Length > 0)
            {
                using var iconFont = new Font("Segoe MDL2 Assets", Font.Size, FontStyle.Regular);
                var iconRect = new Rectangle(b.X + (int)(4 * k), b.Y, IconWidth - (int)(4 * k), b.Height);
                TextRenderer.DrawText(g, _icon, iconFont, iconRect, HasError ? Theme.Danger : focused ? Theme.Primary : Theme.TextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            if (_box.TextLength > 0)
            {
                var cr = ClearRect;
                float d = 20 * k;
                var circle = new RectangleF(cr.X + (cr.Width - d) / 2f, cr.Y + (cr.Height - d) / 2f, d, d);
                if (_hoverClear)
                    using (var hb = new SolidBrush(Color.FromArgb(232, 241, 251))) g.FillEllipse(hb, circle);
                using var xp = new Pen(_hoverClear ? Theme.Primary : Color.FromArgb(148, 163, 184), 1.6f * k) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                float a = d * 0.34f, z = d * 0.66f;
                g.DrawLine(xp, circle.X + a, circle.Y + a, circle.X + z, circle.Y + z);
                g.DrawLine(xp, circle.X + z, circle.Y + a, circle.X + a, circle.Y + z);
            }

            if (HasError)
            {
                var cr = ClearRect;
                int ew = ErrorWidth;
                var er = new Rectangle(cr.X - ew, b.Y, ew, b.Height);
                TextRenderer.DrawText(g, _errorText, ErrorFont, er, Theme.Danger,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
        }
    }

    /// <summary>
    /// Nhãn có phần đuôi khác màu, ví dụ dấu * bắt buộc màu đỏ sau tên trường.
    /// </summary>
    public class SuffixLabel : Label
    {
        private string _suffix = "";

        [DefaultValue("")]
        [Category("Appearance")]
        public string Suffix
        {
            get => _suffix;
            set { _suffix = value ?? ""; Invalidate(); }
        }

        [Category("Appearance")]
        public Color SuffixColor { get; set; } = Theme.Danger;

        [DefaultValue(false)]
        [Category("Appearance")]
        public bool SuffixRegular { get; set; }

        /// <summary>Kiểu tiêu đề mục: có vạch nhấn bên trái.</summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool Heading { get; set; }

        public SuffixLabel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
            var r = ClientRectangle;
            if (Heading)
            {
                float k = DeviceDpi / 96f;
                int barH = (int)(18 * k);
                using (var accent = new SolidBrush(Theme.Primary))
                    g.FillRectangle(accent, 0, (Height - barH) / 2, (int)(4 * k), barH);
                int indent = (int)(14 * k);
                r = new Rectangle(indent, 0, Width - indent, Height);
            }
            int textW = TextRenderer.MeasureText(g, Text, Font, r.Size, flags).Width;
            TextRenderer.DrawText(g, Text, Font, r, ForeColor, flags | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            if (_suffix.Length == 0 || textW >= r.Width)
                return;
            using var suffixFont = SuffixRegular ? new Font(Font, FontStyle.Regular) : new Font(Font, Font.Style);
            var sr = new Rectangle(r.X + textW, r.Y, r.Width - textW, r.Height);
            TextRenderer.DrawText(g, _suffix, suffixFont, sr, SuffixColor, flags | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>
    /// Ô tích chọn kèm dòng chữ; phần đặt trong [ ] của Text được tô màu <see cref="HighlightColor"/>.
    /// Nếu có đăng ký <see cref="LinkClicked"/> thì đoạn [ ] đầu tiên là liên kết: bấm vào gọi sự kiện đó thay vì tích chọn.
    /// </summary>
    public class ConsentCheck : Control
    {
        private bool _checked;
        private bool _invalid;
        private bool _hover;
        private bool _linkHover;
        private Rectangle _linkRect;

        public event EventHandler? CheckedChanged;

        public ConsentCheck()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.Selectable, true);
            ForeColor = Theme.TextPrimary;
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value)
                    return;
                _checked = value;
                if (value)
                    _invalid = false;
                Invalidate();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>Viền ô tích màu đỏ khi bắt buộc chọn mà chưa chọn.</summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool Invalid
        {
            get => _invalid;
            set { _invalid = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color HighlightColor { get; set; } = Theme.Danger;

        /// <summary>Màu nền ô tích khi đã chọn (và viền khi rê chuột).</summary>
        [Category("Appearance")]
        public Color CheckColor { get; set; } = Theme.Primary;

        /// <summary>Cạnh ô tích (px ở 96 DPI).</summary>
        [DefaultValue(20)]
        [Category("Appearance")]
        public int BoxSize { get; set; } = 20;

        /// <summary>Bấm vào đoạn [ ] đầu tiên của Text (chỉ là liên kết khi có đăng ký sự kiện này).</summary>
        public event EventHandler? LinkClicked;

        private bool HasLink => LinkClicked != null;

        private bool IsOverLink(Point p) => HasLink && _linkRect.Contains(p);

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Focus();
            if (e.Button != MouseButtons.Left)
                return;
            if (IsOverLink(e.Location))
                LinkClicked?.Invoke(this, EventArgs.Empty);
            else
                Checked = !Checked;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool over = IsOverLink(e.Location);
            if (over != _linkHover)
            {
                _linkHover = over;
                Invalidate();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
                Checked = !Checked;
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; _linkHover = false; Invalidate(); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Theme.Surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float k = DeviceDpi / 96f;

            float d = BoxSize * k;
            var box = new RectangleF(Padding.Left + 1 * k, (Height - d) / 2f, d, d);
            using (var path = Theme.RoundedRect(box, 4 * k))
            {
                if (_checked)
                {
                    using (var fill = new SolidBrush(CheckColor)) g.FillPath(fill, path);
                    using var tick = new Pen(Color.White, 2.2f * k * BoxSize / 20f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
                    g.DrawLines(tick, new[]
                    {
                        new PointF(box.X + d * 0.24f, box.Y + d * 0.52f),
                        new PointF(box.X + d * 0.42f, box.Y + d * 0.70f),
                        new PointF(box.X + d * 0.76f, box.Y + d * 0.32f)
                    });
                }
                else
                {
                    using (var fill = new SolidBrush(Color.White)) g.FillPath(fill, path);
                    Color border = _invalid ? Theme.Danger : (_hover || Focused) ? CheckColor : Color.FromArgb(148, 163, 184);
                    using var pen = new Pen(border, (_invalid ? 2f : 1.5f) * k);
                    g.DrawPath(pen, path);
                }
            }

            // Vẽ từng đoạn chữ, đoạn trong [ ] dùng màu nổi bật
            var flags = TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
            int x = (int)(box.Right + 10 * k);
            int lineH = TextRenderer.MeasureText(g, "Ag", Font, Size.Empty, flags).Height;
            int y = (Height - lineH) / 2;
            var parts = (Text ?? "").Split('[', ']');
            _linkRect = Rectangle.Empty;
            for (int i = 0; i < parts.Length && x < Width; i++)
            {
                if (parts[i].Length == 0)
                    continue;
                Color color = i % 2 == 1 ? HighlightColor : ForeColor;
                int w = TextRenderer.MeasureText(g, parts[i], Font, Size.Empty, flags).Width;
                bool isLink = i == 1 && HasLink;
                if (isLink)
                    _linkRect = new Rectangle(x, y, w, lineH);
                // Liên kết được gạch chân khi rê chuột vào
                using var underline = isLink && _linkHover ? new Font(Font, Font.Style | FontStyle.Underline) : null;
                TextRenderer.DrawText(g, parts[i], underline ?? Font, new Rectangle(x, y, Width - x, lineH), color, flags | TextFormatFlags.EndEllipsis);
                x += w;
            }
        }
    }
}
