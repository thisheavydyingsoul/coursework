using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWorkAdmin
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DataHelper.username = "whl";
            MainWindow = new Views.OfficeWindow();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
