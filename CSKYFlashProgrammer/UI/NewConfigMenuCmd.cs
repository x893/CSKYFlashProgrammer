using MahApps.Metro.Controls.Dialogs;
using Service;
using System;

namespace CskyFlashProgramer.UI
{

    internal class NewConfigMenuCmd : SimpleCommand
    {
        public NewConfigMenuCmd()
        {
            ExecuteDelegate += new Action<object>(NewConfigOperate);
        }

        private void NewConfigOperate(object arg)
        {
            MainWindow window = arg as MainWindow;
            string str;
            while (true)
            {
                str = window.ShowModalInputExternal("Please input!", "What is the new config file name?");
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (AppConfigMgr.Instance.UserConfigExist(str))
                    {
                        window.ShowModalMessageExternal("Error", "Config file has exist!", settings: new MetroDialogSettings()
                        {
                            AnimateHide = true,
                            AnimateShow = true
                        });
                    }
                    else
                    {
						window.OnNewConfig(str);
                        break;
					}
				}
                else
                    break;
            }
        }
    }
}