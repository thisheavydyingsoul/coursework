using CourseWorkAdmin.Models;
using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for OfficeWindow.xaml
    /// </summary>
    public partial class OfficeWindow : Window
    {
        Device[] devices;
        Game[] games;
        Promo[] promos;
        public OfficeWindow()
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            devices = context.Devices.Include(d => d.DevicesGames).ToArray();
            DevicesList.ItemsSource = devices;
            games = context.Games.Include(g => g.DevicesGames).ThenInclude(dg => dg.Device).ToArray();
            GamesList.ItemsSource = games;
            promos = context.Promos.ToArray();
            PromosList.ItemsSource = promos;
        }

        private void ChangeGameList(object sender, RoutedEventArgs e)
        {
            ChangeGameList changeGameList = new ChangeGameList((Device)DevicesList.SelectedItem);
            changeGameList.changedhandler = RefreshDevices;
            changeGameList.changedhandler += RefreshGames;
            changeGameList.Show();
        }
        private void ChangeDeviceList(object sender, RoutedEventArgs e)
        {
            ChangeDeviceList changeDeviceList = new ChangeDeviceList((Game)GamesList.SelectedItem);
            changeDeviceList.changedhandler = RefreshDevices;
            changeDeviceList.changedhandler += RefreshGames;
            changeDeviceList.Show();
        }

        private void SearchDevicesBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Device[] filtered = devices.Where(
                d => d.Name.Contains(searchDevicesBox.Text)).ToArray();
            DevicesList.ItemsSource = filtered;
        }

        private void SearchGamesBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Game[] filtered = games.Where(
                g => g.Name.Contains(searchGamesBox.Text)).ToArray();
            GamesList.ItemsSource = filtered;
        }
        private void SearchPromosBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Promo[] filtered = promos.Where(
                p => p.Description.Contains(searchEventsBox.Text)).ToArray();
            PromosList.ItemsSource = filtered;
        }

        private void ShowDeviceDescription(object sender, RoutedEventArgs e)
        {
            Device selected = (Device)DevicesList.SelectedItem;
            DeviceDescription dd = new DeviceDescription(selected);
            dd.Show();
        }
        private void ShowRents(object sender, RoutedEventArgs e)
        {
            Device selected = (Device)DevicesList.SelectedItem;
            RentList rl = new RentList(selected);
            rl.Show();
        }
        private void ShowGameDescription(object sender, RoutedEventArgs e)
        {
            Game selected = (Game)GamesList.SelectedItem;
            GameDescription gd = new GameDescription(selected);
            gd.Show();
        }

        private void ShowPromoDescription(object sender, RoutedEventArgs e)
        {
            Promo selected = (Promo)PromosList.SelectedItem;
            PromoDescription pd = new PromoDescription(selected);
            pd.Show();
        }
        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Device selected = (Device)DevicesList.SelectedItem;
            ChangeDevice cd = new ChangeDevice(selected);
            cd.changedhandler = RefreshDevices;
            cd.Show();
        }
        private void ChangeGame(object sender, RoutedEventArgs e)
        {
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
             Game selected = (Game)GamesList.SelectedItem;
            ChangeGame cg = new ChangeGame(selected);
            cg.changedhandler = RefreshGames;
            cg.Show();

        }
        private void ChangePromo(object sender, RoutedEventArgs e)
        {
            if (!(DataHelper.CheckActivity()))
            {
                Close();
                return;
            }
            Promo selected = (Promo)PromosList.SelectedItem;
            ChangePromo cp = new ChangePromo(selected);
            cp.changedhandler = RefreshPromos;
            cp.Show();
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            AddDevice addDevice = new AddDevice();
            addDevice.addedhandler = RefreshDevices;
            addDevice.Show();
        }

        private void AddGame(object sender, RoutedEventArgs e)
        {
            AddGame addGame = new AddGame();
            addGame.addedhandler = RefreshGames;

            addGame.Show();
        }
        private void AddPromo(object sender, RoutedEventArgs e)
        {
            AddPromo ap = new AddPromo();
            ap.addedhandler = RefreshPromos;
            ap.Show();
        }

        private void RefreshDevices()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            devices = context.Devices.Include(d => d.DevicesGames).ToArray();
            DevicesList.ItemsSource = devices;
        }
        private void RefreshGames()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            games = context.Games.Include(g => g.DevicesGames).ToArray();
            GamesList.ItemsSource = games;
        }
        private void RefreshPromos()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            promos = context.Promos.ToArray();
            PromosList.ItemsSource = promos;
        }

    }
}
