using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace WaterAndWastewaterAuthorithy.Presentation
{   
    public enum ControlsType
    {
        TextBoxNumber,
        TextBoxText,
        TextBoxFormul,
        ComboBox,
        RadioButton,
        CheckBox
    }
    public struct ControlsTab 
    {        
        public Control Next;
        public Control Prev;
        public ControlsType Type;
    }
    public struct ControlsValidate
    {
        public Control Control;
        public string Message;
        public Type TypeOfControl;
    }
    public enum MessageDialogButtons
    {
        Ok,
        YesNo        
    }
    public enum MessageDialogType
    {
        Information,
        Warning,
        Error
    }
    public struct CurrentPeriod
    {
        public int Id;
        public string Name;
        public string DateFrom;
        public string DateTo;
        public bool IsClosed;
        public int MonthPeriod;
    }
}
