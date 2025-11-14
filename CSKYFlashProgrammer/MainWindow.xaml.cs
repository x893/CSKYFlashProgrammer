using CskyFlashProgramer.UI;
using MahApps.Metro.Controls;
using Service;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace CskyFlashProgramer
{

    public partial class MainWindow : MetroWindow, IComponentConnector
    {
        private readonly bool IsInitOver;

        public MainWindow()
        {
            InitializeComponent();
            SetBindings();
            m_startBn.Command = new ProgrammerWriteCmd();
            SetVersion();
            IsInitOver = true;
        }

        private void SetVersion() => m_version.Content = "version: " + VersionMgr.Version;

        private void SetBindings()
        {
            InitUserCongfigCombox();
            SetBindingUtils.SetGeneralBinding(SessionMgr.Instance.Session, "UserSelectedConfig", m_userConfig, Selector.SelectedValueProperty);
        }

        private void InitUserCongfigCombox()
        {
            if (IsAbsolutePath(SessionMgr.Instance.Session.UserSelectedConfig))
            {
                m_userConfig.Items.Add(SessionMgr.Instance.Session.UserSelectedConfig);
                m_userConfig.IsEnabled = false;
            }
            else
            {
                foreach (object newItem in AppConfigMgr.Instance.GetAllUserConfig())
                    m_userConfig.Items.Add(newItem);
                if (!m_userConfig.Items.IsEmpty)
                    return;
                m_userConfig.Items.Add(SessionMgr.Instance.Session.UserSelectedConfig);
            }
        }

        private bool IsAbsolutePath(string path) => Path.IsPathRooted(path);

        public MainMenu MainMenu => m_mainMenu;

        private void OnUserConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
                SaveUserConfig(e.RemovedItems[0] as string);
            LoadUserConfig(m_userConfig.SelectedItem.ToString());
        }

        private void LoadUserConfig(string nConfig)
        {
            try
            {
                if (IsAbsolutePath(nConfig))
                    AppConfigMgr.Instance.LoadConfig(nConfig);
                else if (AppConfigMgr.Instance.UserConfigExist(nConfig))
                    AppConfigMgr.Instance.LoadUserConfig(nConfig);
                else
                    AppConfigMgr.Instance.LoadDefaultConfig();
                App.ChangeTheme(SessionMgr.Instance.Session.Theme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error type: {ex.GetType()}\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                throw ex;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            m_programFile.Child = new ProgramObjectControl();
            m_programObjCtrl = m_programFile.Child as ProgramObjectControl;
        }

        public void SaveCurrentConfig()
        {
            SaveUserConfig(m_userConfig.SelectedValue.ToString());
        }

        private void SaveUserConfig(string oConfig)
        {
            if (string.IsNullOrEmpty(oConfig))
                return;
            if (!IsInitOver)
                return;
            try
            {
                if (IsAbsolutePath(oConfig))
                    AppConfigMgr.Instance.SaveConfigFile(oConfig);
                else
                    AppConfigMgr.Instance.SaveUserConfig(oConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error type: {ex.GetType()}\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                throw ex;
            }
        }

        public void OnNewConfig(string name)
        {
            SaveCurrentConfig();
            AppConfigMgr.Instance.CreateUserConfig(name);
            m_userConfig.Items.Add(name);
            m_userConfig.SelectionChanged -= new SelectionChangedEventHandler(OnUserConfig_SelectionChanged);
            m_userConfig.SelectedValue = name;
            m_userConfig.SelectionChanged += new SelectionChangedEventHandler(OnUserConfig_SelectionChanged);
            AppConfigMgr.Instance.LoadUserConfig(name);
            UpdateUI();
        }

        private void MetroWindow_Closed(object sender, EventArgs e) => SaveCurrentConfig();

    }
}