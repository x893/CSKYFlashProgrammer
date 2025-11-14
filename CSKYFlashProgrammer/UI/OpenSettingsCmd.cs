using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace CskyFlashProgramer.UI
{

    public class OpenSettingsCmd : SimpleCommand
    {
        public OpenSettingsCmd()
        {
            ExecuteDelegate += new Action<object>(DoExecute);
        }

        private void DoExecute(object sender)
        {
            MainWindow mainwindow = Application.Current.MainWindow as MainWindow;
			Flyout flyout = new Flyout
			{
				Header = "Settings",
				Content = new LocalJtagSettiongs()
			};
			flyout.ClosingFinished += (o, args) => mainwindow.Flyouts.Items.Remove(flyout);
            mainwindow.Flyouts.Items.Add(flyout);
            flyout.Theme = FlyoutTheme.Accent;
            flyout.Position = Position.Top;
            flyout.IsModal = true;
            flyout.IsOpen = true;
        }

        private void Flyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}