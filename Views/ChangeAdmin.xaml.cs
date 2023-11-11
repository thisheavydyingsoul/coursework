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
    /// Interaction logic for ChangeAdmin.xaml
    /// </summary>
    public partial class ChangeAdmin : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }
        Administrator notchangedAdm;
        public ChangeAdmin(Administrator admin)
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            officeBox.ItemsSource = context.Offices.Select(
                o => o.Adress).ToArray();
            notchangedAdm = admin;
            nameBox.Text = notchangedAdm.Fullname;
            if (notchangedAdm.Office != null)
                officeBox.Text = notchangedAdm.Office.Adress;
            phoneBox.Text = notchangedAdm.Phone;
            mailBox.Text = notchangedAdm.Email;
            activeBox.IsChecked = notchangedAdm.IsActive;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TryChange(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            Log log = new Log()
            {
                Contents = String.Format("{0} изменил данные для {1}: ", DataHelper.username, notchangedAdm.Username),
                
            };
            bool changed = false;
            var changedAdm = context.Administrators
                .Where(a => a.Username == notchangedAdm.Username)
                .FirstOrDefault();
            if(changedAdm == null)
            {
                MessageBox.Show("Что-то пошло не так, попробуйте обновить список администраторов");
                return;
            }
            if (notchangedAdm.Fullname != nameBox.Text)
            {
                changedAdm.Fullname = nameBox.Text;
                changed = true;
                log.Contents += "полное имя";
            }
            if (!String.IsNullOrEmpty(passwordBox.Password))
            {
                changedAdm.Password = PasswordHash.hashPassword(passwordBox.Password);
                if (changed) log.Contents += ", пароль";
                else log.Contents += "пароль";
                changed = true;
            }
            if((notchangedAdm.Office == null && !String.IsNullOrEmpty(officeBox.Text))
                || (notchangedAdm.Office != null && String.IsNullOrEmpty(officeBox.Text)))
                {
                if (String.IsNullOrEmpty(officeBox.Text))
                {
                    var temp = context.Offices
                        .Where(o => o.Adress == officeBox.Text)
                        .FirstOrDefault();
                    if (temp == null)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка");
                        return;
                    }
                    changedAdm.Office = temp;
                    changedAdm.IsHr = false;
                }
                else
                {
                    changedAdm.Office = null;
                    changedAdm.IsHr = true;
                }
                if (changed) log.Contents += ", офис";
                else log.Contents += "офис";
            }
            if (notchangedAdm.Phone != phoneBox.Text)
            {
                changedAdm.Phone = phoneBox.Text;
                if (changed) log.Contents += ", номер телефона";
                else log.Contents += "номер телефона";
                changed = true;
            }
            if (notchangedAdm.Email != mailBox.Text)
            {
                changedAdm.Email = mailBox.Text;
                if (changed) log.Contents += ", электронная почта";
                else log.Contents += "электронная почта";
                changed = true;
            }
            if (notchangedAdm.IsActive != activeBox.IsChecked)
            {
                changedAdm.IsActive = (bool)activeBox.IsChecked;
                if (changed) log.Contents += ", действующий сотрудник";
                else log.Contents += "действующий сотрудник";
                changed = true;
            }
            context.Entry(notchangedAdm).CurrentValues.SetValues(changedAdm);
            DataHelper.TrySaving(context);
            log.Administrator = context.Administrators.Single(
                a => a.Username == DataHelper.username);
            log.Contents += DateTime.Now;
            context.Logs.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBoxResult result2 = MessageBox.Show("Администратор успешно изменен", "Успех", MessageBoxButton.OK);
                if (result2 == MessageBoxResult.OK)
                    Close();
            }
            changedhandler();
        }
    }
}
