using System.Drawing.Drawing2D;

namespace IdCard.Hanel
{
    /// <summary>
    /// Vẽ hai bàn tay (nhìn từ mu bàn tay) với 10 ngón có thể bấm chọn.
    /// Mã ngón theo chuẩn ISO/IEC 19794: 1-5 tay phải (cái → út), 6-10 tay trái (cái → út).
    /// </summary>
    public class FingerPicker : Control
    {
        private static readonly string[] FingerNames = { "Ngón cái", "Ngón trỏ", "Ngón giữa", "Ngón áp út", "Ngón út" };

        // Ngón tay trái theo thứ tự từ trái sang: út, áp út, giữa, trỏ, cái (tọa độ khung 200x250)
        // (x gốc, y gốc, độ dài, độ rộng, góc nghiêng độ, chỉ số ngón 0=cái..4=út)
        private static readonly (float X, float Y, float Len, float W, float Angle, int Idx)[] LeftHand =
        {
            (42, 122, 66, 24, -10, 4),
            (72, 110, 92, 26, -4, 3),
            (103, 106, 104, 27, 0, 2),
            (134, 110, 92, 26, 5, 1),
            (160, 166, 66, 28, 48, 0),
        };

        private readonly List<(int Code, GraphicsPath Path)> _fingers = new();
        private readonly List<GraphicsPath> _palms = new();
        private readonly List<(string Text, RectangleF Rect)> _captions = new();
        private int _hover;
        private int _selected;

        public event EventHandler? SelectedFingerChanged;

        public FingerPicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Theme.Surface;
            Cursor = Cursors.Hand;
        }

        /// <summary>Mã ngón đang chọn (1-10), 0 nếu chưa chọn.</summary>
        public int SelectedFinger
        {
            get => _selected;
            set
            {
                if (_selected == value)
                    return;
                _selected = value;
                Invalidate();
                SelectedFingerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private readonly HashSet<int> _captured = new();

        /// <summary>Đánh dấu các ngón đã thu thập (tô xanh).</summary>
        public void SetCapturedFingers(IEnumerable<int> codes)
        {
            _captured.Clear();
            _captured.UnionWith(codes);
            Invalidate();
        }

        public bool IsCaptured(int code) => _captured.Contains(code);

        public static string GetFingerName(int code)
        {
            if (code < 1 || code > 10)
                return "";
            int idx = (code - 1) % 5;
            return $"{FingerNames[idx]} - {(code <= 5 ? "tay phải" : "tay trái")}";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            BuildGeometry();
        }

        private void BuildGeometry()
        {
            foreach (var f in _fingers) f.Path.Dispose();
            foreach (var p in _palms) p.Dispose();
            _fingers.Clear();
            _palms.Clear();
            _captions.Clear();
            if (Width <= 0 || Height <= 0)
                return;

            // Hai bàn tay đặt cạnh nhau, giữ tỉ lệ khung 200x250 mỗi tay
            float handW = 200, handH = 250, gap = 60;
            float scale = Math.Min(Width / (handW * 2 + gap + 20), (Height - 30) / handH);
            float totalW = (handW * 2 + gap) * scale;
            float ox = (Width - totalW) / 2f;
            float oy = 4;

            BuildHand(ox, oy, scale, mirror: false);                          // tay trái
            BuildHand(ox + (handW + gap) * scale, oy, scale, mirror: true);   // tay phải

            float capY = oy + handH * scale + 2;
            _captions.Add(("TAY TRÁI", new RectangleF(ox, capY, handW * scale, 24)));
            _captions.Add(("TAY PHẢI", new RectangleF(ox + (handW + gap) * scale, capY, handW * scale, 24)));
        }

        private void BuildHand(float ox, float oy, float s, bool mirror)
        {
            float MX(float x) => mirror ? 200 - x : x;

            var palm = new GraphicsPath();
            float px = mirror ? 200 - 156 : 28;
            palm.AddPath(Theme.RoundedRect(new RectangleF(px, 112, 128, 128), 34), false);
            using (var m = new Matrix())
            {
                m.Translate(ox, oy);
                m.Scale(s, s);
                palm.Transform(m);
            }
            _palms.Add(palm);

            foreach (var f in LeftHand)
            {
                var path = Theme.RoundedRect(new RectangleF(-f.W / 2, -f.Len, f.W, f.Len + f.W / 2), f.W / 2);
                using (var m = new Matrix())
                {
                    m.Translate(ox, oy);
                    m.Scale(s, s);
                    m.Translate(MX(f.X), f.Y);
                    m.Rotate(mirror ? -f.Angle : f.Angle);
                    path.Transform(m);
                }
                int code = (mirror ? 1 : 6) + f.Idx;
                _fingers.Add((code, path));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var outline = new Pen(Color.FromArgb(176, 192, 212), 1.5f);
            using var palmFill = new SolidBrush(Theme.SurfaceAlt);

            foreach (var (code, path) in _fingers)
            {
                bool captured = _captured.Contains(code);
                // Ngón đã thu: xanh lá; đang chọn: xanh dương (viền xanh lá nếu đã thu)
                Color fill = code == _selected ? Theme.Primary
                    : captured ? (code == _hover ? Color.FromArgb(67, 160, 71) : Theme.Success)
                    : code == _hover ? Color.FromArgb(214, 230, 248)
                    : Color.FromArgb(232, 238, 246);
                Color border = captured ? Color.FromArgb(27, 94, 32)
                    : code == _selected ? Color.FromArgb(13, 71, 161)
                    : Color.FromArgb(176, 192, 212);
                using var brush = new SolidBrush(fill);
                using var pen = new Pen(border, code == _selected || captured ? 2.5f : 1.5f);
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            // Lòng bàn tay vẽ sau để che phần gốc ngón
            foreach (var p in _palms)
            {
                g.FillPath(palmFill, p);
                g.DrawPath(outline, p);
            }

            using var capFont = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            foreach (var (text, rect) in _captions)
                TextRenderer.DrawText(g, text, capFont, Rectangle.Round(rect), Theme.TextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private int HitTest(Point pt)
        {
            foreach (var (code, path) in _fingers)
                if (path.IsVisible(pt))
                    return code;
            return 0;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int hit = HitTest(e.Location);
            if (hit != _hover)
            {
                _hover = hit;
                Cursor = hit > 0 ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hover != 0)
            {
                _hover = 0;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int hit = HitTest(e.Location);
            if (hit > 0)
                SelectedFinger = hit;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var f in _fingers) f.Path.Dispose();
                foreach (var p in _palms) p.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
