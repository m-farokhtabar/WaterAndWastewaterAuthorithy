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
    class StringValidate : ValidationRule
    {

        private int _max;
        private string _title;
        private bool _IsEmpty;

        public StringValidate()
        {
        }
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set { _IsEmpty = value; }
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int TextLen = 0;

            try
            {
                TextLen = ((string)value).Trim().Length;                
                if (_IsEmpty == true && TextLen==0)
                    return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);
            }

            if ((TextLen <= 0))
                return new ValidationResult(false,Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);

            if ((TextLen > Max))
                return new ValidationResult(false,Messages.ValueStringTooLongStart + Title + Messages.ValueStringTooLongMiddle + Max + Messages.ValueStringTooLongEnd);
            else
                return new ValidationResult(true, null);
        }
    }
}
