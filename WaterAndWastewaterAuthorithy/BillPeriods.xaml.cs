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
using System.Globalization;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for BillPeriods.xaml
    /// </summary>
    public partial class BillPeriods : UserControl
    {
        AnimateControls Ac = new AnimateControls();
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        int BillPeriodId = -1;
        string PrevHint = "";
        public BillPeriods()
        {
            InitializeComponent();
            FirstControl = TextBoxPeriodName;
            ControlsArray.Add(TextBoxPeriodName, new ControlsTab { Prev = TextBoxComments, Next = TextBoxCountOfMonth, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxCountOfMonth, new ControlsTab { Prev = TextBoxPeriodName, Next = RadioYes, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(RadioYes, new ControlsTab { Prev = TextBoxCountOfMonth, Next = RadioNo, Type = ControlsType.RadioButton });
            ControlsArray.Add(RadioNo, new ControlsTab { Prev = RadioYes, Next = TextBoxComments, Type = ControlsType.RadioButton });
            ControlsArray.Add(TextBoxComments, new ControlsTab { Prev = RadioNo, Next = null, Type = ControlsType.RadioButton });
        }

        private void UserControlBillPeriods_Loaded(object sender, RoutedEventArgs e)
        {
            BillPeriodId = -1;
        }
        private void UserControlBillPeriods_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue))
            {
                DataGridView.ItemsSource = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
                if (FirstControl.IsEnabled == true)
                    FirstControl.Focus();
                else
                    RadioYes.Focus();
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F5)
                ButtonNew_Click(null, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F11)
                ButtonPeriodClose_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.G)
            {
                DataGridView.Focus();
                if (DataGridView.Items != null && DataGridView.Items.Count > 0)
                {
                    DataGridView.SelectedIndex = 0;
                    DataGridView.SelectedItem = DataGridView.Items[0];
                    DataGridView.CurrentCell = new DataGridCellInfo(DataGridView.Items[0], DataGridView.Columns[0]);
                    DataGridView.Columns[0].GetCellContent(DataGridView.Items[0]).Focus();
                }
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    e.Handled = true;

                    if (((Control)sender).Name == "TextBoxCountOfMonth" && e.Key == Key.Enter && ((Control)sender).IsEnabled == true)
                        TextBoxCountOfMonthCheck();

                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                    if (Ctrl.IsEnabled == true)
                        Ctrl.Focus();
                    else
                        TextBox_KeyDown(Ctrl, e);
                }
                else
                {
                    if (e.Key == Key.Tab)
                    {
                        e.Handled = true;
                        if (FirstControl.IsEnabled == true)
                            FirstControl.Focus();
                        else
                            TextBox_KeyDown(FirstControl, e);
                    }
                    else
                    {
                        e.Handled = true;
                        if (((Control)sender).IsEnabled == true)
                            ButtonSave_Click(null, null);
                        else
                        {
                            if (FirstControl.IsEnabled == true)
                                FirstControl.Focus();
                            else
                                TextBox_KeyDown(FirstControl, e);
                        }
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                if (((ControlsTab)ControlsArray[sender]).Prev != null)
                {
                    e.Handled = true;
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Prev;
                    if (Ctrl.IsEnabled == true)
                        Ctrl.Focus();
                    else
                        TextBox_KeyDown(Ctrl, e);
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
            ((TextBox)sender).SelectAll();
            PrevHint = ((TextBox)sender).Tag.ToString();
            Hint.Text = ((TextBox)sender).Tag.ToString();
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }
        }

        #region Header Buttons
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArray);
            MessageDialog Msg = null;
            if (Vd != null)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {
                int Len = Convert.ToInt32(TextBoxCountOfMonth.Text);
                int Start = 0;
                string[] DateFromTo = null;

                DomainClasses.BillPeriodsTb MyBillPeriod = null;
                MyBillPeriod = Commons.Db.BillPeriods.Find(BillPeriodId);
                if (MyBillPeriod == null)
                {
                    Start = IsPeriodOk(Len);
                    if (Start == -1)
                    {
                        Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.PeriodNotInRange, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        TextBoxCountOfMonth.Focus();
                        TextBoxCountOfMonth.SelectAll();
                        return;
                    }
                    DateFromTo = GetDateFromAndDateTo(Start, Len);

                    if (RadioYes.IsChecked == true)
                    {
                        if (!IsPeriodSelecetedOK(-1, DateFromTo[0]))
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.IsNotPossibeToSelect, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            RadioYes.Focus();
                            return;
                        }
                    }
                    Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.SaveMessageBillPeriods, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            if (RadioYes.IsChecked == true)
                            {
                                var Bls = Commons.Db.BillPeriods.Select(x => x).Where(x => x.IsSelected == true && x.Year == Commons.CurrentYear);
                                foreach (BillPeriodsTb value in Bls)
                                    value.IsSelected = false;
                            }
                            Commons.Db.BillPeriods.Add(new DomainClasses.BillPeriodsTb
                            {
                                Year = Commons.CurrentYear,
                                IsSelected = Convert.ToBoolean(RadioYes.IsChecked),
                                IsClosed = false,
                                Name = TextBoxPeriodName.Text.Trim(),
                                DateFrom = DateFromTo[0].Trim(),
                                DateTo = DateFromTo[1].Trim(),
                                MonthPeriod = Len,
                                Description = TextBoxComments.Text.Trim()
                            });
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("BillPeriods");
                            DataGridView.ItemsSource = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
                            DataGridView.Items.Refresh();
                            Msg = new MessageDialog(Messages.SaveMessageBillPeriods, Messages.SaveMessageSuccessBillPeriods, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
                else
                {
                    DateFromTo = new string[2];
                    DateFromTo[0] = MyBillPeriod.DateFrom;
                    DateFromTo[1] = MyBillPeriod.DateTo;
                    if (RadioYes.IsChecked == true)
                    {
                        if (!IsPeriodSelecetedOK(MyBillPeriod.Id, DateFromTo[0]))
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.IsNotPossibeToSelect, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            RadioYes.Focus();
                            return;
                        }
                    }
                    Msg = new MessageDialog(Messages.EditMessageTitleBillPeriods, Messages.EditMessageBillPeriods, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() == true)
                    {
                        try
                        {
                            if (MyBillPeriod.IsClosed != true)
                            {
                                MyBillPeriod.Description = TextBoxComments.Text.Trim();
                                MyBillPeriod.Name = TextBoxPeriodName.Text.Trim();
                            }
                            if (RadioYes.IsChecked == true)
                            {
                               var Bls =  Commons.Db.BillPeriods.Select(x => x).Where(x=>x.IsSelected == true && x.Year==Commons.CurrentYear);
                               foreach (BillPeriodsTb value in Bls)
                                   value.IsSelected = false;
                            }
                            MyBillPeriod.IsSelected = Convert.ToBoolean(RadioYes.IsChecked);
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("BillPeriods");
                            DataGridView.ItemsSource = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
                            DataGridView.Items.Refresh();
                            Msg = new MessageDialog(Messages.EditMessageTitleBillPeriods, Messages.EditMessageSuccessBillPeriods, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            ButtonNew_Click(null, null);
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.EditMessageTitleBillPeriods, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
            }
        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            TextBoxPeriodName.Text = "";
            TextBoxPeriodName.IsEnabled = true;
            TextBoxComments.Text = "";
            TextBoxComments.IsEnabled = true;
            TextBoxCountOfMonth.Text = "";
            TextBoxCountOfMonth.IsEnabled = true;
            TextBlockMonths.Text = "";
            RadioNo.IsChecked = true;
            BillPeriodId = -1;
            FirstControl.Focus();
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
        private void ButtonPeriodClose_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg = null;
            DomainClasses.BillPeriodsTb MyBillPeriod = null;
            MyBillPeriod = Commons.Db.BillPeriods.Find(BillPeriodId);
            if (MyBillPeriod != null)
            {
                try
                {                    
                    if (MyBillPeriod.IsSelected)
                    {
                        if (MyBillPeriod.IsClosed == false)
                        {
                            Msg = new MessageDialog(Messages.CloseMessageTitleBillPeriods, Messages.PeriodsIsClosed, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            if (Msg.ShowDialog() == true)
                            {
                                MyBillPeriod.IsClosed = true;
                                Commons.Db.SaveChanges();
                                Commons.SetFromEdited("BillPeriods");
                                DataGridView.ItemsSource = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
                                DataGridView.Items.Refresh();
                                Msg = new MessageDialog(Messages.CloseMessageTitleBillPeriods, Messages.ClosedMessageSuccessed, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                            }
                        }
                        else
                        {
                            Msg = new MessageDialog(Messages.CloseMessageTitleBillPeriods, Messages.PeriodClosed, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.CloseMessageTitleBillPeriods, Messages.ImpossibleClosedMessage, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                catch
                {
                    Msg = new MessageDialog(Messages.CloseMessageTitleBillPeriods, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            else
            {
            }
        }
        #endregion Header Buttons

        #region Datagrid
        private void DataGridView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.D)
                ButtonDelete_Click(null, null);
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.E)
                ButtonShowEdit_Click(null, null);
            Commons.GridViewKeyDown(DataGridView, ref e, TextBoxPeriodName, "نام دوره قبض", "عملیات", "توضیحات", 7);
            if (TextBoxPeriodName.IsEnabled == false)
                RadioYes.Focus();
        }
        private void ButtonShowEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            try
            {
                ContentPresenter fe = (ContentPresenter)DataGridView.Columns[7].GetCellContent(DataGridView.SelectedItem);
                StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                string id = ((TextBlock)S.FindName("Id")).Text;
                BillPeriodsTb RowEdited = Commons.Db.BillPeriods.Find(Convert.ToInt32(id));
                if (RowEdited != null)
                {
                    TextBoxPeriodName.IsEnabled = true;
                    TextBoxComments.IsEnabled = true;
                    TextBoxCountOfMonth.IsEnabled = true;                    

                    BillPeriodId = Convert.ToInt32(id);
                    if (RowEdited.IsSelected)
                        RadioYes.IsChecked = true;
                    else
                        RadioNo.IsChecked = true;
                    TextBoxPeriodName.Text = RowEdited.Name.Trim();
                    TextBoxCountOfMonth.Text = RowEdited.MonthPeriod.ToString();
                    TextBoxCountOfMonth.IsEnabled = false;
                    TextBlockMonths.Text = GetNameOFMonthsInPeriod(RowEdited.DateFrom, RowEdited.DateTo);
                    TextBoxComments.Text = RowEdited.Description.Trim();
                    if (RowEdited.IsClosed)
                    {
                        TextBoxPeriodName.IsEnabled = false;
                        TextBoxComments.IsEnabled = false;
                        RadioYes.Focus();
                    }
                    else
                        FirstControl.Focus();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.EditMessageTitleBillPeriods, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg = null;
            try
            {
                ContentPresenter fe = (ContentPresenter)DataGridView.Columns[7].GetCellContent(DataGridView.SelectedItem);
                StackPanel S = (StackPanel)fe.ContentTemplate.FindName("StackEditDelete", fe);
                string id = ((TextBlock)S.FindName("Id")).Text;
                BillPeriodsTb RowDeleted = Commons.Db.BillPeriods.Find(Convert.ToInt32(id));
                if (RowDeleted != null)
                {
                    if (CheckPervRowIsExist(RowDeleted) == true)
                    {
                        Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.PrevRowExits, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                        DataGridView.Focus();
                    }
                    else
                    {
                        if (CheckPeriodIsUsed(RowDeleted) == true)
                        {
                            Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.PeriodInUseDelete, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                            DataGridView.Focus();
                        }
                        else
                        {
                            if (RowDeleted.IsClosed == true)
                            {
                                Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.DeletePeriodIsClosedBillPeriods, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                Msg.ShowDialog();
                                DataGridView.Focus();
                            }
                            else
                            {
                                Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.DeleteMessageBillPeriods, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                                Msg.Owner = Window.GetWindow(this);
                                if (Msg.ShowDialog() == true)
                                {
                                    Commons.Db.BillPeriods.Remove(RowDeleted);
                                    Commons.Db.SaveChanges();
                                    Commons.SetFromEdited("BillPeriods");
                                    DataGridView.ItemsSource = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
                                    DataGridView.Items.Refresh();
                                    Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.DeleteMessageSuccessBillPeriods, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                                    Msg.Owner = Window.GetWindow(this);
                                    Msg.ShowDialog();
                                    DataGridView.Focus();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.DeleteMessageTitleBillPeriods, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                DataGridView.Focus();
            }
        }
        #endregion Datagrid

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("BillPeriods", "MainMenu");
        }
        private void Radio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Right && sender == (object)RadioYes)
            {
                e.Handled = true;
            }
        }

        #region Private function
        private void TextBoxCountOfMonthCheck()
        {
            try
            {
                if (TextBoxCountOfMonth.Text.Trim() != "")
                {
                    int Len = Convert.ToInt32(TextBoxCountOfMonth.Text);
                    int Start = IsPeriodOk(Len);
                    if (Start > -1)
                    {
                        TextBlockMonths.Text = GetNameOFMonthsInPeriod(Start, Len);
                    }
                    else
                    {
                        MessageDialog Msg = new MessageDialog();
                        Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.PeriodNotInRange, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
            }
            catch
            {
                MessageDialog Msg = new MessageDialog();
                Msg = new MessageDialog(Messages.SaveMessageTitleBillPeriods, Messages.PeriodNotInRange, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private bool IsPeriodSelecetedOK(int Id, string DateFrom)
        {
            int Month = Convert.ToInt32(DateFrom.Split(new char[] { '/' })[1]);
            List<BillPeriodsTb> Lst = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
            foreach (BillPeriodsTb value in Lst)
            {
                int TmpMonth = Convert.ToInt32(value.DateFrom.Split(new char[] { '/' })[1]);
                if (TmpMonth < Month && value.IsClosed == false)
                    return false;
            }
            return true;
        }

        private int IsPeriodOk(int len)
        {
            if (len <= 0)
                return -1;
            bool[] IsFullMonth = new bool[12];
            List<BillPeriodsTb> ListP = (List<BillPeriodsTb>)DataGridView.ItemsSource;
            foreach (BillPeriodsTb value in ListP)
            {
                int TmpMonthFrom = Convert.ToInt32(value.DateFrom.Split(new char[] { '/' })[1]) - 1;
                int TmpMonthTo = Convert.ToInt32(value.DateTo.Split(new char[] { '/' })[1]) - 1;
                for (int i = TmpMonthFrom; i <= TmpMonthTo; i++)
                    IsFullMonth[i] = true;
            }
            int j = -1;
            for (int i = 0; i < 12; i++)
            {
                if (IsFullMonth[i] == false)
                {
                    j = i;
                    break;
                }
            }
            if (j >= 0 && j + len <= 12)
            {
                for (int i = j; i < j + len; i++)
                {
                    if (IsFullMonth[i] == true)
                    {
                        j = -1;
                        break;
                    }
                }
            }
            else
                j = -1;
            return j;
        }
        private string[] GetDateFromAndDateTo(int Start, int Len)
        {
            string[] s = new string[2];
            s[0] = Commons.CurrentYear.ToString() + "/" + (Start + 1).ToString("00") + "/01";

            if ((Start + Len) >= 1 && (Start + Len) <= 6)
                s[1] = Commons.CurrentYear.ToString() + "/" + (Start + Len).ToString("00") + "/31";
            else
                if ((Start + Len) >= 7 && (Start + Len) <= 11)
                    s[1] = Commons.CurrentYear.ToString() + "/" + (Start + Len).ToString("00") + "/30";
                else
                {
                    if ((Start + Len) == 12)
                    {
                        PersianCalendar P = new PersianCalendar();
                        if (P.IsLeapYear(Commons.CurrentYear))
                            s[1] = Commons.CurrentYear.ToString() + "/" + (Start + Len).ToString("00") + "/30";
                        else
                            s[1] = Commons.CurrentYear.ToString() + "/" + (Start + Len).ToString("00") + "/29";
                    }
                }
            return s;
        }
        private string GetNameOFMonthsInPeriod(int Start, int Len)
        {
            string s = "از ابتدای ";
            s += Commons.GetPersianNameOfMonths(Start + 1);
            s += " ماه ";
            s += " تا انتهای ";
            s += Commons.GetPersianNameOfMonths(Start + Len);
            s += " ماه ";
            return s;
        }
        private string GetNameOFMonthsInPeriod(string DateFrom, string DateTo)
        {
            int mFrom = Convert.ToInt32(DateFrom.Split(new char[] { '/' })[1]);
            int mTo = Convert.ToInt32(DateTo.Split(new char[] { '/' })[1]);
            string s = "از ابتدای ";
            s += Commons.GetPersianNameOfMonths(mFrom);
            s += " ماه ";
            s += " تا انتهای ";
            s += Commons.GetPersianNameOfMonths(mTo);
            s += " ماه ";
            return s;
        }

        private bool CheckPeriodIsUsed(BillPeriodsTb RowDeleted)
        {
            List<BillsTb> Lb = Commons.Db.Bills.Select(x => x).Where(x => (x.CurrentPeriod == RowDeleted.Id || x.CurrentPeriod == (RowDeleted.Id) * -1) && x.Year == Commons.CurrentYear).Take(1).ToList();
            if (Lb.Count == 0)
                return false;
            else
                return true;
        }
        private bool CheckPervRowIsExist(BillPeriodsTb RowDeleted)
        {
            List<BillPeriodsTb> Lb = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear).OrderBy(x => x.DateFrom).ToList();
            if (Lb.Count == 1 || Lb[Lb.Count - 1].Id == RowDeleted.Id)
                return false;
            else
                return true;
        }
        #endregion
    }
}
