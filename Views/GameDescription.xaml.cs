using CourseWorkAdmin.Models;
using CourseWorkAdmins.Data;
using CourseWorkAdmins.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for GameDescription.xaml
    /// </summary>
    public partial class GameDescription : Window
    {
        public GameDescription(Game game)
        {
            ComputerClubDBContext context = new ComputerClubDBContext();
            InitializeComponent();
            Description.Text = game.Description;
            GameList.ItemsSource = game.DevicesGames;
            Picture.Source = DataHelper.GetBitMap(game.Picture);
        }
    }
}
