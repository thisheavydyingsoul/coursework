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
    /// Interaction logic for ChangeOffice.xaml
    /// </summary>
    public partial class ChangeOffice : Window
    {
        public delegate void Changed();
        public Changed changedhandler { get; set; }

        Office notChangedOffice;
        public ChangeOffice(Office office)
        {
            InitializeComponent();
            notChangedOffice = office;
            adressBox.Text = notChangedOffice.Adress;
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
                Contents = String.Format("{0} изменил адрес офиса с ID:{1} {2}", DataHelper.username, notChangedOffice.Id, DateTime.Now),
                Administrator = context.Administrators.Single(
                    a => a.Username == DataHelper.username)
            };
            bool changed = false;
            var changedOffice = context.Offices
                .Where(o => o.Id == notChangedOffice.Id)
                .FirstOrDefault();
            if (changedOffice == null)
            {
                MessageBox.Show("Что-то пошло не так, попробуйте обновить список офисов");
                return;
            }
            if (notChangedOffice.Adress != adressBox.Text)
            {
                changedOffice.Adress = adressBox.Text;
                changed = true;
            }
            if (!changed)
            {
                MessageBox.Show("Вы не внесли никаких изменений");
                return;
            }
            context.Entry(notChangedOffice).CurrentValues.SetValues(changedOffice);
            context.Logs.Add(log);
            if (DataHelper.TrySaving(context))
            {
                MessageBoxResult result2 = MessageBox.Show("Офис успешно изменен", "Успех", MessageBoxButton.OK);
                if (result2 == MessageBoxResult.OK)
                    Close();
            }
            changedhandler();
        }
    }
}
