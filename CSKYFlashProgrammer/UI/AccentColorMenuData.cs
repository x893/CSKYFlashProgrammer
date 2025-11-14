using MahApps.Metro;
using Service;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CskyFlashProgramer.UI
{
    public class AccentColorMenuData
    {
        private ICommand changeAccentCommand;

        public string Name { get; set; }

        public Brush BorderColorBrush { get; set; }

        public Brush ColorBrush { get; set; }

        public ICommand ChangeAccentCommand
        {
            get
            {
                ICommand changeAccentCommand1 = changeAccentCommand;
                if (changeAccentCommand1 != null)
                    return changeAccentCommand1;
				SimpleCommand simpleCommand = new SimpleCommand
				{
					CanExecuteDelegate = x => true,
					ExecuteDelegate = x => DoChangeTheme(x)
				};
				ICommand changeAccentCommand2 = simpleCommand;
                changeAccentCommand = simpleCommand;
                return changeAccentCommand2;
            }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Name), tuple.Item1);
            SessionMgr.Instance.Session.Theme = Name;
        }
    }
}
