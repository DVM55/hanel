using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Docnet.Core;
using Docnet.Core.Models;

namespace IdCard.Hanel
{
    internal sealed class PdfViewerForm : Form
    {
        private const double RenderScale = 2.0;
        private const int PageGap = 12;

        private readonly FlowLayoutPanel _pages = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.FromArgb(0x52, 0x56, 0x59),
            Padding = new Padding(PageGap)
        };

        private readonly List<Bitmap> _images = new();

        public PdfViewerForm(string title, byte[] pdf)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 1000);
            MinimizeBox = false;
            Controls.Add(_pages);

            RenderPages(pdf);
            _pages.Resize += (_, _) => FitPages();
            FitPages();
        }

        private void RenderPages(byte[] pdf)
        {
            using var reader = DocLib.Instance.GetDocReader(pdf, new PageDimensions(RenderScale));
            for (int i = 0; i < reader.GetPageCount(); i++)
            {
                using var page = reader.GetPageReader(i);
                var bmp = ToBitmap(page.GetImage(), page.GetPageWidth(), page.GetPageHeight());
                _images.Add(bmp);
                _pages.Controls.Add(new PictureBox
                {
                    Image = bmp,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, PageGap)
                });
            }
        }

        /// <summary>PDFium trả về BGRA có nền trong suốt, vẽ lên nền trắng để hiển thị.</summary>
        private static Bitmap ToBitmap(byte[] bgra, int width, int height)
        {
            using var raw = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var data = raw.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                for (int y = 0; y < height; y++)
                    Marshal.Copy(bgra, y * width * 4, data.Scan0 + y * data.Stride, width * 4);
            }
            finally
            {
                raw.UnlockBits(data);
            }

            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(raw, 0, 0, width, height);
            return bmp;
        }

        /// <summary>Co giãn các trang theo chiều rộng khung xem, giữ đúng tỉ lệ.</summary>
        private void FitPages()
        {
            int width = Math.Max(100, _pages.ClientSize.Width - _pages.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);
            _pages.SuspendLayout();
            foreach (PictureBox pb in _pages.Controls)
                pb.Size = new Size(width, (int)((long)width * pb.Image.Height / pb.Image.Width));
            _pages.ResumeLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _images.ForEach(b => b.Dispose());
            base.Dispose(disposing);
        }
    }
}
