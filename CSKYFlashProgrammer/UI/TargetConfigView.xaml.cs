using MahApps.Metro.Controls;
using Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{

    public partial class TargetConfigView : UserControl, IComponentConnector
    {
        private readonly bool _inited;

        public TargetConfig TargetConfig
        {
            get => AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetConfig;
        }

        public TargetConfigView()
        {
            InitializeComponent();
            _inited = true;
            InitDataBinding();
            DoUpdateUI();
        }

        private void InitDataBinding()
        {
            SetAlgorithmFileBinding();
            SetPreScriptFileBinding();
            SetEraseTypeBinding();
            SetResetTypeBinding();
            SetEraseRangBinding();
            SetConnectTypeBinding();
            InitTimeOutInput();
            SetResetValueBinding();
            SetBindingUtils.SetHexNumTextboxBinding(TargetConfig, "CPUSelect", m_cpuSelect);
            SetBindingUtils.SetGeneralBinding(TargetConfig, "ResetAndRun", (FrameworkElement)m_hardRest, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(TargetConfig, "RunMode", (FrameworkElement)m_runMode, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(TargetConfig, "LogEnable", (FrameworkElement)m_logEnable, ToggleButton.IsCheckedProperty);
        }

        private void InitTimeOutInput()
        {
            m_timeOut.NumericInputMode = NumericInput.Decimal;
            SetBinding((FrameworkElement)m_timeOut, NumericUpDown.ValueProperty, "Timeout");
        }

        private void SetConnectTypeBinding()
        {
            m_connect.Items.Clear();
            m_connect.SetBinding(ItemsControl.ItemsSourceProperty, (BindingBase)new Binding()
            {
                Source = ConnectTypeUtil.GetList(),
                Mode = BindingMode.OneTime
            });
            Binding binding = CreateBinding();
            binding.Path = new PropertyPath("ConnectType", new object[0]);
            binding.Converter = new ConnectTpyeConverter();
            m_connect.SetBinding(Selector.SelectedValueProperty, (BindingBase)binding);
        }

        private void SetBinding(FrameworkElement element, DependencyProperty property, string path)
        {
            Binding binding = CreateBinding();
            binding.Path = new PropertyPath(path, new object[0]);
            element.SetBinding(property, (BindingBase)binding);
        }

        private void SetEraseRangBinding()
        {
            SetBindingUtils.SetHexNumTextboxBinding(TargetConfig, "EraseRangStart", m_start);
            SetBindingUtils.SetHexNumTextboxBinding(TargetConfig, "EraseRangLength", m_length);
        }

        private Binding CreateBinding()
        {
            return new Binding()
            {
                Source = TargetConfig
            };
        }

        private void SetEraseTypeBinding()
        {
            m_eraseType.Items.Clear();
            m_eraseType.SetBinding(ItemsControl.ItemsSourceProperty, (BindingBase)new Binding()
            {
                Source = EraseTypeUtil.GetAllTypeAsStrings(),
                Mode = BindingMode.OneTime
            });
            m_eraseType.SetBinding(Selector.SelectedValueProperty, (BindingBase)new Binding()
            {
                Source = TargetConfig,
                Path = new PropertyPath("EraseType", new object[0]),
                Converter = new EraseTpyeConverter()
            });
        }

        private void SetResetTypeBinding()
        {
            m_resetType.Items.Clear();
            m_resetType.SetBinding(ItemsControl.ItemsSourceProperty, (BindingBase)new Binding()
            {
                Source = ResetTypeUtil.GetAllTypeAsStrings(),
                Mode = BindingMode.OneTime
            });
            m_resetType.SetBinding(Selector.SelectedValueProperty, (BindingBase)new Binding()
            {
                Source = TargetConfig,
                Path = new PropertyPath("ResetType", new object[0]),
                Converter = new ResetTpyeConverter()
            });
        }

        private void SetResetValueBinding()
        {
            SetBindingUtils.SetHexNumTextboxBinding(TargetConfig, "ResetValue", m_resetValue);
        }

        private void SetAlgorithmFileBinding()
        {
            m_algorithm.SetBinding(new Binding()
            {
                Source = TargetConfig,
                Path = new PropertyPath("AlgorithmFile", new object[0]),
                Mode = BindingMode.TwoWay
            });
        }

        private void SetPreScriptFileBinding()
        {
            m_preScript.SetBinding(new Binding()
            {
                Source = TargetConfig,
                Path = new PropertyPath("PreScriptFile", new object[0]),
                Mode = BindingMode.TwoWay
            });
        }

        private void OnResetAndRun(object sender, RoutedEventArgs e) => DoUpdateUI();

        private void DoUpdateUI()
        {
            if (!_inited)
                return;
            bool flag = TargetConfig.ResetType.Equals(ResetType.Soft) && TargetConfig.ResetAndRun;
            m_resetValue.IsEnabled = flag;
            m_resetValueLabel.IsEnabled = flag;
        }

        private void OnResetTypeChanged(object sender, EventArgs e) => DoUpdateUI();
    }
}