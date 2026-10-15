using System.Runtime.InteropServices;

namespace IdCard.Hanel
{
    /// <summary>
    /// Tự tạo shortcut "HANEL eKYC" ngoài Desktop ở lần chạy đầu (bản exe chép tay không qua bộ cài).
    /// Mỗi vị trí exe chỉ tạo một lần: người dùng xoá shortcut thì không tạo lại,
    /// chuyển exe sang chỗ khác thì tạo lại cho đúng đường dẫn mới.
    /// </summary>
    internal static class DesktopShortcut
    {
        private const string FileName = "HANEL eKYC.lnk";

        public static void Ensure()
        {
#if DEBUG
            // Chạy từ Visual Studio thì không tạo, tránh shortcut trỏ vào bin\Debug
            return;
#else
            try
            {
                string? exe = Environment.ProcessPath;
                if (string.IsNullOrEmpty(exe))
                    return;
                var settings = AppSettings.Load();
                if (string.Equals(settings.DesktopShortcutExePath, exe, StringComparison.OrdinalIgnoreCase))
                    return;

                string link = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), FileName);
                Create(link, exe);
                settings.DesktopShortcutExePath = exe;
                settings.Save();
            }
            catch (Exception)
            {
                // Không tạo được shortcut thì app vẫn chạy bình thường
            }
#endif
        }

        private static void Create(string link, string exe)
        {
            var shellType = Type.GetTypeFromProgID("WScript.Shell") ?? throw new InvalidOperationException("WScript.Shell not available");
            dynamic shell = Activator.CreateInstance(shellType)!;
            try
            {
                dynamic shortcut = shell.CreateShortcut(link);
                try
                {
                    shortcut.TargetPath = exe;
                    shortcut.WorkingDirectory = Path.GetDirectoryName(exe);
                    shortcut.IconLocation = exe + ",0";
                    shortcut.Description = "Xác thực Căn cước công dân - HANEL eKYC";
                    shortcut.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(shortcut);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }
    }
}
