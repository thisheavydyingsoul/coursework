using CourseWorkAdmin.Models;
using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
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
    /// Interaction logic for ChangePromo.xaml
    /// </summary>
    public partial class ChangePromo : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }
        bool changed = false;
        Log log;
        Promo promo;
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public ChangePromo(Promo selected)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();

            InitializeComponent();
            promo = selected;
            log = new Log()
            {
                Contents = String.Format("{0} изменил акцию с ID: {1}: ", DataHelper.username, promo.Id),
                Administrator = context.Administrators.Single(
                  a => a.Username == DataHelper.username)
            };
            KeyDown += Paste;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            Promo notchanged = context.Promos.Single(p => p.Id == promo.Id);
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
          
            if (promo.StartDate == startBox.SelectedDate && notchanged.EndDate == endBox.SelectedDate && notchanged.Description == descriptionBox.Text && notchanged.Coefficient.ToString() == coefBox.Text && !changed)
            {
                MessageBox.Show("Вы не внесли никаких изменений");
                return;
            }
            if(startBox.SelectedDate > endBox.SelectedDate)
            {
                MessageBox.Show("Даты введены некорректно");
                return;
            }
            if(Int32.Parse(coefBox.Text)>= 100 || Int32.Parse(coefBox.Text) <= 0)
            {
                MessageBox.Show("Скидка введена некорректно");
                return;
            }
            if(promo.StartDate != startBox.SelectedDate)
            {
                promo.StartDate = (DateTime)startBox.SelectedDate;
                log.Contents += (changed)?", дата начала" : "дата начала";
                changed = true;
            }
            if (promo.EndDate != endBox.SelectedDate)
            {
                promo.EndDate = (DateTime)endBox.SelectedDate;
                log.Contents += (changed) ? ", дата окончания" : "дата начала";
                changed = true;
            }
            if(promo.Description != descriptionBox.Text)
            {
                promo.Description = descriptionBox.Text;
                log.Contents += (changed) ? ", описание" : "описание";
                changed = true;
            }
            if(promo.Coefficient.ToString() != coefBox.Text)
            {
                promo.Coefficient = Int32.Parse(coefBox.Text);
                log.Contents += (changed) ? ", скидка" : "скидка";
            }
            context.Entry(notchanged).CurrentValues.SetValues(promo);
            context.Logs.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBox.Show("Акция успешно изменена");
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
                        promo.Picture = DataHelper.GetBytes(Clipboard.GetImage());
                        changed = true;
                        log.Contents += "изображение";
                    }
                    return;
                }
            }
        }
    }
}
