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
        enum APIMode { Translator, LangDetect }
        enum LanguageBoxType { source, target}

        private void TranslateTriggerButtonClick(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = GetHttpWebRequest(APIMode.Translator);

            string sourceLanugageData = GetLanguageData(TranslateFromSelectBox.Text.ToString(), LanguageBoxType.source);
            string targetLanguageData = GetLanguageData(TranslateToSelectBox.Text.ToString(), LanguageBoxType.target);

            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + sourceLanugageData + "&target="+ targetLanguageData + "&text=" + SourceTextBox.Text.ToString());
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;

            using (Stream st = request.GetRequestStream())
            {
                st.Write(byteDataParams, 0, byteDataParams.Length);
            }
            JObject jObject = JObject.Parse(GetWebResponse(request));
            TargetTextBox.Text = jObject["message"]["result"]["translatedText"].ToString();
        }
        private static string GetWebResponse(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            return text;
        }
        private HttpWebRequest GetHttpWebRequest(APIMode Api)
        {
            HttpWebRequest request = null;
            switch (Api)
            {
                case APIMode.Translator:
                    string PapagoUrl = "https://openapi.naver.com/v1/papago/n2mt";
                    request = (HttpWebRequest)WebRequest.Create(PapagoUrl);
                    request.Headers.Add("X-Naver-Client-Id", "I8eNcaC4YF3PueAm0n7u");
                    request.Headers.Add("X-Naver-Client-Secret", "2SIClCjQ8J");
                    request.Method = "POST";
                    break;
                case APIMode.LangDetect:
                    string PapagoLanguageDetectUrl = "https://openapi.naver.com/v1/papago/detectLangs";
                    request = (HttpWebRequest)WebRequest.Create(PapagoLanguageDetectUrl);
                    request.Headers.Add("X-Naver-Client-Id", "ar5rHu2ow2bT2jHC086v");
                    request.Headers.Add("X-Naver-Client-Secret", "qIbmM7KVJu");
                    request.Method = "POST";
                    break;
                default:
                    break;
            }
            return request;
        }
        private string GetLanguageData(string SelectedLanguage, LanguageBoxType languageBoxType)
        {
            if (SelectedLanguage != "") { return SelectedLanguage; }    //콤보 박스를 선택하지 않았을 때
            string LanguageData;
            if (languageBoxType == LanguageBoxType.source)              //번역할 언어를 선택하지 않았을 때
            {
                LanguageData = DetectLanguage(SourceTextBox.Text.ToString());
                TranslateFromSelectBox.Text = LanguageData;
                return LanguageData;
            }
            else //(languageBoxType == LanguageBoxType.target)          //번역될 언어를 선택하지 않았을 때
            {
                LanguageData = TranslateFromSelectBox.Text.ToString() != "ko" ? "ko" : "en";
                TranslateToSelectBox.Text = LanguageData;
                return LanguageData;
            }
        }
        private string DetectLanguage(string userInputString)
        {
            HttpWebRequest request = GetHttpWebRequest(APIMode.LangDetect);

            string query = userInputString;
            byte[] byteDataParams = Encoding.UTF8.GetBytes("query=" + query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;

            using (Stream st = request.GetRequestStream())
            {
                st.Write(byteDataParams, 0, byteDataParams.Length);
            }

            string text = GetWebResponse(request);

            JObject jObject = JObject.Parse(text);

            return jObject["langCode"].ToString();
        }
        private void MinimizeModeButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
