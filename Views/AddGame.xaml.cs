using CourseWorkAdmin.Models;
using CourseWorkAdmins.Data;
using CourseWorkAdmins.Models;
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
using System.Drawing;
using System.IO;

namespace CourseWorkAdmin.Views
{
    
    public partial class AddGame : Window
    {
        Game game;
        public delegate void Added();
        public Added addedhandler { get; set; }
        public AddGame()
        {
            InitializeComponent();
            KeyDown += Paste;
            game = new Game();
        }


        public void AddNewGame(object sender, RoutedEventArgs e)
        {
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            using ComputerClubDBContext context = new ComputerClubDBContext();
            Administrator currentAdmin = (Administrator)context.Administrators.Where(a => a.Username == DataHelper.username).FirstOrDefault();
            string name = namebox.Text;
            string description = descriptionbox.Text;
            //Checking if all neccessary fields are filled
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(description) || game.Picture == null) 
            {
                MessageBox.Show("Введите все необходимые значения!");
                return;
            }
            game.Name = name;
            game.Description = description;
            context.Add(game);
            Log log = new Log()
            {
                Contents = String.Format("{0}: добавил новую игру: {1} {2}", DataHelper.username, game.Name, DateTime.Now),
                Administrator = context.Administrators.Where(
                    a => a.Username == DataHelper.username).FirstOrDefault()
            };
            context.Add(log);

            //Checking if something goes wrong
            if (DataHelper.TrySaving(context))
            {
                MessageBoxResult result2 = MessageBox.Show("Игра успешно добавлена", "Успех", MessageBoxButton.OK);
                if (result2 == MessageBoxResult.OK)
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
                        game.Picture = DataHelper.GetBytes(Clipboard.GetImage());
                        
                    }
                    return;
                }
            }
        }

    }
}
