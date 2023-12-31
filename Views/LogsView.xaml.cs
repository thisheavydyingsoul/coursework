﻿using CourseWorkAdmin.Models;
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

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : Window
    {
        public LogsView(Log[] logs)
        {
            logs = logs.Reverse().ToArray();
            InitializeComponent();
            LogsList.ItemsSource = logs;
        }
    }
}
