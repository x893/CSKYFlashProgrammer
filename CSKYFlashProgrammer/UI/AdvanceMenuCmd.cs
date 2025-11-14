using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace CskyFlashProgramer.UI
{

    internal class AdvanceMenuCmd : SimpleCommand
    {
        public AdvanceMenuCmd()
        {
            ExecuteDelegate += new Action<object>(OpenAdvanceSettingDlg);
        }

        private void OpenAdvanceSettingDlg(object arg)
        {
            MetroWindow metroWindow = arg as MetroWindow;
			Flyout flyout = new Flyout
			{
				Header = "Advance",
				Content = new MainConfigControl()
			};
			flyout.ClosingFinished += ((o, args) => metroWindow.Flyouts.Items.Remove(flyout));
            metroWindow.Flyouts.Items.Add(flyout);
            flyout.Theme = FlyoutTheme.Light;
            flyout.Position = Position.Right;
            flyout.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            flyout.IsModal = true;
            flyout.IsOpen = true;
        }
    }
}