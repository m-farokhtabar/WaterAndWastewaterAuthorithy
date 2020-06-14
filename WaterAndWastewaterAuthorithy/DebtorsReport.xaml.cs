using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WaterAndWastewaterAuthorithy.Presentation;
using System.Data;
using System.Data.Entity;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for DebtorsReport.xaml
    /// </summary>
    public partial class DebtorsReport : UserControl
    {
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        AnimateControls Ac = new AnimateControls();
        string PrevHint = "";

        #region Loading
        public DebtorsReport()
        {
            InitializeComponent();
            FirstControl = TextBoxSubScriptionIdFrom;
            ControlsArray.Add(TextBoxSubScriptionIdFrom, new ControlsTab { Prev = DataGridView, Next = TextBoxSubScriptionIdTo, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxSubScriptionIdTo, new ControlsTab { Prev = TextBoxSubScriptionIdFrom, Next = TextBoxPayableFrom, Type = ControlsType.TextBoxText });

            ControlsArray.Add(TextBoxPayableFrom, new ControlsTab { Prev = TextBoxSubScriptionIdTo, Next = TextBoxPayableTo, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxPayableTo, new ControlsTab { Prev = TextBoxPayableFrom, Next = ButtonBeginSearch, Type = ControlsType.TextBoxNumber });

            ControlsArray.Add(ButtonBeginSearch, new ControlsTab { Prev = TextBoxPayableTo, Next = DataGridView, Type = ControlsType.TextBoxText });

            ControlsArray.Add(DataGridView, new ControlsTab { Prev = TextBoxPayableTo, Next = null, Type = ControlsType.TextBoxText });
        }
        private void UserControlBillsPrint_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControlBillsPrint_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DoSearch();
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
            bool SubScriptionIdFromFlag = true;
            string SubScriptionIdFromText = "";
            bool SubScriptionIdToFlag = true;
            string SubScriptionIdToText = "";

            long? DebFrom = 0;
            bool DebtFromFlag = true;
            long? DebTo = 0;
            bool DebtToFlag = true;

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
            if (TextBoxPayableFrom.Text.Trim() != "")
            {
                DebFrom = Convert.ToInt64(TextBoxPayableFrom.Text);
                DebtFromFlag = false;
            }
            if (TextBoxPayableTo.Text.Trim() != "")
            {
                DebTo = Convert.ToInt64(TextBoxPayableTo.Text);
                DebtToFlag = false;
            }
            DataGridView.ItemsSource = Commons.Db.Subscriptions.AsNoTracking().Include(x => x.Customer).Include(x => x.AccountType).Where(x => x.Year == Commons.CurrentYear &&
                                                x.Debt > 0 &&
                                                (DebtFromFlag == true || x.Debt >= DebFrom.Value) &&
                                                (DebtToFlag == true || x.Debt <= DebTo.Value)).ToList()
                                              .Where(x => (SubScriptionIdFromFlag == true || x.SortableId.CompareTo(SubScriptionIdFromText) >= 0) &&
                                                     (SubScriptionIdToFlag == true || x.SortableId.CompareTo(SubScriptionIdToText) <= 0)).OrderBy(x => x.SortableId).ToList();

        }
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
                    Dt.TableName = "DebtorsReport";

                    PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\DebtorsReport.mrt", GridHeader.Background, "بدهکاران", Dt);
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
            Ac.ShowWindow("DebtorsReport", "Reports");
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
