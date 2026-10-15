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
}
