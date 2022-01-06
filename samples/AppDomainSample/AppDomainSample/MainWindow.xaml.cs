using AppDomainSample.CrossAppDomain;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDomainSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkflowDesignerAddInProxy _workflowDesignerAddInProxy;
        private AppDomain _appDomain;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            AppDomainSetup adSetup = new AppDomainSetup
            {
                LoaderOptimization = LoaderOptimization.MultiDomain,//优化策略设置
            };

            _appDomain = AppDomain.CreateDomain("WorkflowDesignerDomain", null, adSetup);//创建一个名为WorkflowDesignerDomain的应用程序域

            //CreateInstanceAndUnwrap函数其实是两步，第一步为CreateInstance(在应用程序域中创建一个类的实例，该类是通过程序集名称和类型来唯一确定)
            //第二步是Unwrap展开，被展开的类，也就是WorkflowDesignerAddIn，必须继承自MarshalByRefObject以支持跨应用程序域边界使用类型
            _workflowDesignerAddInProxy = new WorkflowDesignerAddInProxy((IWorkflowDesignerAddInContract)_appDomain.CreateInstanceAndUnwrap(
                typeof(WorkflowDesignerAddIn).Assembly.FullName
                , typeof(WorkflowDesignerAddIn).FullName));

            _workflowDesignerAddInProxy.Load();

            ActivitiesBorder.Child = _workflowDesignerAddInProxy.GetActivitiesView();
            DesignerBorder.Child = _workflowDesignerAddInProxy.GetDesignView();
            PropertyBorder.Child = this._workflowDesignerAddInProxy.GetPropertyView();
        }

        private void UnloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_appDomain != null)
            {
                AppDomain.Unload(_appDomain);
                _appDomain = null;
            }
        }


        private void CrossAppDomainGetStringDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (_appDomain != null)
            {
                MessageBox.Show(_workflowDesignerAddInProxy.GetStringData());
            }
            else
            {
                MessageBox.Show("请先加载域后再获取数据！");
            }
        }

        private void CrossAppDomainGetUserDefineDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (_appDomain != null)
            {
                var obj = _workflowDesignerAddInProxy.GetUserDefineData();
                MessageBox.Show($"姓名：{obj.Name} \n年龄：{obj.Age}");
            }
            else
            {
                MessageBox.Show("请先加载域后再获取数据！");
            }
        }
    }
}
