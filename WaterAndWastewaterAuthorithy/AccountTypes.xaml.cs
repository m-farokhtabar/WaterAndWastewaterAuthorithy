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
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.DomainClasses;
using WaterAndWastewaterAuthorithy.Presentation;
using WaterAndWastewaterAuthorithy.Validations;
using System.Data.Entity;
using System.Data;
using NCalc;
namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for AccountTypes.xaml
    /// </summary>
    public partial class AccountTypes : UserControl
    {
        string LastTextBoxSearch = "";
        AnimateControls Ac = new AnimateControls();
        SelectedButton PrvButton = new SelectedButton();
        long AccountTypeId = -1;
        public Control FirstControlNewAccountType = null;
        string PrevHint = "";
        GridLength PrevHeight;
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        System.Collections.Specialized.ListDictionary ControlsArraySearch = new System.Collections.Specialized.ListDictionary();
        public Control FirstControlSearch = null;
        bool SearchAgain = false;
        
        public AccountTypes()
        {
            InitializeComponent();
            FirstControlNewAccountType = TextBoxName;
            ControlsArray.Add(TextBoxName, new ControlsTab { Prev = TextBoxComments, Next = TextBoxFormules, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFormules, new ControlsTab { Prev = TextBoxName, Next = TextBoxComments, Type = ControlsType.TextBoxFormul });
            ControlsArray.Add(TextBoxComments, new ControlsTab { Prev = TextBoxFormules, Next = null, Type = ControlsType.TextBoxText });

            FirstControlSearch = RadioSearchByCode;
            ControlsArraySearch.Add(RadioSearchByCode, new ControlsTab { Prev = TextBoxSearch, Next = RadioSearchByProfile, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(RadioSearchByProfile, new ControlsTab { Prev = RadioSearchByCode, Next = TextBoxSearch, Type = ControlsType.RadioButton });
            ControlsArraySearch.Add(TextBoxSearch, new ControlsTab { Prev = RadioSearchByProfile, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlAccountTypes_Loaded(object sender, RoutedEventArgs e)
        {            
            Ac.SelectButton(ButtonAccountType, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);

            PrevHeight = GridRowSearchAccountType.Height;
            GridRowSearchAccountType.Height = new GridLength(0);
            GridSearchAccountType.Visibility = System.Windows.Visibility.Hidden;            
            RadioSearchByCode.IsChecked = true;
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F4)
                ButtonAccountType_Click(ButtonAccountType, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F5)
                ButtonNew_Click(null, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);

            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F7)
                ButtonSearch_Click(ButtonSearch, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonReport_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }
        #region TextBox
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
                        FirstControlNewAccountType.Focus();
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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tmp = ((TextBox)sender).Text;
            int Pos = ((TextBox)sender).SelectionStart;
            foreach (char c in ((TextBox)sender).Text.ToCharArray())
            {
                if (!(Char.IsDigit(c) || c == '+' || c == '-' || c == '*' || c == '/' || c == '[' || c == ']' || c == ' ' || c == '#'))
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
        #endregion TextBox

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            ControlsValidate? Vd = Commons.ValidateData(FirstControlNewAccountType, ControlsArray);
            MessageDialog Msg = null;
            if (Vd != null)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleAccountTypes, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {
                DomainClasses.AccountTypesTb MyAccountType = null;
                if (AccountTypeId != -1)
                    MyAccountType = Commons.Db.AccountTypes.Find(AccountTypeId, Commons.CurrentYear);

                if (MyAccountType == null)
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleAccountTypes, Messages.SaveMessageAccountTypes, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            int?  MaxId = Commons.Db.AccountTypes.Where(x => x.Year == Commons.CurrentYear).Max(x => (int?)x.Id);
                            if (MaxId == null)
                                MaxId = 0;
                            MaxId++;
                            Commons.Db.AccountTypes.Add(new DomainClasses.AccountTypesTb
                            {
                                Id = Convert.ToInt32(MaxId),
                                Name = TextBoxName.Text.Trim(),
                                Year = Commons.CurrentYear,
                                Formules = TextBoxFormules.Text.Trim(),
                                Description = TextBoxComments.Text.Trim()
                            });
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("AccountTypes");
                            SearchAgain = true;
                            DataGridAccountTypes.Items.Refresh();
                            DataGridAccountTypes.UpdateLayout();
                            Msg = new MessageDialog(Messages.SaveMessageTitleAccountTypes, Messages.SaveMessageSuccessAccountTypes, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderAccountTypes.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleAccountTypes, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderAccountTypes.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.EditMessageTitleAccountTypes, Messages.EditMessageAccountTypes, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            MyAccountType.Name = TextBoxName.Text.Trim();
                            MyAccountType.Formules = TextBoxFormules.Text.Trim();
                            MyAccountType.Description = TextBoxComments.Text.Trim();                            
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("AccountTypes");
                            List<AccountTypesTb> Lc = (List<AccountTypesTb>)DataGridAccountTypes.ItemsSource;
                            if (Lc != null)
                            {
                                AccountTypesTb Rc = Lc.Find(x => x.Id == MyAccountType.Id && x.Year == Commons.CurrentYear);
                                if (Rc != null)
                                {
                                    Rc.Description  = MyAccountType.Description;
                                    Rc.Formules = MyAccountType.Formules;
                                    Rc.Name = MyAccountType.Name;
                                    Rc.RecordDate = MyAccountType.RecordDate;
                                    Rc.Year = MyAccountType.Year;                                    
                                }
                            }
                            DataGridAccountTypes.Items.Refresh();
                            DataGridAccountTypes.UpdateLayout();
                            SearchAgain = true;
                            Msg = new MessageDialog(Messages.EditMessageTitleAccountTypes, Messages.EditMessageSuccessAccountTypes, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderAccountTypes.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.EditMessageTitleAccountTypes, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderAccountTypes.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
            }

        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = "";
            TextBoxFormules.Text = "";
            TextBoxComments.Text = "";
            AccountTypeId = -1;
            FirstControlNewAccountType.Focus();
        }
        private void ButtonAccountType_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, true);
            Ac.AnimateRowGrid(GridRowSearchAccountType, GridRowNewAccountType, GridSearchAccountType, GridNewAccountType, ref PrevHeight);
            FirstControlNewAccountType.Focus();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Ac.AnimateButton(new ColumnDefinition[] { GridColNew, GridColSave }, new Button[] { ButtonNew, ButtonSave }, false);
            Ac.SelectButton(sender, ref PrvButton, this);
            Ac.AnimateButton(new ColumnDefinition[] { GridColReport }, new Button[] { ButtonReport }, true);
            Ac.AnimateRowGrid(GridRowNewAccountType, GridRowSearchAccountType, GridNewAccountType, GridSearchAccountType, ref PrevHeight);
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

        #region Datagrid
        private void DataGridAccountTypes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.D)
                ButtonDelete_Click(null, null);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.E)
                ButtonShowEdit_Click(null, null);
            Commons.GridViewKeyDown(DataGridAccountTypes, ref e, TextBoxSearch, "کد نوع کاربری", "عملیات", "توضیحات", 4);
        }
        private void ButtonShowEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {

                long id = Convert.ToInt64(((TextBlock)DataGridAccountTypes.Columns[0].GetCellContent(DataGridAccountTypes.SelectedItem)).Text);
                AccountTypesTb RowEdited = Commons.Db.AccountTypes.Find(id,Commons.CurrentYear);
                if (RowEdited != null)
                {
                    AccountTypeId = id;
                    TextBoxName.Text = RowEdited.Name.Trim();
                    TextBoxFormules.Text = RowEdited.Formules.Trim();
                    TextBoxComments.Text = RowEdited.Description.Trim();
                    ButtonAccountType_Click(ButtonAccountType, null);
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.EditMessageTitleAccountTypes, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderAccountTypes.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {
                long id = Convert.ToInt64(((TextBlock)DataGridAccountTypes.Columns[0].GetCellContent(DataGridAccountTypes.SelectedItem)).Text);
                AccountTypesTb RowDeleted = Commons.Db.AccountTypes.Find(id,Commons.CurrentYear);
                if (RowDeleted != null)
                {
                    var Sub = Commons.Db.Subscriptions.Local.Where(x => x.AccountTypeId == id);
                    if (Sub.Count<SubscriptionsTb>() == 0)
                    {

                        Msg = new MessageDialog(Messages.DeleteMessageTitleAccountTypes, Messages.DeleteMessageAccountTypes, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                        Msg.Owner = Window.GetWindow(this);
                        if (Msg.ShowDialog() == true)
                        {
                            Commons.Db.AccountTypes.Remove(RowDeleted);
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("AccountTypes");
                            List<AccountTypesTb> L = (List<AccountTypesTb>)DataGridAccountTypes.ItemsSource;
                            if (L != null)
                                L.Remove(RowDeleted);
                            SearchAgain = true;
                            DataGridAccountTypes.Items.Refresh();
                            Msg = new MessageDialog(Messages.DeleteMessageTitleAccountTypes, Messages.DeleteMessageSuccessAccountTypes, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeaderAccountTypes.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.DeleteMessageTitleAccountTypes, Messages.DeleteAccountTypeImpossible, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.DeleteMessageTitleAccountTypes, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeaderAccountTypes.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        #endregion Datagrid

        #region Search
        private void TextBox_NumPreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void TextBox_NumTextChanged(object sender, TextChangedEventArgs e)
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
                if (RadioSearchByCode.IsChecked == true)
                {
                    long id = Convert.ToInt64(LastTextBoxSearch);
                    DataGridAccountTypes.ItemsSource = Commons.Db.AccountTypes.Where(x => x.Id == id && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                }
                else
                {
                    DataGridAccountTypes.ItemsSource = Commons.Db.AccountTypes.Where(x => x.Name.Contains(LastTextBoxSearch) && x.Year == Commons.CurrentYear).Take(Commons.TopRow).ToList();
                }
            }
            if (DataGridAccountTypes.Items != null && DataGridAccountTypes.Items.Count != 0)
            {
                DataGridAccountTypes.SelectedItem = DataGridAccountTypes.Items[0];
                DataGridAccountTypes.ScrollIntoView(DataGridAccountTypes.SelectedItem);
                DataGridAccountTypes.Focus();
            }
        }
        private void RadioSearchByCode_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            TextBoxSearch.MaxLength = 10;
            TextBoxSearch.PreviewTextInput += TextBox_NumPreviewTextInput;
            TextBoxSearch.TextChanged += TextBox_NumTextChanged;
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در کد نوع کاربری جستجو می شود.";
        }
        private void RadioSearchByProfile_Checked(object sender, RoutedEventArgs e)
        {                        
            TextBoxSearch.TextChanged -= TextBox_NumTextChanged;
            TextBoxSearch.PreviewTextInput -= TextBox_NumPreviewTextInput;            
            TextBoxSearch.MaxLength = 40;
            TextBoxSearch.Text = "";
            LastTextBoxSearch = "";
            ((ToolTip)ImageSearch.ToolTip).Content = "متن وارده شده در نام نوع کاربری جستجو می شود.";
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
        
        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            IEnumerable<AccountTypesTb> Tb = (IEnumerable<AccountTypesTb>)DataGridAccountTypes.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    DataTable Dt = Commons.ToDataTable<AccountTypesTb>(Tb);
                    Dt.TableName = "AccountTypes";

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\AccountTypes.mrt", GridHeaderAccountTypes.Background, "انواع کاربری", Dt);
                    PrintPrv.ShowDialog();
                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleAccountTypes, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleAccountTypes, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeaderAccountTypes.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }

        #endregion
        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("AccountTypes", "MainMenu");
        }
    }
}
