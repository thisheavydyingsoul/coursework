using CourseWorkAdmins.Data;
using CourseWorkAdmins.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for ChangeDevice.xaml
    /// </summary>
    public partial class ChangeDevice : Window
    {

        Device device;
        public delegate void Changed();
        public Changed changedhandler { get; set; }
        bool changed = false;
        Log log;
        public ChangeDevice(Device selected)
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            device = selected;
            namebox.Text = device.Name;
            typebox.Text = device.Type;
            conditionbox.Text = device.Condition;
            dayratebox.Text = device.DayRate.ToString();
            nightratebox.Text = device.NightRate.ToString();
            descriptionbox.Text = device.Description;
            Picture.Source = DataHelper.GetBitMap(device.Picture);
            log = new Log()
            {
                Contents = String.Format("{0} изменил данные для {1} с ID: {2}: ", DataHelper.username, device.Name, device.Id),
                Administrator = context.Administrators.Single(a => a.Username == DataHelper.username)
            };
            KeyDown += Paste;

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            Device notChanged = context.Devices.Single(d => d.Id == device.Id);
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            
            if (
                namebox.Text == device.Name &&
                typebox.Text == device.Type &&
                conditionbox.Text == device.Condition &&
                dayratebox.Text == device.DayRate.ToString() &&
                nightratebox.Text == device.NightRate.ToString() &&
                descriptionbox.Text == device.Description &&
                !changed)
            {
                MessageBox.Show("Вы не внесли никаких изменений");
                return;
            }
 
            if (namebox.Text != device.Name)
            {
                log.Contents += (changed) ? ", название":"название";
                device.Name = namebox.Text;
                changed = true;
            }
            if(typebox.Text != device.Type)
            {
                device.Type = typebox.Text;
                log.Contents += (changed) ? "тип" : ", тип";
                changed = true;
            }
            if (conditionbox.Text != device.Condition)
            {
                device.Condition = typebox.Text;
                log.Contents += (changed) ? "состояние" : ", состояние";
                changed = true;
            }
            if (dayratebox.Text != device.DayRate.ToString())
            {
                device.DayRate = Int32.Parse(dayratebox.Text);
                log.Contents += (changed) ? "дневной тариф" : ", дневной тариф";
                changed = true;
            }
            if (nightratebox.Text != device.NightRate.ToString())
            {
                device.NightRate = Int32.Parse(nightratebox.Text);
                log.Contents += (changed) ? "ночной тариф" : ", ночной тариф";
                changed = true;
            }
            if (descriptionbox.Text != device.Description)
            {
                device.Description = descriptionbox.Text;
                log.Contents += (changed) ? "состояние" : ", состояние";
                changed = true;
            }
            context.Entry(notChanged).CurrentValues.SetValues(device);
            log.Contents += DateTime.Now;
            context.Logs.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBox.Show("Устройство успешно изменено");
                Close();
            }
            changedhandler();
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
                        changed = true;
                        log.Contents += "фото";
                    }
                    return;
                }
            }
        }
    }
}
