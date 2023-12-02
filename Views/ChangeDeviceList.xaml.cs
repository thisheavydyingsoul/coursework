using CourseWorkAdmin.Models;
using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
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
    /// Interaction logic for ChangeDeviceList.xaml
    /// </summary>
    
    public partial class ChangeDeviceList : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }

        ComputerClubDBContext context = new ComputerClubDBContext();
        Game game;
        Dictionary<Device, bool> notchanged = new Dictionary<Device, bool>();
        Dictionary<Device, bool> changed = new Dictionary<Device, bool>();
        Device[] devices;
        public ChangeDeviceList(Game selected)
        {
            game = selected;
            InitializeComponent();
            devices = context.Devices.ToArray();
            if (game.DevicesGames.Count == 0)
            {
                foreach (Device device in devices)
                {
                    changed.Add(device, false);
                    notchanged.Add(device, false);
                }
            }
            else
            {
                foreach (DeviceGame dg in game.DevicesGames)
                {
                    changed.Add(dg.Device, true);
                    notchanged.Add(dg.Device, true);
                }
                foreach (Device device in devices)
                {
                    if (changed.GetValueOrDefault(device)) continue;
                    changed.Add(device, false);
                    notchanged.Add(device, false);
                }
            }
            GamesList.ItemsSource = changed;
        }
        void OnChecked(object sender, RoutedEventArgs e)
        {

            Administrator currentAdmin = context.Administrators.Where(a => a.Username == DataHelper.username).FirstOrDefault();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            CheckBox checkBox = sender as CheckBox;
            KeyValuePair<Device, bool> kvp = (KeyValuePair<Device, bool>)checkBox.DataContext;
            int deviceId = kvp.Key.Id;
            if (context.DevicesGames.Where(dg => dg.DeviceId == deviceId  && dg.GameName == game.Name).FirstOrDefault() != null) return;
            context.Add(new DeviceGame()
            {
                DeviceId = deviceId,
                GameName = game.Name
            });
            context.Add(new Log()
            {
                Administrator = currentAdmin,
                Contents = String.Format("{0} установил {1} на {2} с ID: {3} {4}", currentAdmin.Username, game.Name, kvp.Key.Name, deviceId, DateTime.Now)
            });
            DataHelper.TrySaving(context);
            changedhandler();
        }

        void OnUnchecked(object sender, RoutedEventArgs e)
        {
            Administrator currentAdmin = context.Administrators.Where(a => a.Username == DataHelper.username).FirstOrDefault();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            CheckBox checkBox = sender as CheckBox;
            KeyValuePair<Device, bool> kvp = (KeyValuePair<Device, bool>)checkBox.DataContext;
            int deviceId = kvp.Key.Id;
            DeviceGame dg = context.DevicesGames.Where(dg => dg.DeviceId == deviceId && dg.GameName == game.Name).FirstOrDefault();
            if (dg == null) return;
            context.DevicesGames.Remove(dg);
            context.Add(new Log()
            {
                Administrator = currentAdmin,
                Contents = String.Format("{0} удалил {1} с {2} с ID: {3} {4}", currentAdmin.Username, game, kvp.Key.Name, deviceId, DateTime.Now)
            });
            DataHelper.TrySaving(context);
            changedhandler();
        }
    }
}
