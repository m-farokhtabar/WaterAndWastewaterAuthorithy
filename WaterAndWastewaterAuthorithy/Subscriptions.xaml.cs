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
using System.Data.SqlClient;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Subscriptions.xaml
    /// </summary>
    public partial class Subscriptions : UserControl
    {
        string LastTextBoxSearch = "";
        SelectedButton PrvButton = new SelectedButton();
        AnimateControls Ac = new AnimateControls();
        GridLength PrevHeight;
        string PrevHint = "";
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        System.Collections.Specialized.ListDictionary ControlsArraySearch = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        public Control FirstControlSearch = null;
        bool FirstLoad = true;
        bool SearchAgain = false;


        #region Initilize to Show Window
        public Subscriptions()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionId;
            ControlsArray.Add(TextBoxSubScriptionId, new ControlsTab { Prev = TextBoxComments, Next = TextBoxCustomerId, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxCustomerId, new ControlsTab { Prev = TextBoxSubScriptionId, Next = ComboBoxAccountType, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(ComboBoxAccountType, new ControlsTab { Prev = TextBoxCustomerId, Next = ComboBoxPreventType, Type = ControlsType.ComboBox });
            ControlsArray.Add(ComboBoxPreventType, new ControlsTab { Prev = ComboBoxAccountType, Next = TextBoxWaterMeterSerial, Type = ControlsType.ComboBox });
            ControlsArray.Add(TextBoxWaterMeterSerial, new ControlsTab { Prev = ComboBoxPreventType, Next = TextBoxWaterMeterNumber, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxWaterMeterNumber, new ControlsTab { Prev = TextBoxWaterMeterSerial, Next = TextBoxNumberReadDate, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxNumberReadDate, new ControlsTab { Prev = TextBoxWaterMeterNumber, Next = TextBoxDeficit1000, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxDeficit1000, new ControlsTab { Prev = TextBoxNumberReadDate, Next = TextBoxDebt, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxDebt, new ControlsTab { Prev = TextBoxDeficit1000, Next = TextBoxPostalCode, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxPostalCode, new ControlsTab { Prev = TextBoxDebt, Next = TextBoxAddress, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxAddress, new ControlsTab { Prev = TextBoxPostalCode, Next = TextBoxComments, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxComments, new ControlsTab { Prev = TextBoxAddress, Next = null, Type = ControlsType.TextBoxText });

            FirstControlSearch = RadioSearchBySubCode;
            ControlsArraySearch.Add(RadioSearchBySubCode, new ControlsTab { Prev = TextBoxSearch, Next = RadioSearchByCode, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(RadioSearchByCode, new ControlsTab { Prev = RadioSearchBySubCode, Next = RadioSearchByProfile, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(RadioSearchByProfile, new ControlsTab { Prev = RadioSearchByCode, Next = TextBoxSearch, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(TextBoxSearch, new ControlsTab { Prev = RadioSearchByProfile, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlSubscriptions_Loaded(object sender, RoutedEventArgs e)
        {
            Ac.SelectButton(ButtonDefination, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);

            PrevHeight = GridRowSearch.Height;
            GridRowSearch.Height = new GridLength(0);
            GridSearch.Visibility = System.Windows.Visibility.Hidden;
            TextBoxNumberReadDate.Text = Commons.GetCurrentPersianDate();
            RadioSearchBySubCode.IsChecked = true;
        }
        private void UserControlSubscriptions_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue))
            {
                if (FirstLoad == true || Commons.EditCustomer[0] == true || Commons.EditAccountType[0] == true)
                {
                    Commons.LoadComboBox(ComboBoxPreventType, "PreventTypesTb", "کد مانع");
                    ComboBoxPreventType.SelectedIndex = 0;
                    Commons.LoadComboBox(ComboBoxAccountType, "AccountTypesTb", "نوع کاربری");
                    ComboBoxAccountType.SelectedIndex = 0;
                    ButtonNew_Click(null, null);
                    Commons.EditCustomer[0] = false;
                    Commons.EditAccountType[0] = false;
                    FirstLoad = false;
                    FirstControl.Focus();
                }
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F4)
                ButtonDefination_Click(ButtonDefination, null);
            if (GridNew.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F5)
                ButtonNew_Click(null, null);
            if (GridNew.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);

            if (GridNew.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F7)
                ButtonSearch_Click(ButtonSearch, null);
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonReport_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
            if (e.Key == Key.F9)
                ButtonSearchCustomer_Click(null, null);

        }
        #endregion

        #region To Control TextBox For TabStop And Validate Value
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (((Control)sender).Name == "TextBoxCustomerId")
                    TextBoxCustomerId_LostFocus(((Control)sender), null);
                else
                    if (((ControlsTab)ControlsArray[sender]).Next != null)
                    {
                        Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                        Ctrl.Focus();
                        if (((ControlsTab)ControlsArray[Ctrl]).Type == ControlsType.ComboBox)
                            ((ComboBox)Ctrl).IsDropDownOpen = true;
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
                if (((Control)sender).Name == "TextBoxCustomerId")
                    TextBoxCustomerId_LostFocus(((Control)sender), null);
                else
                    if (((ControlsTab)ControlsArray[sender]).Prev != null)
                    {
                        Control Ctrl = ((ControlsTab)ControlsArray[sender]).Prev;
                        Ctrl.Focus();
                        if (((ControlsTab)ControlsArray[Ctrl]).Type == ControlsType.ComboBox)
                            ((ComboBox)Ctrl).IsDropDownOpen = true;

                    }
            }

        }
        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                if (((ComboBox)sender).IsDropDownOpen == true)
                    ((ComboBox)sender).IsDropDownOpen = false;
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                if (((ComboBox)sender).IsDropDownOpen == true)
                    ((ComboBox)sender).IsDropDownOpen = false;
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
        private void TextBoxCustomerId_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            BindingOperations.GetBindingExpression(text, TextBox.TextProperty).UpdateSource();
            MessageDialog Msg = null;
            try
            {
                bool IsValid = true;
                long CId = 0;
                try
                {
                    CId = Convert.ToInt64(((TextBox)sender).Text);
                }
                catch
                {
                    IsValid = false;
                    Msg = new MessageDialog(Messages.SearchMessageTitleCustomers, Messages.ForceToCustomersId, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();

                    TextBlockName.Text = "";
                    TextBlockFamily.Text = "";
                    TextBlockFather.Text = "";
                    TextBlockMelicode.Text = "";
                    TextBlockPostalCode.Text = "";
                    TextBlockAddress.Text = "";
                }
                if (IsValid == true)
                {
                    CustomersTb Row = Commons.Db.Customers.Find(CId, Commons.CurrentYear);

                    if (Row != null)
                    {
                        TextBlockName.Text = Row.Name;
                        TextBlockFamily.Text = Row.Family;
                        TextBlockFather.Text = Row.Father;
                        TextBlockMelicode.Text = Row.MeliCode;
                        TextBlockPostalCode.Text = Row.MeliCode;
                        TextBlockAddress.Text = Row.Address;
                        ((ComboBox)((ControlsTab)ControlsArray[sender]).Next).Focus();
                        ((ComboBox)((ControlsTab)ControlsArray[sender]).Next).IsDropDownOpen = true;
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.SearchMessageTitleCustomers, Messages.NotFoundCustomers, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        TextBlockName.Text = "";
                        TextBlockFamily.Text = "";
                        TextBlockFather.Text = "";
                        TextBlockMelicode.Text = "";
                        TextBlockPostalCode.Text = "";
                        TextBlockAddress.Text = "";
                    }
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.SearchMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            BindingOperations.GetBindingExpression(combo, ComboBox.SelectedIndexProperty).UpdateSource();
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
        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PrevHint = ((ComboBox)sender).Tag.ToString();
            Hint.Text = ((ComboBox)sender).Tag.ToString();
            if (Hint.Text[0] == '.')
                Hint.Text = Hint.Text.Substring(1) + ".";
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

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArray);
            MessageDialog Msg = null;
            if (Vd != null)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {
                DomainClasses.CustomersTb MyCustomer = Commons.Db.Customers.Find(Convert.ToInt32(TextBoxCustomerId.Text), Commons.CurrentYear);
                if (MyCustomer == null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.NotFoundCustomers, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    return;
                }
                DomainClasses.AccountTypesTb MyAccountType = Commons.Db.AccountTypes.Find(((Int32)((ComboBoxItem)ComboBoxAccountType.SelectedItem).Tag), Commons.CurrentYear);
                if (MyAccountType == null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.NotFoundAccountTypes, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    return;
                }

                DomainClasses.SubscriptionsTb MySubscription = null;
                MySubscription = Commons.Db.Subscriptions.Find(TextBoxSubScriptionId.Text, Commons.CurrentYear);

                if (MySubscription == null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.SaveMessageSubscriptions, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            int? MaxId = Commons.Db.WaterMeters.Where(x => x.Year == Commons.CurrentYear).Max(x => (int?)x.Id);                            
                            if (MaxId == null)
                                MaxId = 0;
                            MaxId++;
                            WaterMetersTb Wm = Commons.Db.WaterMeters.Add(new DomainClasses.WaterMetersTb
                            {
                                Id = Convert.ToInt32(MaxId),
                                SubId = TextBoxSubScriptionId.Text.Trim(),
                                Year = Commons.CurrentYear,
                                ReadStart = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim()),
                                ReadDateStart = TextBoxNumberReadDate.Text.Trim(),
                                ReadEnd = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim()),
                                ReadDateEnd = TextBoxNumberReadDate.Text.Trim(),
                                WaterMeterSerial = TextBoxWaterMeterSerial.Text.Trim(),
                                Description = "اولین کنتور آب",
                            });
                            Commons.Db.Subscriptions.Add(new DomainClasses.SubscriptionsTb
                            {
                                Id = TextBoxSubScriptionId.Text.Trim(),
                                Year = Commons.CurrentYear,
                                WaterMeter = Wm,
                                PrevNumber = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim()),
                                PrevReadDate = TextBoxNumberReadDate.Text.Trim(),
                                CurrentNumber = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim()),
                                CurrentReadDate = TextBoxNumberReadDate.Text.Trim(),
                                Debt = Convert.ToInt64(TextBoxDebt.Text),
                                deficit1000 = Convert.ToInt64(TextBoxDeficit1000.Text),
                                BillingInPeriod = 0,
                                PostalCode = TextBoxPostalCode.Text.Trim(),
                                Address = TextBoxAddress.Text.Trim(),
                                CustomerId = Convert.ToInt32(TextBoxCustomerId.Text.Trim()),
                                CustomerYear = Commons.CurrentYear,
                                AccountTypeId = Convert.ToInt32(((ComboBoxItem)ComboBoxAccountType.SelectedItem).Tag),
                                AccountTypeYear = Commons.CurrentYear,
                                PreventTypeId = Convert.ToInt32(((ComboBoxItem)ComboBoxPreventType.SelectedValue).Tag),
                                PreventTypeYear = Commons.CurrentYear,
                                Description = TextBoxComments.Text.Trim()
                            });
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("SubScriptions");
                            SearchAgain = true;
                            DataGridView.Items.Refresh();
                            Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.SaveMessageSuccessSubscriptions, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch (Exception Ex)
                        {
                            if (Ex.InnerException != null)
                            {
                                if (Ex.InnerException.InnerException != null && Ex.InnerException.InnerException is SqlException)
                                {
                                    SqlException SEx = (SqlException)Ex.InnerException.InnerException;
                                    if (SEx.Message.Contains("IX_Unique_SubscriptionsTbs_PostalCode"))
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.PostalCodeIsRepetitive, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                        Msg.Owner = Window.GetWindow(this);
                                        Msg.ShowDialog();
                                        TextBoxPostalCode.Focus();
                                        TextBoxPostalCode.SelectAll();
                                    }
                                }
                            }
                            else
                            {
                                Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.EditMessageSubscriptions, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            if (CheckSubscriptionInUse(MySubscription) == false)
                            {
                                //Save When it doesn't have bill
                                WaterMetersTb WmTb = Commons.Db.WaterMeters.Find(MySubscription.WaterMeterId, Commons.CurrentYear);
                                if (WmTb != null)
                                {
                                    WmTb.ReadDateStart = TextBoxNumberReadDate.Text.Trim();
                                    WmTb.ReadDateEnd = TextBoxNumberReadDate.Text.Trim();
                                    WmTb.ReadEnd = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim());
                                    WmTb.ReadStart = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim());
                                    WmTb.WaterMeterSerial = TextBoxWaterMeterSerial.Text.Trim();
                                }
                                else
                                {
                                    Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.NotWaterMeterExist, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                    Msg.Owner = Window.GetWindow(this);
                                    Msg.ShowDialog();
                                }

                                MySubscription.PrevNumber = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim());
                                MySubscription.PrevReadDate = TextBoxNumberReadDate.Text.Trim();
                                MySubscription.CurrentNumber = Convert.ToInt64(TextBoxWaterMeterNumber.Text.Trim());
                                MySubscription.CurrentReadDate = TextBoxNumberReadDate.Text.Trim();
                                //MySubscription.Debt = 0;
                                //MySubscription.deficit1000 = 0;
                            }
                            else
                            {
                                Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.SubScriptInUseEdit, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }

                            MySubscription.PostalCode = TextBoxPostalCode.Text.Trim();
                            MySubscription.Address = TextBoxAddress.Text.Trim();
                            MySubscription.CustomerId = Convert.ToInt32(TextBoxCustomerId.Text.Trim());
                            MySubscription.AccountTypeId = Convert.ToInt32(((ComboBoxItem)ComboBoxAccountType.SelectedItem).Tag);
                            MySubscription.PreventTypeId = Convert.ToInt32(((ComboBoxItem)ComboBoxPreventType.SelectedValue).Tag);
                            MySubscription.Description = TextBoxComments.Text.Trim();
                            MySubscription.Debt = Convert.ToInt64(TextBoxDebt.Text);
                            MySubscription.deficit1000 = Convert.ToInt64(TextBoxDeficit1000.Text);
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("SubScriptions");

                            List<SubscriptionsTb> Lc = (List<SubscriptionsTb>)DataGridView.ItemsSource;
                            if (Lc != null)
                            {
                                SubscriptionsTb Rc = Lc.Find(x => x.Id.Trim() == MySubscription.Id.Trim() && x.Year == Commons.CurrentYear);
                                if (Rc != null)
                                {
                                    Rc.AccountType = MySubscription.AccountType;
                                    Rc.AccountType = MySubscription.AccountType;
                                    Rc.AccountTypeId = MySubscription.AccountTypeId;
                                    Rc.AccountTypeYear = MySubscription.AccountTypeYear;
                                    Rc.Address = MySubscription.Address;
                                    Rc.BillingInPeriod = MySubscription.BillingInPeriod;
                                    Rc.CurrentReadDate = MySubscription.CurrentReadDate;
                                    Rc.CustomerId = MySubscription.CustomerId;
                                    Rc.CustomerYear = MySubscription.CustomerYear;
                                    Rc.Debt = MySubscription.Debt;
                                    Rc.deficit1000 = MySubscription.deficit1000;
                                    Rc.Description = MySubscription.Description;
                                    Rc.PostalCode = MySubscription.PostalCode;
                                    Rc.PreventTypeId = MySubscription.PreventTypeId;
                                    Rc.PreventTypeYear = MySubscription.PreventTypeYear;
                                    Rc.PrevNumber = MySubscription.PrevNumber;
                                    Rc.PrevReadDate = MySubscription.PrevReadDate;
                                    Rc.RecordDate = MySubscription.RecordDate;
                                    Rc.WaterMeterId = MySubscription.WaterMeterId;
                                    Rc.WaterMeterYear = MySubscription.WaterMeterYear;
                                    Rc.Year = MySubscription.Year;

                                }
                            }

                            SearchAgain = true;
                            DataGridView.Items.Refresh();
                            Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.EditMessageSuccessSubscriptions, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch (Exception Ex)
                        {
                            if (Ex.InnerException != null)
                            {
                                if (Ex.InnerException != null)
                                {
                                    if (Ex.InnerException.InnerException != null && Ex.InnerException.InnerException is SqlException)
                                    {
                                        SqlException SEx = (SqlException)Ex.InnerException.InnerException;
                                        if (SEx.Message.Contains("IX_Unique_SubscriptionsTbs_PostalCode"))
                                        {
                                            Msg = new MessageDialog(Messages.SaveMessageTitleSubscriptions, Messages.PostalCodeIsRepetitive, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                            Msg.Owner = Window.GetWindow(this);
                                            Msg.ShowDialog();
                                            TextBoxPostalCode.Focus();
                                            TextBoxPostalCode.SelectAll();
                                            return;
                                        }
                                    }
                                }
                            }
                            Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
            }
        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            TextBoxCustomerId.Text = "";
            TextBoxSubScriptionId.Text = "";
            ComboBoxAccountType.SelectedIndex = 0;
            ComboBoxPreventType.SelectedIndex = 0;
            TextBoxWaterMeterSerial.Text = "";
            TextBoxWaterMeterNumber.Text = "";
            TextBoxNumberReadDate.Text = Commons.GetCurrentPersianDate();
            TextBoxPostalCode.Text = "";
            TextBoxAddress.Text = "";
            TextBoxComments.Text = "";
            TextBoxDebt.Text = "0";
            TextBoxDeficit1000.Text = "0";

            TextBlockName.Text = "";
            TextBlockFamily.Text = "";
            TextBlockFather.Text = "";
            TextBlockMelicode.Text = "";
            TextBlockPostalCode.Text = "";
            TextBlockAddress.Text = "";

            TextBlockCurrentNumber.Text = "";
            TextBlockCurrentReadDate.Text = "";
            FirstControl.Focus();
        }
        private void ButtonDefination_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, true);
            Ac.AnimateRowGrid(GridRowSearch, GridRowNew, GridSearch, GridNew, ref PrevHeight);
            FirstControl.Focus();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
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
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.D)
                ButtonDelete_Click(null, null);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.E)
                ButtonShowEdit_Click(null, null);
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxSearch, "کد اشتراک", "عملیات", "کسر هزار ریال", 11);
        }
        private void ButtonShowEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {
                string id = ((TextBlock)DataGridView.Columns[0].GetCellContent(DataGridView.SelectedItem)).Text;
                SubscriptionsTb RowEdited = Commons.Db.Subscriptions.Find(id, Commons.CurrentYear);
                if (RowEdited != null)
                {
                    TextBoxSubScriptionId.Text = id.Trim();
                    TextBoxCustomerId.Text = RowEdited.CustomerId.ToString();
                    ReturnInfoCustomer(RowEdited.CustomerId);
                    int Id = 0;
                    foreach (object value in ComboBoxAccountType.Items)
                    {
                        Id = (int)((ComboBoxItem)value).Tag;
                        if (Id == RowEdited.AccountTypeId)
                            ComboBoxAccountType.SelectedItem = value;
                    }
                    foreach (object value in ComboBoxPreventType.Items)
                    {
                        Id = (int)((ComboBoxItem)value).Tag;
                        if (Id == RowEdited.PreventTypeId)
                            ComboBoxPreventType.SelectedItem = value;
                    }
                    TextBoxWaterMeterSerial.Text = RowEdited.WaterMeter.WaterMeterSerial.Trim();
                    TextBoxWaterMeterNumber.Text = RowEdited.PrevNumber.ToString().Trim();
                    TextBoxNumberReadDate.Text = RowEdited.PrevReadDate.Trim();
                    TextBoxPostalCode.Text = RowEdited.PostalCode.Trim();
                    TextBoxAddress.Text = RowEdited.Address.Trim();
                    TextBoxComments.Text = RowEdited.Description.Trim();


                    TextBlockCurrentNumber.Text = RowEdited.CurrentNumber.ToString();
                    TextBlockCurrentReadDate.Text = RowEdited.CurrentReadDate.Trim();
                    TextBoxDebt.Text = Commons.ConvertToMoneyWithSign(RowEdited.Debt);
                    TextBoxDeficit1000.Text = Commons.ConvertToMoneyWithSign(RowEdited.deficit1000);
                    ButtonDefination_Click(ButtonDefination, null);
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.EditMessageTitleSubscriptions, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            string id = ((TextBlock)DataGridView.Columns[0].GetCellContent(DataGridView.SelectedItem)).Text;
            SubscriptionsTb RowDeleted = Commons.Db.Subscriptions.Find(id, Commons.CurrentYear);
            if (RowDeleted != null)
            {
                try
                {
                    if (CheckSubscriptionInUse(RowDeleted) == false)
                    {
                        Msg = new MessageDialog(Messages.DeleteMessageTitleSubscriptions, Messages.DeleteMessageSubscriptions, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        if (Msg.ShowDialog() == true)
                        {
                            WaterMetersTb Wd = Commons.Db.WaterMeters.Find(RowDeleted.WaterMeterId, Commons.CurrentYear);
                            if (Wd != null)
                                Commons.Db.WaterMeters.Remove(Wd);
                            Commons.Db.Subscriptions.Remove(RowDeleted);
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("SubScriptions");
                            List<SubscriptionsTb> L = (List<SubscriptionsTb>)DataGridView.ItemsSource;
                            if (L != null)
                                L.Remove(RowDeleted);
                            SearchAgain = true;
                            DataGridView.Items.Refresh();
                            Msg = new MessageDialog(Messages.DeleteMessageTitleSubscriptions, Messages.DeleteMessageSuccessSubscriptions, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.DeleteMessageTitleSubscriptions, Messages.SubScriptInUseDelete, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                catch
                {
                    Msg = new MessageDialog(Messages.DeleteMessageTitleSubscriptions, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }

        }
        private void ReturnInfoCustomer(long CId)
        {
            MessageDialog Msg = null;
            try
            {
                CustomersTb Row = Commons.Db.Customers.Find(CId, Commons.CurrentYear);
                if (Row != null)
                {
                    TextBlockName.Text = Row.Name;
                    TextBlockFamily.Text = Row.Family;
                    TextBlockFather.Text = Row.Father;
                    TextBlockMelicode.Text = Row.MeliCode;
                    TextBlockPostalCode.Text = Row.MeliCode;
                    TextBlockAddress.Text = Row.Address;
                }
                else
                {
                    Msg = new MessageDialog(Messages.SearchMessageTitleCustomers, Messages.NotFoundCustomers, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    TextBlockName.Text = "";
                    TextBlockFamily.Text = "";
                    TextBlockFather.Text = "";
                    TextBlockMelicode.Text = "";
                    TextBlockPostalCode.Text = "";
                    TextBlockAddress.Text = "";
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.SearchMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        #endregion Datagrid

        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            IEnumerable<SubscriptionsTb> Tb = (IEnumerable<SubscriptionsTb>)DataGridView.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    DataTable Dt = Commons.ToDataTable<SubscriptionsTb>(Tb);
                    Dt.TableName = "Subscriptions";

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\Subscriptions.mrt", GridHeader.Background, "اشتراک ها", Dt);
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
                    DataGridView.ItemsSource = Commons.Db.Subscriptions.Where(x => x.Id == LastTextBoxSearch && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                }
                else
                {
                    if (RadioSearchByCode.IsChecked == true)
                    {
                        long id = Convert.ToInt64(LastTextBoxSearch);
                        DataGridView.ItemsSource = Commons.Db.Subscriptions.Where(x => (x.CustomerId == id || x.PostalCode == LastTextBoxSearch || x.AccountTypeId == id) && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                    }
                    else
                    {
                        DataGridView.ItemsSource = Commons.Db.Subscriptions.Where(x => x.Address.Contains(LastTextBoxSearch) && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                    }
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
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در کد مشترک، کد ملی، کد پستی جستجو می شود.";
        }
        private void RadioSearchByProfile_Checked(object sender, RoutedEventArgs e)
        {

            TextBoxSearch.TextChanged -= TextBoxSubScriptionId_TextChanged;
            TextBoxSearch.PreviewTextInput -= TextBox_PreviewTextInput;
            TextBoxSearch.TextChanged -= TextBox_TextChanged;
            TextBoxSearch.MaxLength = 250;
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در آدرس اشتراک جستجو می شود.";
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
            Ac.ShowWindow("Subscriptions", "MainMenu");
        }
        private void ButtonSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Subscriptions", "Customers");
            if (Commons.MainWindow.UserControlCustomers.GridNewCustomer.Visibility == Visibility.Visible)
                Commons.MainWindow.UserControlCustomers.ButtonSearch_Click(Commons.MainWindow.UserControlCustomers.ButtonSearch, null);
        }

        private bool CheckSubscriptionInUse(SubscriptionsTb Sbc)
        {
            if (Commons.Db.Bills.Where(x => x.SubscriptionId == Sbc.Id).FirstOrDefault() == null)
                return false;
            else
                return true;
        }

    }
}
