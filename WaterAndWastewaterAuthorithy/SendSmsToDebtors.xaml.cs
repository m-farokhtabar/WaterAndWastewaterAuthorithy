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
            ControlsArray.Add(TextBoxSubScriptionIdTo, new ControlsTab { Prev = TextBoxSubScriptionIdFrom, Next = TextBoxMeliCode, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxMeliCode, new ControlsTab { Prev = TextBoxSubScriptionIdTo, Next = TextBoxName, Type = ControlsType.TextBoxNumber });

            ControlsArray.Add(TextBoxName, new ControlsTab { Prev = TextBoxMeliCode, Next = TextBoxFamily, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFamily, new ControlsTab { Prev = TextBoxName, Next = TextBoxFather, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxFather, new ControlsTab { Prev = TextBoxFamily, Next = ButtonBeginSearch, Type = ControlsType.TextBoxText });

            ControlsArray.Add(ButtonBeginSearch, new ControlsTab { Prev = TextBoxFather, Next = DataGridView, Type = ControlsType.TextBoxText });

            ControlsArray.Add(TextBoxSmsMessage, new ControlsTab { Prev = ButtonBeginSearch, Next = DataGridView, Type = ControlsType.TextBoxText });

            ControlsArray.Add(DataGridView, new ControlsTab { Prev = TextBoxSmsMessage, Next = null, Type = ControlsType.TextBoxText });

            TextBoxSmsMessage.Text = Commons.WaterAndWastewaterAuthorityName.Trim() + "\n" +
                                     "نام مشترک:" + " " + "[Name] [Family]" + "\n" +
                                     "شماره اشتراک:" + " " + "[SubscriptionId]" + "\n" +
                                     "شماره کنتور:" + " " + "[WaterMeterSerial]" + "\n" +
                                     "تاریخ قرائت از:" + " " + "[PrevReadDate]" + " " + "تا:" + " " + "[CurrentReadDate]" + "\n" +
                                     "میزان مصرف:" + " " + "[Consumption]" + "\n" +
                                     "مبلغ قابل پرداخت:" + " " + "[Debt]" + "\n" +
                                     "مهلت پرداخت:" + " " + "[PaymentDeadLine]" + "\n" +
                                     "نحوه واریز:" + " " + "بانک صادرات" + " " + "0103076808008";

        }
        private void UserControlSendSmsToDebtors_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNew.Visibility == System.Windows.Visibility.Hidden && e.Key == Key.F8)
                ButtonSendSms_Click(null, null);
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
        private void UserControlSendSmsToDebtors_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataGridView.ItemsSource = Commons.Db.BillsVws.Where(x => x.Debt > 0).ToList().OrderBy(x => x.SortableId);
            FirstControl.Focus();
        }


        private void DoSearch()
        {
            string Sql = "";
            bool SubScriptionIdFromFlag = true;
            string SubScriptionIdFromText = "";
            bool SubScriptionIdToFlag = true;
            string SubScriptionIdToText = "";
            bool NameFlag = true;
            string NameText = "";
            bool FamilyFlag = true;
            string FamilyText = "";
            bool FatherFlag = true;
            string FatherText = "";
            bool CellphoneFlag = true;
            string Cellphone = "";

            string NationalCode = "";
            bool NationalCodeFlag = true;

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
            }
            if (TextBoxName.Text.Trim() != "")
            {
                NameFlag = false;
                NameText = TextBoxName.Text.Trim();
            }
            if (TextBoxFamily.Text.Trim() != "")
            {
                FamilyFlag = false;
                FamilyText = TextBoxFamily.Text.Trim();
            }
            if (TextBoxFather.Text.Trim() != "")
            {
                FatherFlag = false;
                FatherText = TextBoxFather.Text.Trim();
            }
            if (TextBoxMeliCode.Text.Trim() != "")
            {
                NationalCodeFlag = false;
                NationalCode = TextBoxMeliCode.Text.Trim();
            }
            if (TextBoxCellPhone.Text.Trim() != "")
            {
                CellphoneFlag = false;
                Cellphone = TextBoxCellPhone.Text.Trim();
            }
            if (SubScriptionIdFromFlag || SubScriptionIdToFlag || NameFlag || FamilyFlag || FatherFlag || NationalCodeFlag)
            {
                Sql = "Select * from BillsVw";// Where " + Sql;
                DataGridView.ItemsSource = Commons.Db.BillsVws.SqlQuery(Sql).ToList().Where(
                                           x => (SubScriptionIdFromFlag == true || x.SortableId.CompareTo(SubScriptionIdFromText) >= 0) &&
                                                (SubScriptionIdToFlag == true || x.SortableId.CompareTo(SubScriptionIdToText) <= 0) &&
                                                (NameFlag == true || x.Name.Contains(NameText)) &&
                                                (FamilyFlag == true || x.Family.Contains(FamilyText)) &&
                                                (FatherFlag == true || x.Father.Contains(FatherText)) &&
                                                (NationalCodeFlag == true || x.MeliCode == NationalCode) &&
                                                (CellphoneFlag == true || x.CellPhone == Cellphone) &&
                                                x.Debt > 0).OrderBy(x => x.SortableId);
            }
        }
        private void ButtonSendSms_Click(object sender, RoutedEventArgs e)
        {
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            IEnumerable<DomainClasses.BillsVw> Tb = (IEnumerable<DomainClasses.BillsVw>)DataGridView.ItemsSource;
            if (Tb != null)
            {
                if (Tb.ToList().Count > 0)
                {
                    var Msg = new MessageDialog(Messages.TitleSendSmsToDebtors, Messages.AreyouSureToSendSms, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {

                        List<SmsInfo> SmsList = new List<SmsInfo>();
                        foreach (var Row in Tb.ToList())
                        {
                            SmsInfo info = new SmsInfo()
                            {
                                SubId = Row.SubscriptionId,
                                Cellphone = Row.CellPhone,
                                Message = TextBoxSmsMessage.Text.Replace("[Name]", Row.Name.Trim())
                                                                .Replace("[Family]", Row.Family.Trim())
                                                                .Replace("[SubscriptionId]", Row.SubscriptionId.Trim())
                                                                .Replace("[WaterMeterSerial]", Row.WaterMeterSerial.Trim())
                                                                .Replace("[PrevReadDate]", Row.PrevReadDate.Trim())
                                                                .Replace("[CurrentReadDate]", Row.CurrentReadDate.Trim())
                                                                .Replace("[Consumption]", Row.Consumption.ToString().Trim())
                                                                .Replace("[Debt]", Commons.ConvertToMoney(Row.Debt.ToString().Trim()))
                                                                .Replace("[PaymentDeadLine]", Commons.GetDeadLinePaymentDate().Trim())
                            };
                            SmsList.Add(info);
                        }
                        var SmsSentDetials = new SmsSentDetails(SmsList);
                        SmsSentDetials.Owner = Window.GetWindow(this);
                        SmsSentDetials.ShowDialog();
                    }

                }
                else
                {
                    MessageDialog Msg = new MessageDialog(Messages.TitleSendSmsToDebtors, Messages.DataNotFoundForSms, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
                MessageDialog Msg = new MessageDialog(Messages.TitleSendSmsToDebtors, Messages.DataNotFoundForSms, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
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
