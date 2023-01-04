using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GridButton.Click += GridButton_Click;
        }
        private void GridButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    #region
    //try
    //{
    //    RegistryKey key = Registry.CurrentUser.OpenSubKey("DBHitek", true);
    //    if (key == null)
    //    {
    //        Registry.CurrentUser.CreateSubKey("DBHitek", true);
    //        key = Registry.CurrentUser.OpenSubKey("DBHitek", true);
    //    }
    //    key.SetValue("AdminConsoleModePopup", true);
    //}
    //catch (Exception ex)
    //{
    //    MessageBox.Show(ex.Message);
    //}
    #endregion
}