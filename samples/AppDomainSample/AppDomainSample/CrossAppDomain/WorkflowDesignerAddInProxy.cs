using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppDomainSample.CrossAppDomain
{
    /// <summary>
    /// 跨域交互的代理类，在默认域和创建的AppDomain设计器域中进行界面跨域的显示
    /// </summary>
    public class WorkflowDesignerAddInProxy
    {
        IWorkflowDesignerAddInContract _contract;

        public WorkflowDesignerAddInProxy(IWorkflowDesignerAddInContract contract)
        {
            _contract = contract;
        }

        public FrameworkElement GetActivitiesView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(this._contract.GetActivitiesView());
        }

        public FrameworkElement GetDesignView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(this._contract.GetDesignView());
        }

        public FrameworkElement GetPropertyView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(this._contract.GetPropertyView());
        }

        public void Load()
        {
            _contract.Load();
        }

        public string GetStringData()
        {
            return _contract.GetStringData();
        }

        public UserDefineObject GetUserDefineData()
        {
            return _contract.GetUserDefineObject();
        }
    }
}
