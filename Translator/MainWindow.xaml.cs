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
            Loaded += Translator_Opening;
            Closing += Translator_Closing;
        }

        #region ProgramSetting
        private void Translator_Opening(object sender, RoutedEventArgs e)
        {
            // TODO: 언어 세팅이 바뀌질 않음
            SourceTextBox.FontSize = int.Parse(Properties.Settings.Default["LeftFontSize"].ToString());
            TargetTextBox.FontSize = int.Parse(Properties.Settings.Default["RightFontSize"].ToString());
            TranslateFromSelectBox.Text = Properties.Settings.Default["LeftTransLang"].ToString();
            TranslateToSelectBox.Text = Properties.Settings.Default["RightTransLang"].ToString();
            SourceTextBox.Text = Properties.Settings.Default["LeftTransResult"].ToString();
            TargetTextBox.Text = Properties.Settings.Default["RightTransResult"].ToString();
        }
        private void Translator_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default["LeftFontSize"] = SourceTextBox.FontSize.ToString();
            Properties.Settings.Default["RightFontSize"] = TargetTextBox.FontSize.ToString();
            Properties.Settings.Default["LeftTransLang"] = TranslateFromSelectBox.Text.ToString();
            Properties.Settings.Default["RightTransLang"] = TranslateToSelectBox.Text.ToString();
            Properties.Settings.Default["LeftTransResult"] = SourceTextBox.Text.ToString();
            Properties.Settings.Default["RightTransResult"] = TargetTextBox.Text.ToString();
            Properties.Settings.Default.Save();
        }

        #endregion
        private void TranslateTriggerButtonClick(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = GetHttpWebRequest(TranslateAPIType.Papago);
            if (request == null)
            {
                MessageBox.Show("잘못된 요청입니다.");
                return;
            }

            string sourceLanugageData = GetLanguageData(TranslateFromSelectBox.Text.ToString(), LanguageBoxType.source);
            string targetLanguageData = GetLanguageData(TranslateToSelectBox.Text.ToString(), LanguageBoxType.target);
            if (sourceLanugageData == targetLanguageData)
            {
                targetLanguageData = GetLanguageData("", LanguageBoxType.target);
            }

            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + sourceLanugageData + "&target=" + targetLanguageData + "&text=" + SourceTextBox.Text.ToString());
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;

            try
            {
                using (Stream st = request.GetRequestStream())
                {
                    st.Write(byteDataParams, 0, byteDataParams.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            JObject jObject = JObject.Parse(GetWebResponse(request));
            TargetTextBox.Text = jObject["message"]["result"]["translatedText"].ToString();
        }
        #region Get-Methods
        private static string GetWebResponse(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            return text;
        }
        private HttpWebRequest GetHttpWebRequest(TranslateAPIType whichAPI)
        {
            HttpWebRequest request = null;
            string url;
            switch (whichAPI)
            {
                //TODO: API추가하기
                case TranslateAPIType.Papago:
                    url = "https://openapi.naver.com/v1/papago/n2mt";
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Headers.Add("X-Naver-Client-Id", "I8eNcaC4YF3PueAm0n7u");
                    request.Headers.Add("X-Naver-Client-Secret", "2SIClCjQ8J");
                    request.Method = "POST";
                    break;
                case TranslateAPIType.PapagoLangDetect:
                    url = "https://openapi.naver.com/v1/papago/detectLangs";
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Headers.Add("X-Naver-Client-Id", "ar5rHu2ow2bT2jHC086v");
                    request.Headers.Add("X-Naver-Client-Secret", "qIbmM7KVJu");
                    request.Method = "POST";
                    break;
                case TranslateAPIType.Google:
                case TranslateAPIType.Kakao:
                case TranslateAPIType.Bing:
                case TranslateAPIType.Watson:
                case TranslateAPIType.Yandex:
                case TranslateAPIType.Systran:
                case TranslateAPIType.Baidu:
                case TranslateAPIType.Youdao:
                case TranslateAPIType.Sogou:
                case TranslateAPIType.Tencent:
                case TranslateAPIType.Alibaba:
                default:
                    break;
            }
            return request;
        }
        private string GetLanguageData(string SelectedLanguage, LanguageBoxType languageBoxType)
        {
            if (SelectedLanguage != "" && SelectedLanguage != "Detect Language") { return SelectedLanguage; }    //콤보 박스를 선택하지 않았을 때
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
            HttpWebRequest request = GetHttpWebRequest(TranslateAPIType.PapagoLangDetect);

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
        #endregion
        #region ButtonClick
        private void MinimizeModeButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void PasteButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TargetTextBox.Text.ToString());
        }
        private void FontSizeDownTransFromButton(object sender, RoutedEventArgs e)
        {
            SourceTextBox.FontSize--;
        }
        private void FontSizeUpTransFromButton(object sender, RoutedEventArgs e)
        {
            SourceTextBox.FontSize++;
        }
        private void FontSizeDownTransToButton(object sender, RoutedEventArgs e)
        {
            TargetTextBox.FontSize--;
        }
        private void FontSizeUpTransToButton(object sender, RoutedEventArgs e)
        {
            TargetTextBox.FontSize++;
        }
        #endregion
    }
}
