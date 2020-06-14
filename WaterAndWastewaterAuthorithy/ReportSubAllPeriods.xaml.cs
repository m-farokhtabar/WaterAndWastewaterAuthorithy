using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WaterAndWastewaterAuthorithy.Presentation;
using System.Data;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for ReportSubAllPeriods.xaml
    /// </summary>
    public partial class ReportSubAllPeriods : UserControl
    {
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        //به دلیل اینکه بالای برای فوکوس استفاده می وشد برای اعتبارسنجی جدا کردم چون به دکمه و گرید خطا می دهد
        System.Collections.Specialized.ListDictionary ControlsArrayForValidation = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        AnimateControls Ac = new AnimateControls();
        string PrevHint = "";

        #region Loading
        public ReportSubAllPeriods()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionId;
            ControlsArray.Add(TextBoxSubScriptionId, new ControlsTab { Prev = DataGridView, Next = TextBoxDateFrom, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxDateFrom, new ControlsTab { Prev = TextBoxSubScriptionId, Next = TextBoxDateTo, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxDateTo, new ControlsTab { Prev = TextBoxDateFrom, Next = ButtonBeginSearch, Type = ControlsType.TextBoxText });
            ControlsArray.Add(ButtonBeginSearch, new ControlsTab { Prev = TextBoxDateTo, Next = DataGridView, Type = ControlsType.TextBoxText });
            ControlsArray.Add(DataGridView, new ControlsTab { Prev = TextBoxDateTo, Next = null, Type = ControlsType.TextBoxText });

            ControlsArrayForValidation.Add(TextBoxSubScriptionId, new ControlsTab { Prev = null, Next = TextBoxDateFrom, Type = ControlsType.TextBoxText });
            ControlsArrayForValidation.Add(TextBoxDateFrom, new ControlsTab { Prev = TextBoxSubScriptionId, Next = TextBoxDateTo, Type = ControlsType.TextBoxText });
            ControlsArrayForValidation.Add(TextBoxDateTo, new ControlsTab { Prev = TextBoxDateFrom, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlBillsPrint_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControlBillsPrint_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //DataGridView.ItemsSource = Commons.Db.BillsVws.ToList().OrderBy(x => x.SortableId);
            //DoSearch();
            FirstControl.Focus();
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonReport_Click(null, null);
            if (e.Key == Key.F3)
                ButtonBeginSearch_Click(null, null);
            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }
        #endregion

        #region Control
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                e.Handled = true;
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                    Ctrl.Focus();
                }
                else
                {
                    if (e.Key == Key.Tab)
                        FirstControl.Focus();
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                e.Handled = true;
                if (((ControlsTab)ControlsArray[sender]).Prev != null)
                {
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Prev;
                    Ctrl.Focus();
                }
            }

        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch
            {
                e.Handled = true;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tmp = ((TextBox)sender).Text;
            int Pos = ((TextBox)sender).SelectionStart;
            foreach (char c in ((TextBox)sender).Text.ToCharArray())
            {
                if (!Char.IsDigit(c))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            ((TextBox)sender).Text = tmp;
            ((TextBox)sender).SelectionStart = Pos;
        }
        private void TextBoxSubScriptionId_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tmp = ((TextBox)sender).Text;
            int Pos = ((TextBox)sender).SelectionStart;
            foreach (char c in ((TextBox)sender).Text.ToCharArray())
            {
                if (!Char.IsDigit(c) && c != '-')
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            ((TextBox)sender).Text = tmp;
            ((TextBox)sender).SelectionStart = Pos;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            BindingOperations.GetBindingExpression(text, TextBox.TextProperty).UpdateSource();
        }
        private void TextBoxNumberReadDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tmp = ((TextBox)sender).Text;
            int Pos = ((TextBox)sender).SelectionStart;
            foreach (char c in ((TextBox)sender).Text.ToCharArray())
            {
                if (!(Char.IsDigit(c) || c == '/'))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            ((TextBox)sender).Text = tmp;
            ((TextBox)sender).SelectionStart = Pos;
        }
        private void DataGridView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxSubScriptionId, "شماره پرونده", "10");
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
            PrevHint = ((TextBox)sender).Tag.ToString();
            Hint.Text = ((TextBox)sender).Tag.ToString();
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }
        }
        #endregion

        private void DoSearch()
        {            
            string SubScriptionIdFromText = "";
            string DateFrom = "";
            bool DateFromFlag = true;
            string DateTo = "";
            bool DateToFlag = true;
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArrayForValidation);            
            if (Vd != null)
            {
                MessageDialog Msg = null;
                Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {

                if (!string.IsNullOrWhiteSpace(TextBoxSubScriptionId.Text))
                {
                    string[] Tmp = TextBoxSubScriptionId.Text.Trim().Split(new char[] { '-' });
                    if (Tmp != null && Tmp.Length == 2)
                        SubScriptionIdFromText = TextBoxSubScriptionId.Text.Trim(); // Convert.ToInt32(Tmp[0]).ToString("0000") + "-" + Convert.ToInt32(Tmp[1]).ToString("00000");
                    else
                    {
                        MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.ValueIsSubscriptionNumberStart + "از شماره اشتراک" + Messages.ValueIsSubscriptionNumberEnd, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        return;
                    }
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.SubIdIsMandatory, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    return;
                }

                if (TextBoxDateFrom.Text.Trim() != "" && TextBoxDateTo.Text.Trim() != "")
                {
                    DateFrom = TextBoxDateFrom.Text.Trim();
                    DateFromFlag = false;
                    DateTo = TextBoxDateTo.Text.Trim();
                    DateToFlag = false;
                }
                else
                {
                    if (TextBoxDateFrom.Text.Trim() != "")
                    {
                        DateFrom = TextBoxDateFrom.Text.Trim();
                        DateFromFlag = false;
                    }
                    if (TextBoxDateTo.Text.Trim() != "")
                    {
                        DateTo = TextBoxDateTo.Text.Trim();
                        DateToFlag = false;
                    }
                }
                DataGridView.ItemsSource = LoadBillsForDataGrid(SubScriptionIdFromText, DateFromFlag, DateFrom, DateToFlag, DateTo);
            }
        }
        private List<BillingDetails> LoadBillsForDataGrid(string SubId,bool DateFromFlag,string DateFrom,bool DateToFlag, string DateTo)
        {
            try
            {
                var tmp = from sb in Commons.Db.Subscriptions.AsNoTracking()
                          where sb.Id==SubId
                          join ct in Commons.Db.Customers on new { SbC = sb.CustomerId, SbY = sb.CustomerYear } equals new { SbC = ct.Id, SbY = ct.Year }
                          join b in Commons.Db.Bills on new { SbB = sb.Id, SbY = sb.Year } equals new { SbB = b.SubscriptionId, SbY = b.SubscriptionYear }
                          join at in Commons.Db.AccountTypes on new { SbA = b.AccountTypeId, SbY = b.AccountTypeYear } equals new { SbA = at.Id, SbY = at.Year }
                          join pt in Commons.Db.PreventTypes on new { SbP = b.PreventTypeId, SbY = b.PreventTypeYear } equals new { SbP = pt.Id, SbY = pt.Year }
                          join wt in Commons.Db.WaterMeters on new { SbW = b.WaterMeterId, SbY = b.WaterMeterYear } equals new { SbW = wt.Id, SbY = wt.Year }
                          where (DateFromFlag == true || b.CurrentReadDate.CompareTo(DateFrom) >= 0) && (DateToFlag == true || b.CurrentReadDate.CompareTo(DateTo) <= 0) && b.Status != BillsStatus.Cancel
                          orderby b.Id ascending
                          select new BillingDetails
                          {
                              Id = b.Id,
                              SubId = sb.Id,
                              CustId = ct.Id,
                              Name = ct.Name,
                              Family = ct.Family,
                              Father = ct.Father,
                              Prevent = pt.Name,
                              AccounType = at.Name,
                              PrevDebt = b.PrevDebt,
                              CurrentReadDate = b.CurrentReadDate,
                              CurrentNumber = b.CurrentNumber,
                              PrevReadDate = b.PrevReadDate,
                              PrevNumber = b.PrevNumber,
                              AllowableConsumption = b.AllowableConsumption,
                              Consumption = b.Consumption,
                              ExtraConsumption = b.ExtraConsumption,
                              PriceOfAllowableConsumption = b.PriceOfAllowableConsumption,
                              PriceOfConsumption = b.PriceOfConsumption,
                              SubscriptionCost = b.SubscriptionCost,
                              PriceOfExtraConsumption = b.PriceOfExtraConsumption,
                              Vat = b.Vat,
                              SumOfPeriod = b.PriceOfConsumption + b.PrevDebt + b.Vat,
                              AllPrices = b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000 + b.SubscriptionCost,                              
                              Prevdeficit1000 = b.Prevdeficit1000,
                              deficit1000 = (b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000 + b.SubscriptionCost) % 1000,
                              PayablePrice = (b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000 + b.SubscriptionCost) - ((b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000 + b.SubscriptionCost) % 1000),
                              Status = b.Status,
                              Address = sb.Address
                          };
                return tmp.ToList();
            }
            catch
            {
                return null;
            }
        }
        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            IEnumerable<BillingDetails> Tb = (IEnumerable<BillingDetails>)DataGridView.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    DataTable Dt = Commons.ToDataTable<BillingDetails>(Tb);
                    Dt.TableName = "BillsPeriod";
                    string FullName =  Tb.FirstOrDefault().Name.Trim() + " "  + Tb.FirstOrDefault().Family.Trim();
                    string FromDateToDate = "";
                    if (!string.IsNullOrWhiteSpace(TextBoxDateFrom.Text.Trim()))
                        FromDateToDate = " از " + TextBoxDateFrom.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(TextBoxDateTo.Text.Trim()))
                        FromDateToDate += (" تا " + TextBoxDateTo.Text.Trim());
                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\OneSubBillForAllPeriod.mrt", GridHeader.Background, "قبوض دوره های اشتراک" + " : " + TextBoxSubScriptionId.Text.Trim() + " / " + FullName + FromDateToDate, Dt);
                    PrintPrv.ShowDialog();
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;

        }
        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("ReportSubAllPeriods", "Reports");            
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            PrevHint = Hint.Text;
            Control MyControl = (Control)sender;
            if (MyControl.ToolTip is ToolTip)
                Hint.Text = ((ToolTip)MyControl.ToolTip).Content.ToString();
            else
                Hint.Text = MyControl.ToolTip.ToString();
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }

        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Hint.Text = PrevHint;
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }
        }

        private void ButtonBeginSearch_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

    }
}
