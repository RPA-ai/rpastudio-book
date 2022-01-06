using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using NuGet;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using RPARobot.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPARobot.ViewModel
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
        private IServiceLocator _serviceLocator;

        private IRobotPathConfigService _robotPathConfigService;

        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ���������й�����
        /// </summary>
        private IRunManagerService _runManagerService { get; set; }

        /// <summary>
        /// ��Ӧ����ͼ
        /// </summary>
        private Window _view { get; set; }


        /// <summary>
        /// PackagesĿ¼λ��
        /// </summary>
        private string _programDataPackagesDir { get; set; }

        /// <summary>
        /// InstalledPackagesĿ¼λ��
        /// </summary>
        private string _programDataInstalledPackagesDir { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        private IPackageService _packageService { get; set; }


        private ConcurrentDictionary<string, bool> _packageItemEnableDict = new ConcurrentDictionary<string, bool>();


        private StartupViewModel _startupViewModel;


        public string ProgramDataPackagesDir
        {
            get
            {
                return _programDataPackagesDir;
            }
        }

        public string ProgramDataInstalledPackagesDir
        {
            get
            {
                return _programDataInstalledPackagesDir;
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _runManagerService = _serviceLocator.ResolveType<IRunManagerService>();

            _runManagerService.BeginRunEvent += _runManagerService_BeginRunEvent;
            _runManagerService.EndRunEvent += _runManagerService_EndRunEvent;

            Common.InvokeAsyncOnUI(() =>
            {
                _startupViewModel = _serviceLocator.ResolveType<StartupViewModel>();
                _packageService = _serviceLocator.ResolveType<IPackageService>();

                _robotPathConfigService = _serviceLocator.ResolveType<IRobotPathConfigService>();
                _robotPathConfigService.InitDirs();

                _programDataPackagesDir = _robotPathConfigService.ProgramDataPackagesDir;
                _programDataInstalledPackagesDir = _robotPathConfigService.ProgramDataInstalledPackagesDir;
            });
        }

        private void _runManagerService_BeginRunEvent(object sender, EventArgs e)
        {
            var obj = sender as IRunManagerService;

            //�������п�ʼ����
            SharedObject.Instance.Output(SharedObject.enOutputType.Trace, "�������п�ʼ");

            
            Common.InvokeOnUI(() =>
            {
                _view.Hide();

                obj.PackageItem.IsRunning = true;

                IsWorkflowRunning = true;
                WorkflowRunningName = obj.PackageItem.Name;
                WorkflowRunningToolTip = obj.PackageItem.ToolTip;
                WorkflowRunningStatus = "��������";
            });
        }

        private void _runManagerService_EndRunEvent(object sender, EventArgs e)
        {
            var obj = sender as IRunManagerService;

            SharedObject.Instance.Output(SharedObject.enOutputType.Trace, "�������н���"); //�������н���

            Common.InvokeOnUI(() =>
            {
                _view.Show();
                _view.Activate();

                obj.PackageItem.IsRunning = false;

                //�����п����б��Ѿ�ˢ�£�������Ҫ����IsRunning״̬��Ϊ�˷��㣬ȫ������
                foreach (var pkg in PackageItems)
                {
                    pkg.IsRunning = false;
                }

                IsWorkflowRunning = false;
                WorkflowRunningName = "";
                WorkflowRunningStatus = "";
            });
        }


        /// <summary>
        /// ˢ�����а��б�
        /// </summary>
        public void RefreshAllPackages()
        {
            PackageItems.Clear();

            var repo = PackageRepositoryFactory.Default.CreateRepository(_programDataPackagesDir);
            var pkgList = repo.GetPackages();

            var pkgSet = new SortedSet<string>();
            foreach (var pkg in pkgList)
            {
                //ͨ��setȥ��
                pkgSet.Add(pkg.Id);
            }

            Dictionary<string, IPackage> installedPkgDict = new Dictionary<string, IPackage>();

            var packageManager = new PackageManager(repo, _programDataInstalledPackagesDir);
            foreach (IPackage pkg in packageManager.LocalRepository.GetPackages())
            {
                installedPkgDict[pkg.Id] = pkg;//��¼��װ��InstalledPackagesĿ¼�µİ�װ�������ƺͶ�Ӧ�����汾��
            }

            foreach (var name in pkgSet)
            {
                try
                {
                    var item = _serviceLocator.ResolveType<PackageItemViewModel>();
                    item.Name = name;

                    var version = repo.FindPackagesById(name).Max(p => p.Version);
                    item.Version = version.ToString();

                    var pkgNameList = repo.FindPackagesById(name);
                    foreach (var i in pkgNameList)
                    {
                        item._versionList.Add(i.Version.ToString());
                    }

                    bool isNeedUpdate = false;
                    if (installedPkgDict.ContainsKey(item.Name))
                    {
                        var installedVer = installedPkgDict[item.Name].Version;
                        if (version > installedVer)
                        {
                            isNeedUpdate = true;
                        }
                    }
                    else
                    {
                        isNeedUpdate = true;
                    }
                    item.IsNeedUpdate = isNeedUpdate;

                    var pkg = repo.FindPackage(name, version);
                    item.Package = pkg;
                    var publishedTime = pkg.Published.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    string toolTip = "���ƣ�{0}\r\n�汾��{1}\r\n����˵����{2}\r\n��Ŀ������{3}\r\n����ʱ�䣺{4}";
                    toolTip = toolTip.Replace(@"\r\n", "\r\n");//���з�����
                    item.ToolTip = string.Format(toolTip, item.Name, item.Version, pkg.ReleaseNotes, pkg.Description, (publishedTime == null ? "δ֪" : publishedTime));

                    if (IsWorkflowRunning && item.Name == WorkflowRunningName)
                    {
                        item.IsRunning = true;//�����ǰ�ð������Ѿ������У���Ҫ����IsRunning
                    }

                    PackageItems.Add(item);
                }
                catch (Exception err)
                {
                    _logger.Debug($"��ȡ���� <{name}> ����Ϣ�����쳣�������̶�Ӧ��ĳ���汾���ܸ�ʽ�����⣬�쳣���飺" + err.ToString(), _logger);
                }
            }

            doSearch();
        }

        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// ���������ɺ󴥷�
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
                        RefreshAllPackages();
                    }));
            }
        }




       
        private RelayCommand _MouseLeftButtonDownCommand;

        /// <summary>
        /// ����������ʱ����
        /// </summary>
        public RelayCommand MouseLeftButtonDownCommand
        {
            get
            {
                return _MouseLeftButtonDownCommand
                    ?? (_MouseLeftButtonDownCommand = new RelayCommand(
                    () =>
                    {
                        //�������Ĳ���Ҳ���϶�������ʹ��
                        _view.DragMove();
                    }));
            }
        }

        private RelayCommand _activatedCommand;

        /// <summary>
        /// ���崦�ڼ���״̬ʱ����
        /// </summary>
        public RelayCommand ActivatedCommand
        {
            get
            {
                return _activatedCommand
                    ?? (_activatedCommand = new RelayCommand(
                    () =>
                    {
                        RefreshAllPackages();
                    }));
            }
        }



        private RelayCommand<System.ComponentModel.CancelEventArgs> _closingCommand;

        /// <summary>
        /// �������ڹر�ʱ����
        /// </summary>
        public RelayCommand<System.ComponentModel.CancelEventArgs> ClosingCommand
        {
            get
            {
                return _closingCommand
                    ?? (_closingCommand = new RelayCommand<System.ComponentModel.CancelEventArgs>(
                    e =>
                    {
                        e.Cancel = true;//���رմ���
                        _view.Hide();
                    }));
            }
        }


        private RelayCommand<DragEventArgs> _dropCommandCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<DragEventArgs> DropCommand
        {
            get
            {
                return _dropCommandCommand
                    ?? (_dropCommandCommand = new RelayCommand<DragEventArgs>(
                    e =>
                    {
                        if (e.Data.GetDataPresent(DataFormats.FileDrop))
                        {
                            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                            foreach (string fileFullPath in files)
                            {
                                string extension = System.IO.Path.GetExtension(fileFullPath);
                                if (extension.ToLower() == ".nupkg")
                                {
                                    processImportNupkgFile(fileFullPath);
                                }
                            }
                        }
                    }));
            }
        }

        /// <summary>
        /// ����nupkg��������
        /// </summary>
        /// <param name="file">nupkg��·��</param>
        private bool processImportNupkgFile(string fileFullPath)
        {
            //����file��PackagesDir

            //�����ж�PackagesDir����û���������ļ����еĻ�Ҫ��ʾ�û��Ƿ񸲸�
            var fileName = System.IO.Path.GetFileName(fileFullPath);

            var dstFileFullPath = _programDataPackagesDir + @"\" + fileName;
            if (System.IO.File.Exists(dstFileFullPath))
            {
                var ret = MessageBox.Show(App.Current.MainWindow, $"Ŀ��Ŀ¼��{_programDataPackagesDir}������ͬ���ļ���{fileName}�����Ƿ��滻��", "ѯ��", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (ret != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            try
            {
                System.IO.File.Copy(fileFullPath, dstFileFullPath, true);
                RefreshCommand.Execute(null);
                CommonMessageBox.ShowInformation($"���롰{fileName}���ɹ�");
            }
            catch (Exception err)
            {
                _logger.Debug(err);
                CommonMessageBox.ShowInformation($"���롰{fileName}�������쳣��");
                return false;
            }

            return true;
        }

        private RelayCommand _refreshCommand;

        /// <summary>
        /// ˢ��
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(
                    () =>
                    {
                        RefreshAllPackages();
                    }));
            }
        }


        private RelayCommand _viewLogsCommand;

        /// <summary>
        /// �鿴��־
        /// </summary>
        public RelayCommand ViewLogsCommand
        {
            get
            {
                return _viewLogsCommand
                    ?? (_viewLogsCommand = new RelayCommand(
                    () =>
                    {
                        //����־���ڵ�Ŀ¼
                        Common.LocateDirInExplorer(_robotPathConfigService.LogsDir);
                    }));
            }
        }


        private RelayCommand _aboutProductCommand;

        /// <summary>
        /// ���ڲ�Ʒ����
        /// </summary>
        public RelayCommand AboutProductCommand
        {
            get
            {
                return _aboutProductCommand
                    ?? (_aboutProductCommand = new RelayCommand(
                    () =>
                    {
                        if (!_startupViewModel.AboutWindow.IsVisible)
                        {
                            _startupViewModel.AboutWindow.Show();
                        }

                        _startupViewModel.AboutWindow.Activate();
                    }));
            }
        }





        /// <summary>
        /// The <see cref="PackageItems" /> property's name.
        /// </summary>
        public const string PackageItemsPropertyName = "PackageItems";

        private ObservableCollection<PackageItemViewModel> _packageItemsProperty = new ObservableCollection<PackageItemViewModel>();

        /// <summary>
        /// ���б���Ŀ
        /// </summary>
        public ObservableCollection<PackageItemViewModel> PackageItems
        {
            get
            {
                return _packageItemsProperty;
            }

            set
            {
                if (_packageItemsProperty == value)
                {
                    return;
                }

                _packageItemsProperty = value;
                RaisePropertyChanged(PackageItemsPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsSearchResultEmpty" /> property's name.
        /// </summary>
        public const string IsSearchResultEmptyPropertyName = "IsSearchResultEmpty";

        private bool _isSearchResultEmptyProperty = false;

        /// <summary>
        /// ��������Ƿ�Ϊ��
        /// </summary>
        public bool IsSearchResultEmpty
        {
            get
            {
                return _isSearchResultEmptyProperty;
            }

            set
            {
                if (_isSearchResultEmptyProperty == value)
                {
                    return;
                }

                _isSearchResultEmptyProperty = value;
                RaisePropertyChanged(IsSearchResultEmptyPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="SearchText" /> property's name.
        /// </summary>
        public const string SearchTextPropertyName = "SearchText";

        private string _searchTextProperty = "";

        /// <summary>
        /// �����ı�
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

                doSearch();
            }
        }

        /// <summary>
        /// ִ������
        /// </summary>
        private void doSearch()
        {
            var searchContent = SearchText.Trim();
            if (string.IsNullOrEmpty(searchContent))
            {
                //��ԭ��ʼ��ʾ
                foreach (var item in PackageItems)
                {
                    item.IsSearching = false;
                }

                foreach (var item in PackageItems)
                {
                    item.SearchText = searchContent;
                }

                IsSearchResultEmpty = false;
            }
            else
            {
                //��������������ʾ

                foreach (var item in PackageItems)
                {
                    item.IsSearching = true;
                }

                //Ԥ��ȫ����Ϊ��ƥ��
                foreach (var item in PackageItems)
                {
                    item.IsMatch = false;
                }


                foreach (var item in PackageItems)
                {
                    item.ApplyCriteria(searchContent);
                }

                IsSearchResultEmpty = true;
                foreach (var item in PackageItems)
                {
                    if (item.IsMatch)
                    {
                        IsSearchResultEmpty = false;
                        break;
                    }
                }

            }
        }




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The <see cref="IsWorkflowRunning" /> property's name.
        /// </summary>
        public const string IsWorkflowRunningPropertyName = "IsWorkflowRunning";

        private bool _isWorkflowRunningProperty = false;

        /// <summary>
        /// �������Ƿ���������
        /// </summary>
        public bool IsWorkflowRunning
        {
            get
            {
                return _isWorkflowRunningProperty;
            }

            set
            {
                if (_isWorkflowRunningProperty == value)
                {
                    return;
                }

                _isWorkflowRunningProperty = value;
                RaisePropertyChanged(IsWorkflowRunningPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="WorkflowRunningToolTip" /> property's name.
        /// </summary>
        public const string WorkflowRunningToolTipPropertyName = "WorkflowRunningToolTip";

        private string _workflowRunningToolTipProperty = "";

        /// <summary>
        /// ������������ʾ��Ϣ
        /// </summary>
        public string WorkflowRunningToolTip
        {
            get
            {
                return _workflowRunningToolTipProperty;
            }

            set
            {
                if (_workflowRunningToolTipProperty == value)
                {
                    return;
                }

                _workflowRunningToolTipProperty = value;
                RaisePropertyChanged(WorkflowRunningToolTipPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="WorkflowRunningName" /> property's name.
        /// </summary>
        public const string WorkflowRunningNamePropertyName = "WorkflowRunningName";

        private string _workflowRunningNameProperty = "";

        /// <summary>
        /// ��������������
        /// </summary>
        public string WorkflowRunningName
        {
            get
            {
                return _workflowRunningNameProperty;
            }

            set
            {
                if (_workflowRunningNameProperty == value)
                {
                    return;
                }

                _workflowRunningNameProperty = value;
                RaisePropertyChanged(WorkflowRunningNamePropertyName);
            }
        }





        /// <summary>
        /// The <see cref="WorkflowRunningStatus" /> property's name.
        /// </summary>
        public const string WorkflowRunningStatusPropertyName = "WorkflowRunningStatus";

        private string _workflowRunningStatusProperty = "";

        /// <summary>
        /// ����������״̬ 
        /// </summary>
        public string WorkflowRunningStatus
        {
            get
            {
                return _workflowRunningStatusProperty;
            }

            set
            {
                if (_workflowRunningStatusProperty == value)
                {
                    return;
                }

                _workflowRunningStatusProperty = value;
                RaisePropertyChanged(WorkflowRunningStatusPropertyName);
            }
        }




        private RelayCommand _stopCommand;

        /// <summary>
        /// ֹͣ����
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                    ?? (_stopCommand = new RelayCommand(
                    () =>
                    {
                        if (_runManagerService != null)
                        {
                            _runManagerService.Stop();
                        }
                    },
                    () => true));
            }
        }


    }
}