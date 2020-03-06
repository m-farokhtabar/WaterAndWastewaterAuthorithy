using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WaterAndWastewaterAuthorithy.Presentation;
using WaterAndWastewaterAuthorithy.Services;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for SmsSentDetails.xaml
    /// ارسال پیامک به بدهکاران
    /// </summary>
    public partial class SmsSentDetails : Window, INotifyPropertyChanged
    {
        public List<SmsInfo> SmsList { get; set; }
        public bool SmsSendingIsRunning { get; set; }
        public bool AllowToBreakSmsSending { get; set; }
        public Task ResultSmsSender { get; private set; }
        private string _ButtonTitle;
        /// <summary>
        /// عنوان دکمه
        /// </summary>
        public string ButtonTitle
        {
            get
            {
                return _ButtonTitle;
            }
            set
            {
                _ButtonTitle = value;
                OnPropertyRaised("ButtonTitle");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public SmsSentDetails(List<SmsInfo> SmsList)
        { 
            InitializeComponent();
            SmsSendingIsRunning = false;
            AllowToBreakSmsSending = false;
            this.SmsList = SmsList;
            ImageIcon.Source = (ImageSource)ImageIcon.Resources["ImageHeaderDialogInfo"];
            TextBlockTitle.Text = Messages.TitleSendSmsToDebtors;
            GridTitle.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 1, 49, 125));
            DataGridView.ItemsSource = SmsList.ToList();
            ButtonTitle = "  " + "در حال آماده سازی جهت ارسال..." + "  ";
            ResultSmsSender = SmsSender();
        }

        public delegate int sum(int a, int b);


        private async Task SmsSender()
        {
            await Task.Run(async () =>
            {
                ButtonTitle = "  " + "انصراف از ادامه ارسال" + "  ";
                SmsSendingIsRunning = true;
                foreach (var SmsItem in SmsList.ToList())
                {
                    SmsItem.SendSuccessfully = await Sms.SendSms(SmsItem.Cellphone, SmsItem.Message);
                    if (SmsItem.SendSuccessfully)
                        SmsItem.Status = "با موفقیت ارسال شد";
                    else
                        SmsItem.Status = "ارسال نشد";
                    if (AllowToBreakSmsSending)
                    {
                        AllowToBreakSmsSending = false;
                        break;
                    }
                }
                SmsSendingIsRunning = false;
                ButtonOk.Dispatcher.Invoke(() => { ButtonOk.IsEnabled = true; });
                ButtonTitle = "  " + "بازگشت" + "  ";
                if (SmsList.Any(x=>!x.SendSuccessfully))
                    ButtonSave.Dispatcher.Invoke(() => { ButtonSave.Visibility = Visibility.Visible; });
            });
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (SmsSendingIsRunning)
            {
                ButtonOk.IsEnabled = false;
                AllowToBreakSmsSending = true;
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.FileName = "FailureSmsToDebtor_" + Commons.GetCurrentPersianDate().Replace("/","_");
                Dialog.DefaultExt = ".txt"; 
                Dialog.Filter = "Text documents (.txt)|*.txt";
                bool? result = Dialog.ShowDialog();
                if (result == true)
                {
                    string Filename = Dialog.FileName;
                    TextWriter tw = new StreamWriter(Filename);
                    foreach (var item in SmsList)
                    {
                        if (!item.SendSuccessfully)
                            tw.WriteLine(item);
                    }
                    tw.Close();
                }
            }
            catch
            {
                MessageDialog Msg = new MessageDialog(Messages.TitleSendSmsToDebtors, "امکان ثبت اطلاعات در فایل وجود ندارد. لطفا دسترسی خود را در سیستم بررسی نمایید", MessageDialogButtons.Ok, MessageDialogType.Error, GridTitle.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
    }
    public class SmsInfo : INotifyPropertyChanged
    {
        private string _SubId;
        public string SubId
        {
            get
            {
                return _SubId;
            }
            set
            {
                _SubId = value;
                OnPropertyRaised("SubId");
            }
        }
        private string _Cellphone;
        public string Cellphone
        {
            get
            {
                return _Cellphone;
            }
            set
            {
                _Cellphone = value;
                OnPropertyRaised("Cellphone");
            }
        }
        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyRaised("Message");
            }
        }
        private string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyRaised("Status");
            }
        }
        public bool SendSuccessfully { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public override string ToString()
        {
            return _SubId + ": " + _Cellphone;
        }
    }
}
