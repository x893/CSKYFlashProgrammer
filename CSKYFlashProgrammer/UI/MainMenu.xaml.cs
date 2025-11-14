using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace CskyFlashProgramer.UI
{
    public partial class MainMenu : UserControl, IComponentConnector
    {
        private List<AppThemeMenuData> m_appThemes;

        public List<AppThemeMenuData> AppThemes
        {
            get
            {
                if (m_appThemes == null)
                    m_appThemes = ThemeManager.AppThemes.Select<AppTheme, AppThemeMenuData>((Func<AppTheme, AppThemeMenuData>)(a =>
                    {
                        return new AppThemeMenuData()
                        {
                            Name = a.Name,
                            BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                            ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                        };
                    })).ToList<AppThemeMenuData>();
                return m_appThemes;
            }
        }

        public List<AccentColorMenuData> AccentColors { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            AccentColors = ThemeManager.Accents.Select(a => new AccentColorMenuData()
            {
                Name = a.Name,
                ColorBrush = a.Resources["AccentColorBrush"] as Brush
            }).ToList();
            m_advance.Command = new AdvanceMenuCmd();
            m_advance.CommandParameter = Application.Current.MainWindow;
            m_new_menu_item.Command = new NewConfigMenuCmd();
            m_new_menu_item.CommandParameter = Application.Current.MainWindow;
        }
    }
}