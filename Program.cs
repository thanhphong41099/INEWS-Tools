using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using News2025.Services;

namespace News2025
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            // Đăng ký các form để DI có thể inject
            services.AddTransient<frmMain>();
            services.AddTransient<Menu.frmTroiNgang>();
            services.AddTransient<Menu.frmTroiTinTuc>();
            services.AddTransient<Menu.frmConnectCG3>();

            var serviceProvider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(serviceProvider.GetRequiredService<frmMain>());
        }
    }
}
