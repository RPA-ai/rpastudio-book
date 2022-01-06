using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfMVVMApplication.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Window _view;

        private List<SearchItemViewModel> _peopleList = new List<SearchItemViewModel>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }


        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载事件完成后触发
        /// </summary>
        public RelayCommand<RoutedEventArgs> LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        _view = (Window)p.Source;
                    }));
            }
        }


        private RelayCommand _closeCommand;

        /// <summary>
        /// Gets the CloseCommand.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand
                    ?? (_closeCommand = new RelayCommand(
                    () =>
                    {
                        _view.Close();
                    }));
            }
        }



        /// <summary>
        /// 姓名
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _nameProperty = "";

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _nameProperty;
            }

            set
            {
                if (_nameProperty == value)
                {
                    return;
                }

                _nameProperty = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }


        /// <summary>
        /// 是否为男性
        /// </summary>
        public const string IsMalePropertyName = "IsMale";

        private bool _isMaleProperty = true;

        /// <summary>
        /// Sets and gets the IsMale property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsMale
        {
            get
            {
                return _isMaleProperty;
            }

            set
            {
                if (_isMaleProperty == value)
                {
                    return;
                }

                _isMaleProperty = value;
                RaisePropertyChanged(IsMalePropertyName);
            }
        }


        /// <summary>
        /// 生日
        /// </summary>
        public const string BirthdayDatePropertyName = "BirthdayDate";

        private DateTime _birthdayDateProperty = DateTime.Today;

        /// <summary>
        /// Sets and gets the BirthdayDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime BirthdayDate
        {
            get
            {
                return _birthdayDateProperty;
            }

            set
            {
                if (_birthdayDateProperty == value)
                {
                    return;
                }

                _birthdayDateProperty = value;
                RaisePropertyChanged(BirthdayDatePropertyName);
            }
        }


        /// <summary>
        /// 爱好
        /// </summary>
        public const string HobbyPropertyName = "Hobby";

        private string _hobbyProperty = "";

        /// <summary>
        /// Sets and gets the Hobby property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Hobby
        {
            get
            {
                return _hobbyProperty;
            }

            set
            {
                if (_hobbyProperty == value)
                {
                    return;
                }

                _hobbyProperty = value;
                RaisePropertyChanged(HobbyPropertyName);
            }
        }





        /// <summary>
        /// 是否显示搜索界面
        /// </summary>
        public const string ShowSearchViewPropertyName = "ShowSearchView";

        private bool _showSearchViewProperty = false;

        /// <summary>
        /// Sets and gets the ShowSearchView property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool ShowSearchView
        {
            get
            {
                return _showSearchViewProperty;
            }

            set
            {
                if (_showSearchViewProperty == value)
                {
                    return;
                }

                _showSearchViewProperty = value;
                RaisePropertyChanged(ShowSearchViewPropertyName);
            }
        }


        
        private RelayCommand _showSearchViewCommand;

        /// <summary>
        /// 显示搜索界面命令
        /// </summary>
        public RelayCommand ShowSearchViewCommand
        {
            get
            {
                return _showSearchViewCommand
                    ?? (_showSearchViewCommand = new RelayCommand(
                    () =>
                    {
                        ShowSearchView = true;
                    }));
            }
        }


        private RelayCommand _hideSearchViewCommand;

        /// <summary>
        /// 隐藏搜索界面命令
        /// </summary>
        public RelayCommand HideSearchViewCommand
        {
            get
            {
                return _hideSearchViewCommand
                    ?? (_hideSearchViewCommand = new RelayCommand(
                    () =>
                    {
                        ShowSearchView = false;
                    }));
            }
        }


        /// <summary>
        /// 搜索结果
        /// </summary>
        public const string SearchResultsPropertyName = "SearchResults";

        private ObservableCollection<SearchItemViewModel> _searchResultsProperty = new ObservableCollection<SearchItemViewModel>();

        /// <summary>
        /// Sets and gets the SearchResults property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SearchItemViewModel> SearchResults
        {
            get
            {
                return _searchResultsProperty;
            }

            set
            {
                if (_searchResultsProperty == value)
                {
                    return;
                }

                _searchResultsProperty = value;
                RaisePropertyChanged(SearchResultsPropertyName);
            }
        }



        private RelayCommand _addCommand;

        /// <summary>
        /// 新增命令
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(
                    () =>
                    {
                        //新增当前人员信息
                        SearchItemViewModel item = new SearchItemViewModel();
                        item.Name = Name;
                        item.IsMale = IsMale;
                        item.Birthday = BirthdayDate.ToLongDateString();
                        item.Hobby = Hobby;

                        _peopleList.Add(item);

                        //重置人员信息，以便录入新数据

                        Name = "";
                        IsMale = true;
                        BirthdayDate = DateTime.Today;
                        Hobby = "";

                        //重新搜索一下
                        DoSearch(SearchText);
                    },
                    () => !string.IsNullOrEmpty(Name.Trim())));
            }
        }




        /// <summary>
        /// 搜索内容
        /// </summary>
        public const string SearchTextPropertyName = "SearchText";

        private string _searchTextProperty = "";

        /// <summary>
        /// Sets and gets the SearchText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SearchText
        {
            get
            {
                return _searchTextProperty;
            }

            set
            {
                if (_searchTextProperty == value)
                {
                    return;
                }

                _searchTextProperty = value;
                RaisePropertyChanged(SearchTextPropertyName);

                DoSearch(value);
            }
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        /// <param name="value">搜索内容</param>
        private void DoSearch(string value)
        {
            value = value.Trim();

            SearchResults.Clear();

            foreach (var people in _peopleList)
            {
                if(people.Name.Contains(value) || people.Hobby.Contains(value))
                {
                    SearchResults.Add(people);
                }
            }
        }
    }
}