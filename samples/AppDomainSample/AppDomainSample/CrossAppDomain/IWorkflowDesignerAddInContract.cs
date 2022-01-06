using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainSample.CrossAppDomain
{
    //跨域交互时的接口定义
    public interface IWorkflowDesignerAddInContract
    {
        INativeHandleContract GetActivitiesView();//获取组件视图

        INativeHandleContract GetDesignView();//获取设计器视图

        INativeHandleContract GetPropertyView();//获取属性视图

        void Load();//加载默认设计器内容

        string GetStringData();//跨域获取字符串数据

        UserDefineObject GetUserDefineObject();//跨域获取自定义数据
    }
}
