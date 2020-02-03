﻿using System;
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
    /// Interaction logic for SingleBilling.xaml
    /// </summary>
    public partial class SingleBilling : UserControl
    {
        string LastTextBoxSearch = "";
        SelectedButton PrvButton = new SelectedButton();
        AnimateControls Ac = new AnimateControls();
        GridLength PrevHeight;
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        System.Collections.Specialized.ListDictionary ControlsArraySubScriptionId = new System.Collections.Specialized.ListDictionary();
        System.Collections.Specialized.ListDictionary ControlsArraySearch = new System.Collections.Specialized.ListDictionary();
        public TextBox FirstControl = null;
        public Control FirstControlSearch = null;
        bool SearchAgain = false;
        SubscriptionBilling CurrentSubscriptionBilling;
        string PrevHint = "";

        public SingleBilling()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionId;
            ControlsArraySubScriptionId.Add(TextBoxSubScriptionId, new ControlsTab { Prev = null, Next = null, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxSubScriptionId, new ControlsTab { Prev = TextBoxCurrentRead, Next = TextBoxCurrentReadDate, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxCurrentReadDate, new ControlsTab { Prev = TextBoxSubScriptionId, Next = TextBoxCurrentRead, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxCurrentRead, new ControlsTab { Prev = TextBoxCurrentReadDate, Next = null, Type = ControlsType.TextBoxNumber });

            FirstControlSearch = RadioSearchBySubCode;
            ControlsArraySearch.Add(RadioSearchBySubCode, new ControlsTab { Prev = TextBoxSearch, Next = RadioSearchByCode, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(RadioSearchByCode, new ControlsTab { Prev = RadioSearchBySubCode, Next = TextBoxSearch, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(TextBoxSearch, new ControlsTab { Prev = RadioSearchByCode, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlBilling_Loaded(object sender, RoutedEventArgs e)
        {
            Ac.SelectButtonRed(ButtonDefination, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);

            PrevHeight = GridRowSearch.Height;
            GridRowSearch.Height = new GridLength(0);
            GridSearch.Visibility = System.Windows.Visibility.Hidden;
            RadioSearchBySubCode.IsChecked = true;
        }
        private void UserControlBilling_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue))
            {
                int Ch = CheckPeriod();
                if (Ch == -1)
                {
                    EmptyControl();
                    TextBlockPeriod.Text = "";
                    TextBoxSubScriptionId.Text = "";
                    TextBoxCurrentReadDate.Text = "";
                    TextBoxSubScriptionId.IsEnabled = false;
                    TextBoxCurrentRead.IsEnabled = false;
                    TextBoxCurrentReadDate.IsEnabled = false;
                    MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);                    
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    Ac.ShowWindow("SingleBilling", "MainMenu");
                    return;
                }
                else
                {
                    if (Ch == -2)
                    {
                        EmptyControl();
                        TextBlockPeriod.Text = "";
                        TextBoxSubScriptionId.Text = "";
                        TextBoxCurrentReadDate.Text = "";
                        TextBoxSubScriptionId.IsEnabled = false;
                        TextBoxCurrentRead.IsEnabled = false;
                        TextBoxCurrentReadDate.IsEnabled = false;
                        MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.PeriodIsClosedBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                    else
                    {
                        TextBoxSubScriptionId.IsEnabled = true;
                        TextBoxCurrentRead.IsEnabled = true;
                        TextBoxCurrentReadDate.IsEnabled = true;
                        FirstControl.Focus();
                        FirstControl.SelectAll();
                    }
                }
                if (Commons.EditAccountType[1] == true || Commons.EditCustomer[1] == true || Commons.EditSubScription[0] == true || Commons.EditBillPeriod[0] == true || Commons.EditSettings[1] == true)
                {
                    EmptyControl();                    
                    TextBoxSubScriptionId.Text = "";
                    TextBoxCurrentReadDate.Text = Commons.GetCurrentPersianDate();
                    Commons.EditAccountType[1] = false;
                    Commons.EditCustomer[1] = false;
                    Commons.EditSubScription[0] = false;
                    Commons.EditBillPeriod[0] = false;
                    Commons.EditSettings[1] = false;
                }
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F4)
                ButtonDefination_Click(ButtonDefination, null);
            if (GridNew.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);

            if (GridNew.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F7)
                ButtonSearch_Click(ButtonSearch, null);
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonReport_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }

        #region To Control TextBox For TabStop And Validate Value
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (sender == (Control)FirstControl)
                {
                    CheckSubScriptionId(true);
                    e.Handled = true;
                    return;
                }
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                    Ctrl.Focus();
                }
                else
                {
                    if (e.Key == Key.Tab)
                        FirstControl.Focus();
                    else
                    {
                        ButtonSave_Click(null, null);
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                if (sender == (Control)FirstControl)
                {
                    CheckSubScriptionId(false);
                    e.Handled = true;
                    return;
                }
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
        private void TextBoxSubScriptionId_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            BindingOperations.GetBindingExpression(text, TextBox.TextProperty).UpdateSource();
        }
        #endregion

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArray);
            MessageDialog Msg = null;
            if (Commons.CPeriod.Id > 0 && Commons.CPeriod.IsClosed == false)
            {
                if (Vd != null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    Vd.Value.Control.Focus();
                }
                else
                {
                    SubscriptionsTb MySubscription = CurrentSubscriptionBilling.SubScription;
                    if (string.Compare(TextBoxCurrentReadDate.Text.Trim(), Commons.GetCurrentPersianDate()) <= 0)
                    {
                        long NewNumber = Convert.ToInt64(TextBoxCurrentRead.Text);
                        long MyConsumption = NewNumber - MySubscription.CurrentNumber;
                        int ChkDate = 0;
                        if (MyConsumption >= 0)
                        {
                            ChkDate = Commons.CheckDateFromTo(MySubscription.CurrentReadDate, TextBoxCurrentReadDate.Text.Trim());
                            int DistMonth = 0;
                            if (ChkDate == 0)
                            {
                                DistMonth = Commons.DistanceBetweenMonthsForBilling(MySubscription.CurrentReadDate, TextBoxCurrentReadDate.Text.Trim());
                                if (DistMonth > 0)
                                {
                                    long[] Result = Commons.Consumption(MyConsumption, CurrentSubscriptionBilling.AccountTypeFormula, DistMonth);
                                    if (Result != null)
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.SaveMessageSingleBilling, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                                        Msg.Owner = Window.GetWindow(this);
                                        if (Msg.ShowDialog() == true)
                                        {
                                            try
                                            {
                                                BillsTb NewBill = Commons.Db.Bills.Add(new DomainClasses.BillsTb
                                                {
                                                    SubscriptionId = MySubscription.Id,
                                                    SubscriptionYear = Commons.CurrentYear,
                                                    Status = BillsStatus.Billing,
                                                    PrevNumber = MySubscription.CurrentNumber,
                                                    PrevReadDate = MySubscription.CurrentReadDate,
                                                    CurrentNumber = NewNumber,
                                                    CurrentReadDate = TextBoxCurrentReadDate.Text.Trim(),
                                                    Consumption = MyConsumption,
                                                    AllowableConsumption = Result[0],
                                                    ExtraConsumption = Result[1],
                                                    PriceOfConsumption = Result[4],
                                                    PriceOfAllowableConsumption = Result[2],
                                                    PriceOfExtraConsumption = Result[3],
                                                    Vat = Result[5],
                                                    SubscriptionCost = Commons.Subscription * DistMonth,
                                                    Year = Commons.CurrentYear,
                                                    CurrentPeriod = Commons.CPeriod.Id,
                                                    PrevDebt = MySubscription.Debt,
                                                    Prevdeficit1000 = MySubscription.deficit1000,
                                                    PrevPrevNumber = MySubscription.PrevNumber,
                                                    PrevPrevReadDate = MySubscription.PrevReadDate,
                                                    WaterMeterId = MySubscription.WaterMeter.Id,
                                                    WaterMeterYear = Commons.CurrentYear,
                                                    AccountTypeId = MySubscription.AccountTypeId,
                                                    AccountTypeYear = Commons.CurrentYear,
                                                    PreventTypeId = MySubscription.PreventTypeId,
                                                    PreventTypeYear = Commons.CurrentYear,
                                                    ReceivableDate = "",
                                                    CancelDate = ""
                                                });
                                                long AllPrices = Result[4] + Result[5] + MySubscription.Debt + MySubscription.deficit1000 + (Commons.Subscription * DistMonth);
                                                long NewDeficit = Commons.GetDeficit1000(AllPrices);
                                                MySubscription.Debt = AllPrices - NewDeficit;
                                                MySubscription.deficit1000 = NewDeficit;

                                                MySubscription.PrevNumber = MySubscription.CurrentNumber;
                                                MySubscription.PrevReadDate = MySubscription.CurrentReadDate;
                                                MySubscription.CurrentReadDate = TextBoxCurrentReadDate.Text.Trim();
                                                MySubscription.CurrentNumber = NewNumber;
                                                MySubscription.BillingInPeriod = Commons.CPeriod.Id;
                                                //به روز رسانی آخرین قرائت با کنتور فعلی
                                                MySubscription.WaterMeter.ReadEnd = NewNumber;
                                                MySubscription.WaterMeter.ReadDateEnd = TextBoxCurrentReadDate.Text.Trim();

                                                Commons.Db.SaveChanges();
                                                SearchAgain = true;
                                                Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.SaveMessageSuccessSingleBilling, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                                                Msg.Owner = Window.GetWindow(this);
                                                Msg.ShowDialog();
                                                EmptyControl();
                                                TextBoxSubScriptionId.Text = "";
                                                TextBoxCurrentRead.Text = "";
                                                TextBoxCurrentReadDate.Text = Commons.GetCurrentPersianDate().Trim();
                                                TextBoxSubScriptionId.Focus();
                                                TextBoxSubScriptionId.SelectAll();
                                            }
                                            catch
                                            {                                                
                                                Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                                Msg.Owner = Window.GetWindow(this);
                                                Msg.ShowDialog();
                                                TextBoxCurrentRead.SelectAll();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.FormulaIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                        Msg.Owner = Window.GetWindow(this);
                                        Msg.ShowDialog();
                                        TextBoxCurrentRead.SelectAll();
                                    }
                                }
                                else
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.ToDateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.Owner = Window.GetWindow(this);
                                    Msg.ShowDialog();
                                    TextBoxCurrentReadDate.Focus();
                                    TextBoxCurrentReadDate.SelectAll();
                                }
                            }
                            else
                            {
                                if (ChkDate == -1)
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.ToDateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.Owner = Window.GetWindow(this);
                                    Msg.ShowDialog();
                                    TextBoxCurrentReadDate.Focus();
                                    TextBoxCurrentReadDate.SelectAll();
                                }
                                else
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.DateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.Owner = Window.GetWindow(this);
                                    Msg.ShowDialog();
                                    TextBoxCurrentReadDate.Focus();
                                    TextBoxCurrentReadDate.SelectAll();
                                }
                            }
                        }
                        else
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.NumberIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            TextBoxCurrentRead.SelectAll();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.DateIsNotAllowedBiggarThanCurrentDate, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        TextBoxCurrentReadDate.Focus();
                        TextBoxCurrentReadDate.SelectAll();
                    }
                }
            }
        }
        private void ButtonDefination_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButtonRed(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColSave }, new Button[] { ButtonSave }, true);
            Ac.AnimateRowGrid(GridRowSearch, GridRowNew, GridSearch, GridNew, ref PrevHeight);
            FirstControl.Focus();
            TextBoxCurrentRead.SelectAll();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColSave }, new Button[] { ButtonSave }, false);
            Ac.SelectButtonRed(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, true);
            Ac.AnimateRowGrid(GridRowNew, GridRowSearch, GridNew, GridSearch, ref PrevHeight);
            FirstControlSearch.Focus();
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
        #endregion

        #region SearchPart
        #region Datagrid
        private void DataGridCustomers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxSearch, "شماره پرونده", "10");
        }
        #endregion Datagrid

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

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\BillingPeriod.mrt", GridHeader.Background, "قبوض دوره" + " - " + Commons.CPeriod.Name.Trim(), Dt);                    
                    PrintPrv.ShowDialog();
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSingleBilling, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSingleBilling, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }
        private void ControlSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                e.Handled = true;
                if (((ControlsTab)ControlsArraySearch[sender]).Next != null)
                {
                    ((ControlsTab)ControlsArraySearch[sender]).Next.Focus();
                }
                else
                {
                    if (e.Key == Key.Tab)
                        FirstControlSearch.Focus();
                    else
                    {
                        DoSearch();
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                e.Handled = true;
                if (((ControlsTab)ControlsArraySearch[sender]).Prev != null)
                {
                    ((ControlsTab)ControlsArraySearch[sender]).Prev.Focus();
                }
            }
        }
        private void DoSearch()
        {
            if ((LastTextBoxSearch != TextBoxSearch.Text.Trim() || SearchAgain == true) && TextBoxSearch.Text.Trim() != "")
            {
                SearchAgain = false;
                LastTextBoxSearch = TextBoxSearch.Text.Trim();
                if (RadioSearchBySubCode.IsChecked == true)
                {
                    DataGridView.ItemsSource = LoadBillsForDataGrid(LastTextBoxSearch);
                }
                else
                {
                    long id = Convert.ToInt64(LastTextBoxSearch);
                    DataGridView.ItemsSource = LoadBillsForDataGrid(id, id);
                }
            }
            if (DataGridView.Items != null && DataGridView.Items.Count != 0)
            {
                DataGridView.SelectedItem = DataGridView.Items[0];
                DataGridView.ScrollIntoView(DataGridView.SelectedItem);
                DataGridView.Focus();
            }
        }
        private void RadioSearchBySubCode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            TextBoxSearch.MaxLength = 10;
            TextBoxSearch.PreviewTextInput -= TextBox_PreviewTextInput;
            TextBoxSearch.TextChanged -= TextBox_TextChanged;
            TextBoxSearch.TextChanged += TextBoxSubScriptionId_TextChanged;
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در کد اشتراک جستجو می شود.";
        }
        private void RadioSearchByCode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            TextBoxSearch.MaxLength = 10;
            TextBoxSearch.TextChanged -= TextBoxSubScriptionId_TextChanged;
            TextBoxSearch.PreviewTextInput += TextBox_PreviewTextInput;
            TextBoxSearch.TextChanged += TextBox_TextChanged;
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در کد پرونده، کد مشترک جستجو می شود.";
        }
        private void RadioSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            PrevHint = ((RadioButton)sender).Tag.ToString();
            Hint.Text = ((RadioButton)sender).Tag.ToString();
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }
        }

        #endregion SearchPart

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("SingleBilling", "MainMenu");
        }

        #region Private Method
        private void CheckSubScriptionId(bool IsForward)
        {
            CurrentSubscriptionBilling = null;
            BindingOperations.GetBindingExpression(FirstControl, TextBox.TextProperty).UpdateSource();
            MessageDialog Msg = null;
            try
            {
                bool IsValid = true;
                string SId = "";
                ControlsValidate? Vd = Commons.ValidateData((Control)FirstControl, ControlsArraySubScriptionId);
                if (Vd != null)
                {
                    IsValid = false;
                    Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.ForceToSubscriptionId, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    EmptyControl();
                    FirstControl.Focus();
                    FirstControl.SelectAll();
                }
                if (IsValid == true)
                {
                    SId = TextBoxSubScriptionId.Text.Trim();
                    CurrentSubscriptionBilling = LoadSubscriptionForBilling(SId);

                    if (CurrentSubscriptionBilling != null)
                    {
                        FillSubScription(CurrentSubscriptionBilling);
                        if (IsForward)
                        {
                            TextBoxCurrentReadDate.Focus();
                            TextBoxCurrentReadDate.SelectAll();
                        }
                        else
                        {
                            TextBoxCurrentRead.Focus();
                            TextBoxCurrentRead.SelectAll();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.NotFoundSubscription, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        EmptyControl();
                        FirstControl.Focus();
                        FirstControl.SelectAll();
                    }
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                FirstControl.Focus();
                FirstControl.SelectAll();
            }
        }
        private SubscriptionBilling LoadSubscriptionForBilling(string Sid)
        {
            List<SubscriptionBilling> R = null;
            var tmp = from sb in Commons.Db.Subscriptions
                      where sb.Id == Sid && sb.Year == Commons.CurrentYear
                      join wt in Commons.Db.WaterMeters  on new  { SbId = sb.WaterMeterId, SbYear = sb.Year } equals new { SbId = wt.Id, SbYear = wt.Year }
                      join at in Commons.Db.AccountTypes on new  { SbA = sb.AccountTypeId, SbYear = sb.Year } equals new { SbA = at.Id, SbYear = at.Year }
                      join pt in Commons.Db.PreventTypes on new  { SbP=sb.PreventTypeId, SbYear=sb.Year }     equals new { SbP= pt.Id, SbYear = pt.Year }
                      join ct in Commons.Db.Customers    on new  { SbC=sb.CustomerId, SbYear=sb.Year }        equals new { SbC= ct.Id, SbYear = ct.Year }                      
                      select new SubscriptionBilling
                      {
                          SubScription = sb,
                          WaterMeter = wt,
                          CustId = ct.Id,
                          CustName = ct.Name,
                          CustFamily = ct.Family,
                          CustFather = ct.Father,
                          CustMeliCode = ct.MeliCode,
                          CustCellPhone = ct.CellPhone,
                          CustPostalCode = ct.PostalCode,
                          CustAddress = ct.Address,
                          PreventId = pt.Id,
                          PreventName = pt.Name,
                          AccountTypeName = at.Name,
                          AccountTypeId = at.Id,
                          AccountTypeFormula = at.Formules
                      };
            R = tmp.ToList();
            if (R.Count > 0)
                return tmp.ToList()[0];
            else
                return null;
        }
        private void FillSubScription(SubscriptionBilling CurrentSubscriptionBilling)
        {
            TextBoxCurrentRead.Text = "";
            TextBoxCurrentReadDate.Text = Commons.GetCurrentPersianDate().ToString();
            TextBlockCustomerId.Text = CurrentSubscriptionBilling.CustId.ToString();
            TextBlockName.Text = CurrentSubscriptionBilling.CustName;
            TextBlockFamily.Text = CurrentSubscriptionBilling.CustFamily;
            TextBlockFather.Text = CurrentSubscriptionBilling.CustFather;
            TextBlockMelicode.Text = CurrentSubscriptionBilling.CustMeliCode;
            TextBlockPostalCode.Text = CurrentSubscriptionBilling.CustPostalCode;
            TextBlockMobile.Text = CurrentSubscriptionBilling.CustCellPhone;
            TextBlockAddress.Text = CurrentSubscriptionBilling.CustAddress;
            TextBlockAccountType.Text = CurrentSubscriptionBilling.AccountTypeName;
            TextBlockPreventType.Text = CurrentSubscriptionBilling.PreventName;
            TextBlockWaterMeterSerial.Text = CurrentSubscriptionBilling.WaterMeter.WaterMeterSerial;
            TextBlockSubscriptionPostalCode.Text = CurrentSubscriptionBilling.SubScription.PostalCode;
            TextBlockCurrentReadDate.Text = CurrentSubscriptionBilling.SubScription.CurrentReadDate;
            TextBlockCurrentNumber.Text = CurrentSubscriptionBilling.SubScription.CurrentNumber.ToString();
            TextBlockDeficit1000.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.deficit1000);
            FirstControl.Focus();
            TextBlockDebt.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.Debt);
            TextBlockSubscriptionAddress.Text = CurrentSubscriptionBilling.SubScription.Address;
        }
        private List<BillingDetails> LoadBillsForDataGrid(string SubId)
        {
            try
            {
                var tmp = from sb in Commons.Db.Subscriptions         
                          where sb.Year == Commons.CurrentYear && sb.BillingInPeriod == Commons.CPeriod.Id && sb.Id == SubId
                          join ct in Commons.Db.Customers on new { SbC = sb.CustomerId, SbY = sb.CustomerYear } equals new { SbC = ct.Id, SbY = ct.Year }
                          join b in Commons.Db.Bills on new { SbB = sb.Id, SbY = sb.Year } equals new { SbB = b.SubscriptionId, SbY = b.SubscriptionYear }
                          join at in Commons.Db.AccountTypes on new { SbA = b.AccountTypeId, SbY= b.AccountTypeYear }  equals new {SbA = at.Id,  SbY= at.Year }
                          join pt in Commons.Db.PreventTypes on new { SbP = b.PreventTypeId, SbY = b.PreventTypeYear } equals new {SbP = pt.Id, SbY= pt.Year }
                          join wt in Commons.Db.WaterMeters on new { SbW = b.WaterMeterId, SbY = b.WaterMeterYear } equals new { SbW = wt.Id, SbY = wt.Year }                          
                          where b.CurrentPeriod == Commons.CPeriod.Id && b.Year == Commons.CurrentYear
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
                              deficit1000 = sb.deficit1000,
                              Prevdeficit1000 = b.Prevdeficit1000,
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
                              PayablePrice = sb.Debt,
                              Status = b.Status                              
                          };
                return tmp.ToList();
            }
            catch
            {
                return null;
            }
        }
        private List<BillingDetails> LoadBillsForDataGrid(long CId, long BId)
        {
            try
            {
                var tmp = from sb in Commons.Db.Subscriptions
                          where sb.Year == Commons.CurrentYear && sb.BillingInPeriod == Commons.CPeriod.Id
                          join ct in Commons.Db.Customers on new { SbC = sb.CustomerId, SbY = sb.CustomerYear } equals new { SbC = ct.Id, SbY = ct.Year }
                          join b in Commons.Db.Bills on new { SbB = sb.Id, SbY = sb.Year } equals new { SbB = b.SubscriptionId, SbY = b.SubscriptionYear }
                          join at in Commons.Db.AccountTypes on new { SbA = b.AccountTypeId, SbY = b.AccountTypeYear } equals new { SbA = at.Id, SbY = at.Year }
                          join pt in Commons.Db.PreventTypes on new { SbP = b.PreventTypeId, SbY = b.PreventTypeYear } equals new { SbP = pt.Id, SbY = pt.Year }
                          join wt in Commons.Db.WaterMeters on new { SbW = b.WaterMeterId, SbY = b.WaterMeterYear } equals new { SbW = wt.Id, SbY = wt.Year }                          
                          where b.CurrentPeriod == Commons.CPeriod.Id && b.Year == Commons.CurrentYear && (ct.Id == CId || b.Id == BId)
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
                              deficit1000 = sb.deficit1000,
                              Prevdeficit1000 = b.Prevdeficit1000,
                              CurrentReadDate = b.CurrentReadDate,
                              CurrentNumber = b.CurrentNumber,
                              PrevReadDate = b.PrevReadDate,
                              PrevNumber = b.PrevNumber,                              
                              AllowableConsumption = b.AllowableConsumption,
                              Consumption = b.Consumption,
                              ExtraConsumption = b.ExtraConsumption,
                              PriceOfAllowableConsumption = b.PriceOfAllowableConsumption,
                              SubscriptionCost = b.SubscriptionCost,
                              PriceOfConsumption = b.PriceOfConsumption,
                              PriceOfExtraConsumption = b.PriceOfExtraConsumption,
                              Vat = b.Vat,
                              SumOfPeriod = b.PriceOfConsumption + b.PrevDebt + b.Vat,
                              AllPrices = b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000 + b.SubscriptionCost,
                              PayablePrice = sb.Debt,
                              Status = b.Status                              
                          };
                return tmp.ToList();
            }
            catch
            {
                return null;
            }
        }
        public int CheckPeriod()
        {
            Commons.SetPeriod();
            if (Commons.CPeriod.Id >= 1)
            {
                if (Commons.CPeriod.IsClosed == false)
                {
                    TextBlockPeriod.Text = Commons.CPeriod.Name;
                    return 0;
                }
                else
                    return -2;
            }
            else
                return -1;
        }
        private void EmptyControl()
        {
            TextBoxCurrentRead.Text = "";
            TextBlockCustomerId.Text = "";
            TextBlockName.Text = "";
            TextBlockFamily.Text = "";
            TextBlockFather.Text = "";
            TextBlockMelicode.Text = "";
            TextBlockPostalCode.Text = "";
            TextBlockMobile.Text = "";
            TextBlockAddress.Text = "";
            TextBlockAccountType.Text = "";
            TextBlockPreventType.Text = "";
            TextBlockWaterMeterSerial.Text = "";
            TextBlockSubscriptionPostalCode.Text = "";
            TextBlockCurrentReadDate.Text = "";
            TextBlockCurrentNumber.Text = "";
            TextBlockDeficit1000.Text = "";
            TextBlockDebt.Text = "";
            TextBlockSubscriptionAddress.Text = "";
        }
        #endregion
    }
}
