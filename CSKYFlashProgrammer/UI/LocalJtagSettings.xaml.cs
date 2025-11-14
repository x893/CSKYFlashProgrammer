using MahApps.Metro.Controls;
using Service;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
    public partial class LocalJtagSettiongs : UserControl, IComponentConnector
    {
        public LocalJtagSettiongs()
        {
            InitializeComponent();
            InitDDCCtrl();
            SetBindingUtils.SetGeneralBinding(DebugServerSetting, "EnableTRST", m_trst, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(DebugServerSetting, "ICECLk", m_clk, NumericUpDown.ValueProperty);
            SetBindingUtils.SetGeneralBinding(DebugServerSetting, "DelayMTCR", m_delay, NumericUpDown.ValueProperty);
            SetBindingUtils.SetGeneralBinding(DebugServerSetting, "EnableDbgSvrLog", m_enableDbgsvrLog, ToggleButton.IsCheckedProperty);
        }

        private void InitDDCCtrl()
        {
            SetBindingUtils.SetGeneralBinding(DebugServerSetting, "UseDDC", m_ddc, ToggleButton.IsCheckedProperty);
        }

        private DebugServerSetting DebugServerSetting
        {
            get => AppConfigMgr.Instance.AppConfig.DebugServerSetting;
        }
    }
}