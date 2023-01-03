using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Savannah
{
    /// <summary>
    /// Prefix.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputPopup : Window
    {
        public InputPopup()
        {
            InitializeComponent();
        }
        private void InputPopupYesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void InputPopupNoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void TrimType_Checked(object sender, RoutedEventArgs e)
        {
            RadioForTrimm.Visibility = Visibility.Visible;
        }
        private void TrimType_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioForTrimm.Visibility = Visibility.Collapsed;
        }
    }
}
