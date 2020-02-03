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
using WaterAndWastewaterAuthorithy.DomainClasses;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        AnimateControls Ac = new AnimateControls();
        System.Collections.Specialized.ListDictionary ControlsArray = new System.Collections.Specialized.ListDictionary();
        public Control FirstControl = null;
        string PrevHint = "";

        public Settings()
        {
            InitializeComponent();
            FirstControl = ComboBoxCurrentYear;
            ControlsArray.Add(ComboBoxCurrentYear, new ControlsTab { Prev = TextBoxMessage, Next = TextBoxVat, Type = ControlsType.ComboBox });
            ControlsArray.Add(TextBoxVat, new ControlsTab { Prev = ComboBoxCurrentYear, Next = TextBoxSubscription, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxSubscription, new ControlsTab { Prev = TextBoxVat, Next = TextBoxCompanyName, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxCompanyName, new ControlsTab { Prev = TextBoxSubscription, Next = TextBoxAuthorityName, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxAuthorityName, new ControlsTab { Prev = TextBoxCompanyName, Next = TextBoxTel, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxTel, new ControlsTab { Prev = TextBoxAuthorityName, Next = TextBoxAddress, Type = ControlsType.TextBoxNumber });
            ControlsArray.Add(TextBoxAddress, new ControlsTab { Prev = TextBoxTel, Next = TextBoxMessage, Type = ControlsType.TextBoxText });
            ControlsArray.Add(TextBoxMessage, new ControlsTab { Prev = TextBoxAddress, Next = null, Type = ControlsType.TextBoxText });
        }

        private void UserControlBillPeriods_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void UserControlBillPeriods_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBoxCurrentYear.Items.Clear();
            ComboBoxCurrentYear.Items.Add("لطفا سال مالی را انتخاب کنید.");

            List<SettingsTb> Tb = Commons.Db.Settings.ToList();
            List<YearsTb> Yb    = Commons.Db.Years.ToList();

            foreach (YearsTb value in Yb)
            {
                ComboBoxCurrentYear.Items.Add(value.Year);
            }
            ComboBoxCurrentYear.SelectedIndex = 0;                        
            if (Tb.Count > 0)
            {
                for (int i = 1; i < ComboBoxCurrentYear.Items.Count; i++)
                {
                    if (Tb[0].CurrentYear == (int)ComboBoxCurrentYear.Items[i])
                    {
                        ComboBoxCurrentYear.SelectedIndex = i;
                        break;
                    }
                }
                TextBoxVat.Text = Tb[0].Vat.ToString();
                TextBoxSubscription.Text = Tb[0].Subscription.ToString();
                TextBoxCompanyName.Text = Tb[0].CompanyName;
                TextBoxAuthorityName.Text = Tb[0].WaterAndWastewaterAuthorityName;
                TextBoxTel.Text = Tb[0].Tel;
                TextBoxAddress.Text = Tb[0].Address;
                TextBoxMessage.Text = Tb[0].BillMessage;                
            }
            else
            {
                TextBoxVat.Text = "";
                TextBoxSubscription.Text = "";
                TextBoxCompanyName.Text = "";
                TextBoxAuthorityName.Text = "";
                TextBoxTel.Text = "";
                TextBoxAddress.Text = "";
                TextBoxMessage.Text = "";
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {            
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F6)
                ButtonSave_Click(null, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.F)
                ButtonCreateYear_Click(null, null);
            if (GridNewAccountType.Visibility == System.Windows.Visibility.Visible && e.Key == Key.F11)
                ButtonCloseYear_Click(null, null);
            if (e.Key == Key.Escape)
                ButtonReturn_Click(null, null);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || (e.Key == Key.Tab && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (((ControlsTab)ControlsArray[sender]).Next != null)
                {
                    e.Handled = true;
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Next;
                    Ctrl.Focus();
                    if (((ControlsTab)ControlsArray[Ctrl]).Type == ControlsType.ComboBox)
                        ((ComboBox)Ctrl).IsDropDownOpen = true;
                }
                else
                {
                    if (e.Key == Key.Tab)
                    {
                        e.Handled = true;
                        FirstControl.Focus();
                        ((ComboBox)FirstControl).IsDropDownOpen = true;
                    }
                    else
                    {
                        e.Handled = true;
                        ButtonSave_Click(null, null);
                    }
                }
            }

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.Key == Key.Tab)
            {
                if (((ControlsTab)ControlsArray[sender]).Prev != null)
                {
                    e.Handled = true;
                    Control Ctrl = ((ControlsTab)ControlsArray[sender]).Prev;
                    Ctrl.Focus();
                    if (((ControlsTab)ControlsArray[Ctrl]).Type == ControlsType.ComboBox)
                        ((ComboBox)Ctrl).IsDropDownOpen = true;
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
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            BindingOperations.GetBindingExpression(combo, ComboBox.SelectedIndexProperty).UpdateSource();
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
        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PrevHint = ((ComboBox)sender).Tag.ToString();
            Hint.Text = ((ComboBox)sender).Tag.ToString();
            if (Hint.Text != "")
            {

                if (Hint.Text[0] == '.')
                    Hint.Text = Hint.Text.Substring(1) + ".";
            }
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

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ControlsValidate? Vd = Commons.ValidateData(FirstControl, ControlsArray);
            MessageDialog Msg = null;
            if (Vd != null)
            {
                Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Vd.Value.Message, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
                Vd.Value.Control.Focus();
            }
            else
            {
                if (Commons.CurrentYear != Convert.ToInt32(ComboBoxCurrentYear.SelectedValue))
                {
                    Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.CurrentYearConfilctWithThisYearSettings, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog() != true)
                        return;
                }

                DomainClasses.SettingsTb MySettings = Commons.Db.Settings.FirstOrDefault();
                Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.SaveMessageSettings, MessageDialogButtons.YesNo, MessageDialogType.Warning, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                if (Msg.ShowDialog() == true)
                {
                    if (MySettings == null)
                    {
                        try
                        {                            
                            Commons.Db.Settings.Add(new DomainClasses.SettingsTb
                            {
                                CurrentYear = Convert.ToInt32(ComboBoxCurrentYear.SelectedValue),
                                Address = TextBoxAddress.Text.Trim(),
                                BillMessage = TextBoxMessage.Text.Trim(),
                                CompanyName = TextBoxCompanyName.Text.Trim(),
                                Tel = TextBoxTel.Text.Trim(),
                                Vat = Convert.ToInt32(TextBoxVat.Text),
                                Subscription = Convert.ToInt32(TextBoxSubscription.Text),
                                WaterAndWastewaterAuthorityName = TextBoxAuthorityName.Text.Trim()
                            });
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("Settings");
                            
                            Commons.CurrentYear = Convert.ToInt32(ComboBoxCurrentYear.SelectedValue);
                            YearsTb Ly = Commons.Db.Years.Find(Convert.ToInt32(ComboBoxCurrentYear.SelectedValue));
                            if (Ly != null)
                                Commons.IsClosedCurrentYear = Ly.IsClosed;
                            else
                                Commons.IsClosedCurrentYear = false;
                            Commons.Address = TextBoxAddress.Text.Trim();
                            Commons.BillMessage = TextBoxMessage.Text.Trim();
                            Commons.CompanyName = TextBoxCompanyName.Text.Trim();
                            Commons.Tel = TextBoxTel.Text.Trim();
                            Commons.Vat = Convert.ToInt32(TextBoxVat.Text);
                            Commons.Subscription = Convert.ToInt32(TextBoxSubscription.Text);
                            Commons.WaterAndWastewaterAuthorityName = TextBoxAuthorityName.Text.Trim();

                            Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.SaveMessageSuccessSettings, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                    else
                    {
                        try
                        {
                            MySettings.CurrentYear = Convert.ToInt32(ComboBoxCurrentYear.SelectedValue);
                            MySettings.Address = TextBoxAddress.Text.Trim();
                            MySettings.BillMessage = TextBoxMessage.Text.Trim();
                            MySettings.CompanyName = TextBoxCompanyName.Text.Trim();
                            MySettings.Tel = TextBoxTel.Text.Trim();                            
                            MySettings.Vat = Convert.ToInt32(TextBoxVat.Text);
                            MySettings.Subscription = Convert.ToInt32(TextBoxSubscription.Text);
                            MySettings.WaterAndWastewaterAuthorityName = TextBoxAuthorityName.Text.Trim();
                            Commons.Db.SaveChanges();
                            Commons.SetFromEdited("Settings");

                            Commons.CurrentYear = Convert.ToInt32(ComboBoxCurrentYear.SelectedValue);
                            YearsTb Ly = Commons.Db.Years.Find(Convert.ToInt32(ComboBoxCurrentYear.SelectedValue));
                            if (Ly != null)
                                Commons.IsClosedCurrentYear = Ly.IsClosed;
                            else
                                Commons.IsClosedCurrentYear = true;
                            Commons.Address = TextBoxAddress.Text.Trim();
                            Commons.BillMessage = TextBoxMessage.Text.Trim();
                            Commons.CompanyName = TextBoxCompanyName.Text.Trim();
                            Commons.Tel = TextBoxTel.Text.Trim();
                            Commons.Vat = Convert.ToInt32(TextBoxVat.Text);
                            Commons.Subscription = Convert.ToInt32(TextBoxSubscription.Text);
                            Commons.WaterAndWastewaterAuthorityName = TextBoxAuthorityName.Text.Trim();

                            Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.SaveMessageSuccessSettings, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                        catch
                        {
                            Msg = new MessageDialog(Messages.SaveMessageTitleSettings, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                            Msg.Owner = Window.GetWindow(this);
                            Msg.ShowDialog();
                        }
                    }
                }
            }
        }
        private void ButtonCloseYear_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg = null;
            try
            {
                if (ComboBoxCurrentYear.SelectedIndex < 1)
                {
                    Msg = new MessageDialog(Messages.CloseYearTitleSettings, Messages.YearNotSelected, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    ComboBoxCurrentYear.Focus();
                    Msg.ShowDialog();
                    return;
                }
                var Ly = Commons.Db.Years.Select(x => x).ToList();
                bool IsAllowToClose = false;
                YearsTb SelectedYear = null;
                foreach (YearsTb value in Ly)
                {
                    if (value.Year == Commons.CurrentYear && value.IsClosed == false)
                    {
                        IsAllowToClose = true;
                        SelectedYear = value;
                        break;
                    }
                }
                if (IsAllowToClose == false)
                {
                    Msg = new MessageDialog(Messages.CloseYearTitleSettings, Messages.CloseYearImpossible, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
                else
                {
                    Msg = new MessageDialog(Messages.CloseYearTitleSettings, Messages.IsCloseYear, MessageDialogButtons.YesNo, MessageDialogType.Information, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    if (Msg.ShowDialog()==true)
                    {
                        SelectedYear.IsClosed = true;
                        Commons.IsClosedCurrentYear = true;
                        Commons.Db.SaveChanges();
                        Commons.SetFromEdited("Settings");
                        Msg = new MessageDialog(Messages.CloseYearTitleSettings, Messages.CloseYearMessageSuccess, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                        Msg.Owner = Window.GetWindow(this);
                        Msg.ShowDialog();
                    }
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.CloseYearTitleSettings, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }

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

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Ac.ShowWindow("Settings", "MainMenu");
        }

        private void ButtonCreateYear_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Msg = null;
            try
            {
                List<YearsTb>  Ly= Commons.Db.Years.Select(x => x).OrderBy(x=>x.Year).ToList();
                bool IsCreate = true;
                int NewYear = 0;
                foreach(YearsTb value in Ly)
                {
                    NewYear = value.Year;
                    if (value.IsClosed == false)
                    {
                        IsCreate = false;
                        break;
                    }
                }
                if (IsCreate == false)
                {                    
                    Commons.Db.SaveChanges();
                    Msg = new MessageDialog(Messages.CreateYearTitleSettings, Messages.CreateYearImpossible, MessageDialogButtons.Ok, MessageDialogType.Warning, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
                else
                {
                    if (NewYear == 0)
                        NewYear = Commons.GetCurrentYear();
                    else
                        NewYear++;
                    Commons.Db.Years.Add(new YearsTb{
                        IsClosed =false,
                        Year = NewYear
                    });                    
                    Commons.Db.SaveChanges();
                    ComboBoxCurrentYear.Items.Add(NewYear);
                    Msg = new MessageDialog(Messages.CreateYearTitleSettings, Messages.CreateYearMessageSuccessSettings, MessageDialogButtons.Ok, MessageDialogType.Information, GridHeader.Background);
                    Msg.Owner = Window.GetWindow(this);
                    Msg.ShowDialog();
                }
            }
            catch
            {
                Msg = new MessageDialog(Messages.CreateYearTitleSettings, Messages.ErrorSendingDataToDatabase, MessageDialogButtons.Ok, MessageDialogType.Error, GridHeader.Background);
                Msg.Owner = Window.GetWindow(this);
                Msg.ShowDialog();
            }
        }
    }
}
