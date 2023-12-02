using CourseWorkAdmin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CourseWorkAdmin.Models;
using System.Windows.Media.Imaging;
using CourseWorkAdmin.Models;
using System.IO;

namespace CourseWorkAdmin
{
    public class DataHelper
    {
        public static string username;
        public static bool CheckActivity()
        {
            using ComputerClubDBContext context = new ComputerClubDBContext();
            if (!(from check in context.Administrators
                  where check.Username == DataHelper.username
                  select check.IsActive).FirstOrDefault())
            {
                MessageBox.Show("У вас недостаточно прав чтобы выполнить данный запрос, обратитесь к своим коллегам.");
                return false;
            }
            return true;
        }
        public static byte[] GetBytes(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 100;
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
                byte[] img = stream.ToArray();
                stream.Close();
                return img;
            }
        }
        public static BitmapSource GetBitMap(byte[] image)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(image);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            return biImg;
        }
        public static bool TrySaving(ComputerClubDBContext context)
        {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
                return false;
            }
        }
        public static double DataToDouble(DateTime dt)
        {
            return Double.Parse(dt.ToString("HH")) + ((double)dt.Minute) / 60.0;
        }
        public static double TimeSpanToDouble(TimeSpan ts)
        {
            return Math.Abs(ts.TotalMinutes / 60.0);
        }
        public static bool IsNight(DateTime dt)
        {
            return (dt.Hour < 8 || dt.Hour > 23);
        }
    }
}
