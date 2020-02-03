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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterAndWastewaterAuthorithy.Presentation;
using WaterAndWastewaterAuthorithy.Validations;
using System.Data;
using System.Data.Entity;
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.DomainClasses;
using System.Data.SqlClient;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : UserControl
    {
        string LastTextBoxSearch = "";
        long CustomerId = -1;
        SelectedButton PrvButton = new SelectedButton();
        AnimateControls Ac = new AnimateControls();
        GridLength PrevHeight;
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        System.Collections.Specialized.ListDictionary ControlsArraySearch = new System.Collections.Specialized.ListDictionary();
        string PrevHint = "";
        public Control FirstControlNew = null;
        public Control FirstControlSearch = null;
        bool SearchAgain = false;

        #region Initilize to Show Window
        public Customers()
        {
            InitializeComponent();
            FirstControlNew = TextBoxName;
            ControlsArray.Add(TextBoxName, new ControlsTab { Prev = TextBoxComments, Next = TextBoxFamily, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFamily, new ControlsTab { Prev = TextBoxName, Next = TextBoxFather, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFather, new ControlsTab { Prev = TextBoxFamily, Next = TextBoxCityCard, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxCityCard, new ControlsTab { Prev = TextBoxFather, Next = TextBoxIdCard, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxIdCard, new ControlsTab { Prev = TextBoxCityCard, Next = TextBoxMeliCode, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxMeliCode, new ControlsTab { Prev = TextBoxIdCard, Next = TextBoxPhone, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxPhone, new ControlsTab { Prev = TextBoxMeliCode, Next = TextBoxCellPhone, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxCellPhone, new ControlsTab { Prev = TextBoxPhone, Next = TextBoxPostalCode, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxPostalCode, new ControlsTab { Prev = TextBoxCellPhone, Next = TextBoxAddress, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxAddress, new ControlsTab { Prev = TextBoxPostalCode, Next = TextBoxComments, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxComments, new ControlsTab { Prev = TextBoxAddress, Next = null, Type = ControlsType.TextBoxText });

            FirstControlSearch = RadioSearchByCode;
            ControlsArraySearch.Add(RadioSearchByCode, new ControlsTab { Prev = TextBoxSearch, Next = RadioSearchByProfile, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(RadioSearchByProfile, new ControlsTab { Prev = RadioSearchByCode, Next = TextBoxSearch, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(TextBoxSearch, new ControlsTab { Prev = RadioSearchByProfile, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlCustomers_Loaded(object sender, RoutedEventArgs e)
        {            
            Ac.SelectButton(ButtonCustomer, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);

            PrevHeight = GridRowSearchCustomer.Height;
            GridRowSearchCustomer.Height = new GridLength(0);
            GridSearchCustomer.Visibility = System.Windows.Visibility.Hidden;            
            RadioSearchByCode.IsChecked = true;
        }
        private void UserControlCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNewCustomer.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F4)
                ButtonCustomer_Click(ButtonCustomer, null);
            if (GridNewCustomer.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F5)
                ButtonNew_Click(null, null);
            if (GridNewCustomer.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);

            if (GridNewCustomer.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F7)
                ButtonSearch_Click(ButtonSearch, null);
            if (GridNewCustomer.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonReport_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }
        #endregion
        
        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Customers", "MainMenu");
        }

        #region To Control TextBox For TabStop And Validate Value
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    ((ControlsTab)ControlsArray[sender]).Next.Focus();
                }
                else
                {
                    if (e.Key == Key.Tab)
                        FirstControlNew.Focus();
                    else
                    {
                        ButtonSave_Click(null, null);
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                if (((ControlsTab)ControlsArray[sender]).Prev != null)
                {
                    ((ControlsTab)ControlsArray[sender]).Prev.Focus();
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
            foreach (char c in ((TextBox)sender).Text.ToCharArray())
            {
                if (!Char.IsDigit(c))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            ((TextBox)sender).Text = tmp;
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
        #endregion

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControlNew, ControlsArray);
            MessageDialog Msg = null;
            if (Vd != null)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {
                DomainClasses.CustomersTb MyCustomer = null;
                if (CustomerId != -1)
                    MyCustomer = Commons.Db.Customers.Find(CustomerId, Commons.CurrentYear);

                if (MyCustomer == null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.SaveMessageCustomers, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderCustomers.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {                           
                            int?  MaxId = Commons.Db.Customers.Where(x => x.Year == Commons.CurrentYear).Max(x => (int?)x.Id);
                            if (MaxId == null)
                                MaxId = 0;
                            MaxId++;
                            Commons.Db.Customers.Add(new DomainClasses.CustomersTb
                            {
                                Id = Convert.ToInt32(MaxId),
                                Year = Commons.CurrentYear,
                                MeliCode = TextBoxMeliCode.Text.Trim(),
                                Name = TextBoxName.Text.Trim(),
                                Family = TextBoxFamily.Text.Trim(),
                                Father = TextBoxFather.Text.Trim(),
                                Search = TextBoxName.Text.Trim() + " " + TextBoxFamily.Text.Trim() + " " + TextBoxFather.Text.Trim(),
                                IdCard = TextBoxIdCard.Text.Trim(),
                                CityCard = TextBoxCityCard.Text.Trim(),
                                Phone = TextBoxPhone.Text.Trim(),
                                CellPhone = TextBoxCellPhone.Text.Trim(),
                                PostalCode = TextBoxPostalCode.Text.Trim(),
                                Address = TextBoxAddress.Text.Trim(),
                                Description = TextBoxComments.Text.Trim()
                            });
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("Customers");
                            DataGridCustomers.Items.Refresh();
                            SearchAgain = true;
                            Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.SaveMessageSuccessCustomers, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderCustomers.Background);
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
                                    if (SEx.Message.Contains("IX_Unique_CustomersTbs_MeliCode"))
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.MeliCodeIsRepetitive, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                                        Msg.Owner = Window.GetWindow(this);
                                        Msg.ShowDialog();
                                        TextBoxMeliCode.Focus();
                                        TextBoxMeliCode.SelectAll();
                                    }
                                }
                            }
                            else
                            {
                                Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderCustomers.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.EditMessageTitleCustomers, Messages.EditMessageCustomers, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderCustomers.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            MyCustomer.MeliCode = TextBoxMeliCode.Text.Trim();
                            MyCustomer.Name = TextBoxName.Text.Trim();
                            MyCustomer.Family = TextBoxFamily.Text.Trim();
                            MyCustomer.Father = TextBoxFather.Text.Trim();
                            MyCustomer.Search = TextBoxName.Text.Trim() + " " + TextBoxFamily.Text.Trim() + " " + TextBoxFather.Text.Trim();
                            MyCustomer.IdCard = TextBoxIdCard.Text.Trim();
                            MyCustomer.CityCard = TextBoxCityCard.Text.Trim();
                            MyCustomer.Phone = TextBoxPhone.Text.Trim();
                            MyCustomer.CellPhone = TextBoxCellPhone.Text.Trim();
                            MyCustomer.PostalCode = TextBoxPostalCode.Text.Trim();
                            MyCustomer.Address = TextBoxAddress.Text.Trim();
                            MyCustomer.Description = TextBoxComments.Text.Trim();
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("Customers");
                            List<CustomersTb> Lc = (List<CustomersTb>)DataGridCustomers.ItemsSource;
                            if (Lc != null)
                            {
                                CustomersTb Rc = Lc.Find(x => x.Id == CustomerId && x.Year == Commons.CurrentYear);
                                if (Rc != null)
                                {
                                    Rc.MeliCode = TextBoxMeliCode.Text.Trim();
                                    Rc.Name = TextBoxName.Text.Trim();
                                    Rc.Family = TextBoxFamily.Text.Trim();
                                    Rc.Father = TextBoxFather.Text.Trim();
                                    Rc.Search = TextBoxName.Text.Trim() + " " + TextBoxFamily.Text.Trim() + " " + TextBoxFather.Text.Trim();
                                    Rc.IdCard = TextBoxIdCard.Text.Trim();
                                    Rc.CityCard = TextBoxCityCard.Text.Trim();
                                    Rc.Phone = TextBoxPhone.Text.Trim();
                                    Rc.CellPhone = TextBoxCellPhone.Text.Trim();
                                    Rc.PostalCode = TextBoxPostalCode.Text.Trim();
                                    Rc.Address = TextBoxAddress.Text.Trim();
                                    Rc.Description = TextBoxComments.Text.Trim();
                                }
                            }
                            DataGridCustomers.Items.Refresh();
                            SearchAgain = true;
                            Msg = new MessageDialog(Messages.EditMessageTitleCustomers, Messages.EditMessageSuccessCustomers, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderCustomers.Background);
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
                                    if (SEx.Message.Contains("IX_Unique_CustomersTbs_MeliCode"))
                                    {
                                        Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.MeliCodeIsRepetitive, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                                        Msg.Owner = Window.GetWindow(this);
                                        Msg.ShowDialog();
                                        TextBoxMeliCode.Focus();
                                        TextBoxMeliCode.SelectAll();
                                    }
                                }
                            }
                            else
                            {
                                Msg = new MessageDialog(Messages.SaveMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderCustomers.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }
                        }
                    }
                }
            }

        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = "";
            TextBoxFamily.Text = "";
            TextBoxFather.Text = "";
            TextBoxCityCard.Text = "";
            TextBoxIdCard.Text = "";
            TextBoxMeliCode.Text = "";
            TextBoxPhone.Text = "";
            TextBoxCellPhone.Text = "";
            TextBoxPostalCode.Text = "";
            TextBoxAddress.Text = "";
            TextBoxComments.Text = "";
            CustomerId = -1;
            FirstControlNew.Focus();
        }
        private void ButtonCustomer_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, true);
            Ac.AnimateRowGrid(GridRowSearchCustomer, GridRowNewCustomer, GridSearchCustomer, GridNewCustomer, ref PrevHeight);
            FirstControlNew.Focus();
            ((TextBox)FirstControlNew).SelectAll();
        }
        public  void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, true);
            Ac.AnimateRowGrid(GridRowNewCustomer, GridRowSearchCustomer, GridNewCustomer, GridSearchCustomer, ref PrevHeight);
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
            if ((LastTextBoxSearch != TextBoxSearch.Text.Trim() || SearchAgain == true) && TextBoxSearch.Text.Trim()!="")
            {
                SearchAgain = false;
                LastTextBoxSearch = TextBoxSearch.Text.Trim();
                if (RadioSearchByCode.IsChecked == true)
                {
                    long id  = Convert.ToInt64(LastTextBoxSearch);
                    DataGridCustomers.ItemsSource = Commons.Db.Customers.Where(x => (x.Id == id || x.PostalCode == LastTextBoxSearch || x.MeliCode == LastTextBoxSearch) && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                }
                else
                {
                    LastTextBoxSearch = LastTextBoxSearch.Replace(' ', '%');
                    string Query = "select Top " + Commons.TopRow.ToString() + " * from Customerstbs where (Search like N'%" + LastTextBoxSearch + "%') and Year =" + Commons.CurrentYear.ToString();
                    DataGridCustomers.ItemsSource = Commons.Db.Customers.SqlQuery(Query).Take(Commons.TopRow).ToList();
                }
            }
            if (DataGridCustomers.Items != null && DataGridCustomers.Items.Count != 0)
            {
                DataGridCustomers.SelectedItem = DataGridCustomers.Items[0];
                DataGridCustomers.ScrollIntoView(DataGridCustomers.SelectedItem);
                DataGridCustomers.Focus();
            }
        }

        #region Datagrid
        private void DataGridCustomers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.D)
                ButtonDelete_Click(null, null);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.E)
                ButtonShowEdit_Click(null, null);
            Commons.GridViewKeyDown(DataGridCustomers, ref e, TextBoxSearch, "کد مشترک", "عملیات", "محل صدور", 9);
        }
        private void ButtonShowEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {

                long id = Convert.ToInt64(((TextBlock)DataGridCustomers.Columns[0].GetCellContent(DataGridCustomers.SelectedItem)).Text);
                CustomersTb RowEdited = Commons.Db.Customers.Find(id,Commons.CurrentYear);
                if (RowEdited != null)
                {
                    CustomerId = id;
                    TextBoxMeliCode.Text = RowEdited.MeliCode.Trim();
                    TextBoxName.Text = RowEdited.Name.Trim();
                    TextBoxFamily.Text = RowEdited.Family.Trim();
                    TextBoxFather.Text = RowEdited.Father.Trim();
                    TextBoxIdCard.Text = RowEdited.IdCard.Trim();
                    TextBoxCityCard.Text = RowEdited.CityCard.Trim();
                    TextBoxPhone.Text = RowEdited.Phone.Trim();
                    TextBoxCellPhone.Text = RowEdited.CellPhone.Trim();
                    TextBoxPostalCode.Text = RowEdited.PostalCode.Trim();
                    TextBoxAddress.Text = RowEdited.Address.Trim();
                    TextBoxComments.Text = RowEdited.Description.Trim();
                    ButtonCustomer_Click(ButtonCustomer, null);
                }
                else
                {
                    Msg = new MessageDialog(Messages.EditMessageTitleCustomers, Messages.CustomerNotFound, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.EditMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderCustomers.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {
                long id = Convert.ToInt64(((TextBlock)DataGridCustomers.Columns[0].GetCellContent(DataGridCustomers.SelectedItem)).Text);
                CustomersTb RowDeleted = Commons.Db.Customers.Find(id, Commons.CurrentYear);
                if (RowDeleted != null)
                {
                    var Sub = Commons.Db.Subscriptions.Where(x => x.Customer.Id == id && x.Customer.Year == Commons.CurrentYear);
                    if (Sub.Count<SubscriptionsTb>() == 0)
                    {
                        try
                        {
                            Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.DeleteMessageCustomers, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderCustomers.Background);
                            Msg.Owner = Window.GetWindow(this);
                            if (Msg.ShowDialog() == true)
                            {
                                Commons.Db.Customers.Remove(RowDeleted);
                                Commons.Db.SaveChanges();
                                Commons.SetFromEdited("Customers");
                                List < CustomersTb > L= (List<CustomersTb>)DataGridCustomers.ItemsSource;
                                if (L != null)
                                {
                                    L.Remove(RowDeleted);
                                }
                                DataGridCustomers.Items.Refresh();
                                SearchAgain = true;
                                Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.DeleteMessageSuccessCustomers, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderCustomers.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderCustomers.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.DeleteCustomerImpossible, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.CustomerNotFound, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.DeleteMessageTitleCustomers, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderCustomers.Background);
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
            IEnumerable<CustomersTb> Tb = (IEnumerable<CustomersTb>)DataGridCustomers.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    DataTable Dt = Commons.ToDataTable<CustomersTb>(Tb);
                    Dt.TableName = "Customers";

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\Customers.mrt", GridHeaderCustomers.Background, "مشترکین", Dt);
                    PrintPrv.ShowDialog();
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleCustomers, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleCustomers, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderCustomers.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }

        #endregion SearchPart        

        private void RadioSearchByCode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            TextBoxSearch.MaxLength = 10;
            TextBoxSearch.PreviewTextInput += TextBox_PreviewTextInput;
            TextBoxSearch.TextChanged += TextBox_TextChanged;
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در کد مشترک، کد ملی، کد پستی جستجو می شود.";
        }
        private void RadioSearchByProfile_Checked(object sender, RoutedEventArgs e)
        {
            
            TextBoxSearch.PreviewTextInput -= TextBox_PreviewTextInput;
            TextBoxSearch.TextChanged -= TextBox_TextChanged;
            TextBoxSearch.MaxLength = 85;
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در مشخصات شامل نام، نام خانوادگی و نام پدر جستجو می شود.";
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
    }
}
