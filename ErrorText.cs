using System.Text.RegularExpressions;

namespace IdCard.Hanel
{
    /// <summary>
    /// Chuyển thông báo lỗi (tiếng Anh) từ SDK sang tiếng Việt để hiển thị cho người dùng.
    /// SDK không công bố danh sách mã lỗi nên phân loại theo từ khóa trong thông báo.
    /// </summary>
    internal static class ErrorText
    {
        private static readonly (Regex Pattern, string Text)[] Rules =
        {
            (R(@"real face|spoof|fake|liveness|not real"), "Không phát hiện được khuôn mặt thật{0}. Vui lòng nhìn thẳng vào camera, đủ ánh sáng và thử lại."),
            (R(@"multi\w*\s*face|more than one face"), "Phát hiện nhiều khuôn mặt trong khung hình. Vui lòng chỉ để một người trước camera."),
            (R(@"(no|not found|not detect\w*)\s+(any\s+)?face|face not found"), "Không phát hiện được khuôn mặt{0}. Vui lòng nhìn thẳng vào camera và thử lại."),
            (R(@"cancel|abort|ignore|user stop"), "Đã hủy thao tác."),
            (R(@"camera|video|webcam"), "Không mở được camera. Vui lòng kiểm tra kết nối camera và thử lại."),
            (R(@"finger"), "Không lấy được mẫu vân tay. Vui lòng đặt lại ngón tay và thử lại."),
            (R(@"card\s+(was\s+)?(remov|absent|not present)|no card|card empty"), "Thẻ đã bị rút ra hoặc chưa được cắm. Vui lòng cắm lại thẻ."),
            (R(@"access|\bbac\b|\bpace\b|authenticat|\bmrz\b"), "Xác thực truy cập chip thất bại. Vui lòng cắm lại thẻ và thử lại."),
            (R(@"\bsod\b|integrity|hash|signature|certificate"), "Kiểm tra toàn vẹn dữ liệu thẻ thất bại."),
            (R(@"compare|match|template|extract"), "Không so khớp được khuôn mặt. Vui lòng chụp lại ảnh chân dung."),
            (R(@"timeout|time out|timed out"), "Hết thời gian chờ{0}. Vui lòng thử lại."),
            (R(@"device|connect|usb|\bhid\b"), "Mất kết nối với thiết bị đọc thẻ. Vui lòng kiểm tra cáp kết nối."),
            (R(@"chip|apdu|read|card"), "Không đọc được dữ liệu chip. Vui lòng cắm lại thẻ và thử lại."),
        };

        private static readonly Regex DurationMs = R(@"(\d+)\s*ms");

        public static string Translate(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";

            // Đã là tiếng Việt (có ký tự ngoài ASCII) thì giữ nguyên
            if (raw.Any(c => c > 127))
                return raw;

            string duration = "";
            var m = DurationMs.Match(raw);
            if (m.Success && int.TryParse(m.Groups[1].Value, out int ms) && ms >= 1000)
                duration = $" trong {ms / 1000} giây";

            foreach (var (pattern, text) in Rules)
            {
                if (pattern.IsMatch(raw))
                    return string.Format(text, duration);
            }
            return "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";
        }

        private static Regex R(string pattern) => new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
