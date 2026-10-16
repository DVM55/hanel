using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IdCard.Hanel
{
    /// <summary>
    /// Cấu hình người dùng (webhook nhận dữ liệu, so khớp khuôn mặt), lưu tại %AppData%\HanelEKYC\settings.json.
    /// Mật khẩu được mã hóa bằng DPAPI theo tài khoản Windows hiện tại.
    /// </summary>
    internal class AppSettings
    {
        public string WebhookUrl { get; set; } = "";
        public string WebhookUser { get; set; } = "";
        public string WebhookPasswordProtected { get; set; } = "";
        /// <summary>Tự động so khớp ảnh chụp với ảnh trong chip ngay sau khi chụp/đọc thẻ.</summary>
        public bool AutoCompareFace { get; set; } = true;
        /// <summary>Tỷ lệ so khớp khuôn mặt tối thiểu (%) để coi là cùng một người.</summary>
        public int FaceMatchThreshold { get; set; } = 70;
        /// <summary>Tự động chụp khi phát hiện khuôn mặt thật (tắt thì bấm nút Chụp thủ công).</summary>
        public bool AutoCaptureFace { get; set; } = true;
        /// <summary>Thời gian tối thiểu (ms) phải giữ khuôn mặt thật trước khi tự động chụp.</summary>
        public int AutoCaptureFaceMinimumMs { get; set; } = 200;
        /// <summary>Phát hiện khuôn mặt khi chụp.</summary>
        public bool CaptureFaceDetectFace { get; set; } = true;
        /// <summary>Hiển thị đếm ngược thời gian chờ trên màn hình chụp.</summary>
        public bool CaptureFaceShowTimeout { get; set; } = true;
        /// <summary>Thời gian chờ chụp ảnh khuôn mặt (ms).</summary>
        public int CaptureFaceTimeoutMs { get; set; } = 10000;
        /// <summary>Đường dẫn exe đã được tạo shortcut ngoài Desktop (xem DesktopShortcut).</summary>
        public string DesktopShortcutExePath { get; set; } = "";
        /// <summary>Webhook v2 nhận thông tin CCCD (/api/webhook/send-identification), chỉ lấy từ .env.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string WebhookUrlV2 { get; private set; } = "";

        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HanelEKYC", "settings.json");

        /// <summary>Mật khẩu dạng chữ thường lấy từ .env (không ghi lại vào settings.json).</summary>
        private string _defaultPassword = "";

        public string GetWebhookPassword()
        {
            if (!string.IsNullOrEmpty(_defaultPassword))
                return _defaultPassword;
            if (string.IsNullOrEmpty(WebhookPasswordProtected))
                return "";
            try
            {
                var data = ProtectedData.Unprotect(Convert.FromBase64String(WebhookPasswordProtected), null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public void SetWebhookPassword(string password)
        {
            WebhookPasswordProtected = string.IsNullOrEmpty(password)
                ? ""
                : Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(password), null, DataProtectionScope.CurrentUser));
        }

        public static AppSettings Load()
        {
            var settings = new AppSettings();
            try
            {
                if (File.Exists(FilePath))
                    settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
            }
            catch (Exception) { }
            settings.ApplyDefaultConfig();
            return settings;
        }

        private static string DefaultFilePath => Path.Combine(AppContext.BaseDirectory, ".env");

        /// <summary>
        /// Đọc file .env (KEY=VALUE, mỗi dòng một biến; dòng bắt đầu bằng # là chú thích; giá trị có thể bọc trong "" hoặc '').
        /// </summary>
        private static Dictionary<string, string> ReadEnvFile(string path)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;
                if (line.StartsWith("export ", StringComparison.Ordinal))
                    line = line.Substring(7).TrimStart();
                var eq = line.IndexOf('=');
                if (eq <= 0)
                    continue;
                var key = line.Substring(0, eq).Trim();
                var value = line.Substring(eq + 1).Trim();
                if (value.Length >= 2 && (value[0] == '"' || value[0] == '\'') && value[^1] == value[0])
                    value = value.Substring(1, value.Length - 2);
                result[key] = value;
            }
            return result;
        }

        /// <summary>Cấu hình mặc định trong .env cạnh exe (đóng gói cùng app), được ưu tiên hơn settings.json.</summary>
        private void ApplyDefaultConfig()
        {
            try
            {
                if (!File.Exists(DefaultFilePath))
                    return;
                var env = ReadEnvFile(DefaultFilePath);
                if (env.TryGetValue("WEBHOOK_URL", out var url)) WebhookUrl = url;
                if (env.TryGetValue("WEBHOOK_USER", out var user)) WebhookUser = user;
                if (env.TryGetValue("WEBHOOK_PASSWORD", out var pwd) && pwd.Length > 0) _defaultPassword = pwd;
                if (env.TryGetValue("WEBHOOK_URL_V2", out var urlV2)) WebhookUrlV2 = urlV2;
                if (env.TryGetValue("AUTO_COMPARE_FACE", out var auto) && bool.TryParse(auto, out var autoVal)) AutoCompareFace = autoVal;
                if (env.TryGetValue("FACE_MATCH_THRESHOLD", out var th) && int.TryParse(th, out var thVal)) FaceMatchThreshold = thVal;
                if (env.TryGetValue("AUTO_CAPTURE_FACE", out var ac) && bool.TryParse(ac, out var acVal)) AutoCaptureFace = acVal;
                if (env.TryGetValue("AUTO_CAPTURE_FACE_MIN_MS", out var acMs) && int.TryParse(acMs, out var acMsVal)) AutoCaptureFaceMinimumMs = acMsVal;
                if (env.TryGetValue("CAPTURE_FACE_DETECT_FACE", out var df) && bool.TryParse(df, out var dfVal)) CaptureFaceDetectFace = dfVal;
                if (env.TryGetValue("CAPTURE_FACE_SHOW_TIMEOUT", out var st) && bool.TryParse(st, out var stVal)) CaptureFaceShowTimeout = stVal;
                if (env.TryGetValue("CAPTURE_FACE_TIMEOUT_MS", out var to) && int.TryParse(to, out var toVal)) CaptureFaceTimeoutMs = toVal;
            }
            catch (Exception) { }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
