using CourseWorkAdmin.Models;
using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddDevice.xaml
    /// </summary>
    public partial class AddDevice : Window
    {
        public delegate void Added();
        public Added addedhandler { get; set; }
        Device device;
        public AddDevice()
        {
            InitializeComponent();
            device = new Device();
            KeyDown += Paste;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void AddNewDevice(object sender, RoutedEventArgs e)
        {
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            using ComputerClubDBContext context = new ComputerClubDBContext();
            Administrator currentAdmin = context.Administrators.Where(
                    a => a.Username == DataHelper.username).Include(a => a.Office).FirstOrDefault();
            string name = namebox.Text;
            string type = typebox.Text;
            bool condition = (bool)conditionbox.IsChecked;
            string description = descriptionbox.Text;
            //Checking if all neccessary fields are filled
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(type) || String.IsNullOrEmpty(dayratebox.Text) || String.IsNullOrEmpty(nightratebox.Text) || String.IsNullOrEmpty(description) || device.Picture == null)
            {
                MessageBox.Show("Введите все необходимые значения!");
                return;
            }
            int dayrate = Int32.Parse(dayratebox.Text);
            int nightrate = Int32.Parse(nightratebox.Text);
            device.Name = name;
            device.Type = type;
            device.Condition = condition;
            device.DayRate = dayrate;
            device.NightRate = nightrate;
            device.Description = description;
            device.Office = currentAdmin.Office;
            //Checking if admin has rights

            context.Add(device);
            Log log = new Log()
            {
                Contents = String.Format("{0}: добавил новое устройство: {1}, ID: {2} {3}", DataHelper.username, device.Name, device.Id, DateTime.Now),
                Administrator = currentAdmin
            };
            context.Add(log);

            //Checking if something goes wrong
            if (DataHelper.TrySaving(context))
            {
                MessageBox.Show("Устройство успешно добавлено");
                Close();
            }
            addedhandler();
        }
        private void Paste(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.V)
                {
                    if (Clipboard.ContainsImage())
                    {
                        Picture.Source = Clipboard.GetImage();
                        device.Picture = DataHelper.GetBytes(Clipboard.GetImage());
                    }
                    return;
                }
            }
        }
    }
}
