using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.DomainClasses;
using System.Windows.Input;
using System.Windows.Media;
using NCalc;
namespace WaterAndWastewaterAuthorithy.Presentation
{
    public enum BillsStatus
    {
        Billing = 0,
        Cancel = 1,
        ReceivableFull = 2,
        ReceivableNotMatch = 3
    }
    public static class Commons
    {
        public static Main MainWindow = null;
        public static Context Db = new Context();

        public static int CurrentYear;
        public static bool IsClosedCurrentYear;
        public static string Address;
        public static string BillMessage;
        public static string CompanyName;
        public static string Tel;
        public static int Vat;
        public static int Subscription;
        public static string WaterAndWastewaterAuthorityName;
        public static int TopRow = 100;
        public static CurrentPeriod CPeriod = new CurrentPeriod();

        //کلا این حالت برای این است که اگه فرمی را باز گذاشتیم که اطلاعات یک فرم دیگر در آن و به ان فرم می رویم و اطلاعات را تغییر می دهیم به فرم بعد بگوییم که رفرش کند
        /// <summary>
        /// First  Bool = Subscription        
        /// Second Bool = Billing Single
        /// Third  Bool = Bills Receivable
        /// Fourth Bool = Bills Cancelling
        /// Fifth bool = WaterMeter
        /// </summary>
        public static bool[] EditAccountType = new bool[] { false, false, false, false, false };
        /// <summary>
        /// First  Bool = Subscription        
        /// Second Bool = Billing Single
        /// Third  Bool = Bills Receivable
        /// Fourth Bool = Bills Cancelling
        /// </summary>
        public static bool[] EditCustomer = new bool[] { false, false, false, false, false };
        /// <summary>        
        /// First  Bool  = Billing Single
        /// Second Bool  = Bills Receivable
        /// Third  Bool  = Bills Cancelling
        /// </summary>
        public static bool[] EditSubScription = new bool[] { false, false, false, false, false };
        /// <summary>        
        /// First Bool   = Billing Single
        /// Second  Bool = Bills Receivable
        /// Third Bool   = Bills Cancelling
        /// </summary>
        public static bool[] EditBillPeriod = new bool[] { false, false, false, false, false };
        /// <summary>
        /// First  Bool   = All
        /// Second Bool   = Billing Single
        /// Third  Bool   = Bills Receivable
        /// Fourth  Bool  = Bills Cancelling
        /// </summary>        
        public static bool[] EditSettings = new bool[] { false, false, false, false, false };

        public static void SetFromEdited(string Form)
        {
            if (Form == "AccountTypes")
                for (int i = 0; i < EditAccountType.Length; i++)
                    EditAccountType[i] = true;
            if (Form == "Customers")
                for (int i = 0; i < EditCustomer.Length; i++)
                    EditCustomer[i] = true;
            if (Form == "SubScriptions")
                for (int i = 0; i < EditSubScription.Length; i++)
                    EditSubScription[i] = true;
            if (Form == "BillPeriods")
                for (int i = 0; i < EditBillPeriod.Length; i++)
                    EditBillPeriod[i] = true;
            if (Form == "Settings")
                for (int i = 0; i < EditSettings.Length; i++)
                    EditSettings[i] = true;                
        }

