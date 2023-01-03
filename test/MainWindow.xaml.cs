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
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "I8eNcaC4YF3PueAm0n7u");
            request.Headers.Add("X-Naver-Client-Secret", "2SIClCjQ8J");
            request.Method = "POST";
            string query = "오늘 날씨는 어떻습니까?";
            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=ko&target=ko&text=" + query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            Console.WriteLine(text);

            url = "https://openapi.naver.com/v1/papago/detectLangs";
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "ar5rHu2ow2bT2jHC086v");
            request.Headers.Add("X-Naver-Client-Secret", "qIbmM7KVJu");
            request.Method = "POST";
            query = "오늘 날씨는 어떻습니까?";
            byteDataParams = Encoding.UTF8.GetBytes("query=" + query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            response = (HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            Console.WriteLine(text);
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