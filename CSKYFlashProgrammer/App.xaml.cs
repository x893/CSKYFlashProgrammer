using MahApps.Metro;
using Service;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace CskyFlashProgramer
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                AppConfigMgr.Instance.InitUserConfig();
                RegisterConverter.Instance();
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error type: {ex.GetType()}\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                throw ex;
            }
        }

        public static void ChangeTheme(string theme)
        {
            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent(theme), ThemeManager.DetectAppStyle(Current).Item1);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SessionMgr.Instance.SaveSession();
        }

		[STAThread]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
            app.Run();
		}
	}
}