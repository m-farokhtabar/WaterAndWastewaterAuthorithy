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
using System.Windows.Threading;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Billing.xaml
    /// </summary>
    public partial class Billing : UserControl
    {
        string LastTextBoxSearch = "";
        SelectedButton PrvButton = new SelectedButton();
        AnimateControls Ac = new AnimateControls();
        GridLength PrevHeight;
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        BillPeriodsTb CurrentBillPeriod = null;
        List<BillingDetails> Bills;
        public bool FormLoaded = false;
        bool CurrentPeriodIsFilled = false;
        bool CurentPeriodNotSelectedOrNotCreated = false;

        List<SubscriptionBilling> ThisPeriodBillsList;
        SubscriptionBilling CurrentSubscriptionBilling;
        int Index = 0;
        public bool LoadThisPeriodBillsList = false;

        WaitingDialog Wd;

        public Billing()
        {
            InitializeComponent();
            FirstControl = TextBoxCurrentRead;
            ControlsArray.Add(TextBoxCurrentRead, new ControlsTab { Prev = TextBoxCurrentRead, Next = null, Type = ControlsType.TextBoxNumber });
        }
        private void UserControlBilling_Loaded(object sender, RoutedEventArgs e)
        {
            Ac.SelectButton(ButtonDefination, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);

            PrevHeight = GridRowSearch.Height;
            GridRowSearch.Height = new GridLength(0);
            GridSearch.Visibility = System.Windows.Visibility.Hidden;
        }
        private void UserControlBilling_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue))
            {
                if (FormLoaded == true || Commons.EditAccountType[1] == true || Commons.EditCustomer[1] == true || Commons.EditSubScription[0] == true || Commons.EditBillPeriod[0] == true)
                {
                    Wd = new WaitingDialog(Messages.LoadSubTitle, Messages.WaitingLoadSub, GridHeader.Background);
                    Wd.Show();
                    LoadThisPeriodBillsList = true;
                    if (LoadFirst() == -1)
                    {
                        MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.ShowDialog();
                        TextBoxCurrentRead.SelectAll();
                    }
                    Commons.EditAccountType[1] = false;
                    Commons.EditCustomer[1] = false;
                    Commons.EditSubScription[0] = false;
                    Commons.EditBillPeriod[0] = false;
                    FormLoaded = false;
                    Wd.Close();
                }
                else
                {
                    if (CurrentPeriodIsFilled)
                    {
                        MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SubScriptionNextNotFound, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.ShowDialog();
                        TextBoxCurrentRead.SelectAll();
                    }
                    if (CurentPeriodNotSelectedOrNotCreated)
                    {
                        MessageDialog Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.ShowDialog();
                        TextBoxCurrentRead.SelectAll();
                    }
                }
            }
        }

        #region To Control TextBox For TabStop And Validate Value
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
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
                        PreSave();
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
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
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            BindingOperations.GetBindingExpression(text, TextBox.TextProperty).UpdateSource();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox Tb = (TextBox)sender;
            Tb.SelectAll();
        }
        #endregion

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SaveMessageBilling, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                if (Msg.ShowDialog() == true)
                {
                    Commons.Db.SaveChanges();
                    LoadBills();
                    Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SaveMessageSuccessBilling, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                    Msg.ShowDialog();
                    TextBoxCurrentRead.Focus();
                    TextBoxCurrentRead.SelectAll();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.ShowDialog();
                TextBoxCurrentRead.Focus();
                TextBoxCurrentRead.SelectAll();
            }

        }
        private void ButtonDefination_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColSave }, new Button[] { ButtonSave }, true);
            Ac.AnimateRowGrid(GridRowSearch, GridRowNew, GridSearch, GridNew, ref PrevHeight);
            FirstControl.Focus();
            TextBoxCurrentRead.SelectAll();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColSave }, new Button[] { ButtonSave }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, true);
            Ac.AnimateRowGrid(GridRowNew, GridRowSearch, GridNew, GridSearch, ref PrevHeight);
            TextBoxSearch.Focus();
            DataGridView.ItemsSource = Bills;
            DataGridView.Items.Refresh();
        }
        #endregion

        #region SearchPart
        private void TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                DataGridView.Focus();
            }
        }
        private void TextBoxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LastTextBoxSearch != TextBoxSearch.Text.Trim())
            {
                LastTextBoxSearch = TextBoxSearch.Text.Trim();
                DataGridView.ItemsSource = Bills.Where(x => x.Search.Contains(TextBoxSearch.Text.Trim()));
                if (DataGridView.Items != null || DataGridView.Items.Count != 0)
                    DataGridView.SelectedIndex = 0;
            }
        }

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
            IEnumerable<BillingDetails> Tb = Bills.Where(x => x.Search.Contains(TextBoxSearch.Text.Trim()));
            DataTable Dt = Commons.ToDataTable<BillingDetails>(Tb);
            Dt.TableName = "BillsPeriod";

            PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\BillingPeriod.mrt", GridHeader.Background, "قبوض دوره ای" + " - " + CurrentBillPeriod.Name.Trim(), Dt);
            PrintPrv.ShowDialog();
            this.Cursor = tmp;
        }

        #endregion SearchPart

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Billing", "MainMenu");
        }

        #region Private Method
        private void DoNext()
        {
            MessageDialog Msg;
            int FeedBack = LoadNextSubScription();
            if (FeedBack == -1)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SubScriptionNextNotFound, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.ShowDialog();
                TextBoxCurrentRead.SelectAll();
            }
            else if (FeedBack == -2)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SubScriptionIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.ShowDialog();
                TextBoxCurrentRead.SelectAll();
            }
            else if (FeedBack == -3)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SubScriptionNotFound, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.ShowDialog();
                TextBoxCurrentRead.SelectAll();
            }
        }
        private int LoadNextSubScription()
        {
            CurrentSubscriptionBilling = null;
            if (LoadThisPeriodBillsList)
            {
                LoadSubscriptionForBilling();
                LoadThisPeriodBillsList = false;
            }

            if (ThisPeriodBillsList.Count != 0)
                CurrentSubscriptionBilling = ThisPeriodBillsList[Index];
            if (CurrentSubscriptionBilling == null)
            {
                CurrentPeriodIsFilled = true;
                EmptyControl();
                return -1;
            }
            else
                CurrentPeriodIsFilled = false;

            TextBlockSubScriptionId.Text = CurrentSubscriptionBilling.SubScription.Id;
            TextBoxCurrentRead.Text = "";
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
            TextBlockWaterMeterSerial.Text = CurrentSubscriptionBilling.SubScription.WaterMeterSerial;
            TextBlockSubscriptionPostalCode.Text = CurrentSubscriptionBilling.SubScription.PostalCode;
            TextBlockCurrentReadDate.Text = CurrentSubscriptionBilling.SubScription.CurrentReadDate;
            TextBlockCurrentNumber.Text = CurrentSubscriptionBilling.SubScription.CurrentNumber.ToString();
            TextBlockDeficit1000.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.deficit1000);
            FirstControl.Focus();
            TextBlockDebt.Text = Commons.ConvertToMoneyWithSign(CurrentSubscriptionBilling.SubScription.Debt);
            TextBlockSubscriptionAddress.Text = CurrentSubscriptionBilling.SubScription.Address;
            Index++;
            return 0;
        }
        private void LoadBills()
        {
            int Year = Commons.GetCurrentYear();
            var tmp = from sb in Commons.Db.Subscriptions
                      join at in Commons.Db.AccountTypes on sb.AccountTypeId equals at.Id
                      join pt in Commons.Db.PreventTypes on sb.PreventTypeId equals pt.Id
                      join ct in Commons.Db.Customers on sb.CustomerId equals ct.Id
                      join b in Commons.Db.Bills on sb.Id equals b.SubscriptionId
                      where b.Year == Year && b.CurrentPeriod == CurrentBillPeriod.Id
                      orderby b.Id
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
                          PriceOfExtraConsumption = b.PriceOfExtraConsumption,
                          Vat = b.Vat,
                          SumOfPeriod = b.PriceOfConsumption + b.PrevDebt + b.Vat,
                          AllPrices = b.PriceOfConsumption + b.PrevDebt + b.Vat + b.Prevdeficit1000,
                          PayablePrice = sb.Debt
                      };
            Bills = tmp.ToList();
            DataGridView.ItemsSource = Bills;
        }
        private void LoadSubscriptionForBilling()
        {
            long M1 = DateTime.Now.Ticks;
            var tmp = from sb in Commons.Db.Subscriptions
                      join at in Commons.Db.AccountTypes on sb.AccountTypeId equals at.Id
                      join pt in Commons.Db.PreventTypes on sb.PreventTypeId equals pt.Id
                      join ct in Commons.Db.Customers on sb.CustomerId equals ct.Id
                      where sb.BillingInPeriod < CurrentBillPeriod.Id
                      orderby sb.Id
                      select new SubscriptionBilling
                      {
                          SubScription = sb,
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
            ThisPeriodBillsList = tmp.ToList();
            long M2 = DateTime.Now.Ticks;
            //MessageBox.Show((M2 - M1).ToString());
        }
        public int LoadFirst()
        {
            CurrentBillPeriod = null;
            int Cy = Commons.GetCurrentYear();
            CurrentBillPeriod = Commons.Db.BillPeriods.Local.Select(x => x).Where(x => x.Year == Cy && x.IsSelected == true && x.IsClosed == false).FirstOrDefault();
            if (CurrentBillPeriod != null)
            {
                CurentPeriodNotSelectedOrNotCreated = false;
                TextBlockNewDateRead.Text = CurrentBillPeriod.DateTo;
                TextBlockPeriod.Text = CurrentBillPeriod.Name;
                DoNext();
                LoadBills();
            }
            else
            {
                CurentPeriodNotSelectedOrNotCreated = true;
                return -1;
            }
            return 0;
        }
        private void EmptyControl()
        {
            TextBlockSubScriptionId.Text = "";
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
        private void PreSave()
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArray);
            MessageDialog Msg = null;
            bool Next = false;
            if (CurrentBillPeriod != null)
            {
                if (Vd != null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.ShowDialog();
                    Vd.Value.Control.Focus();
                }
                else
                {
                    if (string.Compare(CurrentBillPeriod.DateTo.Trim(), Commons.GetCurrentPersianDate()) <= 0)
                    {
                        DomainClasses.SubscriptionsTb MySubscription = CurrentSubscriptionBilling.SubScription;
                        long NewNumber = Convert.ToInt64(TextBoxCurrentRead.Text);
                        if (NewNumber == 0)
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.SaveMessageBillingNext, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                            if (Msg.ShowDialog() == true)
                            {
                                MySubscription.BillingInPeriod = CurrentBillPeriod.Id;
                                //Commons.Db.Entry(MySubscription).CurrentValues.SetValues(MySubscription);
                                //Commons.Db.SaveChanges();
                                Next = true;
                                DoNext();
                            }
                            TextBoxCurrentRead.SelectAll();
                            return;
                        }
                        long MyConsumption = NewNumber - MySubscription.CurrentNumber;
                        int ChkDate = 0;
                        if (MyConsumption >= 0)
                        {
                            ChkDate = Commons.CheckDateFromTo(MySubscription.CurrentReadDate, CurrentBillPeriod.DateTo);
                            int DistMonth = 0;
                            if (ChkDate == 0)
                            {
                                DistMonth = Commons.DistanceBetweenMonthsForBilling(MySubscription.CurrentReadDate, CurrentBillPeriod.DateTo);
                                if (DistMonth > 0)
                                {
                                    long[] Result = Commons.Consumption(MyConsumption, CurrentSubscriptionBilling.AccountTypeFormula, DistMonth);
                                    if (Result != null)
                                    {
                                        try
                                        {
                                            BillsTb NewBill = Commons.Db.Bills.Add(new DomainClasses.BillsTb
                                            {
                                                SubscriptionId = MySubscription.Id,
                                                Status = BillsStatus.Billing,
                                                PrevNumber = MySubscription.CurrentNumber,
                                                PrevReadDate = MySubscription.CurrentReadDate,
                                                CurrentNumber = NewNumber,
                                                CurrentReadDate = CurrentBillPeriod.DateTo,
                                                Consumption = MyConsumption,
                                                AllowableConsumption = Result[0],
                                                ExtraConsumption = Result[1],
                                                PriceOfConsumption = Result[4],
                                                PriceOfAllowableConsumption = Result[2],
                                                PriceOfExtraConsumption = Result[3],
                                                Vat = Result[5],
                                                Year = Commons.GetCurrentYear(),
                                                CurrentPeriod = CurrentBillPeriod.Id,
                                                PrevDebt = MySubscription.Debt,
                                                Prevdeficit1000 = MySubscription.deficit1000,
                                                PrevPrevNumber = MySubscription.PrevNumber,
                                                PrevPrevReadDate = MySubscription.PrevReadDate,
                                                WaterMeterSerial = MySubscription.WaterMeterSerial,
                                                AccountTypeId = MySubscription.AccountTypeId,
                                                PreventTypeId = MySubscription.PreventTypeId,
                                                ReceivableDate = ""
                                            });
                                            long AllPrices = Result[4] + Result[5] + MySubscription.Debt + MySubscription.deficit1000;
                                            long NewDeficit = Commons.GetDeficit1000(AllPrices);
                                            MySubscription.Debt = AllPrices - NewDeficit;
                                            MySubscription.deficit1000 = NewDeficit;

                                            MySubscription.PrevNumber = MySubscription.CurrentNumber;
                                            MySubscription.PrevReadDate = MySubscription.CurrentReadDate;
                                            MySubscription.CurrentReadDate = CurrentBillPeriod.DateTo;
                                            MySubscription.CurrentNumber = NewNumber;
                                            MySubscription.BillingInPeriod = CurrentBillPeriod.Id;
                                            Bills.Add(new BillingDetails
                                            {
                                                Id = NewBill.Id,
                                                SubId = CurrentSubscriptionBilling.SubScription.Id,
                                                CustId = CurrentSubscriptionBilling.CustId,
                                                Name = CurrentSubscriptionBilling.CustName,
                                                Family = CurrentSubscriptionBilling.CustFamily,
                                                Father = CurrentSubscriptionBilling.CustFather,
                                                Prevent = CurrentSubscriptionBilling.PreventName,
                                                AccounType = CurrentSubscriptionBilling.AccountTypeName,
                                                PrevDebt = CurrentSubscriptionBilling.SubScription.Debt,
                                                deficit1000 = NewDeficit,
                                                Prevdeficit1000 = CurrentSubscriptionBilling.SubScription.deficit1000,
                                                CurrentReadDate = CurrentBillPeriod.DateTo,
                                                CurrentNumber = NewNumber,
                                                PrevReadDate = CurrentSubscriptionBilling.SubScription.CurrentReadDate,
                                                PrevNumber = CurrentSubscriptionBilling.SubScription.CurrentNumber,
                                                AllowableConsumption = Result[0],
                                                Consumption = MyConsumption,
                                                ExtraConsumption = Result[1],
                                                PriceOfAllowableConsumption = Result[2],
                                                PriceOfConsumption = Result[4],
                                                PriceOfExtraConsumption = Result[3],
                                                Vat = Result[5],
                                                SumOfPeriod = Result[4] + CurrentSubscriptionBilling.SubScription.Debt + Result[5],
                                                AllPrices = AllPrices,
                                                PayablePrice = AllPrices - NewDeficit
                                            });
                                            Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.PreSaveMessageSuccessBilling, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                                            Msg.ShowDialog();
                                            Next = true;
                                        }
                                        catch
                                        {
                                            Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                            Msg.ShowDialog();
                                            TextBoxCurrentRead.SelectAll();
                                        }
                                    }
                                    else
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.FormulaIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                        Msg.ShowDialog();
                                        TextBoxCurrentRead.SelectAll();
                                    }
                                }
                                else
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.ToDateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.ShowDialog();
                                    TextBoxCurrentRead.SelectAll();
                                }
                            }
                            else
                            {
                                if (ChkDate == -1)
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.ToDateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.ShowDialog();
                                    TextBoxCurrentRead.SelectAll();
                                }
                                else
                                {
                                    Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.DateIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                    Msg.ShowDialog();
                                    TextBoxCurrentRead.SelectAll();
                                }
                            }
                        }
                        else
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.NumberIsWrong, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                            Msg.ShowDialog();
                            TextBoxCurrentRead.SelectAll();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleSingleBilling, Messages.DateIsNotAllowedBiggarThanCurrentDate, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.ShowDialog();
                        TextBoxCurrentRead.SelectAll();
                    }

                }
            }
            else
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBilling, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.ShowDialog();
                TextBoxCurrentRead.SelectAll();
            }
            if (Next)
                DoNext();
        }
        #endregion
    }
}
