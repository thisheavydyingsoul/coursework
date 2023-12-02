using CourseWorkAdmin.Models;
using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections;
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
        private static string[] CountStatuses = { "Завершена", "Завершена заранее", "Отменена без возврата" };
        Hashtable officeSums;
        Hashtable deviceSums;
        Hashtable clientSums;
        public HRWindow()
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            administrators = context.Administrators.Include(a => a.Logs).ToArray();
            AdminsList.ItemsSource = administrators;
            offices = context.Offices.Include(o => o.Administrators).ToArray();
            OfficeList.ItemsSource = offices;
            officeSums = new Hashtable();
            deviceSums = new Hashtable();
            clientSums = new Hashtable();
            Device[] ds = context.Devices.GroupBy(d => d.Name).Select(d => d.FirstOrDefault()).ToArray();
            Client[] clients = context.Clients.ToArray();
            foreach (Device d in ds)
            {
                deviceSums.Add(d.Name, 0);
            }
            foreach (Office o in offices)
            {
                officeSums.Add(o.Adress, 0);
            }
            foreach (Client c in clients)
            {
                clientSums.Add(c.Username, 0);
            }
            int sum = 0;
            for (int i = 0; i < offices.Length; i++)
            {
                Rent[] rents = context.Rents.Where(r => r.Device.Office.Id == offices[i].Id && CountStatuses.Contains(r.Status)).Include(r => r.Device).ToArray();
                int officeSum = 0;
                foreach (Rent r in rents)
                {
                    if (!(r.StartDT.Month == DateTime.Now.Month && r.StartDT.Year == DateTime.Now.Year))
                        continue;
                    double start = DataHelper.DataToDouble(r.StartDT);
                    double end = DataHelper.DataToDouble(r.EndDT);
                    double full = end - start > 0 ? end - start : end - start + 12.0;
                    double day = 0; double night = 0;
                    if ((start < 8.0 && full < 8.0 - start) || (start > 23.0 && full < 9.0 - start))
                        night = full;
                    else if (start < 23.0 && full < 23.0 - start)
                        day = full;
                    else if (start > 8.0 && full > 23.0 - start)
                    {
                        day = 23.0 - start;
                        night = full - day;
                    }
                    else
                    {
                        day = start - 8.0;
                        night = full - day;
                    }
                    double coef;
                    if (context.Promos.Where(p => p.StartDate < r.StartDT && p.EndDate > r.StartDT).Count() > 0)
                        coef = context.Promos.Where(p => p.StartDate < r.StartDT && p.EndDate > r.StartDT).Max(p => p.Coefficient) / 100.0;
                    else
                        coef = 1;
                    officeSum += (int)((day * r.Device.DayRate + night * r.Device.NightRate) * coef);
                    deviceSums[r.Device.Name] = (int)deviceSums[r.Device.Name] + (int)((day * r.Device.DayRate + night * r.Device.NightRate) * coef);
                    clientSums[r.Client.Username] = (int)clientSums[r.Client.Username] + (int)((day * r.Device.DayRate + night * r.Device.NightRate) * coef);

                }
                officeSums[offices[i].Adress] = (int)officeSums[offices[i].Adress] + officeSum;

                sum += officeSum;
            }
            RevenueList.ItemsSource = officeSums;
            DeviceRevenueList.ItemsSource = deviceSums;
            ClientRevenueList.ItemsSource = clientSums;
            SumBox.Text = string.Format("{0} руб.", sum);
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
            Administrator[] admins = selected.Administrators.ToArray();
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
