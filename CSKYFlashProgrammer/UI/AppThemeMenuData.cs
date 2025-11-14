using MahApps.Metro;
using System.Windows;

namespace CskyFlashProgramer.UI
{
    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.DetectAppStyle(Application.Current).Item2, ThemeManager.GetAppTheme(Name));
        }
    }
}