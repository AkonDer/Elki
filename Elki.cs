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
        private string[] timesElki;
        public Elki(double dt, string fileName) : base(dt) 
        {
            timesElki = File.ReadAllLines("data.txt");
        }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            double timeNow;
            var time1 = "";
            var time2 = "";
            var time3 = "";

            double time;

            var b = new Bitmap(170, 100);
            using (var g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                var drawBrush = new SolidBrush(Color.DarkBlue);
                var drawFont1 = new Font("Arial", 18);
                var drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                var drawFormat = new StringFormat();

                // Рисуем линии
                var ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                var newImage = Image.FromFile("icon.png");

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
                g.DrawImage(newImage, 0, 5, 85, 85);

                // рисуем линии
                g.DrawLine(ePen, 78, 34, 160, 34);
                g.DrawLine(ePen, 78, 68, 160, 68);

                //Рисуем вычесленные времена отправления электричек
                g.DrawString(time1, drawFont1, drawBrush, 87, 7, drawFormat);
                g.DrawString(time3, drawFont1, drawBrush, 87, 70, drawFormat);

                g.DrawString(time2, drawFont2, drawBrush, 72, 33, drawFormat);

                b.Save(@"timeelki.bmp", ImageFormat.Bmp);
                b.Save(@"timeelki2.bmp", ImageFormat.Bmp);
            }
        }

        private double ConvertTimeToDouble(DateTime dt)
        {
            return dt.Hour + Convert.ToDouble(dt.Minute) / 60;
        }
    }    
}
