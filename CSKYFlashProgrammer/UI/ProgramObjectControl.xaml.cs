using Service;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
    public partial class ProgramObjectControl : UserControl, IComponentConnector
    {
        public List<TargetObject> TargetArray
        {
            get => AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetObjectArray;
        }

        public ProgramObjectControl()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            for (int index = 0; index < TargetArray.Count; ++index)
            {
                if (TargetArray[index] is BinObject)
                    m_listView.Items.Add(new BinObjectUI(TargetArray[index] as BinObject));
                else if (TargetArray[index] is HexObject || TargetArray[index] is ElfObject)
                    m_listView.Items.Add(
                        (TargetArray[index] as ElfObject).Type != ProgramFileType.Elf
                        ? (object)new IHexFileView(TargetArray[index] as HexObject)
                        : (object)new ElfFileView(TargetArray[index] as ElfObject)
                        );
                else if (TargetArray[index] is WordObject)
                    m_listView.Items.Add(new WordValuePage(TargetArray[index] as WordObject));
                else if (TargetArray[index] is ScriptObject)
                    m_listView.Items.Add(new ScriptObjectUI(TargetArray[index] as ScriptObject));
            }
        }

        private void OnButtonAddClicked(object sender, RoutedEventArgs e)
        {
            contextMenu.PlacementTarget = (UIElement)m_buttonAdd;
            contextMenu.Placement = PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void OnButtonRemoveClicked(object sender, RoutedEventArgs e)
        {
            object selectedItem = m_listView.SelectedItem;
            if (selectedItem == null)
                return;
            m_listView.Items.Remove(selectedItem);
            TargetArray.Remove((selectedItem as IPageUI).GetObj());
        }

        private void OnButtonUpClicked(object sender, RoutedEventArgs e)
        {
            int selectedIndex = m_listView.SelectedIndex;
            object selectedItem = m_listView.SelectedItem;
            if (selectedIndex == 0)
            {
                int count = m_listView.Items.Count;
                m_listView.Items.RemoveAt(selectedIndex);
                m_listView.Items.Add(selectedItem);
                m_listView.SelectedIndex = count - 1;
                TargetArray.RemoveAt(selectedIndex);
                TargetArray.Add((selectedItem as IPageUI).GetObj());
            }
            else
            {
                if (selectedIndex <= 0)
                    return;
                m_listView.Items.RemoveAt(selectedIndex);
                m_listView.Items.Insert(selectedIndex - 1, selectedItem);
                m_listView.SelectedIndex = selectedIndex - 1;
                TargetArray.RemoveAt(selectedIndex);
                IPageUI pageUi = selectedItem as IPageUI;
                TargetArray.Insert(selectedIndex - 1, pageUi.GetObj());
            }
        }

        private void OnButtonDownClicked(object sender, RoutedEventArgs e)
        {
            int selectedIndex = m_listView.SelectedIndex;
            object selectedItem = m_listView.SelectedItem;
            if (selectedIndex == m_listView.Items.Count - 1)
            {
                m_listView.Items.RemoveAt(selectedIndex);
                m_listView.Items.Insert(0, selectedItem);
                m_listView.SelectedIndex = 0;
                TargetArray.RemoveAt(selectedIndex);
                TargetArray.Insert(0, (selectedItem as IPageUI).GetObj());
            }
            else
            {
                if (selectedIndex < 0)
                    return;
                m_listView.Items.RemoveAt(selectedIndex);
                m_listView.Items.Insert(selectedIndex + 1, selectedItem);
                m_listView.SelectedIndex = selectedIndex + 1;
                TargetArray.RemoveAt(selectedIndex);
                IPageUI pageUi = selectedItem as IPageUI;
                TargetArray.Insert(selectedIndex + 1, pageUi.GetObj());
            }
        }

        private void OnMenuBinClicked(object sender, RoutedEventArgs e)
        {
            BinObject binObject = new BinObject();
            BinObjectUI newItem = new BinObjectUI(binObject);
            m_listView.Items.Add(newItem);
            m_listView.SelectedItem = newItem;
            TargetArray.Add(binObject);
        }

        private void OnMenuElfClicked(object sender, RoutedEventArgs e)
        {
            ElfObject elfObject = new ElfObject();
            ElfFileView newItem = new ElfFileView(elfObject);
            m_listView.Items.Add(newItem);
            m_listView.SelectedItem = newItem;
            TargetArray.Add(elfObject);
        }

        private void OnMenuHexClicked(object sender, RoutedEventArgs e)
        {
            HexObject hexObject = new HexObject();
            IHexFileView newItem = new IHexFileView(hexObject);
            m_listView.Items.Add(newItem);
            m_listView.SelectedItem = newItem;
            TargetArray.Add(hexObject);
        }

        private void OnMenuWordClicked(object sender, RoutedEventArgs e)
        {
            WordObject wordObject = new WordObject();
            WordValuePage newItem = new WordValuePage(wordObject);
            m_listView.Items.Add(newItem);
            m_listView.SelectedItem = newItem;
            TargetArray.Add(wordObject);
        }

        private void OnMenuScriptClicked(object sender, RoutedEventArgs e)
        {
            ScriptObject scriptObject = new ScriptObject();
            ScriptObjectUI newItem = new ScriptObjectUI(scriptObject);
            m_listView.Items.Add(newItem);
            m_listView.SelectedItem = newItem;
            TargetArray.Add(scriptObject);
        }
    }
}