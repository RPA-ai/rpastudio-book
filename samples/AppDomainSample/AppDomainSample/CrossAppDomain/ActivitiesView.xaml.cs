using System;
using System.Activities;
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

namespace AppDomainSample.CrossAppDomain
{
    /// <summary>
    /// ActivitiesView.xaml 的交互逻辑
    /// </summary>
    public partial class ActivitiesView : UserControl
    {
        public ActivitiesView()
        {
            InitializeComponent();
        }

        //组件拖拽处理
        private void ProcessActivityDrag(object sender, MouseEventArgs e, Type t)
        {
            //鼠标处于按下的状态才认为是拖拽
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var name = t.AssemblyQualifiedName;

                //必须为WorkflowItemTypeNameFormat格式才能让组件拖拽到设计器中
                DataObject data = new DataObject(System.Activities.Presentation.DragDropHelper.WorkflowItemTypeNameFormat, name);
                DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.All);
            }
        }

        private void Sequence_MouseMove(object sender, MouseEventArgs e)
        {
            ProcessActivityDrag(sender,e, typeof(System.Activities.Statements.Sequence));
        }

        private void WriteLine_MouseMove(object sender, MouseEventArgs e)
        {
            ProcessActivityDrag(sender, e, typeof(System.Activities.Statements.WriteLine));
        }

        private void Delay_MouseMove(object sender, MouseEventArgs e)
        {
            ProcessActivityDrag(sender, e, typeof(System.Activities.Statements.Delay));
        }


    }
}
