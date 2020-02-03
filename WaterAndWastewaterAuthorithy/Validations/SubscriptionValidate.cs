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
    class SubscriptionValidate : ValidationRule
    {        
        private int _min;
        private int _max;
        private string _title;
        private bool _IsEmpty = false;

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
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set { _IsEmpty = value; }
        }

        public SubscriptionValidate()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int ContentLen = 0;
            string[] Content;
            try
            {
                ContentLen = ((string)value).Trim().Length;
                if (_IsEmpty == true && ContentLen==0)
                    return new ValidationResult(true, null);

                if (ContentLen <= 0)
                    return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);
                if (ContentLen > 0)
                {
                    Content = ((string)value).Split(new char[] { '-' });
                    if (Content == null || Content.Length != 2)
                        return new ValidationResult(false, Messages.ValueIsSubscriptionNumberStart + Title + Messages.ValueIsSubscriptionNumberEnd);
                    long Number1 = Convert.ToInt64(Content[0]);
                    long Number2 = Convert.ToInt64(Content[1]);
                }
            }
            catch
            {
                return new ValidationResult(false, Messages.ValueIsSubscriptionNumberStart + Title + Messages.ValueIsSubscriptionNumberEnd);
            }

            if (ContentLen < Min && ContentLen > 0)
                return new ValidationResult(false, Messages.ValueNumberTooShortStart + Title + Messages.ValueNumberTooShortMiddle + Min + Messages.ValueNumberTooShortEnd);

            if (ContentLen > Max)
                return new ValidationResult(false, Messages.ValueNumberTooLongStart + Title + Messages.ValueNumberTooLongMiddle + Max + Messages.ValueNumberTooLongEnd);
            else
                return new ValidationResult(true, null);
        }
    }
}