        public static void SetPeriod()
        {
            try
            {
                List<DomainClasses.BillPeriodsTb> Tbp = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear && x.IsSelected == true).ToList();
                if (Tbp.Count > 0)
                {
                    Commons.CPeriod.Id = Tbp[0].Id;
                    Commons.CPeriod.Name = Tbp[0].Name;
                    Commons.CPeriod.DateFrom = Tbp[0].DateFrom;
                    Commons.CPeriod.DateTo = Tbp[0].DateTo;
                    Commons.CPeriod.MonthPeriod = Tbp[0].MonthPeriod;
                    Commons.CPeriod.IsClosed = Tbp[0].IsClosed;
                }
                else
                {
                    Commons.CPeriod.Id = 0;
                    Commons.CPeriod.Name = "";
                    Commons.CPeriod.DateFrom = "";
                    Commons.CPeriod.DateTo = "";
                    Commons.CPeriod.MonthPeriod = 0;
                    Commons.CPeriod.IsClosed = false;
                }
            }
            catch
            {
                Commons.CPeriod.Id = 0;
                Commons.CPeriod.Name = "";
                Commons.CPeriod.DateFrom = "";
                Commons.CPeriod.DateTo = "";
                Commons.CPeriod.MonthPeriod = 0;
                Commons.CPeriod.IsClosed = false;
            }
        }
        public static void SetYear()
        {
            try
            {
                List<DomainClasses.SettingsTb> Tb = Commons.Db.Settings.ToList();
                if (Tb.Count > 0)
                {
                    Commons.CurrentYear = Tb[0].CurrentYear;
                    Commons.Address = Tb[0].Address;
                    Commons.BillMessage = Tb[0].BillMessage;
                    Commons.CompanyName = Tb[0].CompanyName;
                    Commons.Tel = Tb[0].Tel;
                    Commons.Vat = Tb[0].Vat;
                    Commons.WaterAndWastewaterAuthorityName = Tb[0].WaterAndWastewaterAuthorityName;
                    YearsTb Ly = Commons.Db.Years.Find(Tb[0].CurrentYear);
                    if (Ly != null)
                        Commons.IsClosedCurrentYear = Ly.IsClosed;
                    else
                        Commons.IsClosedCurrentYear = false;
                }
                else
                {
                    Commons.CurrentYear = 0;
                    Commons.Address = "";
                    Commons.BillMessage = "";
                    Commons.CompanyName = "";
                    Commons.Tel = "";
                    Commons.Vat = 0;
                    Commons.WaterAndWastewaterAuthorityName = "";
                }
            }
            catch
            {
                Commons.CurrentYear = 0;
                Commons.Address = "";
                Commons.BillMessage = "";
                Commons.CompanyName = "";
                Commons.Tel = "";
                Commons.Vat = 0;
                Commons.WaterAndWastewaterAuthorityName = "";
            }
        }
        public static int GetDaysOfDate(string From)
        {
            int FromDays = 0;
            string[] FromS = From.Split(new char[] { '/' });
            int YearFrom = Convert.ToInt32(FromS[0]);
            int MonthFrom = Convert.ToInt32(FromS[1]);
            int DayFrom = Convert.ToInt32(FromS[2]);
            PersianCalendar HijriDate = new PersianCalendar();
            for (int i = 1390; i < YearFrom; i++)
            {
                if (HijriDate.IsLeapYear(i))
                    FromDays += 366;
                else
                    FromDays += 365;
            }
            for (int i = 1; i < MonthFrom; i++)
            {
                if (i >= 1 && i <= 6)
                    FromDays += 31;
                else
                    if (i >= 7 && i <= 11)
                        FromDays += 30;
                    else
                        if (i == 12)
                        {
                            if (HijriDate.IsLeapYear(YearFrom))
                                FromDays += 30;
                            else
                                FromDays += 29;
                        }
            }
            return FromDays + DayFrom; 
        }
        public static int GetDaysOfTwoDate(string From,string To)
        {
            try
            {
                string[] FromS = From.Split(new char[] { '/' });
                string[] ToS = To.Split(new char[] { '/' });
                int DaysFrom = GetDaysOfDate(From);
                int DaysTo = GetDaysOfDate(To);
                return DaysTo - DaysFrom;
            }
            catch
            {
                return 0;
            }
        }
        public static List<int> GetMonthDaysOfTwoDate(string From, string To)
        {
            PersianCalendar HijriDate = new PersianCalendar();
            string[] FromS = From.Split(new char[] { '/' });
            string[] ToS = To.Split(new char[] { '/' });
            int Years = Convert.ToInt32(ToS[0]) - Convert.ToInt32(FromS[0]);
            List<int> MonthsDay = new List<int>();
            for (int i = Convert.ToInt32(FromS[0]); i <= Convert.ToInt32(ToS[0]); i++)
            {

                int FromMonth = 1;
                int ToMonth = 12;
                if (i == Convert.ToInt32(FromS[0]))
                {
                    FromMonth = Convert.ToInt32(FromS[1]);
                }
                if (i == Convert.ToInt32(ToS[0]))
                {
                    ToMonth = Convert.ToInt32(ToS[1]);
                }

                for (int j = FromMonth; j <= ToMonth; j++)
                {
                    if (j >= 1 && j <= 6)
                    {
                        MonthsDay.Add(31);
                    }
                    else
                        if (j >= 7 && j <= 11)
                            MonthsDay.Add(30);
                        else
                            if (!HijriDate.IsLeapYear(i))
                                MonthsDay.Add(29);
                            else
                                MonthsDay.Add(30);
                    if (i == Convert.ToInt32(FromS[0]) && j == Convert.ToInt32(FromS[1]))
                    {
                        MonthsDay[MonthsDay.Count - 1] = MonthsDay[MonthsDay.Count - 1] - Convert.ToInt32(FromS[2]);
                    }
                    else
                        if (i == Convert.ToInt32(ToS[0]) && j == Convert.ToInt32(ToS[1]))
                        {
                            MonthsDay[MonthsDay.Count - 1] = Convert.ToInt32(ToS[2]);
                        }
                }
            }
            return MonthsDay;
        }
        //public static int DistanceBetweenMonths(string From, string To)
        //{
        //    int Dist = -1;
        //    try
        //    {
        //        string[] DateFrom = From.Split(new char[] { '/' });
        //        string[] DateTo = To.Split(new char[] { '/' });

