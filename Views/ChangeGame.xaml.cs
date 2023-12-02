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
    /// Interaction logic for ChangeGame.xaml
    /// </summary>
    public partial class ChangeGame : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }
        bool changed = false;
        Game game;
        Log log;
        public ChangeGame(Game selected)
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            descriptionbox.Text = selected.Description;
            game = selected;
            Picture.Source = DataHelper.GetBitMap(selected.Picture);
            log = new Log()
            {
                Contents = String.Format("{0} изменил игру с названием: {1}:", DataHelper.username, game.Name),
                Administrator = context.Administrators.Single(
                    a => a.Username == DataHelper.username)
            };
            KeyDown += Paste;

        }

        public void SaveChanges(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            
           
            Game notchanged = context.Games.Single(g => g.Name == game.Name);
            if (descriptionbox.Text == game.Description && !changed)
            {
                MessageBox.Show("Вы не внесли никаких изменений");
                return;
            }
            if (descriptionbox.Text != game.Description)
            {
                game.Description = descriptionbox.Text;
                log.Contents += (changed) ? "описание" : ", описание";
            }
            context.Entry(notchanged).CurrentValues.SetValues(game);
            log.Contents += DateTime.Now;
            context.Logs.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBox.Show("Игра успешно изменена");
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
                        game.Picture = DataHelper.GetBytes(Clipboard.GetImage());
                        changed = true;
                        log.Contents += "фото";
                    }
                    return;
                }
            }
        }
    }
}
