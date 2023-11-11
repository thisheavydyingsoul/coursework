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

namespace CourseWorkAdmin.Views
{
    /// <summary>
    /// Interaction logic for AddOffice.xaml
    /// </summary>
    public partial class AddOffice : Window
    {
        public delegate void Added();
        public Added addedhandler { get; set; }
        public AddOffice()
        {
            InitializeComponent();
        }
        private void addOffice(object sender, RoutedEventArgs e)
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!DataHelper.CheckActivity())
            {
                Close();
                return;
            }
            string adress = adressBox.Text;

            if (String.IsNullOrEmpty(adress))
            {
                MessageBox.Show("Введите адрес!");
                return;
            }
            
            var checkOffice = context.Offices
                .Where(o => o.Adress == adress)
                .FirstOrDefault();
            if (checkOffice != null)
            {
                MessageBox.Show("Офис с таким адресом уже существует!");
                return;
            }

            Office office = new Office()
            {
                Adress = adress
            };

            context.Add(office);
            var currentAdmin = context.Administrators.Where(
                    a => a.Username == DataHelper.username).FirstOrDefault();
            if (currentAdmin == null)
            {
                MessageBox.Show("Что-то пошло не так, попробуйте перезапустить приложение");
            }
            Log log = new Log()
            {
                Contents = String.Format("{0}: добавил новый офис с адресом {1} {2}", DataHelper.username, office.Adress, DateTime.Now),
                Administrator = currentAdmin
            };
            context.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBoxResult result2 = MessageBox.Show("Офис успешно добавлен", "Успех", MessageBoxButton.OK);
                if (result2 == MessageBoxResult.OK)
                    Close();
            }
            addedhandler();


        }
    }
}
