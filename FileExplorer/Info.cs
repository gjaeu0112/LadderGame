using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Savannah
{
    public partial class MainWindow
    {
        public class Info : INotifyPropertyChanged
        {
            public static int MethodCounter = 0;
            public Info(DirectoryInfo directoryInfo, FileInfo fileInfo)
            {
                this.DirectoryInfo = directoryInfo;
                this.FileInfo = fileInfo;
                this.FilePath = directoryInfo == null ? fileInfo.FullName : directoryInfo.FullName;
                this.ParentFilePath = fileInfo != null ? fileInfo.Directory.FullName : null;
                if (fileInfo != null)
                {
                    this.Extension = fileInfo.Extension;
                }
                this.NameLength = Name.Length.ToString();
            }
            public DirectoryInfo DirectoryInfo { get; set; }
            public FileInfo FileInfo { get; set; }
            public int Depth { get; set; }
            public string RenameTo {
                get { return renameTo == null ? "" : renameTo ; }
                set { renameTo = value; OnPropertyChanged(); }
            }
            public string renameTo;
            public string Name { get { return DirectoryInfo != null ? DirectoryInfo.Name : Path.GetFileNameWithoutExtension(FileInfo.Name); } }
            public string Extension { get; set; }
            public string FilePath { get; set; }
            public string ParentFilePath { get; set; }
            public string NameLength { get; set; }
            public string PrintInfoForCSV()
            {
                return $"{Name},{(DirectoryInfo == null ? FileInfo.Extension : null)},{RenameTo},{(DirectoryInfo == null ? FileInfo.FullName :DirectoryInfo.FullName)}";
            }
            public void Rename()
            {
                if(FileInfo != null)
                {
                    if (RenameTo != Name)
                    {
                        FileInfo.MoveTo(Path.Combine(FileInfo.Directory.FullName, $"{RenameTo}{FileInfo.Extension}"));
                        MethodCounter++;
                    }
                }
            }
            public void Replace(string filterFind, string filterReplace)
            {
                if (Name.Contains(filterFind))
                {
                    if (!string.IsNullOrEmpty(filterFind))
                    {
                        RenameTo = RenameTo.Replace(filterFind, filterReplace);
                        MethodCounter++;
                    }
                }

            }
            public void PrefixAdd(InputPopup prefix)
            {
                if (FileInfo != null)
                {
                    RenameTo = $"{prefix.InputPopupText.Text}{RenameTo}";
                    MethodCounter++;
                }
            }
            public void SuffixAdd(InputPopup suffix)
            {
                if (FileInfo != null)
                {
                    RenameTo = $"{RenameTo}{suffix.InputPopupText.Text}";
                    MethodCounter++;
                }
            }
            public void Trimm(InputPopup trim)
            {
                if (FileInfo != null)
                {
                    if (trim.TrimFromForeButton.IsChecked == true)
                    {
                        if (RenameTo.Length > int.Parse(trim.InputPopupText.Text))
                        {
                            RenameTo = RenameTo.Substring(int.Parse(trim.InputPopupText.Text));
                        }
                        else
                        {
                            RenameTo = "";
                        }
                    }
                    else if(trim.TrimFromBackButton.IsChecked == true)
                    {
                        if (RenameTo.Length > int.Parse(trim.InputPopupText.Text))
                        {
                            RenameTo = RenameTo.Substring(0, RenameTo.Length - int.Parse(trim.InputPopupText.Text));
                        }
                        else
                        {
                            RenameTo = "";
                        }
                    }
                    MethodCounter++;
                }
            }
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
