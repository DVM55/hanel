using RAR.IdCard.Sdk.Reader;
using RAR.IdCard.Sdk.Reader.HN212;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdCard.Hanel
{
    public partial class FingerForm : Form
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger("App");
        private VnHn212Reader _reader;
        private bool _capturing;
        // Đã nhận sự kiện START của lần quét hiện tại. StopFingerCapture không huỷ được lần quét đang chạy,
        // nên kết quả của lần quét cũ (đến trước START mới) phải bỏ qua
        private bool _scanStarted;
        private int _session;

        /// <summary>Mã ngón đã chọn (1-10, chuẩn ISO/IEC 19794).</summary>
        public int SelectedFinger => fingerPicker.SelectedFinger;
        public string SelectedFingerName => FingerPicker.GetFingerName(fingerPicker.SelectedFinger);
        /// <summary>Ảnh vân tay thu được của ngón đang thu, null nếu chưa thu thành công.</summary>
        public byte[]? FingerImage { get; private set; }
        /// <summary>Các ngón đã thu thập: mã ngón (1-10) → ảnh vân tay. Thu thành công là lưu ngay.</summary>
        public Dictionary<int, byte[]> Fingers { get; }

        public FingerForm(VnHn212Reader reader, IReadOnlyDictionary<int, byte[]>? captured = null)
        {
            _reader = reader;
            Fingers = captured != null ? new Dictionary<int, byte[]>(captured) : new Dictionary<int, byte[]>();
            InitializeComponent();
        }

        private void FingerForm_Load(object sender, EventArgs e)
        {
            _reader.OnStatusChanged += _reader_OnStatusChanged;
            fingerPicker.SetCapturedFingers(Fingers.Keys);
            // Mặc định chọn ngón chưa thu đầu tiên, ưu tiên ngón trỏ tay phải
            fingerPicker.SelectedFinger = new[] { 2, 7, 1, 6, 3, 8, 4, 9, 5, 10 }.FirstOrDefault(c => !Fingers.ContainsKey(c));
            ShowSelectStep();
        }

        private void FingerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCapture();
            try
            {
                _reader.OnStatusChanged -= _reader_OnStatusChanged;
            }
            catch (Exception) { }
        }

        private void fingerPicker_SelectedFingerChanged(object? sender, EventArgs e)
        {
            UpdateSelectedText();
            btnStart.Enabled = fingerPicker.SelectedFinger > 0;
            btnStart.Text = Fingers.ContainsKey(fingerPicker.SelectedFinger) ? "Thu lại ngón này" : "Bắt đầu thu thập";
        }

        private void UpdateSelectedText()
        {
            int code = fingerPicker.SelectedFinger;
            string count = Fingers.Count > 0 ? $"  •  Đã thu thập {Fingers.Count} ngón" : "";
            if (code <= 0)
                lblSelected.Text = "Bấm vào ngón tay để chọn" + count;
            else if (Fingers.ContainsKey(code))
                lblSelected.Text = $"Đã chọn: {SelectedFingerName} (đã thu, bấm để thu lại)" + count;
            else
                lblSelected.Text = $"Đã chọn: {SelectedFingerName}" + count;
        }

        private void ShowSelectStep()
        {
            _session++;
            StopCapture();
            lblHint.Text = "Bước 1/2: Chọn ngón tay cần thu thập";
            pnlCapture.Visible = false;
            pnlSelect.Visible = true;
            btnBack.Visible = false;
            btnRetry.Visible = false;
            btnDone.Visible = false;
            btnCancel.Visible = true;
            btnCancel.Text = Fingers.Count > 0 ? "Hoàn tất" : "Hủy";
            btnStart.Visible = true;
            btnStart.Enabled = fingerPicker.SelectedFinger > 0;
            btnStart.Text = Fingers.ContainsKey(fingerPicker.SelectedFinger) ? "Thu lại ngón này" : "Bắt đầu thu thập";
            fingerPicker.SetCapturedFingers(Fingers.Keys);
            UpdateSelectedText();
        }

        private void ShowCaptureStep()
        {
            lblHint.Text = "Bước 2/2: Đặt ngón tay lên cảm biến và làm theo hướng dẫn";
            lblFingerName.Text = SelectedFingerName.ToUpper();
            pnlSelect.Visible = false;
            pnlCapture.Visible = true;
            btnCancel.Visible = false;
            btnStart.Visible = false;
            btnBack.Visible = true;
            btnRetry.Visible = true;
            btnDone.Visible = true;
            StartCapture();
        }

        private async void StartCapture()
        {
            int session = ++_session;
            FingerImage = null;
            pictureBoxFinger.Image = null;
            btnDone.Enabled = false;
            btnRetry.Enabled = false;
            btnBack.Enabled = false;
            lbMessage.ForeColor = Theme.Info;
            lbMessage.Text = "Vui lòng chờ...";

            // Luôn đóng phiên trước (kể cả đã thu xong) rồi chờ thiết bị giải phóng cảm biến
            StopCapture();
            await Task.Delay(500);
            if (session != _session || IsDisposed)
                return;

            try
            {
                _scanStarted = false;
                _capturing = true;
                _reader.StartFingerCapture(10000);
            }
            catch (Exception ex)
            {
                _log.Error("StartFingerCapture", ex);
                _capturing = false;
                lbMessage.ForeColor = Theme.Danger;
                lbMessage.Text = ErrorText.Translate(ex.Message);
            }
            finally
            {
                btnRetry.Enabled = true;
                btnBack.Enabled = true;
            }
        }

        private void StopCapture()
        {
            // Sự kiện đến sau khi dừng (của phiên cũ) sẽ bị bỏ qua vì _capturing = false
            _capturing = false;
            try
            {
                _reader.StopFingerCapture();
            }
            catch (Exception) { }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (fingerPicker.SelectedFinger > 0)
                ShowCaptureStep();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ShowSelectStep();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            StartCapture();
        }

        private void _reader_OnStatusChanged(object sender, RAR.IdCard.Sdk.Reader.StatusEventArgs e)
        {
            if (e.EventName != EVENT_NAMES.FINGER_PRINT || e is not FingerPrintEventArgs f)
                return;
            // SDK dùng lại MỘT đối tượng sự kiện cho mọi bước, đẩy qua hàng đợi và vẫn sửa nó trên luồng thiết bị.
            // Phải chụp giá trị ngay khi nhận, nếu không có lúc đọc được trạng thái lẫn lộn
            // (Step = FINISH nhưng Status/Message còn của bước trước, vd "Done") → báo lỗi không xác định.
            var step = f.Step;
            var status = f.Status;
            var message = f.Message;
            var data = f.FrameData;
            if (IsDisposed)
                return;
            //Call from other thread
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => ProcessFingerPrintEvent(step, status, message, data)));
            }
            else
            {
                ProcessFingerPrintEvent(step, status, message, data);
            }
        }

        private void ProcessFingerPrintEvent(FINGER_PRINT_STEPS step, FINGER_PRINT_STATUS status, string? message, byte[]? data)
        {
            if (!_capturing || IsDisposed)
                return;
            if (step == FINGER_PRINT_STEPS.START)
            {
                _scanStarted = true;
                lbMessage.ForeColor = Theme.Info;
                lbMessage.Text = "Nhận mẫu vân tay";
                return;
            }
            if (!_scanStarted)
                return;
            if (step == FINGER_PRINT_STEPS.FINISH)
            {
                // Trạng thái chưa ghi xong (SDK đang sửa dở) thì chờ sự kiện FINISH thật đến sau
                if (status == FINGER_PRINT_STATUS.SUCCESS && data == null)
                    return;
                if (status != FINGER_PRINT_STATUS.SUCCESS && status != FINGER_PRINT_STATUS.TIMEOUT && status != FINGER_PRINT_STATUS.FAILURE)
                    return;

                _capturing = false;
                if (status == FINGER_PRINT_STATUS.SUCCESS)
                {
                    lbMessage.ForeColor = Theme.Success;
                    lbMessage.Text = "Thành công!";
                    //Display finger print image
                    var bts = data!;
                    FingerImage = bts;
                    Fingers[SelectedFinger] = bts;
                    pictureBoxFinger.Image = ImageUtils.BytesToBitmap(bts);
                    lbMessage.Text = $"Thành công! Đã thu thập {Fingers.Count} ngón, bấm \"Chọn ngón khác\" để thu tiếp.";
                    btnDone.Enabled = true;
                    return;
                }
                if (status == FINGER_PRINT_STATUS.TIMEOUT)
                {
                    lbMessage.ForeColor = Theme.Danger;
                    lbMessage.Text = "Hết thời gian, vui lòng bấm Thu lại!";
                    return;
                }
                _log.Warn($"FingerCapture failed: {message}");
                lbMessage.ForeColor = Theme.Danger;
                lbMessage.Text = FingerErrorText(message);
                return;
            }
            if (step == FINGER_PRINT_STEPS.CAPTURING)
            {
                lbMessage.ForeColor = Theme.Success;
                switch (status)
                {
                    case FINGER_PRINT_STATUS.WaitForSensorInitialization:
                        lbMessage.Text = $"Đang khởi tạo...";
                        break;
                    case FINGER_PRINT_STATUS.LiftFinger:
                        lbMessage.Text = $"Nhấc tay ra và đặt lại...";
                        break;
                    case FINGER_PRINT_STATUS.PutFingerOnSensor:
                        lbMessage.Text = $"Đặt {SelectedFingerName.ToLower()} vào cảm biến...";
                        break;
                    case FINGER_PRINT_STATUS.KeepFingerOnSensor:
                        lbMessage.Text = $"Giữ tay trên cảm biến...";
                        break;
                    default:
                        lbMessage.Text = "Vui lòng chờ...";
                        break;
                }
            }
        }

        /// <summary>
        /// Thông báo lỗi thu vân tay. SDK trả về "Exception: ..." hoặc tên trạng thái của cảm biến
        /// (BadQuality, Canceled, ...), không khớp quy tắc chung của ErrorText nên xử lý riêng.
        /// </summary>
        private static string FingerErrorText(string? message)
        {
            string m = message ?? "";
            if (m.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return "Không tìm thấy cảm biến vân tay. Vui lòng kiểm tra kết nối thiết bị!";
            if (m.Contains("cancel", StringComparison.OrdinalIgnoreCase))
                return "Đã hủy thu vân tay, vui lòng bấm Thu lại!";
            if (m.Contains("quality", StringComparison.OrdinalIgnoreCase) || m.Contains("size", StringComparison.OrdinalIgnoreCase))
                return "Ảnh vân tay chưa rõ. Vui lòng đặt ngón tay ngay ngắn, ấn đều và bấm Thu lại!";
            return "Không thu được vân tay, vui lòng bấm Thu lại!";
        }
    }
}
