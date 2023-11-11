using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CourseWorkAdmins.Models;
using CourseWorkAdmins.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddAdmin.xaml
    /// </summary>
    public partial class AddAdmin : Window
    {
        public delegate void Added();
        public Added addedhandler { get; set; }
        public AddAdmin()
        {
            InitializeComponent();
            using ComputerClubDBContext context = new ComputerClubDBContext();
            officebox.ItemsSource = context.Offices.Select(
                o => o.Adress).ToArray();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled =regex.IsMatch(e.Text);
        }
        private void AddAdministrator(object sender, RoutedEventArgs e)
        {
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            string name = namebox.Text;
            string email = emailbox.Text;
            string username = usernamebox.Text;
            string password = PasswordHash.hashPassword(passwordbox.Password);
            string phone = phonebox.Text;
            //Checking if all neccessary fields are filled
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Введите все необходимые значения!");
                return;
            }
            bool hrworker = false;

            //Checking if admin wants to add a new hr admin or not
            if (String.IsNullOrEmpty(officebox.Text))
            {
                string message = "Данный администратор будет работать с сотрудниками и офисами?";
                MessageBoxButton add = MessageBoxButton.YesNo;
                MessageBoxResult result;
                result = MessageBox.Show(message, "Внимание!", add);
                if (result == MessageBoxResult.Yes)
                    hrworker = true;
                else
                {
                    MessageBox.Show("Выберите офис!");
                    return;
                }
            }
            string? office = null;
            if (!hrworker)
                office = officebox.Text;
            using ComputerClubDBContext context = new ComputerClubDBContext();

            //Checking if there already is an admin with such username
            var admin = context.Administrators
                .Where(a => a.Username == username)
                .FirstOrDefault();
            if (admin != null)
            {
                MessageBox.Show("Администратор с таким никнеймом уже существует!");
                return;
            }

            Administrator administrator = new Administrator()
            {
                Fullname = name,
                Username = username,
                Password = password,
                Email = email,
                Phone = phone,
                IsActive = true,
                IsHr = hrworker
            };

            //Adding and checking office
            if (office != null)
            {
                var temp = context.Offices
                    .Where(o => o.Adress == office)
                    .FirstOrDefault();
                if (temp == null)
                {
                    MessageBox.Show("Произошла непредвиденная ошибка");
                    return;
                }
                administrator.Office = temp;
            }
            else administrator.Office = null;

            //Checking if admin has rights
            
            context.Add(administrator);
            Log log = new Log()
            {
                Contents = String.Format("{0}: добавил нового администратора под ником {1} {2}", DataHelper.username, administrator.Username, DateTime.Now),
                Administrator = context.Administrators.Where(
                    a => a.Username == DataHelper.username).FirstOrDefault()
            };
            context.Add(log);

            //Checking if something goes wrong
            if (DataHelper.TrySaving(context))
            {
                MessageBoxResult result2 = MessageBox.Show("Администратор успешно добавлен", "Успех", MessageBoxButton.OK);
                if (result2 == MessageBoxResult.OK)
                    Close();
            }
            addedhandler();
        }
    }
}
