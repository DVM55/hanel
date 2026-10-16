using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace IdCard.Hanel
{
    /// <summary>
    /// Xem PDF trực tiếp trong ứng dụng bằng WebView2 (trình xem PDF của Edge).
    /// Nội dung PDF lấy từ bộ nhớ qua một URL ảo, không ghi file xuống máy; ẩn nút tải về / lưu trên thanh công cụ.
    /// </summary>
    internal sealed class PdfViewerForm : Form
    {
        /// <summary>URL ảo chỉ dùng nội bộ để WebView2 yêu cầu nội dung PDF, không đi ra mạng.</summary>
        private const string PdfUrl = "https://pdf.hanel.local/document.pdf";

        private readonly WebView2 _web = new() { Dock = DockStyle.Fill };
        private readonly byte[] _pdf;

        public PdfViewerForm(string title, byte[] pdf)
        {
            _pdf = pdf;
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 1000);
            MinimizeBox = false;
            Controls.Add(_web);
        }

        /// <summary>
        /// Khởi tạo WebView2 trước khi hiển thị; trả về false nếu máy chưa cài WebView2 Runtime.
        /// Thư mục dữ liệu đặt trong %LocalAppData% để chạy được cả khi exe nằm ở thư mục không có quyền ghi.
        /// </summary>
        public async Task<bool> LoadAsync()
        {
            try
            {
                string dataDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HanelEKYC", "WebView2");
                var env = await CoreWebView2Environment.CreateAsync(null, dataDir);
                await _web.EnsureCoreWebView2Async(env);

                var core = _web.CoreWebView2;
                core.Settings.HiddenPdfToolbarItems = CoreWebView2PdfToolbarItems.Save | CoreWebView2PdfToolbarItems.SaveAs;
                core.Settings.AreDefaultContextMenusEnabled = false;
                core.Settings.AreDevToolsEnabled = false;
                core.DownloadStarting += (_, e) => e.Cancel = true;

                core.AddWebResourceRequestedFilter(PdfUrl, CoreWebView2WebResourceContext.All);
                core.WebResourceRequested += (_, e) =>
                {
                    e.Response = core.Environment.CreateWebResourceResponse(
                        new MemoryStream(_pdf), 200, "OK", "Content-Type: application/pdf");
                };
                core.Navigate(PdfUrl);
                return true;
            }
            catch (WebView2RuntimeNotFoundException)
            {
                return false;
            }
        }
    }
}
