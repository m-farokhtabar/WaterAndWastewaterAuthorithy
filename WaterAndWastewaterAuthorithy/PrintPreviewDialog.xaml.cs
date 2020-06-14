using System;
using System.Data;
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
using System.IO;
using System.Windows.Xps.Packaging;
using WaterAndWastewaterAuthorithy.Presentation;
using System.Threading;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using WaterAndWastewaterAuthorithy.DataLayers;
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for PrintPreviewDialog.xaml
    /// </summary>
    public partial class PrintPreviewDialog : Window
    {

        private string ReportPath = "";
        private Brush HeaderColor;
        private string ReportTitle = "";
        private DataTable DataSource = null;
        private bool IsFormPrint;
        private bool IsBillPrint;
        private StiReport Rp;

        public PrintPreviewDialog()
        {
            InitializeComponent();
        }
        public PrintPreviewDialog(string RptPath, Brush BackColor, string RptTitle, DataTable Source)
        {
            InitializeComponent();
            ReportPath = RptPath;
            HeaderColor = BackColor;
            ReportTitle = RptTitle;
            DataSource = Source;
            IsFormPrint = true;
            IsBillPrint = false;
        }
        public PrintPreviewDialog(string RptPath, Brush BackColor, DataTable Source)
        {
            InitializeComponent();
            ReportPath = RptPath;
            HeaderColor = BackColor;
            DataSource = Source;
            IsFormPrint = false;
            IsBillPrint = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Rp = new StiReport();
                if (File.Exists(ReportPath))
                {                    
                    Rp.Load(ReportPath);
                    LoadParameters();
                    Rp.RegData("Customers", "Customers", DataSource);
                    Rp.RenderWithWpf();
                    ReportViewer.Report = Rp;
                }
                else
                    throw new Exception();
            }
            catch(Exception Ex)
            {
                MessageDialog Msg = new MessageDialog(Messages.ReportTitleDialog, Messages.ReportError, MessageDialogButtons.Ok, MessageDialogType.Error, HeaderColor);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) && e.Key == Key.P)
                PrintButton_Click(null, null);

            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }

        private void LoadParameters()
        {
            if (IsFormPrint)
            {
                StiText Pt = (StiText)Rp.GetComponentByName("ProjectTitle");
                Pt.Text = Commons.CompanyName;
                StiText Rt = (StiText)Rp.GetComponentByName("RpTitle");
                Rt.Text = Messages.ReportTitle + ReportTitle;
                StiText Pd = (StiText)Rp.GetComponentByName("PersianDate");
                Pd.Text = "تاریخ: " + Commons.GetCurrentPersianDate();
                StiText Thm = (StiText)Rp.GetComponentByName("TimeHM");
                Thm.Text = "ساعت: " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");
            }
            if (IsBillPrint)
            {
                StiText Wn = (StiText)Rp.GetComponentByName("WaterAndWastewaterAuthorityName");                
                Wn.Text = Commons.WaterAndWastewaterAuthorityName;
                StiText Wn2 = (StiText)Rp.GetComponentByName("WaterAndWastewaterAuthorityName2");
                Wn2.Text = Commons.WaterAndWastewaterAuthorityName;
                StiText Bm = (StiText)Rp.GetComponentByName("BillMessage");
                Bm.Text = Commons.BillMessage;
                StiText Ad = (StiText)Rp.GetComponentByName("Address");
                Ad.Text = Commons.Address;
                StiText Tl = (StiText)Rp.GetComponentByName("Tel");
                Tl.Text = Commons.Tel;
                StiText Cd = (StiText)Rp.GetComponentByName("CurrentDate");
                Cd.Text = Commons.GetCurrentPersianDate();
                StiText Pr = (StiText)Rp.GetComponentByName("Period");
                Pr.Text = Commons.GetShortPeriodName(Commons.CPeriod.DateFrom, Commons.CPeriod.DateTo);
                StiText Cy = (StiText)Rp.GetComponentByName("CurrentYear");
                Cy.Text = Commons.CurrentYear.ToString();
                StiText Dpd = (StiText)Rp.GetComponentByName("DeadLinePaymentDate");
                Dpd.Text = Commons.GetDeadLinePaymentDate();
            }
        }
        
        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Rp.Print(true);
        }

    }
}
