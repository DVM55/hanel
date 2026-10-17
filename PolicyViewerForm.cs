using log4net;
using Microsoft.Win32;

namespace IdCard.Hanel
{
    /// <summary>
    /// Mở trang chính sách bảo mật ngay trong ứng dụng bằng WebBrowser có sẵn của Windows (engine Internet Explorer),
    /// không bật trình duyệt ngoài, không cần cài thêm WebView2.
    /// </summary>
    internal sealed class PolicyViewerForm : Form
    {
        private static readonly ILog _log = LogManager.GetLogger("App");

        /// <summary>11001 = IE11 edge mode; mặc định WebBrowser chạy chế độ IE7 nên trang web hiện đại bị vỡ giao diện.</summary>
        private const int Ie11EdgeMode = 11001;

        private readonly WebBrowser _web = new()
        {
            Dock = DockStyle.Fill,
            ScriptErrorsSuppressed = true,
            IsWebBrowserContextMenuEnabled = false,
            AllowWebBrowserDrop = false,
            WebBrowserShortcutsEnabled = true
        };

        public PolicyViewerForm(string title, string url)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1100, 900);
            MinimizeBox = false;
            Controls.Add(_web);

            // Liên kết mở cửa sổ mới (target=_blank) thì vẫn mở trong khung này, không bật IE ra ngoài
            _web.NewWindow += (_, e) =>
            {
                e.Cancel = true;
                if (_web.Document?.ActiveElement?.GetAttribute("href") is { Length: > 0 } href)
                    _web.Navigate(href);
            };

            EnsureIe11Mode();
            _web.Navigate(url);
        }

        /// <summary>Đăng ký FEATURE_BROWSER_EMULATION cho exe (HKCU, không cần quyền admin) để WebBrowser dùng IE11.</summary>
        private static void EnsureIe11Mode()
        {
            try
            {
                string exe = Path.GetFileName(Application.ExecutablePath);
                using var key = Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                if (key.GetValue(exe) is not int v || v != Ie11EdgeMode)
                    key.SetValue(exe, Ie11EdgeMode, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                _log.Info($"PolicyViewer: không đặt được IE11 mode: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _web.Dispose();
            base.Dispose(disposing);
        }
    }
}
