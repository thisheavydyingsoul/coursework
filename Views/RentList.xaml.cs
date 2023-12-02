using CourseWorkAdmin.Data;
using CourseWorkAdmin.Models;
using CourseWorkAdmin.Models;
using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// Interaction logic for RentList.xaml
    /// </summary>
    public partial class RentList : Window
    {
        ComputerClubDBContext context = new ComputerClubDBContext();
        Rent[] rs;
        public RentList(Device d)
        {
            InitializeComponent();
            rs = context.Rents.Where(r => r.Device == d).Include(r => r.Client).Include(r => r.Review).ToArray();
            RentsList.ItemsSource = rs;
        }
        private void ShowReview(object sender, RoutedEventArgs e)
        {
            Review r = context.Reviews.FirstOrDefault(r => r.Rent == (Rent)RentsList.SelectedItem);
            MessageBox.Show(r == null ? "К данной аренде нет отзыва" : r.Contents.IsNullOrEmpty() ? "Отзыв содержит только оценку" : r.Contents);
        }

    }
}
