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
    class NumberValidate : ValidationRule
    {
        private bool _isempty;
        private int _min;
        private int _max;
        private string _title;

        public bool IsEmpty
        {
            set { _isempty = value;}
            get { return _isempty;}
        }
        public int Max
        {
            set { _max = value; }
            get { return _max;  }
        }
        public int Min
        {
            set { _min = value; }
            get { return _min; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public NumberValidate()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int NumberLen = 0;
            long Number = 0;
            try
            {
                NumberLen = ((string)value).Trim().Length;
                if (NumberLen <= 0 && IsEmpty == false)
                    return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);
                if (NumberLen > 0)
                    Number = long.Parse((string)value);
            }
            catch
            {
                return new ValidationResult(false, Messages.ValueIsNumberStart + Title + Messages.ValueIsNumberEnd);
            }

            if (NumberLen < Min && NumberLen>0)
                return new ValidationResult(false, Messages.ValueNumberTooShortStart + Title + Messages.ValueNumberTooShortMiddle + Min + Messages.ValueNumberTooShortEnd);

            if (NumberLen > Max)
                return new ValidationResult(false, Messages.ValueNumberTooLongStart + Title + Messages.ValueNumberTooLongMiddle + Max + Messages.ValueNumberTooLongEnd);
            else
                return new ValidationResult(true, null);

        }
    }
}
