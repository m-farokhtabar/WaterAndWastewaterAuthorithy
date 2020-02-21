using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WaterAndWastewaterAuthorithy.Presentation;
using System.Data;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        AnimateControls Ac = new AnimateControls();
        string PrevHint = "";
        public Reports()
        {
            InitializeComponent();
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonSub_Click(null, null);
            if (e.Key == Key.D2 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonDebt_Click(null, null);
            if (e.Key == Key.D3 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonTalab_Click(null, null);
            if (e.Key == Key.D4 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonNotBill_Click(null, null);
            if (e.Key == Key.D5 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                ButtonDore_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }
        private void UserControlReports_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonSub.Focus();
        }
        private void UserControlReports_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue))
            {
                int Ch = CheckPeriod();
                if (Ch == -1)
                {
                    MessageDialog Msg = new MessageDialog(Messages.PrintMessageReports, Messages.NotExistPeriodBilling, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);                    
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                    Ac.ShowWindow("Reports", "MainMenu");
                    return;
                }
            }
        }
        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Reports", "MainMenu");
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

        private void ButtonSub_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            try
            {
                IEnumerable<SubscriptionsTb> Tb = (IEnumerable<SubscriptionsTb>)Commons.Db.Subscriptions.Where(x=>x.Year == Commons.CurrentYear).ToList();
                if (Tb != null)
                {
                    if (Tb.ToList().Count > 0)
                    {
                        DataTable Dt = Commons.ToDataTable<SubscriptionsTb>(Tb);
                        Dt.TableName = "Subscriptions";

                        PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\SubscriptionsReport.mrt", GridHeader.Background, "وضعیت مالی اشتراک ها", Dt);
                        PrintPrv.ShowDialog();
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.PrintMessageReports, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }
        private void ButtonDebt_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            try
            {
                IEnumerable<SubscriptionsTb> Tb = (IEnumerable<SubscriptionsTb>)Commons.Db.Subscriptions.Where(x=>x.Debt>0 && x.Year == Commons.CurrentYear);
                if (Tb != null)
                {
                    if (Tb.ToList().Count > 0)
                    {
                        DataTable Dt = Commons.ToDataTable<SubscriptionsTb>(Tb);
                        Dt.TableName = "Subscriptions";

                        PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\SubscriptionsReport.mrt", GridHeader.Background, "بدهکاران اشتراک", Dt);
                        PrintPrv.ShowDialog();
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.PrintMessageReports, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }
        private void ButtonTalab_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            try
            {
                IEnumerable<SubscriptionsTb> Tb = (IEnumerable<SubscriptionsTb>)Commons.Db.Subscriptions.Where(x => x.Debt < 0 && x.Year == Commons.CurrentYear);
                if (Tb != null)
                {
                    if (Tb.ToList().Count > 0)
                    {
                        DataTable Dt = Commons.ToDataTable<SubscriptionsTb>(Tb);
                        Dt.TableName = "Subscriptions";

                        PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\SubscriptionsReport.mrt", GridHeader.Background, "بستانکاران اشتراک", Dt);
                        PrintPrv.ShowDialog();
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.PrintMessageReports, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }
        private void ButtonNotBill_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            this.ForceCursor = true;
            Cursor tmp = this.Cursor;
            this.Cursor = Cursors.Wait;
            try
            {
                IEnumerable<SubscriptionsTb> Tb = (IEnumerable<SubscriptionsTb>)Commons.Db.Subscriptions.SqlQuery("select * from SubscriptionsTbs where SubscriptionsTbs.Year = "+Commons.CurrentYear.ToString()+" AND Not Exists(select * from BillsTbs where BillsTbs.SubscriptionId = SubscriptionsTbs.Id AND BillsTbs.Year = SubscriptionsTbs.Year AND CurrentPeriod = "+Commons.CPeriod.Id.ToString()+" AND BillsTbs.Status != 1)");
                if (Tb != null)
                {
                    if (Tb.ToList().Count > 0)
                    {
                        DataTable Dt = Commons.ToDataTable<SubscriptionsTb>(Tb);
                        Dt.TableName = "Subscriptions";

                        PrintPreviewDialog PrintPrv = new PrintPreviewDialog(@"Reports\SubscriptionsReport.mrt", GridHeader.Background, "اشتراکاتی که در دوره جاری برایشان قبض صادر نشده است", Dt);
                        PrintPrv.ShowDialog();
                    }
                    else
                    {
                        Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
                else
                {
                    Msg = new MessageDialog(Messages.PrintMessageReports, Messages.DataNotFoundForPrint, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.PrintMessageReports, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
            this.Cursor = tmp;
        }
        private void ButtonDore_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Reports", "ReportSubAllPeriods");
        }

        private void ButtonSub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonDebt.Focus();
            if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonNotBill.Focus();
        }
        private void ButtonDebt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonTalab.Focus();
            if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonSub.Focus();
        }
        private void ButtonTalab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonNotBill.Focus();
            if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonDebt.Focus();
        }
        private void ButtonNotBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonDore.Focus();
            if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonTalab.Focus();
        }
        private void ButtonDore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonSub.Focus();
            if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                ButtonNotBill.Focus();
        }
        public int CheckPeriod()
        {
            Commons.SetPeriod();
            if (Commons.CPeriod.Id >= 1)
                return 0;
            else
                return -1;
        }
    }
}
