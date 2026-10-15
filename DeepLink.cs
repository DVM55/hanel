using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace IdCard.Hanel
{
    /// <summary>Một lệnh deep link đã phân tích, ví dụ hanelekyc://read?ref=HS001 -> Action "read", Params["ref"] = "HS001".</summary>
    internal sealed class DeepLinkRequest
    {
        public string RawUrl { get; init; } = "";
        public string Action { get; init; } = "";
        public IReadOnlyDictionary<string, string> Params { get; init; } = new Dictionary<string, string>();

        public string? Get(string key) => Params.TryGetValue(key, out var v) ? v : null;
    }

    /// <summary>
    /// Mở app từ trình duyệt/ứng dụng khác qua URL dạng hanelekyc://&lt;action&gt;?key=value.
    /// - Đăng ký scheme vào HKCU\Software\Classes (không cần quyền admin) nếu bộ cài chưa đăng ký đúng exe này.
    /// - Lần mở đầu: URL nằm trong args của Main.
    /// - App đang chạy: instance mới gửi URL sang instance cũ qua named pipe rồi thoát, instance cũ tự hiện lên trước.
    /// Sự kiện <see cref="Received"/> được gọi trên thread nền; lệnh đến trước khi có người đăng ký sẽ được giữ lại và phát khi đăng ký.
    /// </summary>
    internal static class DeepLink
    {
        public const string Scheme = "hanelekyc";
        private const int MaxUrlLength = 2048;
        private const string PipeName = "HANEL_IDCARD_VERIFY_DEEPLINK";

        private static readonly object _lock = new();
        private static readonly Queue<DeepLinkRequest> _pending = new();
        private static Action<DeepLinkRequest>? _received;

        public static event Action<DeepLinkRequest>? Received
        {
            add
            {
                List<DeepLinkRequest> backlog;
                lock (_lock)
                {
                    _received += value;
                    backlog = _pending.ToList();
                    _pending.Clear();
                }
                foreach (var req in backlog)
                    value?.Invoke(req);
            }
            remove
            {
                lock (_lock)
                    _received -= value;
            }
        }

        /// <summary>Lấy URL deep link trong tham số dòng lệnh (Windows truyền vào dạng "exe" "hanelekyc://...").</summary>
        public static string? FindUrl(string[] args) =>
            args.FirstOrDefault(a => a.StartsWith(Scheme + ":", StringComparison.OrdinalIgnoreCase));

        public static bool TryParse(string? url, out DeepLinkRequest request)
        {
            request = new DeepLinkRequest();
            if (string.IsNullOrWhiteSpace(url) || url.Length > MaxUrlLength)
                return false;
            if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
                || !string.Equals(uri.Scheme, Scheme, StringComparison.OrdinalIgnoreCase))
                return false;

            // hanelekyc://read?x=1 -> Host "read"; hanelekyc:read?x=1 -> AbsolutePath "read"
            string action = !string.IsNullOrEmpty(uri.Host) ? uri.Host : uri.AbsolutePath.Trim('/');
            var query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                int eq = part.IndexOf('=');
                string key = Unescape(eq < 0 ? part : part[..eq]);
                string value = eq < 0 ? "" : Unescape(part[(eq + 1)..]);
                if (key.Length > 0)
                    query[key] = value;
            }

            request = new DeepLinkRequest
            {
                RawUrl = url,
                Action = string.IsNullOrEmpty(action) ? "open" : action.ToLowerInvariant(),
                Params = query
            };
            return true;
        }

        private static string Unescape(string s) => Uri.UnescapeDataString(s.Replace('+', ' '));

        /// <summary>Phân tích URL và phát sự kiện; URL sai định dạng bị bỏ qua.</summary>
        public static void Dispatch(string? url)
        {
            if (!TryParse(url, out var req))
                return;
            Action<DeepLinkRequest>? handler;
            lock (_lock)
            {
                handler = _received;
                if (handler == null)
                {
                    _pending.Enqueue(req);
                    return;
                }
            }
            handler(req);
        }

        /// <summary>
        /// Đăng ký hanelekyc:// cho user hiện tại. Bỏ qua nếu registry (HKCU hoặc HKLM của bộ cài) đã trỏ đúng exe này,
        /// chuyển exe sang chỗ khác thì ghi lại đường dẫn mới.
        /// </summary>
        public static void Register()
        {
            try
            {
                string? exe = Environment.ProcessPath;
                if (string.IsNullOrEmpty(exe))
                    return;
                string command = $"\"{exe}\" \"%1\"";
                using (var current = Registry.ClassesRoot.OpenSubKey($@"{Scheme}\shell\open\command"))
                {
                    if (string.Equals(current?.GetValue("") as string, command, StringComparison.OrdinalIgnoreCase))
                        return;
                }

                using var key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{Scheme}");
                key.SetValue("", "URL:HANEL eKYC");
                key.SetValue("URL Protocol", "");
                using (var icon = key.CreateSubKey("DefaultIcon"))
                    icon.SetValue("", $"\"{exe}\",0");
                using (var cmd = key.CreateSubKey(@"shell\open\command"))
                    cmd.SetValue("", command);
            }
            catch (Exception)
            {
                // Không ghi được registry thì chỉ mất tính năng mở bằng link, app vẫn chạy bình thường
            }
        }

        /// <summary>Instance chính lắng nghe URL do các instance mở sau gửi sang.</summary>
        public static void StartServer()
        {
            new Thread(ServerLoop) { IsBackground = true, Name = "DeepLinkPipe" }.Start();
        }

        private static void ServerLoop()
        {
            while (true)
            {
                try
                {
                    using var server = new NamedPipeServerStream(PipeName, PipeDirection.In, 1,
                        PipeTransmissionMode.Byte, PipeOptions.CurrentUserOnly);
                    server.WaitForConnection();
                    using var reader = new StreamReader(server, Encoding.UTF8);
                    var buffer = new char[MaxUrlLength + 1];
                    int n = reader.ReadBlock(buffer, 0, buffer.Length);
                    Dispatch(new string(buffer, 0, n));
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// Gửi URL sang instance đang chạy (không có URL thì gửi lệnh "open" để đưa cửa sổ lên trước).
        /// Trả về false nếu không liên lạc được.
        /// </summary>
        public static bool SendToRunningInstance(string? url)
        {
            try
            {
                // Process này đang có quyền foreground, nhường cho instance cũ để nó tự Activate được
                AllowSetForegroundWindow(ASFW_ANY);
                using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.CurrentUserOnly);
                client.Connect(3000);
                string message = url ?? $"{Scheme}://open";
                if (message.Length > MaxUrlLength)
                    return false;
                client.Write(Encoding.UTF8.GetBytes(message));
                client.Flush();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private const int ASFW_ANY = -1;

        [DllImport("user32.dll")]
        private static extern bool AllowSetForegroundWindow(int dwProcessId);
    }
}