        //        int YearFrom = Convert.ToInt32(DateFrom[0]);
        //        int MonthFrom = Convert.ToInt32(DateFrom[1]);
        //        int DayFrom = Convert.ToInt32(DateFrom[2]);

        //        int YearTo = Convert.ToInt32(DateTo[0]);
        //        int MonthTo = Convert.ToInt32(DateTo[1]);
        //        int DayTo = Convert.ToInt32(DateTo[2]);

        //        if (YearFrom != YearTo)
        //            return -2;
        //        if (MonthFrom >= MonthTo)
        //            return -3;
        //        if (DayFrom != 1)
        //            return -4;
        //        if ((MonthTo >= 1 && MonthTo <= 6) && DayTo != 31)
        //            return -5;
        //        if ((MonthTo >= 7 && MonthTo <= 11) && DayTo != 30)
        //            return -6;
        //        PersianCalendar HijriDate = new PersianCalendar();
        //        if (MonthTo == 12 && HijriDate.IsLeapYear(YearFrom) == true && DayTo != 30)
        //            return -7;
        //        if (MonthTo == 12 && HijriDate.IsLeapYear(YearFrom) == false && DayTo != 29)
        //            return -8;
        //        if (YearFrom != Commons.GetCurrentYear())
        //            return -9;
        //        Dist = MonthTo - MonthFrom;
        //    }
        //    catch
        //    {
        //        return Dist;
        //    }
        //    return Dist;
        //}
        public static int DistanceBetweenMonthsForBilling(string From, string To)
        {
            int CountMonth = 0;
            try
            {
                int AllDays = GetDaysOfTwoDate(From, To);
                List<int> DayMonths = GetMonthDaysOfTwoDate(From, To);
                int Days = 0;
                
                for (int i = 1; i < DayMonths.Count - 1; i++)
                    CountMonth++;                
                if (DayMonths.Count > 1)
                    Days = DayMonths[0] + DayMonths[DayMonths.Count - 1];
                else
                    Days = DayMonths[0];

                if (Days > 15 && Days <= 45)
                {
                    CountMonth++;
                }
                else
                    if (Days > 45)
                    {
                        CountMonth += 2;
                    }
                
                if (AllDays <= 15)
                {
                    CountMonth++;
                }
            }
            catch
            {
                CountMonth = -1;
            }
            return CountMonth;
        }
        public static string GetDeadLinePaymentDate()
        {
            PersianCalendar HijriDate = new PersianCalendar();
            DateTime Dt = HijriDate.AddDays(DateTime.Now, 10);
            string Date = HijriDate.GetYear(Dt) + "/" + HijriDate.GetMonth(Dt).ToString("00") + "/" + HijriDate.GetDayOfMonth(Dt).ToString("00");
            return Date;
        }
        public static string GetCurrentPersianDate()
        {
            PersianCalendar HijriDate = new PersianCalendar();
            string Date = HijriDate.GetYear(DateTime.Now) + "/" + HijriDate.GetMonth(DateTime.Now).ToString("00") + "/" + HijriDate.GetDayOfMonth(DateTime.Now).ToString("00");
            return Date;
        }
        public static int GetCurrentYear()
        {
            PersianCalendar HijriDate = new PersianCalendar();
            string Date = HijriDate.GetYear(DateTime.Now).ToString();
            return Convert.ToInt32(Date);
        }
        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();


            // column names
            PropertyInfo[] oProps = null;


