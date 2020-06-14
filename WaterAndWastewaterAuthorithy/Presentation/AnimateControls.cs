using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WaterAndWastewaterAuthorithy.Presentation
{

    struct SelectedButton
    {
        public Brush Foreground;
        public Object ImageButton;
        public Button ButtonSelected;
    }

    class AnimateControls
    {
        private Grid AnimateRowGridGridSource = null;

        public void SelectButton(object sender, ref SelectedButton PrvButton, UserControl Parent)
        {
            if (PrvButton.ButtonSelected != null)
            {
                ((Border)(PrvButton.ButtonSelected).Template.FindName("BorderOfHeaderButtons", PrvButton.ButtonSelected)).Background = (Brush)App.Current.TryFindResource("BrushBackgroundButtonCustomerHeader");
                ((TextBlock)(PrvButton.ButtonSelected).Template.FindName("TextOfHeaderButtons", PrvButton.ButtonSelected)).Foreground = PrvButton.Foreground;
                (PrvButton.ButtonSelected).Resources["ImageButton"] = PrvButton.ImageButton;
                PrvButton.ButtonSelected.IsEnabled = true;
            }

            PrvButton.Foreground = ((TextBlock)((Button)sender).Template.FindName("TextOfHeaderButtons", ((Button)sender))).Foreground;
            PrvButton.ImageButton = ((Button)sender).Resources["ImageButton"];
            PrvButton.ButtonSelected = ((Button)sender);


            ((Button)sender).IsEnabled = false;
            ((Border)((Button)sender).Template.FindName("BorderOfHeaderButtons", ((Button)sender))).Background = new SolidColorBrush(Color.FromArgb(255, 235, 233, 253));
            ((Button)sender).Resources["ImageButton"] = Parent.FindResource(((Button)sender).Tag.ToString());
            ((TextBlock)((Button)sender).Template.FindName("TextOfHeaderButtons", ((Button)sender))).Foreground = new SolidColorBrush(Color.FromRgb(1, 49, 125));
        }
        public void SelectButtonRed(object sender, ref SelectedButton PrvButton, UserControl Parent)
        {
            if (PrvButton.ButtonSelected != null)
            {
                ((Border)(PrvButton.ButtonSelected).Template.FindName("BorderOfHeaderButtons", PrvButton.ButtonSelected)).Background = (Brush)App.Current.TryFindResource("BrushBackgroundButtonCustomerHeaderRed");
                ((TextBlock)(PrvButton.ButtonSelected).Template.FindName("TextOfHeaderButtons", PrvButton.ButtonSelected)).Foreground = PrvButton.Foreground;
                (PrvButton.ButtonSelected).Resources["ImageButton"] = PrvButton.ImageButton;
                PrvButton.ButtonSelected.IsEnabled = true;
            }

            PrvButton.Foreground = ((TextBlock)((Button)sender).Template.FindName("TextOfHeaderButtons", ((Button)sender))).Foreground;
            PrvButton.ImageButton = ((Button)sender).Resources["ImageButton"];
            PrvButton.ButtonSelected = ((Button)sender);


            ((Button)sender).IsEnabled = false;
            ((Border)((Button)sender).Template.FindName("BorderOfHeaderButtons", ((Button)sender))).Background = new SolidColorBrush(Color.FromArgb(255, 253, 233, 233));
            ((Button)sender).Resources["ImageButton"] = Parent.FindResource(((Button)sender).Tag.ToString());
            ((TextBlock)((Button)sender).Template.FindName("TextOfHeaderButtons", ((Button)sender))).Foreground = new SolidColorBrush(Color.FromRgb(125, 1, 36));
        }
        public void AnimateButton(ColumnDefinition[] Columns, Button[] Buttons, bool Expand)
        {
            for (int i = 0; i < Columns.Length; i++)
            {
                GridLengthAnimation AnimCol = new GridLengthAnimation();
                if (Expand == false)
                {
                    AnimCol.From = new GridLength(Buttons[i].Width + 10);
                    AnimCol.To = new GridLength(0);
                }
                else
                {
                    AnimCol.From = new GridLength(0);
                    AnimCol.To = new GridLength(Buttons[i].Width + 10);
                }
                AnimCol.Duration = new Duration(TimeSpan.FromSeconds(0.4));
                Columns[i].BeginAnimation(ColumnDefinition.WidthProperty, AnimCol);
            }

        }
        public void AnimateRowGrid(RowDefinition Source, RowDefinition Destination, Grid GridSource, Grid GridDestination, ref GridLength PrevHeight)
        {
            AnimateRowGridGridSource = GridSource;
            GridSource.Visibility = System.Windows.Visibility.Visible;
            GridDestination.Visibility = System.Windows.Visibility.Visible;

            GridLengthAnimation GridAnimHiddenCol = new GridLengthAnimation();
            GridAnimHiddenCol.From = Source.Height;
            GridAnimHiddenCol.To = new GridLength(0);
            GridAnimHiddenCol.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            GridLengthAnimation GridAnimVisibleCol = new GridLengthAnimation();
            GridAnimVisibleCol.From = new GridLength(0);
            GridAnimVisibleCol.To = PrevHeight;
            GridAnimVisibleCol.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            GridAnimVisibleCol.Completed += GridAnimVisibleCol_Completed;


            PrevHeight = Source.Height;
            Source.BeginAnimation(RowDefinition.HeightProperty, GridAnimHiddenCol);
            Destination.BeginAnimation(RowDefinition.HeightProperty, GridAnimVisibleCol);
        }
        private void GridAnimVisibleCol_Completed(object sender, EventArgs e)
        {
            AnimateRowGridGridSource.Visibility = System.Windows.Visibility.Hidden;
        }

        public void ShowWindow(string Source, string Destination)
        {
            Main MainWindowApp = Commons.MainWindow;

            switch (Source)
            {
                case "MainMenu":
                    MainWindowApp.UserControlMainMenu.Visibility = Visibility.Hidden;
                    MainWindowApp.MainMenu.Width = new GridLength(0);
                    break;

                case "Customers":
                    MainWindowApp.UserControlCustomers.Visibility = Visibility.Hidden;
                    MainWindowApp.Customers.Width = new GridLength(0);
                    break;
                case "Subscriptions":
                    MainWindowApp.UserControlSubscriptions.Visibility = Visibility.Hidden;
                    MainWindowApp.Subscriptions.Width = new GridLength(0);
                    break;
                case "AccountTypes":
                    MainWindowApp.UserControlAccountTypes.Visibility = Visibility.Hidden;
                    MainWindowApp.AccountTypes.Width = new GridLength(0);
                    break;
                /*                   case "Billing":
                                       MainWindowApp.UserControlBilling.Visibility = Visibility.Hidden;
                                       MainWindowApp.Billing.Width = new GridLength(0);                        
                                       break;*/
                case "SingleBilling":
                    MainWindowApp.UserControlSingleBilling.Visibility = Visibility.Hidden;
                    MainWindowApp.SingleBilling.Width = new GridLength(0);
                    break;
                case "WaterMeters":
                    MainWindowApp.UserControlWaterMeters.Visibility = Visibility.Hidden;
                    MainWindowApp.WaterMeters.Width = new GridLength(0);
                    break;
                case "BillsReceivable":
                    MainWindowApp.UserControlBillsReceivable.Visibility = Visibility.Hidden;
                    MainWindowApp.BillsReceivable.Width = new GridLength(0);
                    break;
                case "BillsCancelling":
                    MainWindowApp.UserControlBillsCancelling.Visibility = Visibility.Hidden;
                    MainWindowApp.BillsCancelling.Width = new GridLength(0);
                    break;
                case "BillsPrint":
                    MainWindowApp.UserControlBillsPrint.Visibility = Visibility.Hidden;
                    MainWindowApp.BillsPrint.Width = new GridLength(0);
                    break;
                case "BillPeriods":
                    MainWindowApp.UserControlBillPeriods.Visibility = Visibility.Hidden;
                    MainWindowApp.BillPeriods.Width = new GridLength(0);
                    break;
                case "Reports":
                    MainWindowApp.UserControlReports.Visibility = Visibility.Hidden;
                    MainWindowApp.Reports.Width = new GridLength(0);
                    break;
                case "Settings":
                    MainWindowApp.UserControlSettings.Visibility = Visibility.Hidden;
                    MainWindowApp.Settings.Width = new GridLength(0);
                    break;
                case "About":
                    MainWindowApp.UserControlAbout.Visibility = Visibility.Hidden;
                    MainWindowApp.About.Width = new GridLength(0);
                    break;
                case "ReportSubAllPeriods":
                    MainWindowApp.UserControlReportSubAllPeriods.Visibility = Visibility.Hidden;
                    MainWindowApp.ReportSubAllPeriods.Width = new GridLength(0);
                    break;
                case "SendSmsToDebtors":
                    MainWindowApp.UserControlSendSmsToDebtors.Visibility = Visibility.Hidden;
                    MainWindowApp.SendSmsToDebtors.Width = new GridLength(0);
                    break;
                case "ReadingListReport":
                    MainWindowApp.UserControlReadingListReport.Visibility = Visibility.Hidden;
                    MainWindowApp.ReadingListReport.Width = new GridLength(0);
                    break;
                case "DebtorsReport":
                    MainWindowApp.UserControlDebtorsReport.Visibility = Visibility.Hidden;
                    MainWindowApp.DebtorsReport.Width = new GridLength(0);
                    break;
            }
            switch (Destination)
            {
                case "MainMenu":
                    MainWindowApp.MainMenu.Width = new GridLength(920);
                    MainWindowApp.UserControlMainMenu.Visibility = Visibility.Visible;
                    break;

                case "Customers":
                    MainWindowApp.Customers.Width = new GridLength(920);
                    MainWindowApp.UserControlCustomers.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlCustomers.GridNewCustomer.Visibility == Visibility.Visible)
                    {
                        MainWindowApp.UserControlCustomers.FirstControlNew.Focus();
                        ((TextBox)MainWindowApp.UserControlCustomers.FirstControlNew).SelectAll();
                    }
                    else
                    {
                        MainWindowApp.UserControlCustomers.FirstControlSearch.Focus();
                    }
                    break;
                case "Subscriptions":
                    MainWindowApp.Subscriptions.Width = new GridLength(920);
                    MainWindowApp.UserControlSubscriptions.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlSubscriptions.GridNew.Visibility == Visibility.Visible)
                    {
                        MainWindowApp.UserControlSubscriptions.FirstControl.Focus();
                        ((TextBox)MainWindowApp.UserControlSubscriptions.FirstControl).SelectAll();
                    }
                    else
                    {
                        MainWindowApp.UserControlCustomers.FirstControlSearch.Focus();
                    }
                    break;
                case "AccountTypes":
                    MainWindowApp.AccountTypes.Width = new GridLength(920);
                    MainWindowApp.UserControlAccountTypes.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlAccountTypes.GridNewAccountType.Visibility == Visibility.Visible)
                    {
                        MainWindowApp.UserControlAccountTypes.FirstControlNewAccountType.Focus();
                        ((TextBox)MainWindowApp.UserControlAccountTypes.FirstControlNewAccountType).SelectAll();
                    }
                    else
                    {
                        MainWindowApp.UserControlAccountTypes.FirstControlSearch.Focus();
                    }
                    break;
                /*                    case "Billing":
                                        MainWindowApp.Billing.Width = new GridLength(920);
                                        MainWindowApp.UserControlBilling.Visibility = Visibility.Visible;
                                        if (MainWindowApp.UserControlBilling.GridNew.Visibility == Visibility.Visible)
                                            MainWindowApp.UserControlBilling.FirstControl.Focus();
                                        else
                                            MainWindowApp.UserControlBilling.TextBoxSearch.Focus();
                                        break;*/
                case "SingleBilling":
                    MainWindowApp.SingleBilling.Width = new GridLength(920);
                    MainWindowApp.UserControlSingleBilling.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlSingleBilling.GridNew.Visibility == Visibility.Visible)
                        MainWindowApp.UserControlSingleBilling.FirstControl.Focus();
                    else
                        MainWindowApp.UserControlSingleBilling.FirstControlSearch.Focus();
                    break;
                case "WaterMeters":
                    MainWindowApp.WaterMeters.Width = new GridLength(920);
                    MainWindowApp.UserControlWaterMeters.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlWaterMeters.GridNew.Visibility == Visibility.Visible)
                        MainWindowApp.UserControlWaterMeters.FirstControl.Focus();
                    else
                        MainWindowApp.UserControlWaterMeters.FirstControlSearch.Focus();
                    break;
                case "BillsReceivable":
                    MainWindowApp.BillsReceivable.Width = new GridLength(920);
                    MainWindowApp.UserControlBillsReceivable.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlBillsReceivable.GridNew.Visibility == Visibility.Visible)
                        MainWindowApp.UserControlBillsReceivable.FirstControl.Focus();
                    else
                        MainWindowApp.UserControlBillsReceivable.FirstControlSearch.Focus();
                    break;
                case "BillsCancelling":
                    MainWindowApp.BillsCancelling.Width = new GridLength(920);
                    MainWindowApp.UserControlBillsCancelling.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlBillsCancelling.GridNew.Visibility == Visibility.Visible)
                        MainWindowApp.UserControlBillsCancelling.FirstControl.Focus();
                    else
                        MainWindowApp.UserControlBillsCancelling.FirstControlSearch.Focus();
                    break;
                case "BillsPrint":
                    MainWindowApp.BillsPrint.Width = new GridLength(920);
                    MainWindowApp.UserControlBillsPrint.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlBillsPrint.FirstControl.Focus();
                    break;
                case "BillPeriods":
                    MainWindowApp.BillPeriods.Width = new GridLength(920);
                    MainWindowApp.UserControlBillPeriods.Visibility = Visibility.Visible;
                    if (MainWindowApp.UserControlBillPeriods.FirstControl.IsEnabled == true)
                    {
                        MainWindowApp.UserControlBillPeriods.FirstControl.Focus();
                        ((TextBox)MainWindowApp.UserControlBillPeriods.FirstControl).SelectAll();
                    }
                    else
                        MainWindowApp.UserControlBillPeriods.RadioYes.Focus();
                    break;
                case "Reports":
                    MainWindowApp.Reports.Width = new GridLength(920);
                    MainWindowApp.UserControlReports.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlReports.ButtonSub.Focus();
                    break;
                case "Settings":
                    MainWindowApp.Settings.Width = new GridLength(920);
                    MainWindowApp.UserControlSettings.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlSettings.FirstControl.Focus();
                    ((ComboBox)MainWindowApp.UserControlSettings.FirstControl).IsDropDownOpen = true;
                    break;
                case "About":
                    MainWindowApp.About.Width = new GridLength(920);
                    MainWindowApp.UserControlAbout.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlAbout.ButtonReturn.Focus();
                    break;
                case "ReportSubAllPeriods":
                    MainWindowApp.ReportSubAllPeriods.Width = new GridLength(920);
                    MainWindowApp.UserControlReportSubAllPeriods.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlReportSubAllPeriods.ButtonReturn.Focus();
                    break;
                case "SendSmsToDebtors":
                    MainWindowApp.SendSmsToDebtors.Width = new GridLength(920);
                    MainWindowApp.UserControlSendSmsToDebtors.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlSendSmsToDebtors.ButtonReturn.Focus();
                    break;
                case "ReadingListReport":
                    MainWindowApp.ReadingListReport.Width = new GridLength(920);
                    MainWindowApp.UserControlReadingListReport.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlReadingListReport.ButtonReturn.Focus();
                    break;
                case "DebtorsReport":
                    MainWindowApp.DebtorsReport.Width = new GridLength(920);
                    MainWindowApp.UserControlDebtorsReport.Visibility = Visibility.Visible;
                    MainWindowApp.UserControlDebtorsReport.ButtonReturn.Focus();
                    break;
            }
        }
    }
}
