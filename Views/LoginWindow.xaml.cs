using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
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

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = PasswordHash.hashPassword(Password.Password);
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            using ComputerClubDBContext context = new ComputerClubDBContext();
            var tryLogin = context.Administrators
                .Where(a => a.Username == username && a.Password == password)
                .FirstOrDefault();
            if (tryLogin == null)
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
                return;
            }
            if(!tryLogin.IsActive)
            {
                MessageBox.Show("У вас нет прав для доступа в систему, обратитесь к своим коллегам.");
                Close();
                return;
            }
            DataHelper.username = tryLogin.Username;
            if (tryLogin.IsHr)
            {
                App.Current.MainWindow = new HRWindow();
                App.Current.MainWindow.Show();
                this.Close();
            }
            else
            {
                App.Current.MainWindow = new OfficeWindow();
                App.Current.MainWindow.Show();
                this.Close();
            }
            context.Logs.Add(new Log()
            {
                Contents = String.Format("{0} вошел в систему {1}", username, DateTime.Now),
                Administrator = tryLogin
            });
            DataHelper.TrySaving(context);
        }
    }
}
