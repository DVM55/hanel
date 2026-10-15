using RAR.IdCard.Sdk.Reader;
using RAR.IdCard.Sdk.Reader.HN212;

namespace IdCard.Hanel
{
    public partial class CaptureFace : Form
    {
        public FACE_CAPTURE_STATAUS Status { get; set; } = FACE_CAPTURE_STATAUS.UNKNOWN;
        public FACE_DETECT_TYPES FaceType { get; set; } = FACE_DETECT_TYPES.UNKNOWN;
        public String Message { get; set; } = "";
        public byte[]? FaceImage { get; set; } = null;
        private VnHn212Reader _reader;
        // Khung hình gần nhất từ camera, dùng cho chụp thủ công (SDK không có API chụp tay)
        private byte[]? _lastFrame;
        private byte[]? _lastFace;
        private bool _manualCaptured;
        public CaptureFace(VnHn212Reader reader)
        {
            _reader = reader;
            InitializeComponent();
        }

        private void CaptureFace_Shown(object sender, EventArgs e)
        {
            if (_reader == null)
                return;

            //Add event handle
            _reader.OnVideoFrame += reader_OnVideoFrame;
            string selectedCamId = _reader.ReaderConfig.CaptureFaceCameraPath;

            if (string.IsNullOrEmpty(selectedCamId))
            {
                MessageBox.Show("Không tìm thấy Camera để chụp ảnh khuôn mặt!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            
            _reader.FaceCaptureCamId = selectedCamId;

            if (_reader != null)
                _reader.StartFaceCapture();
        }

        private void reader_OnVideoFrame(object sender, StatusEventArgs e)
        {
            //Call from other thread
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    OnEvent(sender, e);
                }));
            }
            else
            {
                OnEvent(sender, e);
            }
        }
        private void OnEvent(object sender, StatusEventArgs e) 
        { 
            CaptureEventArgs? vd = e as CaptureEventArgs;
            if (vd == null || _manualCaptured)
                return;
            this.FaceType = vd.FaceType;
            this.FaceImage = vd.FaceData;
            switch (vd.Status)
            {
                case FACE_CAPTURE_STATAUS.CAPTURING:
                    {
                        if (vd.FrameData != null)
                            _lastFrame = vd.FrameData;
                        if (vd.FaceData != null)
                            _lastFace = vd.FaceData;
                        try
                        {
                            //Display video on form
                            using (var ms = new MemoryStream(vd.FrameData))
                            {
                                picFromCamera.Image = Image.FromStream(ms);
                            }
                        } catch (Exception) { }
                    }
                    break;
                case FACE_CAPTURE_STATAUS.FAILURE:
                    {
                        Status = FACE_CAPTURE_STATAUS.FAILURE;
                        Message = vd.Message ?? "";
                        //Close this form
                        this.Close();
                    }
                    break;
                case FACE_CAPTURE_STATAUS.SUCCESS:
                    {
                        Status = FACE_CAPTURE_STATAUS.SUCCESS;
                        Message = $"FaceCapture success";
                        //Close this form
                        this.Close();
                    }
                    break;
                case FACE_CAPTURE_STATAUS.IGNORE:
                    {
                        Status = FACE_CAPTURE_STATAUS.IGNORE;
                        Message = vd.Message;
                        //Close this form
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        private void CaptureFace_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void CaptureFace_Load(object sender, EventArgs e)
        {
            btnCapture.Enabled = !_reader.ReaderConfig.AutoCaptureFaceEnabled;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            var image = _lastFace ?? _lastFrame;
            if (image == null)
            {
                MessageBox.Show("Chưa nhận được hình ảnh từ camera, vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _manualCaptured = true;
            FaceImage = image;
            Status = FACE_CAPTURE_STATAUS.SUCCESS;
            Message = "FaceCapture manual";
            _reader.StopFaceCapture();
            this.Close();
        }

        private void CaptureFace_FormClosed(object sender, FormClosedEventArgs e)
        {
            // remove event handle
            _reader.OnVideoFrame -= reader_OnVideoFrame;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _reader.StopFaceCapture();
        }
    }
}
