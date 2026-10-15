using IdCard.Hanel;

namespace IdCard.Hanel_obj
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string? deepLinkUrl = DeepLink.FindUrl(args);
            using var mutex = new Mutex(true, "HANEL_IDCARD_VERIFY", out var createdNew);
            if (!createdNew)
            {
                // App đã chạy: chuyển link (hoặc lệnh mở) sang cửa sổ đang mở rồi thoát
                DeepLink.SendToRunningInstance(deepLinkUrl);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            DesktopShortcut.Ensure();
            DeepLink.Register();
            DeepLink.StartServer();
            DeepLink.Dispatch(deepLinkUrl);
            Application.Run(new MainForm());
            // Application.Run(new EKycForm());
        }
    }
}
