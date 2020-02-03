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
    class PersianDateValidate : ValidationRule
    {
        private bool _isempty;
        private string _title;

        public bool IsEmpty
        {
            set { _isempty = value; }
            get { return _isempty; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public PersianDateValidate()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int ContentLen = 0;
            string Content = "";
            int Year = 0;
            int Month = 0;
            int Day = 0;
            try
            {
                Content = ((string)value).Trim();
                ContentLen = Content.Length;                
                if (ContentLen == 0 && IsEmpty == true)
                    return new ValidationResult(true, null);
                if (ContentLen == 0 && IsEmpty == false)
                    return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);
                string[] PDate = Content.Split(new char[] { '/' });
                if (PDate.Length != 3)
                    throw new Exception();
                if (PDate[0].Length != 4)
                    throw new Exception();
                if (PDate[1].Length != 2)
                    throw new Exception();
                if (PDate[2].Length != 2)
                    throw new Exception();
                Year = Convert.ToInt32(PDate[0]);
                Month = Convert.ToInt32(PDate[1]);
                Day = Convert.ToInt32(PDate[2]);
            }
            catch
            {
                return new ValidationResult(false, Messages.ValueIsDateStart + Title + Messages.ValueIsDateEnd);
            }

            if (!(Year >= 1300 && Year < 1500))
                return new ValidationResult(false, Messages.YearIsValid);

            if (!(Month >= 1 && Month <= 12))
                return new ValidationResult(false, Messages.MonthIsValid);

            if (!((Month >= 1 && Month <= 6 && Day >= 1 && Day <= 31) || (Month >= 7 && Month <= 12 && Day >= 1 && Day <= 30)))
                return new ValidationResult(false, Messages.DayIsValid);
            else
                return new ValidationResult(true, null);
        }
    }
}
