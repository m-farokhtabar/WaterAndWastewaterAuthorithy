using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy.Validations
{
    class ComboValidate : ValidationRule
    {                
        private int _start;        
        private string _title;

        public int Start
        {
            set { _start = value; }
            get { return _start;  }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public ComboValidate()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {            
            long Number = 0;
            try
            {                
                Number = (Int32)value;
            }
            catch
            {
                return new ValidationResult(false, Messages.ForceToInputComboStart + Title + Messages.ForceToInputComboEnd);
            }

            if (Number < _start)
                return new ValidationResult(false, Messages.ForceToInputComboStart + Title + Messages.ForceToInputComboEnd);
            else
                return new ValidationResult(true, null);

        }
    }
}
