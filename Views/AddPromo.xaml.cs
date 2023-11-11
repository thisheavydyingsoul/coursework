using CourseWorkAdmin.Models;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddPromo.xaml
    /// </summary>
    public partial class AddPromo : Window
    {
        public delegate void Added();
        public Added addedhandler { get; set; }
        Promo promo;
        public AddPromo()
        {
            InitializeComponent();
            promo = new Promo();
            KeyDown += Paste;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PromoAdd(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            if (startBox.SelectedDate == null || endBox.SelectedDate == null || String.IsNullOrEmpty(descriptionBox.Text) || promo.Picture == null)
            {
                MessageBox.Show("Введите все необходимые значения");
                return;
            }
            if(startBox.SelectedDate > endBox.SelectedDate || endBox.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Даты введены некорректно");
                return;
            }
            if (Int32.Parse(coefBox.Text) >= 100 || Int32.Parse(coefBox.Text) <= 0)
            {
                MessageBox.Show("Скидка введена некорректно");
                return;
            }

            promo.StartDate = (DateTime)startBox.SelectedDate;
            promo.EndDate = (DateTime)endBox.SelectedDate;
            promo.Description = descriptionBox.Text;
            context.Add(promo);
            Log log = new Log()
            {
                Contents = String.Format("{0}: добавил новую акцию с ID:{1} {2}", DataHelper.username, promo.Id, DateTime.Now),
                Administrator = context.Administrators.Single(a => a.Username == DataHelper.username)
            };
            context.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBox.Show("Акция успешно добавлена");
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
                        promo.Picture = DataHelper.GetBytes(Clipboard.GetImage());
                    }
                    return;
                }
            }
        }
    }
}
