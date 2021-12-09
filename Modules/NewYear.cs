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
    internal class TimeUntilNewYear : ElkiTimer
    {
        private readonly string[] timesElki;

        Image _newImage; // Изображение иконки
        DateTime _timeNow;        
        DateTime _timeNewYear;
        TimeSpan _timeUntilNewYear;
        public TimeUntilNewYear(double dt) : base(dt) 
        {
            _newImage = Image.FromFile(@"resources\elka.png");

            _timeNewYear = new DateTime(2022, 1, 1, 0, 0, 0);
        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {
            _timeNow = DateTime.Now;
            _timeUntilNewYear =  _timeNewYear - _timeNow;

            Console.WriteLine($"Дней до нового года {_timeUntilNewYear.Days}");
            Console.WriteLine($"Часов до нового года {(int)_timeUntilNewYear.TotalHours}");
            Console.WriteLine($"Минут до нового года {(int)_timeUntilNewYear.TotalMinutes}");

            Bitmap b = new Bitmap(300, 100);
            using (Graphics g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.DarkBlue);
                Font drawFont1 = new Font("Arial", 18);
                Font drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                StringFormat drawFormat = new StringFormat();

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(_newImage, 63, 5, 85, 85);

                g.DrawString("ДО", new Font("Arial", 30, FontStyle.Italic), new SolidBrush(Color.Red), 5, 35, drawFormat);               

                //Рисуем вычесленные времена отправления электричек
                g.DrawString(_timeUntilNewYear.Days.ToString(), new Font("Arial", 18, FontStyle.Bold), drawBrush, 137, 7, drawFormat);
                g.DrawString("дня", drawFont1, new SolidBrush(Color.Black), 217, 7, drawFormat);

                g.DrawString($"{(int)_timeUntilNewYear.TotalHours}", new Font("Arial", 18, FontStyle.Bold), drawBrush, 137, 35, drawFormat);
                g.DrawString("часов", drawFont1, new SolidBrush(Color.Black), 217, 35, drawFormat);

                g.DrawString($"{(int)_timeUntilNewYear.TotalMinutes}", new Font("Arial", 18, FontStyle.Bold), drawBrush, 137, 63, drawFormat);
                g.DrawString("минут", drawFont1, new SolidBrush(Color.Black), 217, 63, drawFormat);
               

                b.Save(@"output\timenewyear1.bmp", ImageFormat.Bmp);
                b.Save(@"output\timenewyear2.bmp", ImageFormat.Bmp);
            }
        }
    }    
}
