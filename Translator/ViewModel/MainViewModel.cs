using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Core;
using Translator.Views;

namespace Translator.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand WithLogViewCommand;
        public RelayCommand CompareViewCommand;
        public RelayCommand MinimalViewCommand;
        public WithLogView WithLogV { get; set; }
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
            WithLogV = new WithLogView();
            CurrentView = WithLogV;

            WithLogViewCommand = new RelayCommand(o => { CurrentView = WithLogV; });
        }
    }
}
