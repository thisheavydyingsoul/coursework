using CourseWorkAdmins.Data;
using CourseWorkAdmins.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class HRWindow : Window
    {
        Administrator[] administrators;
        Office[] offices;
        public HRWindow()
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            administrators = context.Administrators.Include(a => a.Logs).ToArray();
            AdminsList.ItemsSource = administrators;
            offices = context.Offices.Include(o => o.Administrators).ToArray();
            OfficeList.ItemsSource = offices;

        }

        private void SearchAdminBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Administrator[] filtered = administrators.Where(
                a => a.Username.Contains(searchAdminBox.Text) || a.Fullname.Contains(searchAdminBox.Text)).ToArray();
            AdminsList.ItemsSource = filtered;
        }
        private void SearchOfficeBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Office[] filtered = offices.Where(
                o => o.Id.ToString().Contains(searchOfficeBox.Text) || o.Adress.Contains(searchOfficeBox.Text)).ToArray();
            OfficeList.ItemsSource = filtered;
        }

        private void ShowLogs(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Administrator selected = (Administrator)AdminsList.SelectedItem;
            if (selected.Logs == null)
            {
                MessageBox.Show("Данный администратор пока не вносил никаких изменений");
                return;
            }
            Log[] logs = selected.Logs.ToArray();
            LogsView logsView = new LogsView(logs);
            logsView.Show();
        }

        private void ShowAdmins(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Office selected = (Office)OfficeList.SelectedItem;
            if (selected.Administrators == null)
            {
                MessageBox.Show("В данном офисе пока никто не работает");
                return;
            }
            Administrator[] admins= selected.Administrators.ToArray();
            AdminsView adminsView = new AdminsView(admins);
            adminsView.Show();
        }

        private void ChangeAdmin(object sender, RoutedEventArgs e)
        {
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Administrator selected = (Administrator)AdminsList.SelectedItem;
            ChangeAdmin changeAdmin = new ChangeAdmin(selected);
            changeAdmin.changedhandler = RefreshAdmins;

            changeAdmin.Show();
        }
        private void ChangeOffice(object sender, RoutedEventArgs e)
        {
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Office selected = (Office)OfficeList.SelectedItem;
            ChangeOffice changeOffice = new ChangeOffice(selected);
            changeOffice.changedhandler = RefreshOffices;
            changeOffice.Show();
        }

        private void AddAdmin(object sender, RoutedEventArgs e)
        {
            AddAdmin addAdmin = new AddAdmin();
            addAdmin.addedhandler = RefreshAdmins;
            addAdmin.Show();
            
        }

        private void AddOffice(object sender, RoutedEventArgs e)
        {
            AddOffice addOffice = new AddOffice();
            addOffice.addedhandler = RefreshOffices;
            addOffice.Show();
        }

        private void RefreshAdmins()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            administrators = context.Administrators.Include(a => a.Logs).ToArray();
            AdminsList.ItemsSource = administrators;
        }
        private void RefreshOffices()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            offices = context.Offices.Include(o => o.Administrators).ToArray();
            OfficeList.ItemsSource = offices;
        }

    }
}
