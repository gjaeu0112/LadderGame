using Savannah.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savannah.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand TreeViewCommand { get; set; }
        public RelayCommand ButtonCmd { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public TreeViewModel TreeVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            TreeVM = new TreeViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVM; });
            TreeViewCommand = new RelayCommand(o => { CurrentView = TreeVM; });
            
            
            ButtonCmd = new RelayCommand(o => System.Windows.MessageBox.Show("버튼클릭"));
        }
    }
}
