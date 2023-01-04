using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AdonisUI;

namespace Savannah
{
    public partial class MainWindow : Window
    {
        List<Info> infos;
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\Users\user\Downloads\SM_OM_Sample_20220830";
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);
                FileReadPathTextBox.Text = fbd.SelectedPath;
                infos = new List<Info>();
                DirectoryInfo(dirInfo, 0, infos);
                FilterFind.DataContext = infos;
                FilterReplace.DataContext = infos;
                RuntimeFilter.DataContext = infos;
                foreach(var info in infos)
                {
                    info.RenameTo = info.Name;
                }
                var _itemSourceList = new CollectionViewSource() { Source = infos };
                ICollectionView Itemlist = _itemSourceList.View;
                var yourCostumFilter = new Predicate<object>(item => ((Info)item).Name.Contains(RuntimeFilter.Text));
                Itemlist.Filter = yourCostumFilter;
                PreviewGrid.DataContext = PreviewGrid.ItemsSource = Itemlist;
            }
        }
        public string PrintAll()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("현재 이름,확장자,바뀔 이름,경로");
            foreach (Info info in infos)
            {
                stringBuilder.AppendLine(info.PrintInfoForCSV());
            }
            string result = stringBuilder.ToString();
            return result;
        }
        public void DirectoryInfo(DirectoryInfo directoryInfo, int depth, List<Info> infos)
        {
            DirectoryInfo[] directoryInfos = null; 
            FileInfo[] fileNames = null;
            try
            {
                directoryInfos = directoryInfo.GetDirectories();
                fileNames = directoryInfo.GetFiles();
            }
            catch (Exception) {

            }
            if (directoryInfos != null && fileNames != null)
            {
                foreach (DirectoryInfo info in directoryInfos)
                {
                    Info info1 = new Info(info, null);
                    infos.Add(info1);
                    DirectoryInfo(info, depth + 1, infos);
                }
                foreach (FileInfo info in fileNames)
                {
                    Info info1 = new Info(null, info);
                    infos.Add(info1);
                }
            }
        }
        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = ".csv";
            var current = this.infos;
            saveFileDialog.FileName = "Untitle";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                File.WriteAllText(saveFileDialog.FileName, PrintAll(), Encoding.UTF8);
            }
        }
        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (infos == null) return;
            int renameWorkCount = 0;
            foreach(var current in this.infos)
            {
                current.Rename();
            }
            System.Windows.MessageBox.Show($"총 {renameWorkCount}개의 파일명이 수정되었습니다.");
        }
        private void FileNameFindAndReplace_Click(object sender, RoutedEventArgs e)
        {
            if (infos == null) return;
            int renameWorkCount = 0;
            foreach (var current in this.infos)
            {
                current.Replace(FilterFind.Text,FilterReplace.Text);
            }
            //this.PreviewGrid.ItemsSource/
            System.Windows.MessageBox.Show($"총 {renameWorkCount}개의 파일명이 수정되었습니다.");
        }
        private void PrefixButton_Click(object sender, RoutedEventArgs e)
        {
            if (infos == null) return;
            InputPopup inputPopup = new InputPopup();
            inputPopup.Width = 300;
            inputPopup.Height = 200;
            inputPopup.ShowDialog();
            if (inputPopup.DialogResult == true)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"입력하신 접두사는 {inputPopup.InputPopupText.Text}입니다");
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    int renameWorkCount = 0;
                    foreach(var current2 in PreviewGrid.SelectedItems)
                    {
                        (current2 as Info).PrefixAdd(inputPopup);
                    }
                    System.Windows.MessageBox.Show($"총 {renameWorkCount}개의 파일명이 수정되었습니다.");
                }
            }
        }
        private void SuffixButton_Click(object sender, RoutedEventArgs e)
        {
            if (infos == null) return;
            InputPopup inputPopup = new InputPopup();
            inputPopup.Width = 300;
            inputPopup.Height = 200;
            inputPopup.ShowDialog();
            if (inputPopup.DialogResult == true)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"입력하신 접미사는 {inputPopup.InputPopupText.Text}입니다");
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    int renameWorkCount = 0;
                    foreach (var current in this.infos)
                    {
                        current.SuffixAdd(inputPopup);
                    }
                    System.Windows.MessageBox.Show($"총 {renameWorkCount}개의 파일명이 수정되었습니다.");
                }
            }
        }
        private void TrimButton_Click(object sender, RoutedEventArgs e)
        {
            if (infos == null) return;
            InputPopup inputPopup = new InputPopup();
            inputPopup.Width = 300;
            inputPopup.Height = 200;
            inputPopup.RadioForTrimm.Visibility = Visibility.Visible;
            inputPopup.ShowDialog();
            if(inputPopup.DialogResult == true)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"지울 문자의 개수는 {inputPopup.InputPopupText.Text}입니다.");
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    int renameWorkCount = 0;
                    if (this.infos != null)
                    {
                        foreach (var current in this.infos)
                        {
                            current.Trimm(inputPopup);
                        }
                    }
                    System.Windows.MessageBox.Show($"총 {renameWorkCount}개의 파일명이 수정되었습니다.");
                }
            }
            inputPopup.RadioForTrimm.Visibility=Visibility.Collapsed;
        }
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new OpenFileDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && fbd.CheckPathExists)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(fbd.FileName);
                FileReadPathTextBox.Text = fbd.FileName;
                infos = new List<Info>();
                //DirectoryInfo(dirInfo, 0, infos); 수정 필요!####
                ImportButton_Click(fbd, infos);
                FilterFind.DataContext = infos;
                FilterReplace.DataContext = infos;
                RuntimeFilter.DataContext = infos;
                foreach (var info in infos)
                {
                    info.RenameTo = info.Name;
                }
                var _itemSourceList = new CollectionViewSource() { Source = infos };
                ICollectionView Itemlist = _itemSourceList.View;
                var yourCostumFilter = new Predicate<object>(item => ((Info)item).Name.Contains(RuntimeFilter.Text));
                Itemlist.Filter = yourCostumFilter;
                PreviewGrid.DataContext = PreviewGrid.ItemsSource = Itemlist;
            }
        }
        public void ImportButton_Click(OpenFileDialog fbd, List<Info> infos)
        {
            StreamReader sr = new StreamReader(fbd.FileName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] data = line.Split(',');
            }
        }
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {

            ResourceLocator.SetColorScheme(System.Windows.Application.Current.Resources, _isDark ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);

            _isDark = !_isDark;
        }

        private bool _isDark;

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            ResourceLocator.SetColorScheme(System.Windows.Application.Current.Resources, _isDark ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);

            _isDark = !_isDark;
        }
        */
    }
}
