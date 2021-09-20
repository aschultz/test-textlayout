using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextLayout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new TestData();

            ApplicationView.PreferredLaunchViewSize = new Size(1000, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ZoomBox.ViewChanged += ZoomBox_ViewChanged;
            ZoomFactorText.LostFocus += ZoomFactorLabel_LostFocus; ;
        }

        private void ZoomBox_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                ZoomFactorText.Text = ZoomBox.ZoomFactor.ToString();
            }
        }

        private void ZoomFactorLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            float zoom;
            if (float.TryParse(ZoomFactorText.Text, out zoom) && ZoomBox.ZoomFactor != zoom && zoom > 0)
            {
                ZoomBox.ChangeView(null, null, zoom);
            }
        }
    }

    public class TestData : INotifyPropertyChanged
    {
        public int SmLineHeight { get { return m_SmLineHeight; } set { m_SmLineHeight = value; RaisePropertyChanged(); } }
        private int m_SmLineHeight = 10;
        public int MdLineHeight { get { return m_MdLineHeight; } set { m_MdLineHeight = value; RaisePropertyChanged(); } }
        private int m_MdLineHeight = 20;
        public int LgLineHeight { get { return m_LgLineHeight; } set { m_LgLineHeight = value; RaisePropertyChanged(); } }
        private int m_LgLineHeight = 30;
        public int TestFontSize { 
            get { return m_TestFontSize; } 
            set { 
                m_TestFontSize = value; 
                RaisePropertyChanged(); 
                RaisePropertyChanged("RowHeight"); 
                RaisePropertyChanged("StripesPoint");
                SmLineHeight = TestFontSize / 2;
                MdLineHeight = TestFontSize;
                LgLineHeight = (int)(TestFontSize * 1.5);
            }
        }
        private int m_TestFontSize = 20;
        public string TestText { get { return m_TestText; } set { m_TestText = value; RaisePropertyChanged(); } }
        private string m_TestText = "XǼhj\nXǼhj";

        public GridLength RowHeight { get { return new GridLength(m_TestFontSize * 3 + 2); } }
        public Point StripesPoint {  get { return new Point(0, m_TestFontSize / 2); } }

        public List<string> TextLineBoundsValues => Enum.GetNames(typeof(TextLineBounds)).ToList();
        public string TextLineBoundsSelection
        {
            get { return Enum.GetName(typeof(TextLineBounds), TextLineBoundsSetting); }
            set { TextLineBoundsSetting = (TextLineBounds)Enum.Parse(typeof(TextLineBounds), value); RaisePropertyChanged(); }
        }
        public TextLineBounds TextLineBoundsSetting
        {
            get { return m_TextLineBoundsSetting; }
            set { m_TextLineBoundsSetting = value; RaisePropertyChanged(); }
        }
        private TextLineBounds m_TextLineBoundsSetting = TextLineBounds.Tight;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberNameAttribute] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
