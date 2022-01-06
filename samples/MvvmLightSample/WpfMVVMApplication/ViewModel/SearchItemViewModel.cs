using GalaSoft.MvvmLight;

namespace WpfMVVMApplication.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SearchItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SearchItemViewModel class.
        /// </summary>
        public SearchItemViewModel()
        {
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
        /// 性别
        /// </summary>
        public string Sex
        {
            get
            {
                return IsMale ? "男" : "女"; ;
            }
        }



        /// <summary>
        /// 是否为男性
        /// </summary>
        public const string IsMalePropertyName = "IsMale";

        private bool _isMaleProperty = false;

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
        public const string BirthdayPropertyName = "Birthday";

        private string _birthdayProperty = "";

        /// <summary>
        /// Sets and gets the Birthday property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Birthday
        {
            get
            {
                return _birthdayProperty;
            }

            set
            {
                if (_birthdayProperty == value)
                {
                    return;
                }

                _birthdayProperty = value;
                RaisePropertyChanged(BirthdayPropertyName);
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




    }
}