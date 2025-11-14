using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{
    public class SetBindingUtils
    {
        public static void SetHexNumTextboxBinding(object source, string propPath, TextBox textBox)
        {
            textBox.SetBinding(TextBox.TextProperty, new Binding()
            {
                Source = source,
                Path = new PropertyPath(propPath, new object[0]),
                Converter = new HexToStringConverter().SetIfAddPrefix(false)
            });
        }

        public static void SetGeneralBinding(
            object source,
            string propPath,
            FrameworkElement ui,
            DependencyProperty property)
        {
            ui.SetBinding(property, new Binding()
            {
                Source = source,
                Path = new PropertyPath(propPath, new object[0])
            });
        }
    }
}