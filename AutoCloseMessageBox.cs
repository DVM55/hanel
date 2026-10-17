namespace IdCard.Hanel
{
    /// <summary>Hộp thông báo có nút OK, tự đóng sau một khoảng thời gian nếu người dùng không bấm.</summary>
    internal sealed class AutoCloseMessageBox : Form
    {
        private readonly System.Windows.Forms.Timer _timer = new();

        private AutoCloseMessageBox(string text, string caption, TimeSpan timeout)
        {
            Text = caption;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 11.25F);

            var icon = new PictureBox
            {
                Image = SystemIcons.Information.ToBitmap(),
                SizeMode = PictureBoxSizeMode.AutoSize,
                Margin = new Padding(20, 24, 12, 12)
            };
            var label = new Label
            {
                Text = text,
                AutoSize = true,
                MaximumSize = new Size(420, 0),
                ForeColor = Theme.TextPrimary,
                Margin = new Padding(0, 28, 24, 12)
            };
            var ok = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(100, 34),
                Anchor = AnchorStyles.Right,
                Margin = new Padding(0, 8, 20, 16)
            };
            AcceptButton = ok;
            CancelButton = ok;

            var layout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 2
            };
            layout.Controls.Add(icon, 0, 0);
            layout.Controls.Add(label, 1, 0);
            layout.Controls.Add(ok, 1, 1);
            Controls.Add(layout);

            _timer.Interval = (int)timeout.TotalMilliseconds;
            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                DialogResult = DialogResult.OK;
            };
            Shown += (_, _) => _timer.Start();
        }

        public static void Show(IWin32Window owner, string text, string caption, TimeSpan timeout)
        {
            using var box = new AutoCloseMessageBox(text, caption, timeout);
            box.ShowDialog(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
