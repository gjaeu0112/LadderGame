using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;

namespace Translator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TranslateTriggerButtonClick(object sender, RoutedEventArgs e)
        {
            string PapagoUrl = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PapagoUrl);
            request.Headers.Add("X-Naver-Client-Id", "I8eNcaC4YF3PueAm0n7u");
            request.Headers.Add("X-Naver-Client-Secret", "2SIClCjQ8J");
            request.Method = "POST";

            string query = TranslateFromSelectBox.Text.ToString();

            string translateTo = "ko";
            if (TranslateToSelectBox.Text.ToString() == "")
            {
                string defaultLanguage = DetectLanguage(TranslateFromSelectBox.Text.ToString());

            }
            else
            {
                translateTo = GetTranslateTo(translateTo);
            }

            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + translateTo + "&target=en&text=" + query);
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
        }

        private string GetTranslateTo(string translateTo)
        {
            switch (TranslateToSelectBox.Text.ToString())
            {
                case "Korean":
                    translateTo = "ko";
                    break;
                case "English":
                    translateTo = "en";
                    break;
                case "Japanese":
                    translateTo = "ja";
                    break;
                case "Chinese":
                    translateTo = "ch";
                    break;
            }

            return translateTo;
        }

        private string DetectLanguage(string userInputString)
        {
            string PapagoLanguageDetectUrl = "https://openapi.naver.com/v1/papago/detectLangs";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PapagoLanguageDetectUrl);
            request.Headers.Add("X-Naver-Client-Id", "ar5rHu2ow2bT2jHC086v");
            request.Headers.Add("X-Naver-Client-Secret", "qIbmM7KVJu");
            request.Method = "POST";

            string query = userInputString;
            byte[] byteDataParams = Encoding.UTF8.GetBytes("query=" + query);
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

            return text;
        }

        private void MinimizeModeButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
