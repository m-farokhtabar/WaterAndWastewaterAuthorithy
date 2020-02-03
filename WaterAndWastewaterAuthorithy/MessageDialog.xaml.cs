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
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog()
        {
            InitializeComponent();
        }
        public MessageDialog(string Title, string Message, MessageDialogButtons Buttons,MessageDialogType Type, Brush BackColor)
        {
            InitializeComponent();
            switch (Buttons)
            {
                case MessageDialogButtons.Ok:
                                    ButtonNo.Visibility = System.Windows.Visibility.Hidden;
                                    ButtonYes.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case MessageDialogButtons.YesNo:
                                    ButtonOk.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }

            switch (Type)
            {
                case MessageDialogType.Information :
                    ImageIcon.Source = (ImageSource)ImageIcon.Resources["ImageHeaderDialogInfo"];
                    break;
                case MessageDialogType.Warning:
                    ImageIcon.Source = (ImageSource)ImageIcon.Resources["ImageHeaderDialogWarning"];
                    break;
                case MessageDialogType.Error:
                    ImageIcon.Source = (ImageSource)ImageIcon.Resources["ImageHeaderDialogError"];
                    break;
            }

            TextBlockMessage.Text = Message;
            TextBlockTitle.Text   = Title;
            GridTitle.Background = BackColor;            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main MainWindowApp = null;
            foreach (Window Value in App.Current.Windows)
            {
                if (Value.Title == "نرم افزار آب و فاضلاب طرقرود")
                    MainWindowApp = (Main)Value;
            }
            if (ButtonYes.Visibility == System.Windows.Visibility.Visible)
                ButtonYes.Focus();
            else
                ButtonOk.Focus();
        }

        private void ButtonYes_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.Right || e.Key == Key.Left)
                ButtonNo.Focus();            
        }

        private void ButtonNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.Left)
                ButtonYes.Focus();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
