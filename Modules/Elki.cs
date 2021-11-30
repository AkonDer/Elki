using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Elki
{
    internal class Elki : ElkiTimer
    {
        private readonly string[] timesElki;
        double timeNow;
        Image _newImage; // Изображение иконки
        public Elki(double dt, string fileName) : base(dt) 
        {
            timesElki = File.ReadAllLines(fileName);
            
            _newImage = Image.FromFile(@"resources\icon.png");
        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {

            string time1 = "";
            string time2 = "";
            string time3 = "";

            double time;

            Bitmap b = new Bitmap(170, 100);
            using (Graphics g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.DarkBlue);
                Font drawFont1 = new Font("Arial", 18);
                Font drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                StringFormat drawFormat = new StringFormat();

                // Рисуем линии
                Pen ePen = new Pen(Color.DarkBlue, 1);               

                int i;

                // находим текущее время и переводим его в число с плавающей точкой
                timeNow = ConvertTimeToDouble(DateTime.Now);

                for (i = 0; i < timesElki.Length; i++)
                {
                    // Если текущее время меньше минимального за день
                    if (i == 0)
                    {
                        time1 = timesElki[timesElki.Length - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[i + 1];
                    }
                    else if (i > 0 && i < timesElki.Length - 1)
                    {
                        time1 = timesElki[i - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[i + 1];
                    }
                    else if (i == timesElki.Length - 1)
                    {
                        time1 = timesElki[i - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[0];
                    }

                    time = ConvertTimeToDouble(Convert.ToDateTime(time2));
                    if (time - timeNow >= 0) break;
                }

                // Если текущее время больше максимального в расписании за день
                if (timeNow > ConvertTimeToDouble(Convert.ToDateTime(timesElki[timesElki.Length - 1])))
                {
                    time1 = timesElki[timesElki.Length - 1];
                    time2 = timesElki[0];
                    time3 = timesElki[1];
                }

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(_newImage, 0, 5, 85, 85);

                // рисуем линии
                g.DrawLine(ePen, 78, 34, 160, 34);
                g.DrawLine(ePen, 78, 68, 160, 68);

                //Рисуем вычесленные времена отправления электричек
                g.DrawString(time1, drawFont1, drawBrush, 87, 7, drawFormat);
                g.DrawString(time3, drawFont1, drawBrush, 87, 70, drawFormat);

                g.DrawString(time2, drawFont2, drawBrush, 72, 33, drawFormat);

                b.Save(@"output\timeelki.bmp", ImageFormat.Bmp);
                b.Save(@"output\timeelki2.bmp", ImageFormat.Bmp);
            }
        }

        private double ConvertTimeToDouble(DateTime dt)
        {
            return dt.Hour + Convert.ToDouble(dt.Minute) / 60;
        }
    }    
}
