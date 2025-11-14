using MahApps.Metro.Controls.Dialogs;
using Service;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CskyFlashProgramer.UI
{

	public class ProgrammerCmdBase : SimpleCommand
	{
		public ProgrammerCmdBase()
		{
			Enable = true;
			ExecuteDelegate = (Action<object>)Delegate.Combine(ExecuteDelegate, new Action<object>(DoStartProgram));
		}

		public bool Enable { get; set; }

		protected FlashProgramSession FlashProgramSession
		{
			get => AppConfigMgr.Instance.AppConfig.FlashProgramSession;
		}

		protected virtual bool DoCheck() => true;

		protected virtual async void DoStartProgram(object parameter)
		{
			MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow.SaveCurrentConfig();
			if (AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetConfig.AlgorithmFile.Equals(""))
			{
				await mainWindow.ShowMessageAsync("Error", "algorithm is not exist", MessageDialogStyle.Affirmative, null);
			}
			else if (DoCheck())
			{
				MetroDialogSettings settings = new MetroDialogSettings()
				{
					NegativeButtonText = "Cancel",
					AnimateShow = false,
					AnimateHide = false
				};
				ProgressDialogController dlgCtrl = await mainWindow.ShowProgressAsync("Please wait...", "Start Program...", settings: settings);
				dlgCtrl.SetIndeterminate();
				dlgCtrl.SetCancelable(true);
				dlgCtrl.Maximum = 100.0;
				Exception obj = null;
				try
				{
					try
					{
						int num2 = await Task<bool>.Factory.StartNew(() => DoProgramAsycn(dlgCtrl)) ? 1 : 0;
						await dlgCtrl.CloseAsync();
						if (!dlgCtrl.IsCanceled)
							await DoShownSuccessMsg(mainWindow);
					}
					catch (WarningException ex)
					{
						await dlgCtrl.CloseAsync();
						await DoShownSuccessMsg(mainWindow, ex.Message);
					}
					catch (Exception ex)
					{
						await dlgCtrl.CloseAsync();
						await mainWindow.ShowMessageAsync("Error", ex.Message);
					}
				}
				catch (Exception ex)
				{
					obj = ex;
				}

				if (dlgCtrl.IsOpen)
					await dlgCtrl.CloseAsync();
				if (obj != null)
					throw obj;
			}
		}

		private static async Task DoShownSuccessMsg(MainWindow mainWindow, string warning = null)
		{
			string message = "Success";
			if (!string.IsNullOrWhiteSpace(warning))
				message = $"{message}\n{warning}";
			await mainWindow.ShowMessageAsync("Information", message);
		}

		protected bool DoProgramAsycn(ProgressDialogController controller)
		{
			ProgrammerProcess.ExecuteProgrammer(AppConfigMgr.Instance.GetUserConfigPath(SessionMgr.Instance.Session.UserSelectedConfig), FlashProgrammerHandler.Create(controller));
			return true;
		}
	}
}
