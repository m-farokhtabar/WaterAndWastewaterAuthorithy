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
using System.Windows.Media.Animation;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        AnimateControls Ac = new AnimateControls();
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg;
            if (Commons.EditSettings[0] == true)
            {                
                Commons.SetYear();
                Commons.EditSettings[0] = false;
            }
            if (Commons.CurrentYear <= 0 && ((Button)sender).Tag.ToString() != "Settings" && ((Button)sender).Tag.ToString() != "About")
            {
                Msg = new MessageDialog(Messages.SplashLoading, Messages.MainMenuYearNotSelected, MessageDialogButtons.Ok, MessageDialogType.Warning,new SolidColorBrush(Color.FromRgb(0,25,65)));
                Msg.ShowDialog();
            }
            else
                Ac.ShowWindow("MainMenu",((Button)sender).Tag.ToString());
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(100);
        }
    }
}
