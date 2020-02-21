using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WaterAndWastewaterAuthorithy.Presentation;
using System.Data;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for SendSmsToDebtors.xaml
    /// </summary>
    public partial class SendSmsToDebtors : UserControl
    {
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        AnimateControls Ac = new AnimateControls();
        string PrevHint = "";

        #region Loading
        public SendSmsToDebtors()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionIdFrom;
            ControlsArray.Add(TextBoxSubScriptionIdFrom, new ControlsTab { Prev = DataGridView, Next = TextBoxSubScriptionIdTo, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxSubScriptionIdTo, new ControlsTab { Prev = TextBoxSubScriptionIdFrom, Next = TextBoxBillId, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxBillId, new ControlsTab { Prev = TextBoxSubScriptionIdTo, Next = TextBoxCustomerId, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxCustomerId, new ControlsTab { Prev = TextBoxBillId, Next = TextBoxName, Type = ControlsType.TextBoxNumber });

            ControlsArray.Add(TextBoxName, new ControlsTab { Prev = TextBoxCustomerId, Next = TextBoxFamily, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFamily, new ControlsTab { Prev = TextBoxName, Next = TextBoxFather, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFather, new ControlsTab { Prev = TextBoxFamily, Next = ComboBoxAccountType, Type = ControlsType.TextBoxText });

            ControlsArray.Add(ComboBoxAccountType, new ControlsTab { Prev = TextBoxFather, Next = TextBoxMeliCode, Type = ControlsType.ComboBox });
            ControlsArray.Add(TextBoxMeliCode, new ControlsTab { Prev = ComboBoxAccountType, Next = TextBoxPostalCode, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxPostalCode, new ControlsTab { Prev = TextBoxMeliCode, Next = TextBoxDateFrom, Type = ControlsType.TextBoxNumber });

            ControlsArray.Add(TextBoxDateFrom, new ControlsTab { Prev = TextBoxPostalCode, Next = TextBoxDateTo, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxDateTo, new ControlsTab { Prev = TextBoxDateFrom, Next = TextBoxAddress, Type = ControlsType.TextBoxText });

            ControlsArray.Add(TextBoxAddress, new ControlsTab { Prev = TextBoxDateTo, Next = ButtonBeginSearch, Type = ControlsType.TextBoxText });
            ControlsArray.Add(ButtonBeginSearch, new ControlsTab { Prev = TextBoxAddress, Next = DataGridView, Type = ControlsType.TextBoxText });

            ControlsArray.Add(DataGridView, new ControlsTab { Prev = TextBoxAddress, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlSendSmsToDebtors_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControlSendSmsToDebtors_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commons.LoadComboBox(ComboBoxAccountType, "AccountTypesTb", "نوع کاربری");
            ComboBoxAccountType.SelectedIndex = 0;
            DataGridView.ItemsSource = Commons.Db.BillsVws.ToList().OrderBy(x => x.SortableId);
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
        private void DataGridView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxSubScriptionIdFrom, "شماره پرونده", "10");
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

        private void DoSearch()
        {
            string Sql = "";
            bool SubScriptionIdFromFlag = true;
            string SubScriptionIdFromText = "";
            bool SubScriptionIdToFlag = true;
            string SubScriptionIdToText = "";
            bool BillIdFlag = true;
            int BillIdInt = 0;
            bool CustomerIdFlag = true;
            int CustomerIdInt = 0;
            bool NameFlag = true;
            string NameText = "";
            bool FamilyFlag = true;
            string FamilyText = "";
            bool FatherFlag = true;
            string FatherText = "";
            bool AccountTypeIdFlag = true;
            int AccountTypeId = 0;
            string NationalCode = "";
            bool NationalCodeFlag = true;
            string PostalCode = "";
            bool PostalCodeFlag = true;
            string DateFrom = "";
            bool DateFromFlag = true;
            string DateTo = "";
            bool DateToFlag = true;
            string Address = "";
            bool AddressFlag = true;

            if (TextBoxSubScriptionIdFrom.Text.Trim() != "")
            {
                SubScriptionIdFromFlag = false;
                string[] Tmp = TextBoxSubScriptionIdFrom.Text.Trim().Split(new char[] { '-' });
                if (Tmp != null && Tmp.Length == 2)
                    SubScriptionIdFromText = Convert.ToInt32(Tmp[0]).ToString("0000") + "-" + Convert.ToInt32(Tmp[1]).ToString("00000");
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.ValueIsSubscriptionNumberStart + "از شماره اشتراک" + Messages.ValueIsSubscriptionNumberEnd, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    return;

                }
                //Sql = "SubscriptionId='" + TextBoxSubScriptionId.Text.Trim() + "'";
            }
            if (TextBoxSubScriptionIdTo.Text.Trim() != "")
            {
                SubScriptionIdToFlag = false;
                string[] Tmp = TextBoxSubScriptionIdTo.Text.Trim().Split(new char[] { '-' });
                if (Tmp != null && Tmp.Length == 2)
                    SubScriptionIdToText = Convert.ToInt32(Tmp[0]).ToString("0000") + "-" + Convert.ToInt32(Tmp[1]).ToString("00000");
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageTitleSubScription, Messages.ValueIsSubscriptionNumberStart + "تا شماره اشتراک" + Messages.ValueIsSubscriptionNumberEnd, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    return;
                }
                //Sql = "SubscriptionId='" + TextBoxSubScriptionId.Text.Trim() + "'";
            }
            if (TextBoxBillId.Text.Trim() != "")
            {
                BillIdFlag = false;
                BillIdInt = Convert.ToInt32(TextBoxBillId.Text);
                //if (Sql.Trim() != "")
                //    Sql += " AND BillId=" + TextBoxBillId.Text;
                //else
                //    Sql += " BillId=" + TextBoxBillId.Text;
            }
            if (TextBoxCustomerId.Text.Trim() != "")
            {
                CustomerIdFlag = false;
                CustomerIdInt = Convert.ToInt32(TextBoxCustomerId.Text);
                //if (Sql.Trim() != "")
                //    Sql += " AND CustomerId=" + TextBoxCustomerId.Text.Trim();
                //else
                //    Sql += " CustomerId=" + TextBoxCustomerId.Text.Trim();
            }
            if (TextBoxName.Text.Trim() != "")
            {
                NameFlag = false;
                NameText = TextBoxName.Text.Trim();
                //if (Sql.Trim() != "")
                //    Sql += " AND Name like N'%" + TextBoxName.Text.Trim() + "%'";
                //else
                //    Sql += " Name like N'%" + TextBoxName.Text.Trim() + "%'";
            }
            if (TextBoxFamily.Text.Trim() != "")
            {
                FamilyFlag = false;
                FamilyText = TextBoxFamily.Text.Trim();
                //if (Sql.Trim() != "")
                //    Sql += " AND Family like N'%" + TextBoxFamily.Text.Trim() + "%'";
                //else
                //    Sql += " Family like N'%" + TextBoxFamily.Text.Trim() + "%'";
            }
            if (TextBoxFather.Text.Trim() != "")
            {
                FatherFlag = false;
                FatherText = TextBoxFather.Text.Trim();
                //if (Sql.Trim() != "")
                //    Sql += " AND Father like N'%" + TextBoxFather.Text.Trim() + "%'";
                //else
                //    Sql += " Father like N'%" + TextBoxFather.Text.Trim() + "%'";
            }
            if (ComboBoxAccountType.SelectedIndex > 0)
            {
                AccountTypeIdFlag = false;
                AccountTypeId = ComboBoxAccountType.SelectedIndex;
                //if (Sql.Trim() != "")
                //    Sql += " AND AccountTypeId = " + ComboBoxAccountType.SelectedIndex.ToString();
                //else
                //    Sql += " AccountTypeId = " + ((ComboBoxItem)ComboBoxAccountType.SelectedItem).Tag.ToString().Trim();
            }
            if (TextBoxMeliCode.Text.Trim() != "")
            {
                NationalCodeFlag = false;
                NationalCode = TextBoxMeliCode.Text.Trim();
                //if (Sql.Trim() != "")
                //    Sql += " AND MeliCode = " + TextBoxMeliCode.Text.Trim();
                //else
                //    Sql += " MeliCode = " + TextBoxMeliCode.Text.Trim();
            }
            if (TextBoxPostalCode.Text.Trim() != "")
            {
                PostalCode = TextBoxPostalCode.Text.Trim();
                PostalCodeFlag = false;
                //if (Sql.Trim() != "")
                //    Sql += " AND PostalCode = " + TextBoxPostalCode.Text.Trim();
                //else
                //    Sql += " PostalCode = " + TextBoxPostalCode.Text.Trim();
            }
            if (TextBoxDateFrom.Text.Trim() != "" && TextBoxDateTo.Text.Trim() != "")
            {
                DateFrom = TextBoxDateFrom.Text.Trim();
                DateFromFlag = false;
                DateTo = TextBoxDateTo.Text.Trim();
                DateToFlag = false;

                //if (Sql.Trim() != "")
                //    Sql += " AND (CurrentReadDate >= '" + TextBoxDateFrom.Text.Trim() + "' AND CurrentReadDate <= '" + TextBoxDateTo.Text.Trim() + "')";
                //else
                //    Sql += " (CurrentReadDate >= '" + TextBoxDateFrom.Text.Trim() + "' AND CurrentReadDate <= '" + TextBoxDateTo.Text.Trim() + "')";
            }
            else
            {
                if (TextBoxDateFrom.Text.Trim() != "")
                {
                    DateFrom = TextBoxDateFrom.Text.Trim();
                    DateFromFlag = false;
                    //if (Sql.Trim() != "")
                    //    Sql += " AND CurrentReadDate >= '" + TextBoxDateFrom.Text.Trim() + "'";
                    //else
                    //    Sql += " CurrentReadDate >= '" + TextBoxDateFrom.Text.Trim() + "'";
                }
                if (TextBoxDateTo.Text.Trim() != "")
                {
                    DateTo = TextBoxDateTo.Text.Trim();
                    DateToFlag = false;
                    //if (Sql.Trim() != "")
                    //    Sql += " AND CurrentReadDate <= '" + TextBoxDateTo.Text.Trim() + "'";
                    //else
                    //    Sql += " CurrentReadDate <= '" + TextBoxDateTo.Text.Trim() + "'";
                }
            }
            if (TextBoxAddress.Text.Trim() != "")
            {
                Address = TextBoxAddress.Text.Trim();
                AddressFlag = false;
                //if (Sql.Trim() != "")
                //    Sql += " AND Address like N'%" + TextBoxAddress.Text.Trim() + "%'";
                //else
                //    Sql += " Address like N'%" + TextBoxAddress.Text.Trim() + "%'";
            }
            if (SubScriptionIdFromFlag || SubScriptionIdToFlag || BillIdFlag || CustomerIdFlag || NameFlag || FamilyFlag || FatherFlag || AccountTypeIdFlag || NationalCodeFlag || PostalCodeFlag || DateFromFlag || DateToFlag || AddressFlag)
            {
                Sql = "Select * from BillsVw";// Where " + Sql;
                DataGridView.ItemsSource = Commons.Db.BillsVws.SqlQuery(Sql).ToList().Where(
                                           x => (SubScriptionIdFromFlag == true || x.SortableId.CompareTo(SubScriptionIdFromText) >= 0) &&
                                                (SubScriptionIdToFlag == true || x.SortableId.CompareTo(SubScriptionIdToText) <= 0) &&
                                                (BillIdFlag == true || x.BillId == BillIdInt) &&
                                                (CustomerIdFlag == true || x.CustomerId == CustomerIdInt) &&
                                                (NameFlag == true || x.Name.Contains(NameText)) &&
                                                (FamilyFlag == true || x.Family.Contains(FamilyText)) &&
                                                (FatherFlag == true || x.Father.Contains(FatherText)) &&
                                                (AccountTypeIdFlag == true || x.AccountTypeId == AccountTypeId) &&
                                                (NationalCodeFlag == true || x.MeliCode == NationalCode) &&
                                                (PostalCodeFlag == true || x.PostalCode == PostalCode) &&
                                                (DateFromFlag == true || x.PrevReadDate.CompareTo(DateFrom) >= 0) &&
                                                (DateToFlag == true || x.PrevReadDate.CompareTo(DateTo) <= 0) &&
                                                (AddressFlag == true || x.Address.Contains(Address))
                                           ).OrderBy(x => x.SortableId);
            }
        }
        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            IEnumerable<DomainClasses.BillsVw> Tb = (IEnumerable<DomainClasses.BillsVw>)DataGridView.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    DataTable Dt = Commons.ToDataTable<DomainClasses.BillsVw>(Tb);
                    Dt.TableName = "BillsPrint";

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\BillsPrint.mrt", GridHeader.Background, Dt);
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
            Ac.ShowWindow("SendSmsToDebtors", "MainMenu");
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
