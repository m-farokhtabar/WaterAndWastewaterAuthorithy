using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterAndWastewaterAuthorithy.Presentation;
using WaterAndWastewaterAuthorithy.Validations;
using System.Data;
using System.Data.Entity;
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for BillsCancelling.xaml
    /// </summary>
    public partial class BillsCancelling : UserControl
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
        SubscriptionBilling CurrentSubscriptionBilling = null;
        string PrevHint = "";
        bool SearchAgain = false;

        public BillsCancelling()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionId;
            ControlsArraySubScriptionId.Add(TextBoxSubScriptionId, new ControlsTab { Prev = null, Next = null, Type = ControlsType.TextBoxText });

            ControlsArray.Add(TextBoxSubScriptionId, new ControlsTab { Prev = null, Next = null, Type = ControlsType.TextBoxText });

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
                    TextBoxSubScriptionId.IsEnabled = false;
                    ButtonCancel.IsEnabled = false;
                    MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    Ac.ShowWindow("BillsReceivable", "MainMenu");
                    return;
                }
                else
                {
                    if (Ch == -2)
                    {
                        EmptyControl();
                        TextBlockPeriod.Text = "";
                        TextBoxSubScriptionId.Text = "";
                        TextBoxSubScriptionId.IsEnabled = false;
                        ButtonCancel.IsEnabled = false;
                        MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.PeriodIsClosedBillingCancel, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                    else
                    {
                        TextBoxSubScriptionId.IsEnabled = true;
                        ButtonCancel.IsEnabled = true;
                        FirstControl.Focus();
                        FirstControl.SelectAll();
                    }
                }
                if (Commons.EditAccountType[3] == true || Commons.EditCustomer[3] == true || Commons.EditSubScription[2] == true || Commons.EditBillPeriod[2] == true || Commons.EditSettings[3] == true)
                {
                    EmptyControl();
                    TextBoxSubScriptionId.Text = "";
                    Commons.EditAccountType[3] = false;
                    Commons.EditCustomer[3] = false;
                    Commons.EditSubScription[2] = false;
                    Commons.EditBillPeriod[2] = false;
                    Commons.EditSettings[3] = false;
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

            if (e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonSave_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }

        #region To Control TextBox For TabStop And Validate Value
        private void ButtonCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                FirstControl.Focus();
                ((TextBox)FirstControl).SelectAll();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (sender == (Control)FirstControl)
                {
                    CheckSubScriptionId();
                    e.Handled = true;
                    return;
                }
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    e.Handled = true;
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                    Ctrl.Focus();
                }
                else
                {
                    e.Handled = true;
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
                    CheckSubScriptionId();
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
                    Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    Vd.Value.Control.Focus();
                }
                else
                {
                    DomainClasses.SubscriptionsTb MySubscription = CurrentSubscriptionBilling.SubScription;
                    //در صورتی که قبض مربوطه مربوط به کنتور قبلی باشد امکان ابطال آن وجود ندارد
                    if (CurrentSubscriptionBilling.SubScription.WaterMeterId == CurrentSubscriptionBilling.Bill.WaterMeterId && CurrentSubscriptionBilling.SubScription.WaterMeterYear == CurrentSubscriptionBilling.Bill.WaterMeterYear)
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.SaveMessageBillsCancelling, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        if (Msg.ShowDialog() == true)
                        {
                            try
                            {
                                long PrevDebt = CurrentSubscriptionBilling.Bill.PrevDebt;
                                long PrevDeficit1000 = CurrentSubscriptionBilling.Bill.Prevdeficit1000;
                                string PrevPrevReadDate = CurrentSubscriptionBilling.Bill.PrevPrevReadDate
                                     , PrevReadDate = MySubscription.PrevReadDate;
                                long PrevPrevNumber = CurrentSubscriptionBilling.Bill.PrevPrevNumber
                                     , PrevNumber = MySubscription.PrevNumber;

                                if (CurrentSubscriptionBilling.Bill.Status == BillsStatus.ReceivableFull || CurrentSubscriptionBilling.Bill.Status == BillsStatus.ReceivableNotMatch)
                                {
                                    PrevDebt = PrevDebt - CurrentSubscriptionBilling.Bill.ReceivablePrice;
                                    PrevDeficit1000 = PrevDeficit1000 - CurrentSubscriptionBilling.Bill.ReceivableDefict1000;
                                }

                                CurrentSubscriptionBilling.Bill.CancelDate = Commons.GetCurrentPersianDate();
                                CurrentSubscriptionBilling.Bill.Status = BillsStatus.Cancel;

                                MySubscription.Debt = PrevDebt;
                                MySubscription.deficit1000 = PrevDeficit1000;
                                MySubscription.PrevReadDate = PrevPrevReadDate;
                                MySubscription.PrevNumber = PrevPrevNumber;
                                MySubscription.CurrentReadDate = PrevReadDate;
                                MySubscription.CurrentNumber = PrevNumber;
                                //به روز رسانی آخرین قرائت با کنتور فعلی
                                MySubscription.WaterMeter.ReadEnd = PrevNumber;
                                MySubscription.WaterMeter.ReadDateEnd = PrevReadDate;

                                Commons.Db.SaveChanges();

                                Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.SaveMessageSuccessBillsCancelling, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                                EmptyControl();
                                TextBoxSubScriptionId.Text = "";
                                TextBoxSubScriptionId.Focus();
                                TextBoxSubScriptionId.SelectAll();
                            }
                            catch
                            {
                                Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                                ButtonCancel.Focus();
                            }
                        }
                    }
                    //در صورتی که قبض مربوطه مربوط به کنتور قبلی باشد امکان ابطال آن وجود ندارد
                    else
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.CanNotCancelBillBecauseOfWaterMeter, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        ButtonCancel.Focus();
                    }
                }
            }
            else
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                TextBoxSubScriptionId.Text = "";
                TextBoxSubScriptionId.Focus();
                TextBoxSubScriptionId.SelectAll();
            }
        }
        private void ButtonDefination_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButtonRed(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { }, new Button[] { }, true);
            Ac.AnimateRowGrid(GridRowSearch, GridRowNew, GridSearch, GridNew, ref PrevHeight);
            FirstControl.Focus();
            TextBoxSubScriptionId.SelectAll();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { }, new Button[] { }, false);
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

        #region Datagrid
        private void DataGridCustomers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxSearch, "شماره پرونده", "مبلغ ابطالی قبض");
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

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\BillingCancel.mrt", GridHeader.Background, "قبوض ابطال شده دوره" + " - " + Commons.CPeriod.Name.Trim(), Dt);
                    PrintPrv.ShowDialog();
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleBillsReceivable, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleBillsReceivable, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }

        #endregion SearchPart

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("BillsCancelling", "MainMenu");
        }

        #region Private Method
        private void CheckSubScriptionId()
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
                    Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.ForceToSubscriptionId, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
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
                        ButtonCancel.Focus();
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.NotFoundSubscriptionOrBillForCancelling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
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
                Msg = new MessageDialog(Messages.SaveMessageTitleBillsCancelling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
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
                      where sb.Year == Commons.CurrentYear && sb.Id == Sid
                      join ct in Commons.Db.Customers on new { SbC = sb.CustomerId, SbY = sb.CustomerYear } equals new { SbC = ct.Id, SbY = ct.Year }
                      join b in Commons.Db.Bills on new { SbB = sb.Id, SbY = sb.Year } equals new { SbB = b.SubscriptionId, SbY = b.SubscriptionYear }
                      join at in Commons.Db.AccountTypes on new { SbA = b.AccountTypeId, SbY = b.AccountTypeYear } equals new { SbA = at.Id, SbY = at.Year }
                      join atSub in Commons.Db.AccountTypes on new { SbA = sb.AccountTypeId, SbY = sb.AccountTypeYear } equals new { SbA = atSub.Id, SbY = atSub.Year }
                      join wtSub in Commons.Db.WaterMeters on new { SbA = sb.WaterMeterId, SbY = sb.WaterMeterYear } equals new { SbA = wtSub.Id, SbY = wtSub.Year }
                      join pt in Commons.Db.PreventTypes on new { SbP = b.PreventTypeId, SbY = b.PreventTypeYear } equals new { SbP = pt.Id, SbY = pt.Year }
                      join wt in Commons.Db.WaterMeters on new { SbW = b.WaterMeterId, SbY = b.WaterMeterYear } equals new { SbW = wt.Id, SbY = wt.Year }
                      where b.Status != BillsStatus.Cancel
                      orderby b.Id descending
                      select new SubscriptionBilling
                      {
                          SubScription = sb,
                          WaterMeter = wt,
                          Bill = b,
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
                          AccountTypeFormula = at.Formules,
                          SubAccountTypeName = atSub.Name,
                          SubWaterMeterSerial = wtSub.WaterMeterSerial
                      };
            R = tmp.ToList();
            if (R.Count > 0)
                return tmp.ToList()[0];
            else
                return null;
        }
        private void FillSubScription(SubscriptionBilling CurrentSubscriptionBilling)
        {
            TextBlockCustomerId.Text = CurrentSubscriptionBilling.CustId.ToString();
            TextBlockName.Text = CurrentSubscriptionBilling.CustName;
            TextBlockFamily.Text = CurrentSubscriptionBilling.CustFamily;
            TextBlockFather.Text = CurrentSubscriptionBilling.CustFather;
            TextBlockMelicode.Text = CurrentSubscriptionBilling.CustMeliCode;
            TextBlockPostalCode.Text = CurrentSubscriptionBilling.CustPostalCode;
            TextBlockMobile.Text = CurrentSubscriptionBilling.CustCellPhone;
            TextBlockAddress.Text = CurrentSubscriptionBilling.CustAddress;
            TextBlockAccountType.Text = CurrentSubscriptionBilling.SubAccountTypeName;
            TextBlockPreventType.Text = CurrentSubscriptionBilling.PreventName;
            TextBlockWaterMeterSerial.Text = CurrentSubscriptionBilling.SubWaterMeterSerial;
            TextBlockSubscriptionPostalCode.Text = CurrentSubscriptionBilling.SubScription.PostalCode;
            TextBlockCurrentReadDate.Text = CurrentSubscriptionBilling.SubScription.CurrentReadDate;
            TextBlockCurrentNumber.Text = CurrentSubscriptionBilling.SubScription.CurrentNumber.ToString();
            TextBlockDeficit1000.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.deficit1000);
            FirstControl.Focus();
            TextBlockDebt.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.Debt);
            TextBlockSubscriptionAddress.Text = CurrentSubscriptionBilling.SubScription.Address;


            TextBlockBillId.Text = CurrentSubscriptionBilling.Bill.Id.ToString();

            TextBlockCurrentNumberBill.Text = CurrentSubscriptionBilling.Bill.CurrentNumber.ToString();
            TextBlockCurrentReadDateBill.Text = CurrentSubscriptionBilling.Bill.CurrentReadDate;
            TextBlockPrevNumberBill.Text = CurrentSubscriptionBilling.Bill.PrevNumber.ToString();
            TextBlockPrevReadDateBill.Text = CurrentSubscriptionBilling.Bill.PrevReadDate;

            TextBlockDebtBill.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.Bill.PrevDebt);
            TextBlockPriceOFAllConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.PriceOfConsumption.ToString());
            TextBlockSubscriptionCost.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.SubscriptionCost.ToString());
            TextBlockPriceOFConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.PriceOfAllowableConsumption.ToString());
            TextBlockPriceOfExtraConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.PriceOfExtraConsumption.ToString());
            TextBlockVat.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.Vat.ToString());

            long PriceOFAll = CurrentSubscriptionBilling.Bill.PriceOfConsumption + CurrentSubscriptionBilling.Bill.PrevDebt + CurrentSubscriptionBilling.Bill.Vat + CurrentSubscriptionBilling.Bill.Prevdeficit1000;
            long PriceOFPeriod = CurrentSubscriptionBilling.Bill.PriceOfConsumption + CurrentSubscriptionBilling.Bill.PrevDebt + CurrentSubscriptionBilling.Bill.Vat;
            TextBlockPrevdeficit1000.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.Bill.Prevdeficit1000);
            TextBlockDeficit1000Bill.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.deficit1000);
            TextBlockPriceOFPeriod.Text = Commons.ConvertToMoneyWithSign(PriceOFPeriod);
            TextBlockPriceOFAll.Text = Commons.ConvertToMoneyWithSign(PriceOFAll);
            TextBlockPayablePrice.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.Debt);

            TextBlockAllConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.Consumption.ToString());
            TextBlockAllowableConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.AllowableConsumption.ToString());
            TextBlockExtraConsumption.Text = Commons.ConvertToMoney(CurrentSubscriptionBilling.Bill.ExtraConsumption.ToString());

            TextBlockAccountTypeBill.Text = CurrentSubscriptionBilling.AccountTypeName;
            TextBlockPreventTypeBill.Text = CurrentSubscriptionBilling.PreventName;
            TextBlockWaterMeterSerialBill.Text = CurrentSubscriptionBilling.WaterMeter.WaterMeterSerial;

        }
        private void EmptyControl()
        {
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

            TextBlockBillId.Text = "";

            TextBlockCurrentNumberBill.Text = "";
            TextBlockCurrentReadDateBill.Text = "";
            TextBlockPrevNumberBill.Text = "";
            TextBlockPrevReadDateBill.Text = "";

            TextBlockDebtBill.Text = "";

            TextBlockPriceOFAll.Text = "";
            TextBlockPriceOFAllConsumption.Text = "";
            TextBlockSubscriptionCost.Text = "";
            TextBlockPriceOFConsumption.Text = "";
            TextBlockPriceOfExtraConsumption.Text = "";
            TextBlockPriceOFPeriod.Text = "";
            TextBlockPayablePrice.Text = "";

            TextBlockAllConsumption.Text = "";
            TextBlockAllowableConsumption.Text = "";
            TextBlockExtraConsumption.Text = "";

            TextBlockAccountTypeBill.Text = "";
            TextBlockPreventTypeBill.Text = "";


            TextBlockWaterMeterSerialBill.Text = "";
            TextBlockVat.Text = "";

            TextBlockPrevdeficit1000.Text = "";
            TextBlockDeficit1000Bill.Text = "";
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
        private List<BillingDetails> LoadBillsForDataGrid(string SubId)
        {
            try
            {
                var tmp = from sb in Commons.Db.Subscriptions
                          where sb.Year == Commons.CurrentYear && sb.BillingInPeriod == Commons.CPeriod.Id && sb.Id == SubId
                          join ct in Commons.Db.Customers on new { SbC = sb.CustomerId, SbY = sb.CustomerYear } equals new { SbC = ct.Id, SbY = ct.Year }
                          join b in Commons.Db.Bills on new { SbB = sb.Id, SbY = sb.Year } equals new { SbB = b.SubscriptionId, SbY = b.SubscriptionYear }
                          join at in Commons.Db.AccountTypes on new { SbA = b.AccountTypeId, SbY = b.AccountTypeYear } equals new { SbA = at.Id, SbY = at.Year }
                          join pt in Commons.Db.PreventTypes on new { SbP = b.PreventTypeId, SbY = b.PreventTypeYear } equals new { SbP = pt.Id, SbY = pt.Year }
                          join wt in Commons.Db.WaterMeters on new { SbW = b.WaterMeterId, SbY = b.WaterMeterYear } equals new { SbW = wt.Id, SbY = wt.Year }
                          where b.CurrentPeriod == Commons.CPeriod.Id && b.Year == Commons.CurrentYear && b.Status == BillsStatus.Cancel
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
                              Status = b.Status,
                              ReceivablePrevDebt = b.ReceivablePrevDebt,
                              ReceivableDate = b.ReceivableDate,
                              ReceivablePrice = b.ReceivablePrice,
                              CancelDate = b.CancelDate,
                              CancelPrice = b.PriceOfConsumption + b.Vat
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
                          where b.CurrentPeriod == Commons.CPeriod.Id && b.Year == Commons.CurrentYear && (ct.Id == CId || b.Id == BId) && b.Status == BillsStatus.Cancel
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
                              Status = b.Status,
                              ReceivablePrevDebt = b.ReceivablePrevDebt,
                              ReceivableDate = b.ReceivableDate,
                              ReceivablePrice = b.ReceivablePrice,
                              ReceivablePrevDefict1000 = b.ReceivablePrevdeficit1000,
                              CancelDate = b.CancelDate
                          };
                return tmp.ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
