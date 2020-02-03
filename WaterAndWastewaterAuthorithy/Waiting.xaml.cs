﻿using System;
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

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for WaitingDialog.xaml
    /// </summary>
    public partial class WaitingDialog : Window
    {        
        public WaitingDialog()
        {
            InitializeComponent();
        }
        public WaitingDialog(string Title, string Message, Brush BackColor)
        {
            InitializeComponent();
            TextBlockMessage.Text = Message;
            TextBlockTitle.Text   = Title;
            GridTitle.Background = BackColor;            
        }
    }
}
