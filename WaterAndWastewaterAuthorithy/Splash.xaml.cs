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
using System.Windows.Shapes;
using System.Data;
using System.Data.Entity;
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {        
        public Splash()
        {
            InitializeComponent();
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            MessageDialog Msg;
            try
            {
                Database.SetInitializer<Context>(null);                
                List<DomainClasses.SettingsTb> Tb = Commons.Db.Settings.ToList();
                if (Tb.Count > 0)
                {                    
                    Commons.CurrentYear = Tb[0].CurrentYear;
                    Commons.Address = Tb[0].Address;
                    Commons.BillMessage = Tb[0].BillMessage;
                    Commons.CompanyName = Tb[0].CompanyName;
                    Commons.Tel = Tb[0].Tel;
                    Commons.Vat = Tb[0].Vat;
                    Commons.Subscription = Tb[0].Subscription;
                    Commons.WaterAndWastewaterAuthorityName = Tb[0].WaterAndWastewaterAuthorityName;
                    if (Commons.CurrentYear <= 0)
                    {
                        Msg = new MessageDialog(Messages.SplashLoading, Messages.MainMenuYearNotSelected, MessageDialogButtons.Ok, MessageDialogType.Warning, this.Background);
                        Msg.Owner = this;
                        Msg.ShowDialog();
                    }

                }
                else
                {
                    Commons.CurrentYear = 0;
                    Commons.Address = "";
                    Commons.BillMessage = "";
                    Commons.CompanyName = "";
                    Commons.Tel = "";
                    Commons.Vat = 0;
                    Commons.WaterAndWastewaterAuthorityName = "";
                    Commons.Subscription = 0;
                    Msg = new MessageDialog(Messages.SplashLoading, "مشخصات اولیه سیستم شامل سال مالی، ارزش افزوده، نام سازمان و ...تنظیم نمی باشد.", MessageDialogButtons.Ok, MessageDialogType.Warning, this.Background);
                    Msg.Owner = this;
                    Msg.ShowDialog();
                }
                
                List<DomainClasses.BillPeriodsTb> Tbp = Commons.Db.BillPeriods.Select(x => x).Where(x => x.Year == Commons.CurrentYear && x.IsSelected == true).ToList();
                if (Tbp.Count > 0)
                {
                    Commons.CPeriod.Id = Tbp[0].Id;
                    Commons.CPeriod.Name = Tbp[0].Name;
                    Commons.CPeriod.DateFrom = Tbp[0].DateFrom;
                    Commons.CPeriod.DateTo = Tbp[0].DateTo;
                    Commons.CPeriod.MonthPeriod = Tbp[0].MonthPeriod;
                    Commons.CPeriod.IsClosed = Tbp[0].IsClosed;
                }
                else
                {
                    if (Commons.CurrentYear != 0)
                    {
                        Commons.CPeriod.Id = 0;
                        Commons.CPeriod.Name = "";
                        Commons.CPeriod.DateFrom = "";
                        Commons.CPeriod.DateTo = "";
                        Commons.CPeriod.MonthPeriod = 0;
                        Commons.CPeriod.IsClosed = false;
                        Msg = new MessageDialog(Messages.SplashLoading, "لطفا دوره قبوض سیستم را انتخاب نمایید.", MessageDialogButtons.Ok, MessageDialogType.Warning, this.Background);
                        Msg.Owner = this;
                        Msg.ShowDialog();
                    }
                }

                Commons.MainWindow = new Main();
                //Commons.MainWindow.UserControlBilling.LoadThisPeriodBillsList = true;
                //Commons.MainWindow.UserControlBilling.LoadFirst();
                //Commons.MainWindow.UserControlBilling.FormLoaded = false;
                Commons.MainWindow.Show();
                this.Hide();
            }
            catch
            {
                Msg = new MessageDialog(Messages.SplashLoading, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, this.Background);
                Msg.Owner = this;
                Msg.ShowDialog();
                Environment.Exit(0);
            }
        }
    }
}
