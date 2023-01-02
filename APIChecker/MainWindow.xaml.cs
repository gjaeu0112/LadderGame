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

        private void CreateRequestHTTP()
        {
            ResultBox.Text = "";
            StatusBox.Text = "";

            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(PathBox.Text.ToString());
            myWebRequest.Method = MethodType.Text.ToString();

            switch (myWebRequest.Method)
            {
                case "GET":
                    foreach (var currentUserInput in HeadersPanel.Children)
                    {
                        if (!(currentUserInput is UserInputBox)) continue;
                        if ((currentUserInput as UserInputBox).NameBox.Text.ToString() == "") continue;
                        myWebRequest.Headers.Add((currentUserInput as UserInputBox).NameBox.Text.ToString(), (currentUserInput as UserInputBox).ValueBox.Text.ToString());
                    }
                    break;

                case "POST":
                    foreach (var currentUserInput in HeadersPanel.Children)
                    {
                        if (!(currentUserInput is UserInputBox)) continue;
                        if ((currentUserInput as UserInputBox).NameBox.Text.ToString() == "") continue;
                        myWebRequest.Headers.Add((currentUserInput as UserInputBox).NameBox.Text.ToString(), (currentUserInput as UserInputBox).ValueBox.Text.ToString());
                    }

                    foreach (var currentUserInput in ParametersPanel.Children)
                    {
                        if (!(currentUserInput is UserInputBox)) continue;
                        myWebRequest.Headers.Add((currentUserInput as UserInputBox).NameBox.Text.ToString(), (currentUserInput as UserInputBox).ValueBox.Text.ToString());
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
