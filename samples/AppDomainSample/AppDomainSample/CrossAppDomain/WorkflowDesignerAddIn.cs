using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppDomainSample.CrossAppDomain
{
    /// <summary>
    /// 工作流设计器插件，在单独的AppDomain中创建，必须继承MarshalByRefObject来实现跨域的序列化传输
    /// </summary>
    public class WorkflowDesignerAddIn : MarshalByRefObject, IWorkflowDesignerAddInContract
    {
        private WorkflowDesigner designer;

        public WorkflowDesignerAddIn()
        {
            new DesignerMetadata().Register();//元数据注册
            designer = new WorkflowDesigner();
        }

        public System.AddIn.Contract.INativeHandleContract GetActivitiesView()
        {
            var view = new ActivitiesView();
            return FrameworkElementAdapters.ViewToContractAdapter(view);
        }

        public System.AddIn.Contract.INativeHandleContract GetDesignView()
        {
            return FrameworkElementAdapters.ViewToContractAdapter((FrameworkElement)this.designer.View);
        }


        public System.AddIn.Contract.INativeHandleContract GetPropertyView()
        {
            return FrameworkElementAdapters.ViewToContractAdapter((FrameworkElement)this.designer.PropertyInspectorView);
        }

        public void Load()
        {
            designer.Load(new ActivityBuilder { Implementation = new Sequence() });
        }

        public string GetStringData()
        {
            return "这是在名称为WorkflowDesignerDomain的应用程序域中的数据";
        }

        public UserDefineObject GetUserDefineObject()
        {
            var obj = new UserDefineObject();
            obj.Name = "张三";
            obj.Age = 33;

            return obj;
        }
    }
}
