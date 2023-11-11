using CourseWorkAdmin.Migrations;
using CourseWorkAdmin.Models;
using CourseWorkAdmins.Data;
using CourseWorkAdmins.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
    /// Interaction logic for ChangeGameList.xaml
    /// </summary>
    public partial class ChangeGameList : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }

        ComputerClubDBContext context = new ComputerClubDBContext();
        Game[] games;
        Dictionary<string, bool> notchanged = new Dictionary<string, bool>();
        Dictionary<string, bool> changed = new Dictionary<string, bool>();
        Device device;
        public ChangeGameList(Device selected)
        {
            device = selected;
            InitializeComponent();
            games = context.Games.ToArray();
            if (device.DevicesGames.Count == 0)
            {
                foreach (Game game in games)
                {
                    changed.Add(game.Name, false);
                    notchanged.Add(game.Name, false);
                }
            }
            else
            {
                foreach (DeviceGame dg in device.DevicesGames)
                {
                    changed.Add(dg.GameName, true);
                    notchanged.Add(dg.GameName, true);
                }
                foreach (Game game in games)
                {
                    if (changed.GetValueOrDefault(game.Name)) continue;
                    changed.Add(game.Name, false);
                    notchanged.Add(game.Name, false);
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
            KeyValuePair<string,bool> kvp = (KeyValuePair<string,bool>)checkBox.DataContext;
            string game = kvp.Key;
            if (context.DevicesGames.Where(dg => dg.DeviceId == device.Id && dg.GameName == game).FirstOrDefault() != null) return;
            context.Add(new DeviceGame()
            {
                DeviceId = device.Id,
                GameName = game
            });
            context.Add(new Log()
            {
                Administrator = currentAdmin,
                Contents = String.Format("{0} установил {1} на {2} с ID: {3} {4}", currentAdmin.Username, game, device.Name, device.Id, DateTime.Now)
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
            KeyValuePair<string, bool> kvp = (KeyValuePair<string, bool>)checkBox.DataContext;
            string game = kvp.Key;
            DeviceGame dg = context.DevicesGames.Where(dg => dg.Device == device && dg.GameName == game).FirstOrDefault();
            if ( dg == null) return;
            context.DevicesGames.Remove(dg);
            context.Add(new Log()
            {
                Administrator = currentAdmin,
                Contents = String.Format("{0} удалил {1} с {2} с ID: {3} {4}", currentAdmin.Username, game, device.Name, device.Id, DateTime.Now)
            });
            DataHelper.TrySaving(context);
            changedhandler();
        }
    }
}
