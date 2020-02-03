using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using WaterAndWastewaterAuthorithy.Presentation;
using NCalc;
namespace WaterAndWastewaterAuthorithy.Validations
{
    class FormuleValidate : ValidationRule
    {
        private int _max;
        private string _title;

        public FormuleValidate()
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

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int TextLen = 0;
            string Formule = "";
            try
            {
                Formule = ((string)value).Trim();
                TextLen = Formule.Trim().Length;
            }
            catch
            {
                return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);
            }

            if ((TextLen <= 0))
                return new ValidationResult(false, Messages.ForceToInputStart + Title + Messages.ForceToInputEnd);

            if ((TextLen > Max))
                return new ValidationResult(false, Messages.ValueStringTooLongStart + Title + Messages.ValueStringTooLongMiddle + Max + Messages.ValueStringTooLongEnd);

            if (!CheckTheContent(Formule))
                return new ValidationResult(false, Messages.FormuleIsNotValid);
            if (!Range(Formule))
                return new ValidationResult(false, Messages.FormuleIsNotValid);

            return new ValidationResult(true, null);
        }
        private bool CheckTheContent(string Formule)
        {
            bool IsValid = true;
            try
            {
                string tmp = Formule;
                tmp = Formule.Replace('[', '(');
                tmp = Formule.Replace('[', ')');
                tmp = Formule.Replace('#', '0');
                Expression e = new Expression(tmp);
                IsValid = !e.HasErrors();
            }
            catch
            {
                IsValid = false;
            }
            return IsValid;
        }
        private bool Range(string Formule)
        {
            try
            {
                bool Open = false;
                bool Minus = false;
                string Ranges = "";
                for (int i = 0; i < Formule.Length; i++)
                {
                    if (Formule[i] == '[')
                    {
                        Open = true;
                        Minus = false;
                        continue;
                    }
                    if (Formule[i] == ']')
                    {
                        if (Minus == false)
                            return false;
                        Open = false;
                        Ranges += "-";
                        continue;
                    }
                    if (Open)
                    {
                        if (Formule[i] == '-')
                            Minus = true;
                        if (Formule[i] != ' ')
                            Ranges += Formule[i];
                    }
                }
                char[] SplitChar = new char[] { '-' };
                if (Ranges[Ranges.Length - 1] == '-')
                     Ranges = Ranges.Remove(Ranges.Length - 1, 1);
                string[] SNumbers = Ranges.Split(SplitChar);
                if (SNumbers[0] != "0")
                    return false;
                if (SNumbers[SNumbers.Length - 1] != "#")
                    return false;
                if (SNumbers.Length > 2)
                {
                    long[] Numbers = new long[SNumbers.Length - 2];

                    for (int i = 1; i < SNumbers.Length - 1; i++)
                    {
                        if (SNumbers[i][0] == '0')
                            return false;
                        Numbers[i - 1] = Convert.ToInt64(SNumbers[i]);
                    }

                    for (int i = 0; i < Numbers.Length; i += 2)
                    {
                        if ((i + 1) < Numbers.Length)
                            if (Numbers[i] != Numbers[i + 1])
                                return false;
                        if ((i + 2) < Numbers.Length)
                            if (Numbers[i] >= Numbers[i + 2])
                                return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
