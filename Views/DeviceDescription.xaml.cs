﻿using CourseWorkAdmins.Models;
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
    /// Interaction logic for DeviceDescription.xaml
    /// </summary>
    public partial class DeviceDescription : Window
    {
        public DeviceDescription(Device device)
        {
            InitializeComponent();
            Description.Text = device.Description;
            GameList.ItemsSource = device.DevicesGames;
            Picture.Source = DataHelper.GetBitMap(device.Picture);
        }
    }
}
