using log4net;
using RAR.IdCard.Sdk.Reader;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace IdCard.Hanel
{
    /// <summary>Dữ liệu một lần đọc thẻ gửi lên webhook.</summary>
    internal sealed record WebhookCardData(
        CardFullData Card,
        byte[] ChipFace,
        byte[]? CamFace,
        byte[]? OcrFront,
        byte[]? OcrBack,
        string DeviceId,
        string Phone,
        string Email);

    /// <summary>Lỗi nghiệp vụ máy chủ trả về (status = 0), Message là thông báo hiển thị cho người dùng.</summary>
    internal sealed class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }

    /// <summary>
    /// Dựng và gửi request lên webhook:
    /// v1 (đăng nhập lấy token rồi gửi /api/nfc-scanner/send-data) và v2 (/api/webhook/send-identification).
    /// </summary>
    internal static class WebhookClient
    {
        private const string AuthenticatePath = "/api/authenticate";
        private const string SendDataPath = "/api/certificate-orders/register-certificate-order-with-nfc-info";
        private const string GetCertificateRegisterPath = "/api/certificate-orders/get-certificate-register";
        private const string SendSignImagePath = "/api/certificate-orders/send-sign-image";
        private const string SendIdentificationPath = "/api/webhook/send-identification";
        private const string Nationality = "VNM";
        /// <summary>policyStatus khi người dùng đã tích đồng ý điều khoản.</summary>
        private const string PolicyStatusAccepted = "OK";
        /// <summary>Định dạng acceptTime theo Constants.DatePattern.DateTime phía server.</summary>
        private const string AcceptTimeFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>Giá trị status trong response báo thành công: webhook v1 là 1, webhook v2 là 0.</summary>
        private const int V1SuccessStatus = 1;
        private const int V2SuccessStatus = 0;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(30) };
        private static readonly ILog _log = LogManager.GetLogger("App");

        // Máy sau router chỉ có IP nội bộ, IP public phải hỏi dịch vụ bên ngoài; thử lần lượt để có dự phòng
        private static readonly string[] PublicIpServices =
        {
            "https://api.ipify.org",
            "https://icanhazip.com",
            "https://checkip.amazonaws.com",
            "https://ifconfig.me/ip"
        };

        /// <summary>
        /// Đăng nhập lấy token rồi gửi NFCInfo, trả về requestId, sessionId từ máy chủ
        /// và token để gọi tiếp các API khác (lấy phiếu đăng ký...).
        /// </summary>
        public static async Task<(string? RequestId, string? SessionId, string Token)> SendNfcInfoAsync(
            string webhookUrl, string user, string password, string username, WebhookCardData d)
        {
            string baseUrl = webhookUrl.TrimEnd('/');
            var auth = await SendApiAsync(HttpMethod.Post, baseUrl + AuthenticatePath, new { username = user, password }, null, V1SuccessStatus);
            string? token = auth.ValueKind == JsonValueKind.Object && auth.TryGetProperty("token", out var t) && t.ValueKind == JsonValueKind.String ? t.GetString() : null;
            if (string.IsNullOrEmpty(token))
                throw new ApiException("Máy chủ không trả về token đăng nhập.");

            var payload = await BuildNfcInfoPayloadAsync(username, d);
            var data = await SendApiAsync(HttpMethod.Post, baseUrl + SendDataPath, payload, token, V1SuccessStatus);
            string? Str(string name) => data.ValueKind == JsonValueKind.Object && data.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
            return (Str("requestId"), Str("sessionId"), token);
        }

        /// <summary>
        /// Lấy file PDF phiếu đăng ký: GET {webhookUrl}/api/certificate-orders/get-certificate-register?sessionId=...,
        /// response {status: 1, data: "base64 pdf"}.
        /// </summary>
        public static async Task<byte[]> GetCertificateRegisterAsync(string webhookUrl, string token, string sessionId)
        {
            string url = webhookUrl.TrimEnd('/') + GetCertificateRegisterPath + "?sessionId=" + Uri.EscapeDataString(sessionId);
            var data = await SendApiAsync(HttpMethod.Get, url, null, token, V1SuccessStatus, sessionId);
            string? base64 = data.ValueKind == JsonValueKind.String ? data.GetString() : null;
            if (string.IsNullOrEmpty(base64))
                throw new ApiException("Máy chủ không trả về phiếu đăng ký.");
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Gửi ảnh chữ ký tay: POST {webhookUrl}/api/certificate-orders/send-sign-image
        /// với body {sessionId, signBase64} (base64 thuần của ảnh PNG, không có tiền tố data:image).
        /// </summary>
        public static async Task SendSignImageAsync(string webhookUrl, string token, string sessionId, byte[] signPng)
        {
            string url = webhookUrl.TrimEnd('/') + SendSignImagePath;
            var body = new { sessionId, signBase64 = Convert.ToBase64String(signPng) };
            await SendApiAsync(HttpMethod.Post, url, body, token, V1SuccessStatus, sessionId);
        }

        /// <summary>
        /// Gửi thông tin CCCD sang webhook v2: POST {webhookUrlV2}/api/webhook/send-identification
        /// với body {status: 0, msg, data: {...}} (status = 0 là đọc thẻ thành công).
        /// </summary>
        public static async Task SendIdentificationV2Async(string webhookUrlV2, string? transactionId, WebhookCardData d)
        {
            string url = webhookUrlV2.TrimEnd('/');
            if (!url.EndsWith(SendIdentificationPath, StringComparison.OrdinalIgnoreCase))
                url += SendIdentificationPath;

            // Webhook v2 trả về {status, msg, data}: status = 0 là thành công, khác 0 là thất bại kèm msg
            await SendApiAsync(HttpMethod.Post, url, BuildIdentificationV2Body(transactionId, d), null, V2SuccessStatus,
                transactionId, "transactionId");
        }

        /// <summary>Thông báo lỗi gửi webhook hiển thị cho người dùng.</summary>
        public static string DescribeSendError(Exception ex) => ex switch
        {
            ApiException => ex.Message,
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden } => "Sai tài khoản hoặc mật khẩu.",
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.NotFound } => "Không tìm thấy địa chỉ API, vui lòng kiểm tra lại Webhook URL.",
            HttpRequestException { StatusCode: not null } h => $"Máy chủ trả về lỗi {(int)h.StatusCode.Value}.",
            TaskCanceledException => "Hết thời gian chờ phản hồi từ máy chủ.",
            _ => "Không kết nối được tới máy chủ."
        };

        private static async Task<object> BuildNfcInfoPayloadAsync(string username, WebhookCardData d)
        {
            var dg13 = d.Card.Dg13File!;
            string ip = await GetPublicIpAsync();

            return new
            {
                username,
                deviceId = d.DeviceId,
                number = dg13.DocumentNumber,
                name = dg13.Name,
                dob = dg13.DateOfBirth,
                sex = ToSexCode(dg13.Sex),
                nationality = Nationality,
                doe = dg13.ExpiredDate,
                registerDate = dg13.IssueDate,
                por = dg13.Address,
                poo = dg13.Hometown,
                religion = dg13.Religion,
                nation = dg13.Nation,
                father = dg13.FatherName,
                mother = dg13.MotherName,
                couple = dg13.PartnerName,
                identity = dg13.PreviousNumber,
                symbol = dg13.Character,
                mrz = d.Card.Dg1File?.Mrz ?? "",
                // SOD thô đọc từ chip (base64)
                sodDg = d.Card.Sod is { Length: > 0 } sod ? Convert.ToBase64String(sod) : null,
                nfcImg = Convert.ToBase64String(d.ChipFace),
                faceImg = d.CamFace != null ? Convert.ToBase64String(d.CamFace) : "",
                extraInfo = BuildExtraInfo(d.Card),
                ocrBackImg = (string?)null,
                ocrFrontImg = (string?)null,
                policyAgreement = new
                {
                    policyStatus = PolicyStatusAccepted,
                    acceptTime = DateTime.Now.ToString(AcceptTimeFormat, CultureInfo.InvariantCulture),
                    device = Environment.MachineName,
                    ip
                },
                phone = d.Phone,
                email = string.IsNullOrWhiteSpace(d.Email) ? ValidationConstants.DefaultEmail : d.Email
            };
        }

        private static string BuildExtraInfo(CardFullData card)
        {
            var info = new
            {
                dsCert = card.DSCert,
                dg15File = card.Dg15 is { Length: > 0 } dg15 ? Convert.ToBase64String(dg15) : null
            };
            return JsonSerializer.Serialize(info, JsonWebOptions);
        }

        /// <summary>Body webhook v2: data.transactionId là mã nhận từ deep link hanelekyc://read?transactionId=...</summary>
        private static object BuildIdentificationV2Body(string? transactionId, WebhookCardData d)
        {
            var dg13 = d.Card.Dg13File!;
            return new
            {
                status = 0,
                msg = (string?)null,
                data = new
                {
                    transactionId,
                    identification = dg13.DocumentNumber,
                    previousNumber = dg13.PreviousNumber,
                    name = dg13.Name,
                    dob = dg13.DateOfBirth,
                    sex = ToSexCode(dg13.Sex),
                    nationality = Nationality,
                    nation = dg13.Nation,
                    doe = dg13.ExpiredDate,
                    por = dg13.Address,
                    poo = dg13.Hometown,
                    religion = dg13.Religion,
                    registerDate = dg13.IssueDate,
                    symbol = dg13.Character,
                    father = dg13.FatherName,
                    mother = dg13.MotherName,
                    couple = dg13.PartnerName,
                    mrz = d.Card.Dg1File?.Mrz ?? "",
                    nfcImg = Convert.ToBase64String(d.ChipFace)
                }
            };
        }

        /// <summary>
        /// Gọi API trả về dạng {status, data, msg}: status = <paramref name="successStatus"/> thì trả về data,
        /// ngược lại ném ApiException kèm msg.
        /// </summary>
        private static async Task<JsonElement> SendApiAsync(HttpMethod method, string url, object? body, string? token, int successStatus,
            string? sentId = null, string sentIdName = "sessionId")
        {
            using var request = new HttpRequestMessage(method, url)
            {
                Content = body != null ? JsonContent.Create(body) : null
            };
            if (token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request);
            string text = await response.Content.ReadAsStringAsync();

            JsonElement root = default;
            bool isJson;
            try
            {
                using var doc = JsonDocument.Parse(text);
                root = doc.RootElement.Clone();
                isJson = true;
            }
            catch (JsonException)
            {
                isJson = false;
            }

            LogResponse(method, url, response, isJson ? root : null, sentId, sentIdName);

            bool ok = response.IsSuccessStatusCode && isJson && root.ValueKind == JsonValueKind.Object
                && root.TryGetProperty("status", out var st) && st.ValueKind == JsonValueKind.Number
                && st.TryGetInt32(out var code) && code == successStatus;
            if (ok)
                return root.TryGetProperty("data", out var data) ? data : default;

            // Thất bại thì ưu tiên hiển thị nguyên văn thông báo máy chủ trả về (kể cả khi HTTP lỗi)
            // Phản hồi dạng chữ thuần thì lấy luôn, bỏ qua trang lỗi HTML
            string plain = text.Trim();
            string? serverMsg = isJson ? GetServerMessage(root)
                : plain.Length is > 0 and <= 500 && !plain.StartsWith('<') ? plain : null;
            if (!string.IsNullOrWhiteSpace(serverMsg))
                throw new ApiException(serverMsg);
            response.EnsureSuccessStatusCode();
            throw new ApiException(isJson ? "Máy chủ trả về trạng thái thất bại." : "Phản hồi từ máy chủ không hợp lệ.");
        }


        private static readonly JsonSerializerOptions JsonWebOptions = new(JsonSerializerDefaults.Web);

        private static void LogResponse(HttpMethod method, string url, HttpResponseMessage response, JsonElement? root,
            string? sentId, string sentIdName)
        {
            string sentPart = sentId != null ? $" | {sentIdName} gửi={sentId}" : "";
            // Bỏ query string (sessionId đã ghi riêng)
            int q = url.IndexOf('?');
            var sb = new StringBuilder($"{method} {(q >= 0 ? url[..q] : url)} ->{(int)response.StatusCode}");
            if (root is { ValueKind: JsonValueKind.Object } r)
            {
                string? Field(JsonElement e, string name) =>
                    e.TryGetProperty(name, out var v) && v.ValueKind is not (JsonValueKind.Null or JsonValueKind.Undefined)
                        ? (v.ValueKind == JsonValueKind.String ? v.GetString() : v.GetRawText()) : null;

                sb.Append($" status={Field(r, "status")}");
                if (Field(r, "msg") is { Length: > 0 } msg)
                    sb.Append($" msg={msg}");
                if (Field(r, "errorCode") is { Length: > 0 } err)
                    sb.Append($" errorCode={err}");
                sb.Append(sentPart);
                if (r.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object && Field(data, "sessionId") is { } returned)
                    sb.Append($" | sessionId trả về={returned}");
            }
            else
            {
                sb.Append(sentPart);
                sb.Append(" (phản hồi không phải JSON)");
            }
            _log.Info(sb.ToString());
        }

        /// <summary>Thông báo trong response: lấy trường msg, không có thì message / error.</summary>
        private static string? GetServerMessage(JsonElement root)
        {
            if (root.ValueKind != JsonValueKind.Object)
                return null;
            foreach (var name in new[] { "msg", "message", "error" })
            {
                if (root.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(v.GetString()))
                    return v.GetString();
            }
            return null;
        }

        /// <summary>IP public của máy, không dịch vụ nào trả lời thì dùng IP nội bộ.</summary>
        private static async Task<string> GetPublicIpAsync()
        {
            foreach (var url in PublicIpServices)
            {
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                    string ip = (await _http.GetStringAsync(url, cts.Token)).Trim();
                    if (System.Net.IPAddress.TryParse(ip, out _))
                        return ip;
                }
                catch (Exception ex)
                {
                    _log.Info($"GetPublicIp {url}: {ex.Message}");
                }
            }
            try
            {
                var local = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
                    .FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return local?.ToString() ?? "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>"Nam" → "M", "Nữ" → "F".</summary>
        private static string? ToSexCode(string? sex)
        {
            string s = (sex ?? "").Trim().Normalize(NormalizationForm.FormC).ToLowerInvariant();
            return s switch
            {
                "nam" or "m" or "male" => "M",
                "nữ" or "nu" or "f" or "female" => "F",
                _ => sex
            };
        }
    }
}
