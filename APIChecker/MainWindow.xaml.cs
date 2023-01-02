using System;
using System.IO;
using System.Net;
using System.Windows;

namespace APIChecker
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RequestSendButton.Click += RequestSendButton_Click;

        }
        public WebClient client = new WebClient();
        private void RequestSendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateRequestHTTP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void CreateRequest()
        {
            ResultBox.Text = "";
            StatusBox.Text = "";

            WebRequest myWebRequest = WebRequest.Create(PathBox.Text.ToString());
            myWebRequest.Method = MethodType.Text.ToString();


            switch (myWebRequest.Method)
            {
                case "GET":
                    MakeRequestHeader(myWebRequest);
                    break;

                case "POST":
                    MakeRequestHeader(myWebRequest);

                    foreach (var currentUserInput in ParametersPanel.Children)
                    {
                        if (!(currentUserInput is UserInputBox)) continue;
                        client.UploadString((currentUserInput as UserInputBox).NameBox.Text.ToString(), (currentUserInput as UserInputBox).ValueBox.Text.ToString());
                    }
                    break;
            }

            using (WebResponse webResponse = myWebRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    ResultBox.Text = reader.ReadToEnd();
                }
            }
        }

        private void CreateRequestHTTP()
        {
            ResultBox.Text = "";
            StatusBox.Text = "";

            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(PathBox.Text.ToString());
            myWebRequest.Method = MethodType.Text.ToString();

            switch (myWebRequest.Method)
            {
                case "GET":
                    MakeRequestHeader(myWebRequest);
                    break;

                case "POST":
                    MakeRequestHeader(myWebRequest);

                    foreach (var currentUserInput in ParametersPanel.Children)
                    {
                        if (!(currentUserInput is UserInputBox)) continue;
                    }
                    break;
            }

            using (HttpWebResponse resp = (HttpWebResponse)myWebRequest.GetResponse())
            {
                HttpStatusCode respStatus = resp.StatusCode;
                StatusBox.Text = respStatus.ToString();
                using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                {
                    ResultBox.Text = reader.ReadToEnd();
                }
            }
        }

        private void MakeRequestHeader(WebRequest myWebRequest)
        {
            foreach (var currentUserInput in HeadersPanel.Children)
            {
                if (!(currentUserInput is UserInputBox)) continue;
                if ((currentUserInput as UserInputBox).NameBox.Text.ToString() == "") continue;
                myWebRequest.Headers.Add((currentUserInput as UserInputBox).NameBox.Text.ToString(), (currentUserInput as UserInputBox).ValueBox.Text.ToString());
            }
        }

        private void AddHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            HeadersPanel.Children.Add(new UserInputBox());
        }
        private void AddParameterButton_Click(object sender, RoutedEventArgs e)
        {
            ParametersPanel.Children.Add(new UserInputBox());
        }
    }
}