            if (varlist == null) return dtReturn;


            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;


                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }


                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }


                DataRow dr = dtReturn.NewRow();


                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }


                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        public static ControlsValidate? ValidateData(Control FirstControl, System.Collections.Specialized.ListDictionary ControlsArray)
        {

            ControlsValidate ErrorInControl;
            ValidationError Validate = null;
            BindingExpression BExp = null;
            Control Tmp = FirstControl;
            while (Tmp != null)
            {
                if (Tmp != null && Tmp.IsVisible == false && (Tmp.Name == "TextBoxCurrentReadDate" || Tmp.Name == "TextBoxCurrentRead"))
                {
                    Tmp = ((ControlsTab)ControlsArray[Tmp]).Next;
                    continue;
                }

                if (((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxFormul || ((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxText || ((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxNumber)
                    BExp = BindingOperations.GetBindingExpression((TextBox)Tmp, TextBox.TextProperty);
                else
                    if (((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.ComboBox)
                        BExp = BindingOperations.GetBindingExpression((ComboBox)Tmp, ComboBox.SelectedIndexProperty);

                if (BExp != null)
                {
                    BExp.UpdateSource();
                    if (((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxFormul || ((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxText || ((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.TextBoxNumber)
                        Validate = BindingOperations.GetBindingExpression((TextBox)Tmp, TextBox.TextProperty).ValidationError;
                    else
                        if (((ControlsTab)ControlsArray[Tmp]).Type == ControlsType.ComboBox)
                            Validate = BindingOperations.GetBindingExpression((ComboBox)Tmp, ComboBox.SelectedIndexProperty).ValidationError;
                    if (Validate != null)
                        break;
                }

                Tmp = ((ControlsTab)ControlsArray[Tmp]).Next;
            }
            if (Validate != null)
            {
                ErrorInControl.Control = Tmp;
                ErrorInControl.Message = Validate.ErrorContent.ToString();
                ErrorInControl.TypeOfControl = Tmp.GetType();
                return ErrorInControl;
            }
            else
                return null;
        }
        public static void LoadComboBox(ComboBox Cb, string Table, string Message)
        {
            Cb.Items.Clear();
            ComboBoxItem CbItem = new ComboBoxItem();
            CbItem.Content = "لطفا " + Message + " را مشخص نمایید.";
            CbItem.Tag = 0;
            Cb.Items.Add(CbItem);
            int i = 1;
            if (Table == "AccountTypesTb")
            {
                foreach (AccountTypesTb Value in Commons.Db.AccountTypes.Where(x=>x.Year == CurrentYear))
                {
                    CbItem = new ComboBoxItem();
                    CbItem.Content = Value.Name;
                    CbItem.Tag = Value.Id;
                    if (i % 2 == 0)
                        CbItem.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    else
                        CbItem.Background = new SolidColorBrush(Color.FromRgb(235, 233, 253));
                    Cb.Items.Add(CbItem);
                    i++;
                }
            }
            if (Table == "PreventTypesTb")
            {
                foreach (PreventTypesTb Value in Commons.Db.PreventTypes.Where(x => x.Year == CurrentYear))
                {
                    CbItem = new ComboBoxItem();
                    CbItem.Content = Value.Name;
                    CbItem.Tag = Value.Id;
                    if (i % 2 == 0)
                        CbItem.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    else
                        CbItem.Background = new SolidColorBrush(Color.FromRgb(235, 233, 253));
                    Cb.Items.Add(CbItem);
                    i++;
                }
            }
        }
        public static string ConvertToMoney(string Money)
        {
            string My = Money;
            for (int i = Money.Length - 3; i > 0; i -= 3)
            {
                if (i != 0)
                    My = My.Insert(i, ",");
            }
            return My;
        }
        public static string ConvertToMoneyWithSign(long Money)
        {
            long Tmp = Money;
            if (Tmp < 0)
                Tmp = Tmp * -1;
            string Mo = Tmp.ToString();
            Mo = ConvertToMoney(Mo);
            string Sign = "";
            if (Money > 0)
                Sign = "-";
            else if (Money < 0)
                Sign = "+";
            Mo = Mo + Sign;
            return Mo;
        }
        public static void GridViewKeyDown(DataGrid DataGridView, ref KeyEventArgs e, TextBox TextBoxSearch, string FirstColName, string LastColName, string LastButOneColName, int LastColIndex)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (TextBoxSearch != null)
                {
                    if (TextBoxSearch.IsEnabled == true)
                        TextBoxSearch.Focus();
                    return;
                }
            }
            if (DataGridView.Items != null || DataGridView.Items.Count != 0)
            {
                if (e.Key == Key.Up)
                {
                    if (DataGridView.SelectedIndex == 0)
                        e.Handled = true;
                    else
                    {
                        if (DataGridView.CurrentCell.Column != null)
                            if (DataGridView.CurrentCell.Column.Header.ToString() == LastColName)
                            {
                                e.Handled = true;
                                if (DataGridView.SelectedIndex > -1)
                                    DataGridView.SelectedIndex -= 1;
                                ContentPresenter fe = (ContentPresenter)DataGridView.Columns[LastColIndex].GetCellContent(DataGridView.Items[DataGridView.SelectedIndex]);
                                if (fe != null)
                                {
                                    StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                                    ((Button)S.FindName("ButtonShowEdit")).Focus();
                                }
                            }
                    }
                }
                if (e.Key == Key.Down)
                {
                    if (DataGridView.SelectedIndex == DataGridView.Items.Count - 1)
                        e.Handled = true;
                    else
                    {
                        if (DataGridView.CurrentCell.Column != null)
                            if (DataGridView.CurrentCell.Column.Header.ToString() == LastColName)
                            {
                                e.Handled = true;
                                DataGridView.SelectedIndex += 1;
                                ContentPresenter fe = (ContentPresenter)DataGridView.Columns[LastColIndex].GetCellContent(DataGridView.Items[DataGridView.SelectedIndex]);
                                if (fe != null)
                                {

                                    StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                                    ((Button)S.FindName("ButtonShowEdit")).Focus();
                                }
                            }
                    }
                }
                if (e.Key == Key.Right)
                {
                    if (DataGridView.CurrentCell.Column != null)
                    {
                        if (DataGridView.CurrentCell.Column.Header.ToString() == FirstColName)
                            e.Handled = true;
                        else
                        {
                            if (DataGridView.CurrentCell.Column.Header.ToString() == LastColName)
                            {
                                e.Handled = true;
                                if (DataGridView.SelectedItem != null)
                                {
                                    ContentPresenter fe = (ContentPresenter)DataGridView.Columns[LastColIndex].GetCellContent(DataGridView.SelectedItem);
                                    if (fe != null)
                                    {

                                        StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                                        if (((Button)S.FindName("ButtonDelete")).IsFocused == true)
                                            ((Button)S.FindName("ButtonShowEdit")).Focus();
                                        else
                                            if (((Button)S.FindName("ButtonShowEdit")).IsFocused == true)
                                                DataGridView.CurrentCell = new DataGridCellInfo(DataGridView.SelectedItem, DataGridView.Columns[LastColIndex - 1]);
                                            else
                                                ((Button)S.FindName("ButtonShowEdit")).Focus();
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (DataGridView.CurrentCell.Column != null)
                    {
                        if (DataGridView.CurrentCell.Column.Header.ToString() == LastButOneColName)
                        {
                            e.Handled = true;
                            ContentPresenter fe = (ContentPresenter)DataGridView.Columns[LastColIndex].GetCellContent(DataGridView.SelectedItem);
                            StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                            ((Button)S.FindName("ButtonShowEdit")).Focus();
                        }
                        else
                        {
                            if (DataGridView.CurrentCell.Column.Header.ToString() == LastColName)
                            {
                                e.Handled = true;
                                if (DataGridView.SelectedItem != null)
                                {
                                    ContentPresenter fe = (ContentPresenter)DataGridView.Columns[LastColIndex].GetCellContent(DataGridView.SelectedItem);
                                    if (fe != null)
                                    {

                                        StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                                        if (((Button)S.FindName("ButtonShowEdit")).IsFocused == true)
                                            ((Button)S.FindName("ButtonDelete")).Focus();
                                        else
                                            ((Button)S.FindName("ButtonShowEdit")).Focus();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void GridViewKeyDown(DataGrid DataGridView, ref KeyEventArgs e, TextBox TextBoxSearch, string FirstColName, string LastColName)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (TextBoxSearch != null)
                    TextBoxSearch.Focus();
            }
            if (DataGridView.Items != null || DataGridView.Items.Count != 0)
            {
                if (e.Key == Key.Up)
                {
                    if (DataGridView.SelectedIndex == 0)
                        e.Handled = true;
                }
                if (e.Key == Key.Down)
                {
                    if (DataGridView.SelectedIndex == DataGridView.Items.Count - 1)
                        e.Handled = true;
                }
                if (e.Key == Key.Right)
                {
                    if (DataGridView.CurrentCell.Column != null)
                    {
                        if (DataGridView.CurrentCell.Column.Header.ToString() == FirstColName)
                            e.Handled = true;
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (DataGridView.CurrentCell.Column != null)
                    {
                        if (DataGridView.CurrentCell.Column.Header.ToString() == LastColName)
                            e.Handled = true;
                    }
                }
            }
        }
        public static int CheckDateFromTo(string From, string To)
        {
            try
            {
                From = From.Replace("/", "");
                To = To.Replace("/", "");
                long NFrom = Convert.ToInt64(From);
                long NTo = Convert.ToInt64(To);
                if (NFrom >= NTo)
                    return -1;
                else
                    return 0;
            }
            catch
            {
                return -2;
            }
        }
        public static int IsDateReplicated(string From, string To, List<BillPeriodsTb> Table)
        {
            int FromMonth = Convert.ToInt32(From.Split(new char[] { '/' })[1]);
            int FromDay = Convert.ToInt32(From.Split(new char[] { '/' })[2]);
            int ToMonth = Convert.ToInt32(To.Split(new char[] { '/' })[1]);
            int ToDay = Convert.ToInt32(To.Split(new char[] { '/' })[2]);

            foreach (BillPeriodsTb Value in Table)
            {
                int TmpFromMonth = Convert.ToInt32(Value.DateFrom.Split(new char[] { '/' })[1]);
                int TmpFromDay = Convert.ToInt32(Value.DateFrom.Split(new char[] { '/' })[2]);
                int TmpToMonth = Convert.ToInt32(Value.DateTo.Split(new char[] { '/' })[1]);
                int TmpToDay = Convert.ToInt32(Value.DateTo.Split(new char[] { '/' })[2]);

                if (FromMonth <= TmpFromMonth && ToMonth >= TmpToMonth)
                    return -1;
                if (FromMonth >= TmpFromMonth && FromMonth < TmpToMonth)
                    return -1;
                if (ToMonth > TmpFromMonth && ToMonth <= TmpToMonth)
                    return -1;
            }
            return 1;
        }
        public static long[] Consumption(long Consumption, string Formula, int Months)
        {
            try
            {
                //double tmpConsump = (double)Consumption / (double)Months;
                long Consump = Consumption;
                int FndIxStart = -1;
                List<string> FormulaRangeList = new List<string>();
                List<string> FormulaExpList = new List<string>();
                List<string> FormulaOperandList = new List<string>();
                long[] Result = new long[6];
                for (int i = 0; i < 5; i++)
                    Result[i] = 0;
                for (int i = 0; i < Formula.Length; i++)
                {
                    if (Formula[i] == '[' || i == Formula.Length - 1)
                    {
                        if (i > 0 && FndIxStart > 0)
                        {
                            if (i != Formula.Length - 1)
                            {
                                string Tmp = Formula.Substring(FndIxStart + 1, i - FndIxStart - 1).Trim();
                                FormulaExpList.Add(Tmp.Substring(0, Tmp.Length - 1).Trim());
                                FormulaOperandList.Add(Tmp.Substring(Tmp.Length - 1));
                            }
                            else
                                FormulaExpList.Add(Formula.Substring(FndIxStart + 1, i - FndIxStart).Trim());
                        }
                        FndIxStart = i;
                        continue;
                    }
                    if (Formula[i] == ']')
                    {
                        FormulaRangeList.Add(Formula.Substring(FndIxStart + 1, (i - 1) - FndIxStart).Trim());
                        FndIxStart = i;
                    }
                }
                Expression Exp;
                bool Exit = false;
                for (int i = 0; i < FormulaRangeList.Count; i++)
                {
                    string[] Ranges = FormulaRangeList[i].Split(new char[] { '-' });
                    long Start = Convert.ToInt32(Ranges[0].Trim()) * Months;
                    long End = 0;
                    if (Ranges[1].Trim() != "#")
                        End = Convert.ToInt32(Ranges[1].Trim()) * Months;
                    else
                        End = 999999999999;
                    long Dist = End - Start;
                    long R;
                    if (Consump > Dist)
                    {
                        Consump = Consump - Dist;
                        string TmpFrm = Dist.ToString() + FormulaExpList[i];
                        Exp = new Expression(TmpFrm);
                        if (Exp.HasErrors() == false)
                            R = (int)Exp.Evaluate();
                        else
                            throw new Exception();
                    }
                    else
                    {
                        string TmpFrm = Consump.ToString() + FormulaExpList[i];
                        Exp = new Expression(TmpFrm);
                        if (Exp.HasErrors() == false)
                            R = (int)Exp.Evaluate();
                        else
                            throw new Exception();
                        Exit = true;
                    }
                    if (i == 0)
                    {
                        if (Exit)
                            Result[0] = Consump;
                        else
                            Result[0] = Dist;
                        Result[2] = R;
                    }
                    else
                    {
                        if (Exit)
                            Result[1] += Consump;
                        else
                            Result[1] += Dist;
                        if (i > 1)
                        {
                            Exp = new Expression(Result[3].ToString() + FormulaOperandList[i - 1].Trim() + R.ToString());
                            if (Exp.HasErrors() == false)
                                Result[3] = (int)Exp.Evaluate();
                            else
                                throw new Exception();
                        }
                        else
                            Result[3] = R;
                    }
                    if (Exit)
                        break;

                }
                //Result[0] = Result[0] * Months;
                //Result[1] = Result[1] * Months;
                //Result[2] = Result[2] * Months;
                //Result[3] = Result[3] * Months;
                if (FormulaOperandList.Count > 0)
                {
                    Exp = new Expression(Result[2].ToString() + FormulaOperandList[0].Trim() + Result[3].ToString());
                    if (Exp.HasErrors() == false)
                        Result[4] = (int)Exp.Evaluate();
                    else
                        throw new Exception();
                }
                else
                    Result[4] = Result[2];                
                Result[5] = Convert.ToInt64(Math.Round((((double)Result[4]) * Vat) / 100));
                return Result;
            }
            catch
            {
                return null;
            }
        }
        public static long GetDeficit1000(long Price)
        {
            long D = Price % 1000;
            return D;
        }

        public static string GetShortPeriodName(string DateFrom, string DateTo)
        {
            string[] From = DateFrom.Split(new char[] { '/' });
            string[] To = DateTo.Split(new char[] { '/' });
            int MonthFrom = Convert.ToInt32(From[1]);
            int MonthTo = Convert.ToInt32(To[1]);
            return GetPersianNameOfMonths(MonthFrom) + "-" + GetPersianNameOfMonths(MonthTo);
        }

        public static string GetPersianNameOfMonths(int Month)
        {
            string Name = "";
            switch (Month)
            {
                case 1:
                    Name = "فروردین";
                    break;
                case 2:
                    Name = "اردیبهشت";
                    break;
                case 3:
                    Name = "خرداد";
                    break;
                case 4:
                    Name = "تیر";
                    break;
                case 5:
                    Name = "مرداد";
                    break;
                case 6:
                    Name = "شهریور";
                    break;
                case 7:
                    Name = "مهر";
                    break;
                case 8:
                    Name = "آبان";
                    break;
                case 9:
                    Name = "آذر";
                    break;
                case 10:
                    Name = "دی";
                    break;
                case 11:
                    Name = "بهمن";
                    break;
                case 12:
                    Name = "اسفند";
                    break;
                default:
                    Name = "";
                    break;
            }
            return Name;
        }
        public static string GetCodeOfStringForSearch(string Text)
        {
            string[] TextSpilit = Text.Split(new char[] { ' ' });
            string TextCode = @"""";
            int Code;
            foreach (string Value in TextSpilit)
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    Code = (int)Value[i];
                    TextCode += Code.ToString("X");
                }
                TextCode += "* ";
            }
            TextCode = TextCode.Trim();
            TextCode += @"""";
            return TextCode.Trim();
        }
        //public static string GetCodeOfStringForUpdate(string Text)
        //{
        //    string[] TextSpilit = Text.Split(new char[] { ' ' });
        //    string TextCode = "";
        //    int Code;
        //    foreach (string Value in TextSpilit)
        //    {
        //        for (int i = 0; i < Value.Length; i++)
        //        {
        //            Code = (int)Value[i];
        //            TextCode += Code.ToString("X");
        //        }
        //        TextCode += " ";
        //    }
        //    return TextCode.Trim();
        //}
    }
}
